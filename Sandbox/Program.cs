using System;
using Leviathan;
using Leviathan.Input.Listeners;
using Leviathan.Core;
using Leviathan.Util;
using OpenTK.Mathematics;
using System.Collections.Generic;
using Leviathan.Core.Graphics;
using Leviathan.Core;
using System.Threading;

namespace Sandbox
{
    class Program : IWindowListener, IMouseListener
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            
            return;
        }

        private Queue<int> keys;
        private Window window;
        private Camera c;
        public Program()
        {
            window = new Window("Test", 1080, 720, false, true);
            window.mouse.AddListener(this);
            window.AddListener(this);
            keys = new Queue<int>();
            FileManager.GetInstance().AddDirectoryPath(@"C:\Users\rladestein\source\repos\Leviathan\Sandbox\resources\models\", "default");
            Mesh[] m = Mesh.Load("default", "cube.obj");
            c = new Camera();
            window.Run();
        }

        public void OnWindowFocusChanged(bool isFocussed)
        {
        }

        public void OnWindowMove(Vector2i new_pos)
        {
        }

        public void OnWindowRender(double frametime)
        {
            window.keyboard.GetPressedKeys(keys);
            while(keys.Count != 0)
            {
                int key = keys.Dequeue();
                switch(key)
                {
                    case 87: //W
                        c.MoveForeward((float)frametime, 0.10f);
                        break;
                    case 65: //A
                        c.MoveLeft((float)frametime, 0.10f);
                        break;
                    case 83: //S
                        c.MoveBackward((float)frametime, 0.10f);
                        break;
                    case 68: //D
                        c.MoveRight((float)frametime, 0.10f);
                        break;
                    case 32: //space
                        c.MoveUp((float)frametime, 0.10f);
                        break;
                    case 340: //lshift
                        c.MoveDown((float)frametime, 0.10f);
                        break;
                    case 69:
                        var current_mode = window.mouse.cursor_mode;
                        if(current_mode == Leviathan.Input.CursorMode.FPS)
                        {
                            window.mouse.SetCursorMode(this.window.w_handle, Leviathan.Input.CursorMode.VISIBLE);
                        } else
                        {
                            window.mouse.SetCursorMode(this.window.w_handle, Leviathan.Input.CursorMode.FPS);
                        }
                        Thread.Sleep(100);
                        break;
                }
                Logger.GetInstance().LogDebug($"Camera pos {c.position}");
            }
            return;
        }

        public void OnWindowResize(Vector2i new_size)
        {
        }

        public void OnMouseMove(Vector2 pos, Vector2 delta)
        {
            if(window.mouse.cursor_mode == Leviathan.Input.CursorMode.FPS)
            {
                c.Rotate(0, delta.Y, delta.X);
                Logger.GetInstance().LogDebug($"Camera rot [roll:{c.Roll} pitch{c.Pitch} yaw:{c.Yaw}]");
            }
        }

        public void OnMouseWheel(Vector2 pos, Vector2 scroll)
        {
        }

        public void OnMousePress(Vector2 pos, int btn)
        {
        }

        public void OnMouseRelease(Vector2 pos, int btn)
        {
        }

        public void OnMouseDrag(Vector2 pos, Vector2 delta)
        {
            return;
        }
    }
}
