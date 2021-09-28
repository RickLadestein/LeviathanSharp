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
            Leviathan.Core.Graphics.ShaderFile file = new Leviathan.Core.Graphics.ShaderFile("C:\\Users\\dazle\\source\\default.shad");
            Leviathan.Core.Graphics.ShaderFile.WriteShader(file);
            Window.Start(w);
            return;
        }
    }
}
