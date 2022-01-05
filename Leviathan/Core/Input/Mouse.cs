using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Core.Graphics;
using Leviathan.Core.Windowing;
using Leviathan.Math;
using Silk.NET.GLFW;

namespace Leviathan.Core.Input
{
    /*
    public delegate void CursorEnterCallback(WindowHandle* window, bool entered);
    public delegate void MouseButtonCallback(WindowHandle* window, MouseButton button, InputAction action, KeyModifiers mods);
    public delegate void ScrollCallback(WindowHandle* window, double offsetX, double offsetY);  
    public delegate void CursorPosCallback(WindowHandle* window, double x, double y);
     */

    public delegate void CursorEnterFunc(bool entered);
    public delegate void MouseButtonFunc(MouseButton btn, InputAction action);
    public delegate void MouseReleaseCallback(MouseButton btn);
    public delegate void MousePressFunc(MouseButton btn);
    public delegate void MouseRepeatFunc(MouseButton btn);
    public delegate void MouseClickFunc(MouseButton btn, Vector2d pos);
    public delegate void MouseScrollFunc(Vector2d scroll);
    public delegate void MouseMoveFunc(Vector2d pos, Vector2d delta);

    public class Mouse
    {
        private Vector2d oldpos;
        private MouseButton[] btns;

        private unsafe IntPtr parent_window;
        
        public readonly uint MAX_PRESSED_BTNS = 3;
        public Vector2d Position { get; private set; }
        public MouseMode Mode { get; private set; }
        public bool InWindow { get; private set; }
        public bool RawMotion { get; private set; }

        public event CursorEnterFunc Enter;
        public event MouseButtonFunc Button;
        public event MousePressFunc Press;
        public event MouseReleaseCallback Release;
        public event MouseRepeatFunc Repeat;
        public event MouseClickFunc Click;
        public event MouseScrollFunc Scroll;
        public event MouseMoveFunc Move;

        public unsafe Mouse(ref NativeWindow wnd)
        {
            Mode = MouseMode.Normal;
            this.parent_window = new IntPtr(wnd.w_handle);
            btns = new MouseButton[MAX_PRESSED_BTNS];
            for(int i = 0; i < MAX_PRESSED_BTNS; i++)
            {
                btns[i] = MouseButton.Invalid;
            }


            wnd.graphics_context.glfw_context.GetCursorPos(wnd.w_handle, out double x_pos, out double y_pos);
            Position = new Vector2d((float)x_pos, (float)y_pos);
            oldpos = Position;

            Context.glfw_context.SetCursorEnterCallback(wnd.w_handle, OnMouseEnter);
            Context.glfw_context.SetMouseButtonCallback(wnd.w_handle, OnMouseButton);
            Context.glfw_context.SetScrollCallback(wnd.w_handle, OnMouseScroll);
            Context.glfw_context.SetCursorPosCallback(wnd.w_handle, OnCursorPosChanged);
        }

        public List<MouseButton>GetPressedButtons()
        {
            return new List<MouseButton>(btns);
        }


        public unsafe void SetCursorPos(Vector2d newpos)
        {
            WindowHandle* ptr = (WindowHandle*)this.parent_window.ToPointer();
            Context.glfw_context.SetCursorPos(ptr, newpos.X, newpos.Y);
        }

        public unsafe void SetMouseMode(MouseMode mode)
        {
            WindowHandle* ptr = (WindowHandle*)this.parent_window.ToPointer();
            switch (mode)
            {
                case MouseMode.Normal:
                    Context.glfw_context.SetInputMode(ptr, CursorStateAttribute.Cursor, CursorModeValue.CursorNormal);
                    break;
                case MouseMode.FPS:
                    Context.glfw_context.SetInputMode(ptr, CursorStateAttribute.Cursor, CursorModeValue.CursorDisabled);
                    break;
                case MouseMode.Invisible:
                    Context.glfw_context.SetInputMode(ptr, CursorStateAttribute.Cursor, CursorModeValue.CursorHidden);
                    break;
            }
            this.Mode = mode;
        }

        public unsafe void SetRawInputMode(bool enabled)
        {
            WindowHandle* ptr = (WindowHandle*)this.parent_window.ToPointer();
            if (enabled)
            {
                Context.glfw_context.SetInputMode(ptr, CursorStateAttribute.RawMouseMotion, true);
            } else
            {
                Context.glfw_context.SetInputMode(ptr, CursorStateAttribute.RawMouseMotion, false);
            }
            RawMotion = enabled;
        }


        #region Callbacks
        private bool ContainsButton(MouseButton btn)
        {
            for (int i = 0; i < MAX_PRESSED_BTNS; i++)
            {
                if (btns[i] == btn)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddButton(MouseButton btn)
        {
            if (!ContainsButton(btn))
            {
                for (int i = 0; i < MAX_PRESSED_BTNS; i++)
                {
                    if (btns[i] == MouseButton.Invalid)
                    {
                        btns[i] = btn;
                        return;
                    }
                }
            }

        }

        private void RemoveButton(MouseButton btn)
        {
            for (int i = 0; i < MAX_PRESSED_BTNS; i++)
            {
                if (btns[i] == btn)
                {
                    btns[i] = MouseButton.Invalid;
                    Click?.Invoke(btn, this.Position);
                    return;
                }
            }
        }
        private unsafe void OnMouseEnter(WindowHandle* window, bool entered) 
        {
            InWindow = entered;
            Enter?.Invoke(entered);
        }

        private unsafe void OnMouseButton(WindowHandle* window, Silk.NET.GLFW.MouseButton button, Silk.NET.GLFW.InputAction action, Silk.NET.GLFW.KeyModifiers mods) 
        {
            MouseButton btn = (MouseButton)button;
            InputAction act = (InputAction)action;
            Button?.Invoke(btn, act);

            switch(action)
            {
                case Silk.NET.GLFW.InputAction.Press:
                    AddButton(btn);
                    Press?.Invoke(btn);
                    Console.WriteLine($"Mousebutton {btn} pressed");
                    break;
                case Silk.NET.GLFW.InputAction.Release:
                    RemoveButton(btn);
                    Release?.Invoke(btn);
                    Console.WriteLine($"Mousebutton {btn} released");
                    break;
                case Silk.NET.GLFW.InputAction.Repeat:
                    Repeat?.Invoke(btn);
                    break;
            }
        }

        private unsafe void OnMouseScroll(WindowHandle* window, double offsetX, double offsetY) 
        {
            Scroll?.Invoke(new Vector2d(offsetX, offsetY));
        }

        private unsafe void OnCursorPosChanged(WindowHandle* window, double x, double y) 
        {
            Position = new Vector2d(x,y);
            Vector2d delta = Position - oldpos;


            oldpos = Position;
            Move?.Invoke(Position, delta);
        }
        #endregion
    }

    public enum MouseButton
    {
        Invalid = -1,
        Left = Silk.NET.GLFW.MouseButton.Left,
        Right = Silk.NET.GLFW.MouseButton.Right,
        Middle = Silk.NET.GLFW.MouseButton.Middle,
        Button4 = Silk.NET.GLFW.MouseButton.Button4,
        Button5 = Silk.NET.GLFW.MouseButton.Button5,
        Button6 = Silk.NET.GLFW.MouseButton.Button6,
        Button7 = Silk.NET.GLFW.MouseButton.Button7,
        Button8 = Silk.NET.GLFW.MouseButton.Button8
    }

    public enum InputAction
    {
        Release = Silk.NET.GLFW.InputAction.Release,
        Press = Silk.NET.GLFW.InputAction.Press,
        Repeat = Silk.NET.GLFW.InputAction.Repeat
    }

    public enum MouseMode
    {
        Normal,
        FPS,
        Invisible
    }

    
}
