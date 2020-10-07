using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Input.Listeners
{
    public interface IWindowListener
    {
        void OnWindowResize(Vector2i new_size);
        void OnWindowMove(Vector2i new_pos);
        void OnWindowRender(double frametime);
        void OnWindowFocusChanged(bool isFocussed);

    }
}
