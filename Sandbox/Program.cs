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

namespace Sandbox
{
    class Program
    {
        public static Window w;
        public static Camera cam;
        public static Entity en;
        public static Entity en2;
        public static Entity en3;
        public static World world;
        public static List<KeyboardKey> keys;

        public static DHTable dhtable;
        static void Main(string[] args)
        {
            keys = new List<KeyboardKey>();
            w = new Window(1080, 720, WindowMode.WINDOWED);
            InitResources();
            w.refresh += W_refresh;
            w.Keyboard.Press += Keyboard_Press;
            w.Mouse.Move += Mouse_Move;
            world = World.Instance;
            world.PrimaryCam.Position = Vector3f.UnitY;
            Window.Start(w);
            return;
        }

        

        private static void InitResources()
        {
            ResourceManager<Texture>.Init(); //TODO Add a more pretty way to initialise default resources

            ShaderFile sf = ShaderFile.Import("./assets/plane.glsl");
            ShaderProgram.Import(sf, "plane");

            //basic entity construction
            en = new Entity("platform");
            //en.AddComponent(new MaterialComponent());
            en.AddComponent(new MeshComponent());
            en.AddComponent(new MaterialComponent());
            en.AddComponent(new RenderComponent());
            en.Transform.Position = Vector3f.Zero;
            en.Transform.Scale = new Vector3f(100, 1, 100);
            en.GetComponent<MeshComponent>().SetMesh("Plane");
            en.GetComponent<MaterialComponent>().SetShader("plane");
            World.Instance.AddEntity(en);

            //Entity modification


            Context.gl_context.Enable(Silk.NET.OpenGL.EnableCap.Blend);
            Context.gl_context.Enable(Silk.NET.OpenGL.EnableCap.DepthTest);
            Context.gl_context.BlendFunc(Silk.NET.OpenGL.BlendingFactor.SrcAlpha, Silk.NET.OpenGL.BlendingFactor.OneMinusSrcAlpha);
            Context.gl_context.Enable(Silk.NET.OpenGL.EnableCap.CullFace);
            Context.gl_context.CullFace(Silk.NET.OpenGL.CullFaceMode.Back);
        }

        private static void W_refresh()
        {
            //en.GetComponent<RenderComponent>().Render(cam);
            keys = w.Keyboard.GetPressedKeys();
            foreach(KeyboardKey k in keys)
            {
                switch(k)
                {
                    case KeyboardKey.W:
                        world.PrimaryCam.Translate(world.PrimaryCam.Foreward * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.A:
                        world.PrimaryCam.Translate( -world.PrimaryCam.Right * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.S:
                        world.PrimaryCam.Translate(-world.PrimaryCam.Foreward * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.D:
                        world.PrimaryCam.Translate(world.PrimaryCam.Right * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.Space:
                        world.PrimaryCam.Translate(world.PrimaryCam.Up * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.ShiftLeft:
                        world.PrimaryCam.Translate(-world.PrimaryCam.Up * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.Enter:
                        world.PrimaryCam.SetMode(CameraMode.FPS);
                        break;
                    case KeyboardKey.Keypad1:
                        en2.Transform.Rotate(Vector3f.UnitY, Time.FrameDelta * 50);
                        break;
                    case KeyboardKey.Keypad3:
                        en2.Transform.Rotate(Vector3f.UnitY, -Time.FrameDelta * 50);
                        break;
                    case KeyboardKey.Keypad4:
                        en3.Transform.Rotate(Vector3f.UnitY, Time.FrameDelta * 50);
                        break;
                    case KeyboardKey.Keypad6:
                        en3.Transform.Rotate(Vector3f.UnitY, -Time.FrameDelta * 50);
                        break;
                }
            }
        }

        private static void Mouse_Move(Vector2d pos, Vector2d delta)
        {
            if(w.Mouse.Mode == MouseMode.FPS)
            {
                float yaw = (float)delta.X * Time.FrameDelta * 2.0f;
                float pitch = -(float)delta.Y * Time.FrameDelta * 2.0f;
                world.PrimaryCam.Rotate2D(pitch, yaw);            
            }
        }

        private static void Keyboard_Press(KeyboardKey key, int scanCode)
        {
            if(key == KeyboardKey.E)
            {
                if(w.Mouse.Mode == MouseMode.FPS)
                {
                    w.Mouse.SetMouseMode(MouseMode.Normal);
                } else
                {
                    w.Mouse.SetMouseMode(MouseMode.FPS);
                }
            }
        }
    }
}
