using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Input.Listeners
{
    public interface IKeyboardListener
    {
        void OnKeyPress(int scancode, bool repeat, bool shift, bool alt, bool control);
        void OnKeyRelease(int scancode);
    }
}
