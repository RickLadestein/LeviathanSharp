using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Leviathan.Util;
using Leviathan.Input;
using Leviathan.Input.Listeners;
using OpenTK.Graphics.OpenGL;

namespace Leviathan.Core
{
    public class Window
    {
        public GameWindow w_handle { get; private set; }
        public Mouse mouse { get; private set; }
        public Keyboard keyboard { get; private set; }
        private InterfaceHandler<IWindowListener>   window_listeners;

        public Window(string title, int width, int height, bool fullscreen, bool resizable = false) {
            GameWindowSettings gw = GameWindowSettings.Default;
            NativeWindowSettings nw = NativeWindowSettings.Default;
            nw.Title = title;
            nw.Size = new Vector2i(width, height);
            nw.IsFullscreen = fullscreen;
            nw.WindowBorder = resizable == true ? WindowBorder.Resizable : WindowBorder.Fixed;
#if DEBUG
            nw.Flags = ContextFlags.Debug;
#endif
            this.w_handle = new GameWindow(gw, nw);
            this.mouse = new Mouse(this.w_handle);
            this.keyboard = new Keyboard(this.w_handle);
            RegisterEventFunctions();
            
            window_listeners = new InterfaceHandler<IWindowListener>();
            w_handle.MakeCurrent();
            GL.ClearColor(1.0f, 0.0f, 0.0f, 1.0f);
        }

        private void RegisterEventFunctions()
        {
            w_handle.Move += Window_Move;
            w_handle.Resize += Window_Resize;
            w_handle.FocusedChanged += Window_FocusedChanged;
            w_handle.RenderFrame += Window_RenderFrame;
        }

        public void SetCursorMode(CursorMode mode)
        {
            this.mouse.SetCursorMode(this.w_handle, mode);
        }

        public void Run()
        {
            this.w_handle.Run();
        }

        public void Close()
        {
            this.w_handle.Close();
        }

        public void AddListener(IWindowListener sub)
        {
            this.window_listeners.AddSubscriber(sub);
        }

        public void RemoveListener(IWindowListener sub)
        {
            this.window_listeners.RemoveSubscriber(sub);
        }

        #region Window_Funcs
        private void Window_RenderFrame(FrameEventArgs obj)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            window_listeners.Invoke(new Action<IWindowListener>((item) => { item?.OnWindowRender(obj.Time); }));
            w_handle.SwapBuffers();
        }

        private void Window_FocusedChanged(FocusedChangedEventArgs obj)
        {
            window_listeners.Invoke(new Action<IWindowListener>((item) => { item?.OnWindowFocusChanged(obj.IsFocused); }));
        }

        private void Window_Resize(ResizeEventArgs obj)
        {
            GL.Viewport(0, 0, obj.Width, obj.Height);
            window_listeners.Invoke(new Action<IWindowListener>((item) => { item?.OnWindowResize(obj.Size); }));
        }

        private void Window_Move(WindowPositionEventArgs obj)
        {
            window_listeners.Invoke(new Action<IWindowListener>((item) => { item?.OnWindowMove(obj.Position); }));
        }
        #endregion
    }
}
