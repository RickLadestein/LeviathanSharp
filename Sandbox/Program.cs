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
using Leviathan.Core.Sound;
using System.IO.Ports;

namespace Sandbox
{
    class Program
    {
        public static Window w;
        public static Entity en;
        public static Entity en2;
        public static World world;
        public static List<KeyboardKey> keys;

        static void Main(string[] args)
        {

            keys = new List<KeyboardKey>();
            //w = new Window(1080, 720, WindowMode.FULLSCREEN);
            w = new Window(2560, 1440, WindowMode.WINDOWED_FULLSCREEN);
            InitResources();
            world = World.Instance;
            Window.Start(w);
            return;
        }

        private static void InitResources()
        {
            ShaderFile sf = ShaderFile.Import("./assets/plane.glsl");
            ShaderProgram.Import(sf, "plane");

            //basic entity construction
            en = new Entity("platform");
            //en.AddComponent(new MaterialComponent());
            en.AddComponent(new MeshComponent());
            en.AddComponent(new MaterialComponent());
            en.AddComponent(new RenderComponent());
            en.Transform.LocalPosition = Vector3f.Zero;
            en.Transform.LocalScale = new Vector3f(10, 1, 10);
            en.GetComponent<MeshComponent>().SetMesh("Plane");
            en.GetComponent<MaterialComponent>().SetShader("plane");
            en.AddComponent(new SoundSourceComponent());
            World.Instance.AddEntity(en);

            en2 = new Entity("platform2");
            //en.AddComponent(new MaterialComponent());
            en2.AddComponent(new MeshComponent());
            en2.AddComponent(new MaterialComponent());
            en2.AddComponent(new RenderComponent());
            en2.Transform.LocalPosition = Vector3f.Zero;
            en2.Transform.LocalScale = new Vector3f(0.5f, 0.5f, 0.5f);
            en2.GetComponent<MeshComponent>().SetMesh("Cube");
            en2.GetComponent<MaterialComponent>().SetShader("default");
            World.Instance.AddEntity(en2);

            Entity en3 = new Entity("platform3");
            //en.AddComponent(new MaterialComponent());
            en3.AddComponent(new MeshComponent());
            en3.AddComponent(new MaterialComponent());
            en3.AddComponent(new RenderComponent());
            en3.Transform.LocalPosition = Vector3f.UnitX * 2;
            en3.Transform.LocalScale = new Vector3f(0.5f, 0.5f, 0.5f);
            en3.GetComponent<MeshComponent>().SetMesh("Cube");
            en3.GetComponent<MaterialComponent>().SetShader("default");
            en2.AddChild(en3);
            World.Instance.AddEntity(en3);

            Entity en4 = new Entity("platform3");
            //en.AddComponent(new MaterialComponent());
            en4.AddComponent(new MeshComponent());
            en4.AddComponent(new MaterialComponent());
            en4.AddComponent(new RenderComponent());
            en4.Transform.LocalPosition = Vector3f.UnitX * 2;
            en4.Transform.LocalScale = new Vector3f(0.5f, 0.5f, 0.5f);
            en4.GetComponent<MeshComponent>().SetMesh("Cube");
            en4.GetComponent<MaterialComponent>().SetShader("default");
            en3.AddChild(en4);
            World.Instance.AddEntity(en4);

            Entity camera = new Entity("camera");
            camera.AddComponent(new CameraComponent());
            camera.GetComponent<CameraComponent>().Primary = true;
            camera.Transform.LocalPosition = Vector3f.Zero;

            Entity player = new Entity("player");
            player.AddChild(camera);
            player.Transform.LocalPosition = new Vector3f(0.0f, 1.0f, 2.0f);
            PlayerScript ps = new PlayerScript
            {
                camera = camera,
                rotate_boy = en2,
                end_boy = en4,
                lcomp = en.GetComponent<SoundSourceComponent>()
            };
            player.AddScript(ps);
            player.AddComponent(new SoundListenerComponent());
            World.Instance.AddEntity(player);

            //Entity modification


            Context.GLContext.Enable(Silk.NET.OpenGL.EnableCap.Blend);
            Context.GLContext.Enable(Silk.NET.OpenGL.EnableCap.DepthTest);
            Context.GLContext.BlendFunc(Silk.NET.OpenGL.BlendingFactor.SrcAlpha, Silk.NET.OpenGL.BlendingFactor.OneMinusSrcAlpha);
            //Context.gl_context.Enable(Silk.NET.OpenGL.EnableCap.CullFace);
            //Context.gl_context.CullFace(Silk.NET.OpenGL.CullFaceMode.Back);
        }
    }
}
