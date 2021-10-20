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
        public static List<KeyboardKey> keys;
        static void Main(string[] args)
        {
            keys = new List<KeyboardKey>();
            w = new Window(1080, 720, WindowMode.WINDOWED);
            InitResources();
            w.refresh += W_refresh;
            w.Keyboard.Press += Keyboard_Press;
            w.Mouse.Move += Mouse_Move;
            Window.Start(w);
            return;
        }

        

        private static void InitResources()
        {
            ResourceManager<Texture>.Init(); //TODO Add a more pretty way to initialise default resources
            cam = new Camera();

            
            //basic entity construction
            en = new Entity("cubetest");
            en.AddComponent(new MaterialComponent());
            en.AddComponent(new MeshComponent());
            en.AddComponent(new RenderComponent());
            en.Transform.Position = Vector3f.UnitZ * 4;


            //Entity modification
            en.GetComponent<MaterialComponent>().SetShader("default_instance");
            Random rnd = new Random();
            FloatAttribute fa = new FloatAttribute(DataCollectionType.VEC3);
            FloatAttribute fa1 = new FloatAttribute(DataCollectionType.VEC3);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    fa.AddData(new Vector3f((i + (i * 10)), 0.0f, (j + (j * 10))));
                    fa1.AddData(new Vector3f((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
                }
            }
            InstanceBuffer ibuff = InstanceBuffer.FromAttribute(fa, 1);
            InstanceBuffer ibuff1 = InstanceBuffer.FromAttribute(fa1, 1);
            en.GetComponent<MeshComponent>().Vbuffer.LoadInstanceBuffers(new InstanceBuffer[2] { ibuff, ibuff1 });
        }

        private static void W_refresh()
        {
            en.GetComponent<RenderComponent>().RenderInstanced(cam, 100);
            keys = w.Keyboard.GetPressedKeys();
            foreach(KeyboardKey k in keys)
            {
                switch(k)
                {
                    case KeyboardKey.W:
                        cam.Translate(cam.Foreward * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.A:
                        cam.Translate( -cam.Right * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.S:
                        cam.Translate(-cam.Foreward * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.D:
                        cam.Translate(cam.Right * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.Space:
                        cam.Translate(cam.Up * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.ShiftLeft:
                        cam.Translate(-cam.Up * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.Enter:
                        cam.SetMode(CameraMode.FPS);
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
                cam.Rotate2D(pitch, yaw);            
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
