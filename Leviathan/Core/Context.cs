using Silk.NET.OpenGL;
using Silk.NET.GLFW;
using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Core.Windowing;
using OpenTK.Audio.OpenAL;
using System.Threading.Tasks;

namespace Leviathan.Core
{
    public class Context : IDisposable
    {
        private GL gl_context;
        private Glfw glfw_context;
        private ALDevice sound_device;
        private ALContext al_context;
        private Window parent_window;
        
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
}
