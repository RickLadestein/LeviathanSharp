using System;
using Leviathan;
using Leviathan.Math;
using Leviathan.Core.Windowing;
using System.IO;
using Leviathan.Core.Graphics;
using Leviathan.Core.Graphics.Buffers;
using Leviathan.Core.Graphics.Buffers.VertexBufferAttributes;
using Leviathan.ECS;
using System.Collections.Generic;
using Leviathan.Core.Input;
using Leviathan.Core;
using Leviathan.Util;
using Leviathan.Util.Collections;
using Leviathan.Core.Sound;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Reflection;
using Silk.NET.Vulkan;

namespace Sandbox
{
    class Program
    {
        public static Window w;
        public static List<KeyboardKey> keys;

        static void Main(string[] args)
        {
            RenderComponent rc = new RenderComponent();
            RenderComponent rc2 = new RenderComponent();

            keys = new List<KeyboardKey>();
            w = new Window(1920, 1080, WindowMode.WINDOWED);

            Context.GLContext.Enable(Silk.NET.OpenGL.EnableCap.Blend);
            Context.GLContext.Enable(Silk.NET.OpenGL.EnableCap.DepthTest);
            Context.GLContext.BlendFunc(Silk.NET.OpenGL.BlendingFactor.SrcAlpha, Silk.NET.OpenGL.BlendingFactor.OneMinusSrcAlpha);
            
            Context.GLContext.Enable(Silk.NET.OpenGL.EnableCap.CullFace);
            Context.GLContext.CullFace(Silk.NET.OpenGL.CullFaceMode.Back);

            //Scene s = Scene.LoadSceneFromObj(".\\assets\\models\\sponza.obj");

            World.Current.LoadScene(Scene.DefaultScene);
            //World.Current.AddScene(s);
            Window.Start(w);
            return;
        }
    }
}
