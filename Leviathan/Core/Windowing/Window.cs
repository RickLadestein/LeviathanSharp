using Leviathan.Core.Input;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.Core.Windowing
{
    public class Window
    {
        private double prev_frametime;
        public NativeWindow nativeWindow;

        public event WindowRefreshFunc refresh;
        public event WindowMoveFunc move;
        public event WindowResizeFunc resize;
        public event WindowFocusFunc focus;
        public event WindowCloseFunc close;

        public bool ShutdownRequested { get; private set; }

        public Keyboard Keyboard { get; private set; }
        public Mouse Mouse { get; private set; }

        private static float MIN_FRAMETIME = 0.002f;

        public Glfw glfw_context
        {
            get {
                return nativeWindow.graphics_context.glfw_context;
            }
        }

        public GL gl_context
        {
            get
            {
                return nativeWindow.graphics_context.gl_context;
            }
        }

        public Window(uint width, uint height, string title, WindowMode mode)
        {
            prev_frametime = 0.0f;
            this.nativeWindow = NativeWindow.CreateWindow(width, height, title, mode);
            Context.RegisterContext(gl_context, glfw_context, this);
            BindGLFWCallbacks();
        }

        public Window(uint width, uint height, WindowMode mode)
        {
            prev_frametime = 0.0f;
            this.nativeWindow = NativeWindow.CreateWindow(width, height, "Window", mode);
            Context.RegisterContext(gl_context, glfw_context, this);
            BindGLFWCallbacks();
        }

        private unsafe void BindGLFWCallbacks()
        {
            Mouse = new Mouse(ref nativeWindow);
            Keyboard = new Keyboard(ref nativeWindow);
            glfw_context.SetWindowIconifyCallback(nativeWindow.w_handle, OnWindowIconified);
            glfw_context.SetWindowPosCallback(nativeWindow.w_handle, OnWindowPosChanged);
            glfw_context.SetWindowSizeCallback(nativeWindow.w_handle, OnWindowSizeChanged);
            glfw_context.SetWindowCloseCallback(nativeWindow.w_handle, OnWindowCloseRequested);
            glfw_context.SetWindowFocusCallback(nativeWindow.w_handle, OnWindowFocusChanged);
        }

        public static void Start(Window wnd)
        {
            wnd.Run();
        }


        private unsafe void Run() {

            gl_context.ClearColor(0.25f, 0.25f, 0.25f, 1.0f);
            gl_context.Enable(EnableCap.DepthTest);
            double curr_frametime;
            while(!this.ShutdownRequested)
            {
                nativeWindow.PollEvents();
                if (!nativeWindow.w_isIconified)
                {
                    nativeWindow.ClearBuffer(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    refresh?.Invoke();
                    nativeWindow.SwapBuffers();
                    nativeWindow.ClearBuffer(ClearBufferMask.DepthBufferBit);
                }

                curr_frametime = glfw_context.GetTime();
                Time.FrameDelta = (float)(curr_frametime - prev_frametime);
                prev_frametime = curr_frametime;

                this.ShutdownRequested = glfw_context.WindowShouldClose(nativeWindow.w_handle);
            }
        }

        private unsafe void OnWindowIconified(WindowHandle* wnd, bool iconified)
        {
            nativeWindow.w_isIconified = iconified;
        }


        private unsafe void OnWindowPosChanged(WindowHandle* wnd, int xpos, int ypos)
        {
            nativeWindow.w_position.X = xpos;
            nativeWindow.w_position.Y = ypos;
            move?.Invoke(new Math.Vector2f(xpos, ypos));
        }

        private unsafe void OnWindowSizeChanged(WindowHandle* wnd, int width, int height)
        {
            nativeWindow.w_size.X = width;
            nativeWindow.w_size.Y = height;
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
