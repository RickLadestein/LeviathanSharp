using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Math;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
namespace Leviathan.Core.Windowing
{
    public unsafe class NativeWindow
    {
        protected String w_title;
        protected Vector2i w_position;
        protected Vector2i w_size;
        protected bool w_hasfocus;
        protected bool w_isIconified;

        protected unsafe WindowHandle* w_handle;
        protected unsafe GlfwNativeWindow n_window;
        protected IntPtr _hwinst;
        protected IntPtr n_proc_id;

        protected Context w_context;

        public NativeWindow(uint width, uint height, string title, WindowMode mode)
        {
            w_title = string.Empty;
            w_position = Vector2i.Zero;
            w_size = Vector2i.Zero;
            w_hasfocus = false;
            w_isIconified = false;
            w_handle = null;
            n_window = default;
            _hwinst = IntPtr.Zero;

            CreateWindow(width, height, title, mode);
        }

        #region Window_Creation
        protected unsafe void CreateWindow(uint width, uint height, string title, WindowMode mode)
        {
            CreateGLFWcontext(out Glfw _glfwcontext);

            _glfwcontext.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGL);
            _glfwcontext.WindowHint(WindowHintInt.ContextVersionMajor, 4);
            _glfwcontext.WindowHint(WindowHintInt.ContextVersionMinor, 3);
            _glfwcontext.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
            _glfwcontext.WindowHint(WindowHintBool.OpenGLForwardCompat, true);
            _glfwcontext.WindowHint(WindowHintBool.Resizable, true);
            _glfwcontext.WindowHint(WindowHintBool.DoubleBuffer, true);
            _glfwcontext.SetErrorCallback(OnGLFWError);

            switch (mode)
            {
                case WindowMode.FULLSCREEN:
                    CreateWindowFull(_glfwcontext, title, (int)width, (int)height);
                    break;
                case WindowMode.WINDOWED:
                    CreateWindowWindowed(_glfwcontext, title, (int)width, (int)height);
                    break;
                case WindowMode.WINDOWED_FULLSCREEN:
                    CreateWindowFullW(_glfwcontext, title, (int)width, (int)height);
                    break;
            }
            if (w_handle == null)
            {
                throw new Exception("Failed to create window");
            }

            _glfwcontext.ShowWindow(w_handle);
            _glfwcontext.MakeContextCurrent(w_handle);
            
            CreateGLcontext(_glfwcontext, out GL _glcontext);
            //CreateOpenALcontext("", out ALDevice aldevice, out ALContext alcontext);
            CreateOpenAlcontext("", out LAudioContext a_context);

            w_size = new Vector2i((int)width, (int)height);
            w_title = title;

            _glfwcontext.GetWindowPos(w_handle, out int x_pos, out int y_pos);
            w_position = new Vector2i(x_pos, y_pos);

            n_window = new GlfwNativeWindow(_glfwcontext, w_handle);
            if (n_window.Win32.HasValue)
            {
                _hwinst = n_window.Win32.Value.Hwnd;
                n_proc_id = n_window.Win32.Value.HInstance;
            }
            else
            {
                throw new Exception("Could not capture native window ptr");
            }

            w_context = new Context(new LGraphicsContext(_glcontext, _glfwcontext), a_context);
            Context.MakeContextCurrent(w_context);
        }

        private unsafe void CreateWindowWindowed(Glfw _glfwcontext, String title, int width, int height)
        {
            w_handle = _glfwcontext.CreateWindow(width, height, title, null, null);
        }

        private unsafe void CreateWindowFull(Glfw _glfwcontext, String title, int width, int height)
        {
            Monitor* mon = _glfwcontext.GetPrimaryMonitor();
            w_handle = _glfwcontext.CreateWindow(width, height, title, mon, null);
        }

        private unsafe void CreateWindowFullW(Glfw _glfwcontext, String title, int width, int height)
        {
            Monitor* mon = _glfwcontext.GetPrimaryMonitor();
            VideoMode* vidmode = _glfwcontext.GetVideoMode(mon);

            _glfwcontext.WindowHint(WindowHintInt.RedBits, vidmode->RedBits);
            _glfwcontext.WindowHint(WindowHintInt.GreenBits, vidmode->GreenBits);
            _glfwcontext.WindowHint(WindowHintInt.BlueBits, vidmode->BlueBits);
            _glfwcontext.WindowHint(WindowHintInt.RefreshRate, vidmode->RefreshRate);
            w_handle = _glfwcontext.CreateWindow(width, height, title, mon, null);

        }

        private unsafe void CreateGLFWcontext(out Glfw glfw)
        {
            glfw = Glfw.GetApi();

            bool init = glfw.Init();
            if (!init)
            {
                throw new Exception("Error while creating GLFW context");
            }
        }

        private unsafe void CreateGLcontext(Glfw glfw, out GL gl)
        {
            gl = GL.GetApi(glfw.GetProcAddress);
            if (gl == null)
            {
                throw new Exception("Failed to get OpenGL process address");
            }
        }

        private unsafe void CreateOpenAlcontext(string device_id, out LAudioContext a_context)
        {
            a_context = new LAudioContext();
            a_context.al_context = Silk.NET.OpenAL.ALContext.GetApi();
            a_context.al_api = Silk.NET.OpenAL.AL.GetApi();
            a_context.device_ptr = a_context.al_context.OpenDevice(device_id);
            if(a_context.device_ptr == null)
            {
                throw new Exception("Sound device opening failed");
            }

            a_context.context_ptr = a_context.al_context.CreateContext(a_context.device_ptr, null);
            a_context.al_context.MakeContextCurrent(a_context.context_ptr);
            
            var error = a_context.al_api.GetError();
            if(error != Silk.NET.OpenAL.AudioError.NoError)
            {
                throw new Exception($"OpenAL has encountered an exception: {error}");
            }
        }


        #endregion

        public unsafe void SwapBuffers()
        {
            Context.GLFWContext.SwapBuffers(w_handle);
        }

        public unsafe void ClearBuffer(ClearBufferMask mask)
        {
            Context.GLContext.Clear((uint)mask);
        }

        public unsafe void PollEvents()
        {
            Context.GLFWContext.PollEvents();
        }

        public unsafe void RequestFocus()
        {
            Context.GLFWContext.FocusWindow(w_handle);
        }

        public static unsafe void OnGLFWError(Silk.NET.GLFW.ErrorCode error, string description)
        {
            throw new Exception($"GLFW encountered error code {error}:{description}");
        }

        public static unsafe void OnGLDebug(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userParam)
        {
            throw new NotImplementedException();
        }



        #region Setters
        public unsafe void SetDrawArea(uint x, uint y, uint width, uint height)
        {
            Context.GLContext.Viewport((int)x, (int)y, width, height);
        }

        public unsafe void SetTitle(string title)
        {
            Context.GLFWContext.SetWindowTitle(w_handle, title);
            this.w_title = title;
        }
        public unsafe void SetPosition(Vector2i pos)
        {
            Context.GLFWContext.SetWindowPos(w_handle, pos.X, pos.Y);
        }

        public unsafe void SetSize(Vector2i size)
        {
            Context.GLFWContext.SetWindowSize(w_handle, size.X, size.Y);
        }
        #endregion

        #region Getters

        public unsafe WindowHandle* GetGlfwWindowHandle()
        {
            return this.w_handle;
        }

        public unsafe GlfwNativeWindow GetGlfNativewWindow()
        {
            return this.n_window;
        }

        public unsafe IntPtr GetWinApiWindowHandle()
        {
            return this._hwinst;
        }

        public String GetTitle()
        {
            return this.w_title;
        }

        public Vector2i GetPosition()
        {
            return this.w_position;
        }

        public Vector2i GetSize()
        {
            return this.w_size;
        }

        public bool HasFocus()
        {
            return this.w_hasfocus;
        }

        public bool IsIconified()
        {
            return this.w_isIconified;
        }
        #endregion



        public unsafe void Destroy()
        {
            w_context.Dispose();
        }
    }

    
}
