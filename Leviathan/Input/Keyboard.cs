using Leviathan.Input.Listeners;
using Leviathan.Util;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Input
{
    public class Keyboard
    {
        public readonly int MAX_PRESSED_KEYS = 5;
        private Keys[] pressed_keys;
        private InterfaceHandler<IKeyboardListener> keyboard_listeners;
        public Keyboard(in GameWindow window)
        {
            keyboard_listeners = new InterfaceHandler<IKeyboardListener>();
            pressed_keys = new Keys[MAX_PRESSED_KEYS];
            for(int i = 0; i < MAX_PRESSED_KEYS; i++)
            {
                pressed_keys[i] = Keys.Unknown;
            }
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

        public void GetPressedKeys(in Queue<Keys> keys)
        {
            if(keys == null) { return; }
            for(int i = 0; i < MAX_PRESSED_KEYS; i++)
            {
                Keys current = pressed_keys[i];
                if(current != Keys.Unknown)
                {
                    keys.Enqueue(current);
                }
            }
        }

        public void GetPressedKeys(in Queue<int> keys)
        {
            if (keys == null) { return; }
            for (int i = 0; i < MAX_PRESSED_KEYS; i++)
            {
                Keys current = pressed_keys[i];
                if (current != Keys.Unknown)
                {
                    keys.Enqueue((int)current);
                }
            }
        }

        public void GetPressedKeys(in List<Keys> keys)
        {
            if (keys == null) { return; }
            for (int i = 0; i < MAX_PRESSED_KEYS; i++)
            {
                Keys current = pressed_keys[i];
                if (current != Keys.Unknown)
                {
                    keys.Add(current);
                }
            }
        }

        public void GetPressedKeys(in List<int> keys)
        {
            if (keys == null) { return; }
            for (int i = 0; i < MAX_PRESSED_KEYS; i++)
            {
                Keys current = pressed_keys[i];
                if (current != Keys.Unknown)
                {
                    keys.Add((int)current);
                }
            }
        }

        private bool IsAlreadyPressed(Keys scancode)
        {
            for(int i = 0; i < MAX_PRESSED_KEYS; i++)
            {
                if(pressed_keys[i] == scancode)
                {
                    return true;
                }
            }
            return false;
        }

        private void InsertKey(Keys key)
        {
            if (!IsAlreadyPressed(key))
            {
                for(int i = 0; i < MAX_PRESSED_KEYS; i++)
                {
                    if(pressed_keys[i] == Keys.Unknown)
                    {
                        pressed_keys[i] = key;
                        return;
                    }
                }
            }
        }

        private void RemoveKey(Keys key)
        {
            if (IsAlreadyPressed(key))
            {
                for (int i = 0; i < MAX_PRESSED_KEYS; i++)
                {
                    if (pressed_keys[i] == key)
                    {
                        pressed_keys[i] = Keys.Unknown;
                        return;
                    }
                }
            }
        }

        private void Window_KeyUp(OpenTK.Windowing.Common.KeyboardKeyEventArgs obj)
        {
            RemoveKey(obj.Key);
            this.keyboard_listeners.Invoke(new Action<IKeyboardListener>((item) =>
            {
                item?.OnKeyRelease(obj.ScanCode);
            }));
            return;
        }

        private void Window_KeyDown(OpenTK.Windowing.Common.KeyboardKeyEventArgs obj)
        {
            InsertKey(obj.Key);
            this.keyboard_listeners.Invoke(new Action<IKeyboardListener>((item) =>
            {
                item?.OnKeyPress(obj.ScanCode, obj.IsRepeat, obj.Shift, obj.Alt, obj.Control);
            }));
            return;
        }
    }

    public enum KeyboardMode
    {
        NORMAL,
        TYPING
    }
}
