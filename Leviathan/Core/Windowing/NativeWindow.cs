using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Math;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
namespace Leviathan.Core.Windowing
{
    public struct NativeWindow
    {
        public String w_title;
        public Vector2d w_position;
        public Vector2d w_size;
        public bool w_hasfocus;
        public bool w_isIconified;

        public GL gl_context;
        public Glfw glfw_context;
        public unsafe WindowHandle* w_handle;
        public unsafe GlfwNativeWindow n_window;
        public IntPtr _hwinst;


        public static unsafe NativeWindow CreateWindow(uint width, uint height, string title, WindowMode mode)
        {
            NativeWindow output = new NativeWindow();

            output.glfw_context = Glfw.GetApi();
            bool init = output.glfw_context.Init();
            if (!init)
            {
                throw new Exception("Error while creating GLFW context");
            }

            output.glfw_context.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGL);
            output.glfw_context.WindowHint(WindowHintInt.ContextVersionMajor, 4);
            output.glfw_context.WindowHint(WindowHintInt.ContextVersionMinor, 3);
            output.glfw_context.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
            output.glfw_context.WindowHint(WindowHintBool.OpenGLForwardCompat, true);
            output.glfw_context.WindowHint(WindowHintBool.Resizable, true);
            output.glfw_context.WindowHint(WindowHintBool.DoubleBuffer, true);
            output.glfw_context.SetErrorCallback(OnGLFWError);

            switch (mode)
            {
                case WindowMode.FULLSCREEN:
                    CreateWindowFull(ref output, title, (int)width, (int)height);
                    break;
                case WindowMode.WINDOWED:
                    CreateWindowWindowed(ref output, title, (int)width, (int)height);
                    break;
                case WindowMode.WINDOWED_FULLSCREEN:
                    CreateWindowFullW(ref output, title, (int)width, (int)height);
                    break;
            }
            if (output.w_handle == null)
            {
                throw new Exception("Failed to create window");
            }

            output.glfw_context.ShowWindow(output.w_handle);
            output.glfw_context.MakeContextCurrent(output.w_handle);
            if (output.gl_context == null)
            {
                output.gl_context = GL.GetApi(output.glfw_context.GetProcAddress);
                if (output.gl_context == null)
                {
                    throw new Exception("Could not bind to GLFW PROC");
                }
            }

            output.w_size = new Vector2d((int)width, (int)height);
            output.w_title = title;
            output.glfw_context.GetWindowPos(output.w_handle, out int x_pos, out int y_pos);
            output.w_position = new Vector2d(x_pos, y_pos);

            output.n_window = new GlfwNativeWindow(output.glfw_context, output.w_handle);
            if (output.n_window.Win32.HasValue)
            {
                output._hwinst = output.n_window.Win32.Value.Hwnd;
            }
            else
            {
                throw new Exception("Could not capture native window ptr");
            }

            return output;

        }

        private static unsafe void CreateWindowWindowed(ref NativeWindow wnd, String title, int width, int height)
        {
            Glfw context = Glfw.GetApi();
            wnd.w_handle = context.CreateWindow(width, height, title, null, null);
        }

        private static unsafe void CreateWindowFull(ref NativeWindow wnd, String title, int width, int height)
        {
            Glfw context = Glfw.GetApi();
            Monitor* mon = context.GetPrimaryMonitor();
            wnd.w_handle = context.CreateWindow(width, height, title, mon, null);
        }

        private static unsafe void CreateWindowFullW(ref NativeWindow wnd, String title, int width, int height)
        {
            Glfw context = Glfw.GetApi();
            Monitor* mon = context.GetPrimaryMonitor();
            VideoMode* vidmode = context.GetVideoMode(mon);

            context.WindowHint(WindowHintInt.RedBits, vidmode->RedBits);
            context.WindowHint(WindowHintInt.GreenBits, vidmode->GreenBits);
            context.WindowHint(WindowHintInt.BlueBits, vidmode->BlueBits);
            context.WindowHint(WindowHintInt.RefreshRate, vidmode->RefreshRate);
            wnd.w_handle = context.CreateWindow(width, height, title, mon, null);
            
        }

        public unsafe void SetDrawArea(uint x, uint y, uint width, uint height)
        {
            gl_context.Viewport((int)x, (int)y, width, height);
        }

        public unsafe void SwapBuffers()
        {
            glfw_context.SwapBuffers(w_handle);
        }

        public unsafe void ClearBuffer(ClearBufferMask mask)
        {
            gl_context.Clear((uint)mask);
        }

        public unsafe void PollEvents()
        {
            glfw_context.PollEvents();
        }

        public static unsafe void OnGLFWError(Silk.NET.GLFW.ErrorCode error, string description)
        {
            Console.WriteLine($"GLFW encountered an error: {description}");
        }

        


    }
}
