using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Mathematics;

namespace Leviathan.Input.Listeners
{
    public interface IMouseListener
    {

        void OnMouseMove(Vector2 pos, Vector2 delta);
        void OnMouseWheel(Vector2 pos, Vector2 scroll);
        void OnMousePress(Vector2 pos, int btn);
        void OnMouseRelease(Vector2 pos, int btn);
    }
}
