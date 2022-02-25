using Leviathan.Core.Input;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.Core.Windowing
{
    public class Window : NativeWindow
    {
        private double prev_frametime;

        public event WindowRefreshFunc refresh;
        public event WindowMoveFunc move;
        public event WindowResizeFunc resize;
        public event WindowFocusFunc focus;
        public event WindowCloseFunc close;

        public bool ShutdownRequested { get; private set; }

        public Keyboard Keyboard { get; private set; }
        public Mouse Mouse { get; private set; }

        private static float MIN_FRAMETIME = 0.002f;

        public Window(uint width, uint height, string title, WindowMode mode) : base(width, height, title, mode)
        {
            prev_frametime = 0.0f;
            w_context.SetParentForFirstTimeWindow(this);
            Context.MakeContextCurrent(w_context);
            BindGLFWCallbacks();
        }

        public Window(uint width, uint height, WindowMode mode) : base(width, height, "Window", mode)
        {
            prev_frametime = 0.0f;
            w_context.SetParentForFirstTimeWindow(this);
            Context.MakeContextCurrent(w_context);
            BindGLFWCallbacks();
        }

        private unsafe void BindGLFWCallbacks()
        {
            Mouse = new Mouse();
            Keyboard = new Keyboard();
            Context.GLFWContext.SetWindowIconifyCallback(w_handle, OnWindowIconified);
            Context.GLFWContext.SetWindowPosCallback(w_handle, OnWindowPosChanged);
            Context.GLFWContext.SetWindowSizeCallback(w_handle, OnWindowSizeChanged);
            Context.GLFWContext.SetWindowCloseCallback(w_handle, OnWindowCloseRequested);
            Context.GLFWContext.SetWindowFocusCallback(w_handle, OnWindowFocusChanged);
        }

        public static void Start(Window wnd)
        {
            wnd.Run();
        }


        private unsafe void Run() {
            Context.GLContext.ClearColor(0.25f, 0.25f, 0.25f, 1.0f);
            Context.GLContext.Enable(EnableCap.DepthTest);
            double curr_frametime;
            while(!this.ShutdownRequested)
            {
                PollEvents();
                if (!w_isIconified)
                {
                    ClearBuffer(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    refresh?.Invoke();
                    SwapBuffers();
                    ClearBuffer(ClearBufferMask.DepthBufferBit);
                }

                curr_frametime = Context.GLFWContext.GetTime();
                Time.FrameDelta = (float)(curr_frametime - prev_frametime);
                prev_frametime = curr_frametime;

                this.ShutdownRequested = Context.GLFWContext.WindowShouldClose(w_handle);
            }
        }


        #region WindowCallbacks
        private unsafe void OnWindowIconified(WindowHandle* wnd, bool iconified)
        {
            w_isIconified = iconified;
        }


        private unsafe void OnWindowPosChanged(WindowHandle* wnd, int xpos, int ypos)
        {
            w_position.X = xpos;
            w_position.Y = ypos;
            move?.Invoke(new Math.Vector2f(xpos, ypos));
        }

        private unsafe void OnWindowSizeChanged(WindowHandle* wnd, int width, int height)
        {
            w_size.X = width;
            w_size.Y = height;
            resize?.Invoke(new Math.Vector2f(width, height));
        }

        private unsafe void OnWindowFocusChanged(WindowHandle* wnd, bool focused)
        {
            w_hasfocus = focused;
            focus?.Invoke(focused);
        }

        private unsafe void OnWindowCloseRequested(WindowHandle* wnd)
        {
            ShutdownRequested = true;
            close?.Invoke();
        }

        #endregion
    }
}
