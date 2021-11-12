using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Core.Windowing;
using Leviathan.Core.Graphics;
using Leviathan.Math;
using Silk.NET.GLFW;

namespace Leviathan.Core.Input
{
    public delegate void KeyboardTypingFunc(uint codepoint);
    public delegate void KeyboardKeyFunc(Keys key, int scanCode, InputAction action, KeyModifiers mods);
    public delegate void KeyboardKeyPressFunc(KeyboardKey key, int scanCode);
    public delegate void KeyboardKeyReleaseFunc(KeyboardKey key, int scanCode);
    public delegate void KeyboardKeyRepeatFunc(KeyboardKey key, int scanCode);
    public class Keyboard
    {
        private Window parent;
        private unsafe WindowHandle* parent_window;

        private KeyboardKey[] keys;
        public readonly uint MAX_PRESSED_BTNS = 5;
        public KeyboardMode Mode { get; private set; }

        public event KeyboardTypingFunc KeyType;
        public event KeyboardKeyFunc Key;
        public event KeyboardKeyPressFunc Press;
        public event KeyboardKeyReleaseFunc Release;
        public event KeyboardKeyRepeatFunc Repeat;

        public unsafe Keyboard(Window _parent, ref NativeWindow wnd)
        {
            this.parent = _parent;
            this.parent_window = wnd.w_handle;
            Mode = KeyboardMode.INPUT;
            keys = new KeyboardKey[MAX_PRESSED_BTNS];
            for(int i = 0; i < MAX_PRESSED_BTNS; i++)
            {
                keys[i] = KeyboardKey.Unknown;
            }


            Context.glfw_context.SetKeyCallback(parent_window, OnKeyAction);
            Context.glfw_context.SetCharCallback(parent_window, OnKeyboardChar);
        }

        public List<KeyboardKey> GetPressedKeys()
        {
            List<KeyboardKey> output = new List<KeyboardKey>();
            for(int i = 0; i < MAX_PRESSED_BTNS; i++)
            {
                if (keys[i] != KeyboardKey.Unknown)
                {
                    output.Add(keys[i]);
                }
            }
            return output;
        }

        public void SetKeyboardMode(KeyboardMode mode)
        {
            this.Mode = mode;
        }


        #region Callbacks
        private bool ContainsButton(KeyboardKey key)
        {
            for (int i = 0; i < MAX_PRESSED_BTNS; i++)
            {
                if (keys[i] == key)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddButton(KeyboardKey key)
        {
            if (!ContainsButton(key))
            {
                for (int i = 0; i < MAX_PRESSED_BTNS; i++)
                {
                    if (keys[i] == KeyboardKey.Unknown)
                    {
                        keys[i] = key;
                        return;
                    }
                }
            }

        }

        private void RemoveButton(KeyboardKey btn)
        {
            for (int i = 0; i < MAX_PRESSED_BTNS; i++)
            {
                if (keys[i] == btn)
                {
                    keys[i] = KeyboardKey.Unknown;
                    return;
                }
            }
        }
        private unsafe void OnKeyAction(WindowHandle* window, Silk.NET.GLFW.Keys key, int scanCode, Silk.NET.GLFW.InputAction action, Silk.NET.GLFW.KeyModifiers mods)
        {
            if(Mode == KeyboardMode.INPUT)
            {
                KeyboardKey k = (KeyboardKey)key;
                switch(action)
                {
                    case Silk.NET.GLFW.InputAction.Press:
                        AddButton(k);
                        Press?.Invoke(k, scanCode);
                        break;
                    case Silk.NET.GLFW.InputAction.Release:
                        RemoveButton(k);
                        Release?.Invoke(k, scanCode);
                        break;
                    case Silk.NET.GLFW.InputAction.Repeat:
                        Repeat?.Invoke(k, scanCode);
                        break;
                }
            }
        }

        private unsafe void OnKeyboardChar(WindowHandle* window, uint codepoint)
        {
            if (Mode == KeyboardMode.TYPING)
            {
                KeyType?.Invoke(codepoint);
            }
        }
        #endregion
    }

    public enum KeyboardMode
    {
        INPUT,
        TYPING
    }
}
