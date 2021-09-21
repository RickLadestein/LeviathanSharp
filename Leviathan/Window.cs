using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan
{
    public class Window
    {
        private NativeWindow nativeWindow;

        public bool ShutdownRequested { get; private set; }

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
                nativeWindow.PollEvents();

                nativeWindow.ClearBuffer(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                nativeWindow.SwapBuffers();
                this.ShutdownRequested = nativeWindow.glfw_context.WindowShouldClose(nativeWindow.w_handle);
            }
        }

        private unsafe void OnWindowIconified(WindowHandle* wnd, bool iconified)
        {
            nativeWindow.w_isIconified = iconified;
        }


        private unsafe void OnWindowPosChanged(WindowHandle* wnd, int xpos, int ypos)
        {
            nativeWindow.w_position = new Math.Vector2f(xpos, ypos);
        }

        private unsafe void OnWindowSizeChanged(WindowHandle* wnd, int width, int height)
        {
            nativeWindow.w_size = new Math.Vector2f(width, height);
        }

        private unsafe void OnWindowFocusChanged(WindowHandle* wnd, bool focused)
        {
            nativeWindow.w_hasfocus = focused;
        }

        private unsafe void OnWindowCloseRequested(WindowHandle* wnd)
        {
            ShutdownRequested = true;
        }
    }
}
