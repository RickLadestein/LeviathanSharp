using OpenTK.Graphics.OpenGL;

namespace Leviathan.Core.Graphics.Buffers
{

    public enum StencilOp
    {
        KEEP = OpenTK.Graphics.OpenGL.StencilOp.Keep,
	    ZERO = OpenTK.Graphics.OpenGL.StencilOp.Zero,
	    REPLACE = OpenTK.Graphics.OpenGL.StencilOp.Replace,
	    INCREMENT = OpenTK.Graphics.OpenGL.StencilOp.Incr,
	    INCREMENT_WRAP = OpenTK.Graphics.OpenGL.StencilOp.IncrWrap,
	    DECREMENT = OpenTK.Graphics.OpenGL.StencilOp.Decr,
	    DECREMENT_WRAP = OpenTK.Graphics.OpenGL.StencilOp.DecrWrap,
	    INVERT = OpenTK.Graphics.OpenGL.StencilOp.Invert
    };

    public enum StencilFunc
    {
        NEVER = StencilFunction.Never,
	    LESS = StencilFunction.Less,
	    LESS_EQUAL = StencilFunction.Lequal,
	    GREATER = StencilFunction.Greater,
	    GREATER_EQUAL = StencilFunction.Gequal,
	    EQUAL = StencilFunction.Equal,
	    NOT_EQUAL = StencilFunction.Notequal,
	    ALWAYS = StencilFunction.Always
    };
    class StencilBuffer
    {
        public static bool Enabled { get; private set; } = false;
        public static void Enable()
        {
            if(!Enabled)
            {
                GL.Enable(EnableCap.StencilTest);
                Enabled = true;
            }
        }

        public static void Disable()
        {
            if (Enabled)
            {
                GL.Disable(EnableCap.StencilTest);
                Enabled = false;
            }
        }

        public static void Reset()
        {
            SetStencilOperation(StencilOp.KEEP, StencilOp.KEEP, StencilOp.KEEP);
            SetStencilFunc(StencilFunc.ALWAYS, 1, 0xFF);
            SetStencilWritePerm(true);
        }

        public static void SetStencilOperation(StencilOp stencil_fail, StencilOp depth_fail, StencilOp depth_pass)
        {
            OpenTK.Graphics.OpenGL.StencilOp s_fail = (OpenTK.Graphics.OpenGL.StencilOp)((int)stencil_fail);
            OpenTK.Graphics.OpenGL.StencilOp d_fail = (OpenTK.Graphics.OpenGL.StencilOp)((int)depth_fail);
            OpenTK.Graphics.OpenGL.StencilOp d_pass = (OpenTK.Graphics.OpenGL.StencilOp)((int)depth_pass);
            GL.StencilOp(s_fail, d_fail, d_pass);
        }

        public static void SetStencilFunc(StencilFunc func, int reference, int mask)
        {
            StencilFunction funct = (StencilFunction)((int)func);
            GL.StencilFunc(funct, reference, mask);
        }

        public static void SetStencilWritePerm(bool write)
        {
            if (write)
                GL.StencilMask(0xFF);
            else
                GL.StencilMask(0x0);
            return;
        }
    }
}
