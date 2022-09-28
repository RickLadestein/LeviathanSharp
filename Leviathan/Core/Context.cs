using Silk.NET.OpenGL;
using Silk.NET.GLFW;
using Silk.NET.OpenAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Leviathan.Util;
using Leviathan.Core.Sound;
using Leviathan.Core.Graphics;
using Leviathan.Core.Windowing;

namespace Leviathan.Core
{
    public class Context : IDisposable
    {

        private LGraphicsContext graphics_context;
        private LAudioContext audio_context;

        private Window parent_window;

        private MeshResourceManager m_manager;
        private ShaderResourceManager s_manager;
        private TextureResourceManager t_manager;
        private AudioResourceManager a_manager;
        private MaterialResourceManager material_manager;

        private static Context current;

        public unsafe Context(LGraphicsContext g_context, LAudioContext a_context)
        {
            if(g_context != null && g_context.IsValid())
            {
                graphics_context = g_context;
            } else
            {
                throw new Exception("Supplied graphics context was invalid or null");
            }

            if(a_context != null && a_context.IsValid())
            {
                audio_context = a_context;
            }
            else
            {
                throw new Exception("Supplied audio context was invalid or null");
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
                material_manager = new MaterialResourceManager();

                MeshResourceManager.InitDefaultResources();
                m_manager.InitDefaultModels();
                this.parent_window = w_parent;
            }
        }

        ~Context()
        {
            Dispose();
        }

        public void Dispose()
        {
            if(graphics_context != null)
            {
                graphics_context.Destroy();
            }

            if(audio_context != null)
            {
                audio_context.Destroy();
            }
        }

        public static GL GLContext { 
            get {
                if(!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.graphics_context.gl_context;
            }
        }

        public static Glfw GLFWContext {
            get
            {
                if (!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.graphics_context.glfw_context;
            }
        }

        public static AL ALApi { 
            get
            {
                if (!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.audio_context.al_api;
            }
        }

        public static ALContext ALContext {
            get
            {
                if(!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.audio_context.al_context;
            }
        }

        public static unsafe Device* AudioDevicePtr
        {
            get
            {
                if(!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.audio_context.device_ptr;
            }
        }

        public static unsafe Silk.NET.OpenAL.Context* AudioContextPtr
        {
            get
            {
                if (!CheckContext())
                {
                    throw new InvalidContextException();
                }
                return current.audio_context.context_ptr;
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

        public static MaterialResourceManager MaterialManager
        {
            get
            {
                return current.material_manager;
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

    public unsafe abstract class LContextBase
    {
        public abstract bool IsValid();
        public abstract void Destroy();
    }

    public unsafe class LAudioContext : LContextBase
    {
        public Silk.NET.OpenAL.AL al_api;
        public Silk.NET.OpenAL.ALContext al_context;
        public Silk.NET.OpenAL.Context* context_ptr;
        public Silk.NET.OpenAL.Device* device_ptr;

        public LAudioContext() { }

        public LAudioContext(AL al_api, ALContext al_context, Silk.NET.OpenAL.Context* context_ptr, Device* device_ptr)
        {
            this.al_api = al_api;
            this.al_context = al_context;
            this.context_ptr = context_ptr;
            this.device_ptr = device_ptr;
        }

        public override void Destroy()
        {
            if (al_context != null)
            {
                al_context.DestroyContext(context_ptr);
                al_context.CloseDevice(device_ptr);
                al_context.Dispose();
            }

            if(al_api != null)
            {
                al_api.Dispose();
            }
        }

        public override bool IsValid()
        {
            return al_api != null && al_context != null;
        }
    }

    public unsafe class LGraphicsContext : LContextBase
    {
        public Silk.NET.OpenGL.GL gl_context;
        public Silk.NET.GLFW.Glfw glfw_context;

        public LGraphicsContext(GL gl_context, Glfw glfw_context)
        {
            this.gl_context = gl_context;
            this.glfw_context = glfw_context;
        }

        public override void Destroy()
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
        }

        public override bool IsValid()
        {
            return gl_context != null && glfw_context != null;
        }
    }



    #region ResourceManagers

    public class MeshResourceManager : ResourceManager<Mesh>
    {
        private bool initialised;
        public MeshResourceManager() : base()
        {
            initialised = false;
        }
        
        public void InitDefaultModels()
        {
            if(initialised)
            {
                return;
            }
            IEnumerable<string> eFiles = System.IO.Directory.EnumerateFiles(".\\assets\\default");
            MeshLoader loader = new MeshLoader();
            foreach (string file in eFiles)
            {
                if (file.EndsWith(".obj"))
                {
                    String[] tokens = file.Split("\\");
                    string full_name = tokens[tokens.Length - 1];
                    string name = full_name.Remove(full_name.Length - 4);
                    loader.Import(file, LPrimitiveType.TRIANGLES);
                    //Mesh.Import(name, file, LPrimitiveType.TRIANGLES, out Mesh mesh);
                    //this.AddResource(name, mesh);
                }
            }
            loader.PushToRenderContext();
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

    public class MaterialResourceManager : ResourceManager<Material>
    {
        public MaterialResourceManager() : base()
        {
            Material defaultmaterial = Material.Default;
            this.AddResource("default", defaultmaterial);
        }
    }

    public class AudioResourceManager : ResourceManager<AudioSample>
    {
        public AudioResourceManager() : base()
        {

        }
    }

    #endregion
}
