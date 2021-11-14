using Silk.NET.OpenGL;
using Silk.NET.GLFW;
using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Core.Windowing;
using OpenTK.Audio.OpenAL;

namespace Leviathan.Core
{
    public class Context
    {
        public static GL gl_context { get; private set; }
        public static Glfw glfw_context { get; private set; }
        public static Window parent_window { get; private set; }

        public static void RegisterContext(GL glc, Glfw glfwc, Window pwindow)
        {
            gl_context = glc;
            glfw_context = glfwc;
            parent_window = pwindow;
        }

        public static void InvalidateContext()
        {
            gl_context = null;
            glfw_context = null;
        }
    }
}
