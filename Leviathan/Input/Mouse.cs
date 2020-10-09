using Leviathan.Core;
using Leviathan.Input.Listeners;
using Leviathan.Util;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Input
{
    public class Mouse
    {
        private InterfaceHandler<IMouseListener> mouse_listeners;
        private Vector2 position;
        public CursorMode cursor_mode { get; private set; }

        public Mouse(in GameWindow window)
        {
            mouse_listeners = new InterfaceHandler<IMouseListener>();
            window.MouseDown += Window_MouseDown;
            window.MouseUp += Window_MouseUp;
            window.MouseMove += Window_MouseMove;
            window.MouseWheel += Window_MouseWheel;
            this.cursor_mode = CursorMode.VISIBLE;
            position = window.MousePosition;
        }

        

        
        public void AddListener(IMouseListener sub)
        {
            this.mouse_listeners.AddSubscriber(sub);
        }

        public void RemoveListener(IMouseListener sub)
        {
            this.mouse_listeners.RemoveSubscriber(sub);
        }

        public void SetCursorMode(in GameWindow window, CursorMode mode)
        {
            switch(mode)
            {
                case CursorMode.VISIBLE:
                    window.CursorVisible = true;
                    window.CursorGrabbed = false;
                    break;
                case CursorMode.INVISIBLE:
                    window.CursorVisible = false;
                    window.CursorGrabbed = false;
                    break;
                case CursorMode.FPS:
                    window.CursorVisible = true;
                    window.CursorGrabbed = true;
                    break;
            }
            this.cursor_mode = mode;
        }

        private void Window_MouseWheel(OpenTK.Windowing.Common.MouseWheelEventArgs obj)
        {
            mouse_listeners.Invoke(new Action<IMouseListener>((item) =>
            {
                item?.OnMouseWheel(this.position, obj.Offset);
            }));
        }

        private void Window_MouseMove(OpenTK.Windowing.Common.MouseMoveEventArgs obj)
        {
            this.position = obj.Position;
            mouse_listeners.Invoke(new Action<IMouseListener>((item) =>
            {
                item?.OnMouseMove(this.position, obj.Delta);
            }));
        }

        private void Window_MouseUp(OpenTK.Windowing.Common.MouseButtonEventArgs obj)
        {
            mouse_listeners.Invoke(new Action<IMouseListener>((item) =>
            {
                item?.OnMousePress(this.position, (int)obj.Button);
            }));
        }

        private void Window_MouseDown(OpenTK.Windowing.Common.MouseButtonEventArgs obj)
        {
            mouse_listeners.Invoke(new Action<IMouseListener>((item) =>
            {
                item?.OnMouseRelease(this.position, (int)obj.Button);
            }));
        }
    }

    public enum CursorMode
    {
        VISIBLE,
        INVISIBLE,
        FPS
    }
}
