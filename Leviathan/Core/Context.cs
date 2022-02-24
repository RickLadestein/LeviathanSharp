using Silk.NET.OpenGL;
using Silk.NET.GLFW;
using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Core.Windowing;
using OpenTK.Audio.OpenAL;
using System.Threading.Tasks;
using Leviathan.Util;
using Leviathan.Core.Graphics;
using Leviathan.Core.Sound;

namespace Leviathan.Core
{
    public class Context : IDisposable
    {
        private GL gl_context;
        private Glfw glfw_context;
        private ALDevice sound_device;
        private ALContext al_context;
        private Window parent_window;

        private MeshResourceManager m_manager;
        private ShaderResourceManager s_manager;
        private TextureResourceManager t_manager;
        private AudioResourceManager a_manager;

        private static Context current;

        public Context(GL glc, Glfw glfwc, ALDevice aldevice, ALContext alcontext)
        {
            if(glc == null)
            {
                throw new ArgumentNullException(nameof(glc));
            } else
            {
                gl_context = glc;
            }

            if(glfwc == null)
            {
                throw new ArgumentNullException(nameof(glc));
            }
            else
            {
                glfw_context = glfwc;
            }

            if(aldevice == ALDevice.Null)
            {
                throw new ArgumentException("Cannot initialise ALDevice with null");
            } else
            {
                sound_device = aldevice;
            }

            if(alcontext == ALContext.Null)
            {
                throw new ArgumentException("Cannot initialise ALContext with null");
            }
            else
            {
                al_context = alcontext;
            }
        }

        public void SetParentForFirstTimeWindow(Window w_parent)
        {
            if(this.parent_window != null)
            {
                throw new Exception("Parent window may only be set once during window creation");
            } else
            {
                m_manager = new MeshResourceManager();
                s_manager = new ShaderResourceManager();
                t_manager = new TextureResourceManager();
                a_manager = new AudioResourceManager();

                MeshResourceManager.InitDefaultResources();
                this.parent_window = w_parent;
            }
        }

        ~Context()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (gl_context != null)
            {
                gl_context.Dispose();
            }

            if (glfw_context != null)
            {
                glfw_context.Terminate();
                glfw_context.Dispose();
            }

            if (al_context != ALContext.Null && sound_device != ALDevice.Null)
            {
                ALC.DestroyContext(al_context);
                ALC.CloseDevice(sound_device);
            }
        }

        public static GL GLContext { 
            get {
                if(!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.gl_context;
            }
        }

        public static Glfw GLFWContext {
            get
            {
                if (!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.glfw_context;
            }
        }

        public static ALDevice SoundDevice { 
            get
            {
                if (!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.sound_device;
            }
        }

        public static ALContext ALContext {
            get
            {
                if(!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.al_context;
            }
        }

        public static Window ParentWindow {
            get
            {
                if (!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.parent_window;
            }
        }

        public static MeshResourceManager MeshManager
        {
            get
            {
                if(!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.m_manager;
            }
        }

        public static ShaderResourceManager Shadermanager
        {
            get
            {
                if (!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.s_manager;
            }
        }

        public static TextureResourceManager TextureManager
        {
            get
            {
                if (!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.t_manager;
            }
        }

        public static AudioResourceManager AudioManager
        {
            get
            {
                if (!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.a_manager;
            }
        }

        

        private static bool CheckContext()
        {
            return current != null;
        }

        public static void MakeContextCurrent(Context new_context)
        {
            if(new_context == null)
            {
                throw new ArgumentNullException(nameof(new_context));
            } else
            {
                current = new_context;
            }
        }
    }

    public class InvalidContextException : Exception
    {
        public InvalidContextException() : base("Tried to access an invalid or NULL context") {}
    }



    #region ResourceManagers

    public class MeshResourceManager : ResourceManager<Mesh>
    {
        public MeshResourceManager() : base()
        {
            IEnumerable<string> eFiles = System.IO.Directory.EnumerateFiles(".\\assets\\default");
            foreach (string file in eFiles)
            {
                if (file.EndsWith(".obj"))
                {
                    String[] tokens = file.Split("\\");
                    string full_name = tokens[tokens.Length - 1];
                    string name = full_name.Remove(full_name.Length - 4);
                    Mesh.Import(name, file, ElementType.TRIANGLES, out Mesh mesh);
                    this.AddResource(name, mesh);
                }
            }
        }

    }

    public class ShaderResourceManager : ResourceManager<ShaderProgram>
    {
        public ShaderResourceManager() : base()
        {
            IEnumerable<string> eFiles = System.IO.Directory.EnumerateFiles(".\\assets\\default");
            foreach (string file in eFiles)
            {
                if (file.EndsWith(".glsl"))
                {
                    String[] tokens = file.Split("\\");
                    string full_name = tokens[tokens.Length - 1];
                    string name = full_name.Remove(full_name.Length - 5);
                    ShaderFile sfile = ShaderFile.Import(file);
                    ShaderProgram sp = null;

                    if (sfile.HasVertex && sfile.HasFragment)
                    {
                        VertexShader vshader = new VertexShader(sfile.Vertex_src);
                        FragmentShader fshader = new FragmentShader(sfile.Fragment_src);
                        if (sfile.HasGeometry)
                        {
                            GeometryShader gshader = new GeometryShader(sfile.Geometry_src);
                            sp = new ShaderProgram(vshader, gshader, fshader);
                        }
                        sp = new ShaderProgram(vshader, fshader);
                    }
                    else if (sfile.HasCompute)
                    {
                        ComputeShader cshader = new ComputeShader(sfile.Compute_src);
                        sp = new ShaderProgram(cshader);
                    }
                    else
                    {
                        throw new Exception("Shaderfile was not complete: please fix");
                    }
                    AddResource(name, sp);
                }
            }
        }
    }

    public class TextureResourceManager : ResourceManager<Graphics.Texture>
    {
        public TextureResourceManager() : base()
        {
            IEnumerable<string> eFiles = System.IO.Directory.EnumerateFiles(".\\assets\\default");
            foreach (string file in eFiles)
            {
                if (file.EndsWith(".jpeg"))
                {
                    String[] tokens = file.Split("\\");
                    string full_name = tokens[tokens.Length - 1];
                    string name = full_name.Remove(full_name.Length - 4);
                    Leviathan.Core.ImageResource image = Leviathan.Core.ImageResource.Load(file, true);
                    Texture2D tex = new Texture2D(image);
                    AddResource(name, tex);
                }
            }
        }
    }

    public class AudioResourceManager : ResourceManager<AudioSample>
    {

    }

    #endregion
}
