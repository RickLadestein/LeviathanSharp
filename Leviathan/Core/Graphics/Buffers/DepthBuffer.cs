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
            Context.gl_context.DepthFunc(dfunct);
        }

        public static void Clear()
        {
            if(IsEnabled)
            {
                Context.gl_context.Clear(Silk.NET.OpenGL.ClearBufferMask.DepthBufferBit);
            }
        }

        public static void Enable()
        {
            Context.gl_context.DepthMask(true);
            IsEnabled = true;
        }

        public static void Disable()
        {
            Context.gl_context.DepthMask(false);
            IsEnabled = false;
        }
    }
}
