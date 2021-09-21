using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Math;

namespace Leviathan
{
    /// <summary>
    /// The refresh function of the window
    /// </summary>
    public delegate void WindowRefreshFunc();

    /// <summary>
    /// The fixed update function of the window
    /// </summary>
    public delegate void WindowFixedUpdateFunc();

    /// <summary>
    /// The move function of the window
    /// </summary>
    /// <param name="newpos">The new position</param>
    public delegate void WindowMoveFunc(Vector2f newpos);

    /// <summary>
    /// The resize function of the window
    /// </summary>
    /// <param name="newsize">The new size</param>
    public delegate void WindowResizeFunc(Vector2f newsize);

    /// <summary>
    /// The focus function of the window
    /// </summary>
    /// <param name="focussed">A bool which shows whether the window is focused</param>
    public delegate void WindowFocusFunc(bool focussed);

    /// <summary>
    /// The close function of the window
    /// </summary>
    public delegate void WindowCloseFunc();
}
