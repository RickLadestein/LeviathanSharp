using OpenTK.Graphics.OpenGL;

namespace Leviathan.Core.Graphics.Buffers
{
    public enum DepthFunc
    {
        ALWAYS = DepthFunction.Always,
        NEVER = DepthFunction.Never,
        LESS = DepthFunction.Less,
        EQUAL = DepthFunction.Equal,
        LESS_EQUAL = DepthFunction.Lequal,
        GREATER = DepthFunction.Greater,
        GREATER_EQUAL = DepthFunction.Gequal,
        NOT_EQUAL = DepthFunction.Notequal
    }
    class DepthBuffer
    {
        public static bool Enabled { get; private set; } = false;
        public static void Enable()
        {
            if(!Enabled)
            {
                GL.Enable(EnableCap.DepthTest);
                Enabled = true;
            }
        }

        public static void Disable()
        {
            if (Enabled)
            {
                GL.Disable(EnableCap.DepthTest);
                Enabled = false;
            }
        }

        public static void Reset()
        {
            GL.DepthFunc(DepthFunction.Always);
        }

        public static void SetDepthFunction(DepthFunc func)
        {
            DepthFunction function = (DepthFunction)((int)func);
            GL.DepthFunc(function);
        }

    }
}
