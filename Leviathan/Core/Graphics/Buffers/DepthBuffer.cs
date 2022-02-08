using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers
{
    public class DepthBuffer
    {
        public static bool IsEnabled { get; private set; }
        public static void SetBlendFunc(Silk.NET.OpenGL.DepthFunction dfunct )
        {
            Context.GLContext.DepthFunc(dfunct);
        }

        public static void Clear()
        {
            if(IsEnabled)
            {
                Context.GLContext.Clear(Silk.NET.OpenGL.ClearBufferMask.DepthBufferBit);
            }
        }

        public static void Enable()
        {
            Context.GLContext.DepthMask(true);
            IsEnabled = true;
        }

        public static void Disable()
        {
            Context.GLContext.DepthMask(false);
            IsEnabled = false;
        }
    }
}
