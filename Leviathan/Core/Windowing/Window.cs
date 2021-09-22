using Leviathan.Core.Input;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Windowing
{
    public class Window
    {
        public NativeWindow nativeWindow;

        public event WindowRefreshFunc refresh;
        public event WindowMoveFunc move;
        public event WindowResizeFunc resize;
        public event WindowFocusFunc focus;
        public event WindowCloseFunc close;

        public bool ShutdownRequested { get; private set; }

        public Keyboard Keyboard { get; private set; }
        public Mouse Mouse { get; private set; }

        public Glfw glfw_context
        {
            get {
                return nativeWindow.glfw_context;
            }
        }

        public GL gl_context
        {
            get
            {
                return nativeWindow.gl_context;
            }
        }

        public Window(uint width, uint height, string title, WindowMode mode)
        {
            this.nativeWindow = NativeWindow.CreateWindow(width, height, title, mode);
            BindGLFWCallbacks();
        }

        public Window(uint width, uint height, WindowMode mode)
        {
            this.nativeWindow = NativeWindow.CreateWindow(width, height, "Window", mode);
            BindGLFWCallbacks();
        }

        private unsafe void BindGLFWCallbacks()
        {
            Mouse = new Mouse(this, ref nativeWindow);
            Keyboard = new Keyboard(this, ref nativeWindow);
            nativeWindow.glfw_context.SetWindowIconifyCallback(nativeWindow.w_handle, OnWindowIconified);
            nativeWindow.glfw_context.SetWindowPosCallback(nativeWindow.w_handle, OnWindowPosChanged);
            nativeWindow.glfw_context.SetWindowSizeCallback(nativeWindow.w_handle, OnWindowSizeChanged);
            nativeWindow.glfw_context.SetWindowCloseCallback(nativeWindow.w_handle, OnWindowCloseRequested);
            nativeWindow.glfw_context.SetWindowFocusCallback(nativeWindow.w_handle, OnWindowFocusChanged);
        }

        public static void Start(Window wnd)
        {
            wnd.Run();
        }


        private unsafe void Run() {

            nativeWindow.gl_context.ClearColor(System.Drawing.Color.Red);
            while(!this.ShutdownRequested)
            {
                if (!nativeWindow.w_isIconified)
                {
                    nativeWindow.PollEvents();
                    nativeWindow.ClearBuffer(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    refresh?.Invoke();


                    nativeWindow.SwapBuffers();
                }
                this.ShutdownRequested = nativeWindow.glfw_context.WindowShouldClose(nativeWindow.w_handle);
            }
        }

        private unsafe void OnWindowIconified(WindowHandle* wnd, bool iconified)
        {
            nativeWindow.w_isIconified = iconified;
        }


        private unsafe void OnWindowPosChanged(WindowHandle* wnd, int xpos, int ypos)
        {
            nativeWindow.w_position.Set(xpos, ypos);
            move?.Invoke(new Math.Vector2f(xpos, ypos));
        }

        private unsafe void OnWindowSizeChanged(WindowHandle* wnd, int width, int height)
        {
            nativeWindow.w_size.Set(width, height);
            resize?.Invoke(new Math.Vector2f(width, height));
        }

        private unsafe void OnWindowFocusChanged(WindowHandle* wnd, bool focused)
        {
            nativeWindow.w_hasfocus = focused;
            focus?.Invoke(focused);
        }

        private unsafe void OnWindowCloseRequested(WindowHandle* wnd)
        {
            ShutdownRequested = true;
            close?.Invoke();
        }
    }
}
