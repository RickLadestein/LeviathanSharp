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
            if(!Directory.Exists("./assets"))
            {
                Directory.CreateDirectory("./assets");
            }
            cam = new Camera();

            ShaderFile sf = new ShaderFile("./assets/default.glsl");
            ShaderProgram.Import(sf, "default");
            Mesh.Import("cube", "./assets/cube.obj", ElementType.TRIANGLES);

            MaterialComponent matcomp = new MaterialComponent();
            matcomp.SetShader("default");

            MeshComponent meshcomp = new MeshComponent();
            meshcomp.SetMesh("cube");

            RenderComponent rcomp = new RenderComponent();

            en = new Entity("cubetest");
            en.AddComponent(matcomp);
            en.AddComponent(meshcomp);
            en.AddComponent(rcomp);
            en.Transform.Position = Vector3f.UnitZ * 4;
        }

        private static void W_refresh()
        {
            en.GetComponent<RenderComponent>().Render(cam);
            keys = w.Keyboard.GetPressedKeys();
            foreach(KeyboardKey k in keys)
            {
                switch(k)
                {
                    case KeyboardKey.W:
                        cam.Position += cam.Foreward * Time.FrameDelta;
                        break;
                    case KeyboardKey.A:
                        cam.Position -= cam.Right * Time.FrameDelta;
                        break;
                    case KeyboardKey.S:
                        cam.Position -= cam.Foreward * Time.FrameDelta;
                        break;
                    case KeyboardKey.D:
                        cam.Position += cam.Right * Time.FrameDelta;
                        break;
                    case KeyboardKey.Space:
                        cam.Position += cam.Up * Time.FrameDelta;
                        break;
                    case KeyboardKey.ShiftLeft:
                        cam.Position -= cam.Up * Time.FrameDelta;
                        break;
                    
                }
            }
        }

        private static void Mouse_Move(Vector2d pos, Vector2d delta)
        {
            if(w.Mouse.Mode == MouseMode.FPS)
            {
                float yaw = -(float)delta.X * Time.FrameDelta;
                float pitch = (float)delta.Y * Time.FrameDelta;
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
