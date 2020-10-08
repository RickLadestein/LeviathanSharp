using System;
using Leviathan;
using Leviathan.Input.Listeners;
using Leviathan.Core;
using Leviathan.Util;
using OpenTK.Mathematics;
using System.Collections.Generic;

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
        public Program()
        {
            window = new Window("Test", 1080, 720, false, true);
            window.mouse.AddListener(this);
            window.AddListener(this);
            //window.SetCursorMode(Leviathan.Input.CursorMode.FPS);
            keys = new Queue<int>();
            window.Run();
        }

        public void OnWindowFocusChanged(bool isFocussed)
        {
            Console.WriteLine($"Window focus changed to: {isFocussed}");
        }

        public void OnWindowMove(Vector2i new_pos)
        {
            Console.WriteLine($"Window moved to: {new_pos.X}, {new_pos.Y}");
        }

        public void OnWindowRender(double frametime)
        {
            window.keyboard.GetPressedKeys(keys);
            while(keys.Count != 0)
            {
                int key = keys.Dequeue();
                Console.WriteLine($"Pressed key : {key}");
            }
            return;
        }

        public void OnWindowResize(Vector2i new_size)
        {
            Console.WriteLine($"Window resized to: {new_size.X}, {new_size.Y}");
        }

        public void OnMouseMove(Vector2 pos, Vector2 delta)
        {
            //Console.WriteLine($"Mouse move: pos[{pos.X},{pos.Y}] delta[{delta.X},{delta.Y}]");
        }

        public void OnMouseWheel(Vector2 pos, Vector2 scroll)
        {
            Console.WriteLine($"Mouse scroll: pos[{pos.X},{pos.Y}] scroll[{scroll.X},{scroll.Y}]");
        }

        public void OnMousePress(Vector2 pos, int btn)
        {
            Console.WriteLine($"Mouse press: pos[{pos.X},{pos.Y}] btn[{btn}]");
        }

        public void OnMouseRelease(Vector2 pos, int btn)
        {
            Console.WriteLine($"Mouse release: pos[{pos.X},{pos.Y}] btn[{btn}]");
        }

        public void OnMouseDrag(Vector2 pos, Vector2 delta)
        {
            return;
        }
    }
}
