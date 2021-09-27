using System;
using Leviathan;
using Leviathan.Math;
using Leviathan.Core.Windowing;

namespace Sandbox
{
    class Program
    {
        public static Window w;
        static void Main(string[] args)
        {
            w = new Window(1080, 720, WindowMode.WINDOWED);
            Window.Start(w);
            return;
        }
    }
}
