using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Input.Listeners
{
    public interface IKeyboardListener
    {
        void OnKeyPress(int btn);
        void OnKeyRelease(int btn);
        void OnKeyTyped(int unicode_char);
    }
}
