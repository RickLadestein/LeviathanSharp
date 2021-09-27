using Silk.NET.OpenGL;
using Silk.NET.GLFW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics
{
    public class Context
    {
        public static GL gl_context { get; private set; }
        public static Glfw glfw_context { get; private set; }

        public static void RegisterContext(GL glc, Glfw glfwc)
        {
            gl_context = glc;
            glfw_context = glfwc;
        }

        public static void InvalidateContext()
        {
            gl_context = null;
            glfw_context = null;
        }
    }
}
