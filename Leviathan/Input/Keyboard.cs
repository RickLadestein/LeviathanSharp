using Leviathan.Input.Listeners;
using Leviathan.Util;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Input
{
    public class Keyboard
    {
        private InterfaceHandler<IKeyboardListener> keyboard_listeners;
        public Keyboard(in GameWindow window)
        {
            keyboard_listeners = new InterfaceHandler<IKeyboardListener>();
            window.KeyDown += Window_KeyDown;
            window.KeyUp += Window_KeyUp;
        }

        public void AddListener(IKeyboardListener sub)
        {
            this.keyboard_listeners.AddSubscriber(sub);
        }

        public void RemoveListener(IKeyboardListener sub)
        {
            this.keyboard_listeners.RemoveSubscriber(sub);
        }

        private void Window_KeyUp(OpenTK.Windowing.Common.KeyboardKeyEventArgs obj)
        {
            this.keyboard_listeners.Invoke(new Action<IKeyboardListener>((item) =>
            {
                item?.OnKeyRelease(obj.ScanCode);
            }));
            return;
        }

        private void Window_KeyDown(OpenTK.Windowing.Common.KeyboardKeyEventArgs obj)
        {
            this.keyboard_listeners.Invoke(new Action<IKeyboardListener>((item) =>
            {
                item?.OnKeyPress(obj.ScanCode);
            }));
            return;
        }
    }
}
