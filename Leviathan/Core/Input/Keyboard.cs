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
        private unsafe IntPtr parent_window;
        private KeyboardKey[] keys;
        public readonly uint MAX_PRESSED_BTNS = 5;
        public KeyboardMode Mode { get; private set; }

        public event KeyboardTypingFunc KeyType;
        public event KeyboardKeyFunc Key;
        public event KeyboardKeyPressFunc Press;
        public event KeyboardKeyReleaseFunc Release;
        public event KeyboardKeyRepeatFunc Repeat;

        public unsafe Keyboard(ref NativeWindow wnd)
        {
            parent_window = new IntPtr(wnd.w_handle);

            Mode = KeyboardMode.INPUT;
            keys = new KeyboardKey[MAX_PRESSED_BTNS];
            for(int i = 0; i < MAX_PRESSED_BTNS; i++)
            {
                keys[i] = KeyboardKey.Unknown;
            }


            Context.glfw_context.SetKeyCallback(wnd.w_handle, OnKeyAction);
            Context.glfw_context.SetCharCallback(wnd.w_handle, OnKeyboardChar);
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

    public enum KeyModifiers
    {
        Shift = Silk.NET.GLFW.KeyModifiers.Shift,
        Control = Silk.NET.GLFW.KeyModifiers.Control,
        Alt = Silk.NET.GLFW.KeyModifiers.Alt,
        Super = Silk.NET.GLFW.KeyModifiers.Super
    }

    public enum KeyboardKey
    {
        Unknown = Silk.NET.GLFW.Keys.Unknown,
        Space = Silk.NET.GLFW.Keys.Space,
        Apostrophe = Silk.NET.GLFW.Keys.Apostrophe,
        Comma = Silk.NET.GLFW.Keys.Comma,
        Minus = Silk.NET.GLFW.Keys.Minus,
        Period = Silk.NET.GLFW.Keys.Period,
        Slash = Silk.NET.GLFW.Keys.Slash,
        Number0 = Silk.NET.GLFW.Keys.Number0,
        D0 = Silk.NET.GLFW.Keys.D0,
        Number1 = Silk.NET.GLFW.Keys.Number1,
        Number2 = Silk.NET.GLFW.Keys.Number2,
        Number3 = Silk.NET.GLFW.Keys.Number3,
        Number4 = Silk.NET.GLFW.Keys.Number4,
        Number5 = Silk.NET.GLFW.Keys.Number5,
        Number6 = Silk.NET.GLFW.Keys.Number6,
        Number7 = Silk.NET.GLFW.Keys.Number7,
        Number8 = Silk.NET.GLFW.Keys.Number8,
        Number9 = Silk.NET.GLFW.Keys.Number9,
        Semicolon = Silk.NET.GLFW.Keys.Semicolon,
        Equal = Silk.NET.GLFW.Keys.Equal,
        A = Silk.NET.GLFW.Keys.A,
        B = Silk.NET.GLFW.Keys.B,
        C = Silk.NET.GLFW.Keys.C,
        D = Silk.NET.GLFW.Keys.D,
        E = Silk.NET.GLFW.Keys.E,
        F = Silk.NET.GLFW.Keys.F,
        G = Silk.NET.GLFW.Keys.G,
        H = Silk.NET.GLFW.Keys.H,
        I = Silk.NET.GLFW.Keys.I,
        J = Silk.NET.GLFW.Keys.J,
        K = Silk.NET.GLFW.Keys.K,
        L = Silk.NET.GLFW.Keys.L,
        M = Silk.NET.GLFW.Keys.M,
        N = Silk.NET.GLFW.Keys.N,
        O = Silk.NET.GLFW.Keys.O,
        P = Silk.NET.GLFW.Keys.P,
        Q = Silk.NET.GLFW.Keys.Q,
        R = Silk.NET.GLFW.Keys.R,
        S = Silk.NET.GLFW.Keys.S,
        T = Silk.NET.GLFW.Keys.T,
        U = Silk.NET.GLFW.Keys.U,
        V = Silk.NET.GLFW.Keys.V,
        W = Silk.NET.GLFW.Keys.W,
        X = Silk.NET.GLFW.Keys.X,
        Y = Silk.NET.GLFW.Keys.Y,
        Z = Silk.NET.GLFW.Keys.Z,
        LeftBracket = Silk.NET.GLFW.Keys.LeftBracket,
        BackSlash = Silk.NET.GLFW.Keys.BackSlash,
        RightBracket = Silk.NET.GLFW.Keys.RightBracket,
        GraveAccent = Silk.NET.GLFW.Keys.GraveAccent,
        World1 = Silk.NET.GLFW.Keys.World1,
        World2 = Silk.NET.GLFW.Keys.World2,
        Escape = Silk.NET.GLFW.Keys.Escape,
        Enter = Silk.NET.GLFW.Keys.Enter,
        Tab = Silk.NET.GLFW.Keys.Tab,
        Backspace = Silk.NET.GLFW.Keys.Backspace,
        Insert = Silk.NET.GLFW.Keys.Insert,
        Delete = Silk.NET.GLFW.Keys.Delete,
        Right = Silk.NET.GLFW.Keys.Right,
        Left = Silk.NET.GLFW.Keys.Left,
        Down = Silk.NET.GLFW.Keys.Down,
        Up = Silk.NET.GLFW.Keys.Up,
        PageUp = Silk.NET.GLFW.Keys.PageUp,
        PageDown = Silk.NET.GLFW.Keys.PageDown,
        Home = Silk.NET.GLFW.Keys.Home,
        End = Silk.NET.GLFW.Keys.End,
        CapsLock = Silk.NET.GLFW.Keys.CapsLock,
        ScrollLock = Silk.NET.GLFW.Keys.ScrollLock,
        NumLock = Silk.NET.GLFW.Keys.NumLock,
        PrintScreen = Silk.NET.GLFW.Keys.PrintScreen,
        Pause = Silk.NET.GLFW.Keys.Pause,
        F1 = Silk.NET.GLFW.Keys.F1,
        F2 = Silk.NET.GLFW.Keys.F2,
        F3 = Silk.NET.GLFW.Keys.F3,
        F4 = Silk.NET.GLFW.Keys.F4,
        F5 = Silk.NET.GLFW.Keys.F5,
        F6 = Silk.NET.GLFW.Keys.F6,
        F7 = Silk.NET.GLFW.Keys.F7,
        F8 = Silk.NET.GLFW.Keys.F8,
        F9 = Silk.NET.GLFW.Keys.F9,
        F10 = Silk.NET.GLFW.Keys.F10,
        F11 = Silk.NET.GLFW.Keys.F11,
        F12 = Silk.NET.GLFW.Keys.F12,
        F13 = Silk.NET.GLFW.Keys.F13,
        F14 = Silk.NET.GLFW.Keys.F14,
        F15 = Silk.NET.GLFW.Keys.F15,
        F16 = Silk.NET.GLFW.Keys.F16,
        F17 = Silk.NET.GLFW.Keys.F17,
        F18 = Silk.NET.GLFW.Keys.F18,
        F19 = Silk.NET.GLFW.Keys.F19,
        F20 = Silk.NET.GLFW.Keys.F20,
        F21 = Silk.NET.GLFW.Keys.F21,
        F22 = Silk.NET.GLFW.Keys.F22,
        F23 = Silk.NET.GLFW.Keys.F23,
        F24 = Silk.NET.GLFW.Keys.F24,
        F25 = Silk.NET.GLFW.Keys.F25,
        Keypad0 = Silk.NET.GLFW.Keys.Keypad0,
        Keypad1 = Silk.NET.GLFW.Keys.Keypad1,
        Keypad2 = Silk.NET.GLFW.Keys.Keypad2,
        Keypad3 = Silk.NET.GLFW.Keys.Keypad3,
        Keypad4 = Silk.NET.GLFW.Keys.Keypad4,
        Keypad5 = Silk.NET.GLFW.Keys.Keypad5,
        Keypad6 = Silk.NET.GLFW.Keys.Keypad6,
        Keypad7 = Silk.NET.GLFW.Keys.Keypad7,
        Keypad8 = Silk.NET.GLFW.Keys.Keypad8,
        Keypad9 = Silk.NET.GLFW.Keys.Keypad9,
        KeypadDecimal = Silk.NET.GLFW.Keys.KeypadDecimal,
        KeypadDivide = Silk.NET.GLFW.Keys.KeypadDivide,
        KeypadMultiply = Silk.NET.GLFW.Keys.KeypadMultiply,
        KeypadSubtract = Silk.NET.GLFW.Keys.KeypadSubtract,
        KeypadAdd = Silk.NET.GLFW.Keys.KeypadAdd,
        KeypadEnter = Silk.NET.GLFW.Keys.KeypadEnter,
        KeypadEqual = Silk.NET.GLFW.Keys.KeypadEqual,
        ShiftLeft = Silk.NET.GLFW.Keys.ShiftLeft,
        ControlLeft = Silk.NET.GLFW.Keys.ControlLeft,
        AltLeft = Silk.NET.GLFW.Keys.AltLeft,
        SuperLeft = Silk.NET.GLFW.Keys.SuperLeft,
        ShiftRight = Silk.NET.GLFW.Keys.ShiftRight,
        ControlRight = Silk.NET.GLFW.Keys.ControlRight,
        AltRight = Silk.NET.GLFW.Keys.AltRight,
        SuperRight = Silk.NET.GLFW.Keys.SuperRight,
        Menu = Silk.NET.GLFW.Keys.Menu,
        LastKey = Silk.NET.GLFW.Keys.LastKey
    }
}
