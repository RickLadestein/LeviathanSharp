using Leviathan.Core;
using Leviathan.Core.Graphics;
using Leviathan.Core.Input;
using Leviathan.ECS;
using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sandbox
{
    public class PlayerScript : MonoScript
    {
        public Entity camera;

        private List<KeyboardKey> keys;
        private Mouse mouse;
        private Keyboard keyboard;

        private float xRotation;
        public PlayerScript()
        {
            keys = new List<KeyboardKey>();
            mouse = Context.parent_window.Mouse;
            keyboard = Context.parent_window.Keyboard;
            mouse.Move += Mouse_Move;
            keyboard.Press += Keyboard_Press;
            xRotation = 0.0f;
        }

        private void Keyboard_Press(KeyboardKey key, int scanCode)
        {
            if (key == KeyboardKey.E)
            {
                if (mouse.Mode == MouseMode.FPS)
                {
                    mouse.SetMouseMode(MouseMode.Normal);
                }
                else
                {
                    mouse.SetMouseMode(MouseMode.FPS);
                }
            }
        }

        private void Mouse_Move(Leviathan.Math.Vector2d pos, Leviathan.Math.Vector2d delta)
        {
            if (mouse.Mode == MouseMode.FPS)
            {
                float yaw = (float)delta.X * Time.FrameDelta * 2.0f;
                float pitch = -(float)delta.Y * Time.FrameDelta * 2.0f;
                //world.Rotate2D(pitch, yaw);
                
                xRotation -= pitch;
                xRotation = Math.Clamp(xRotation, -90, 90);
                
                camera.Transform.Orientation = Quaternion.FromEulerAngles(new Vector3f(MathL.DegreesToRadians(xRotation), 0.0f, 0.0f));
                entity.Transform.Rotate(Vector3f.UnitY, -yaw);
                //Console.WriteLine(yaw);
            }
        }

        public override void Update()
        {
            base.Update();
            keys = Context.parent_window.Keyboard.GetPressedKeys();
            if(keys.Count != 0)
            {
                Console.WriteLine(camera.Transform.Forward);
            }
            foreach (KeyboardKey k in keys)
            {
                switch (k)
                {
                    case KeyboardKey.W:
                        entity.Transform.LocalPosition += (entity.Transform.Forward * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.A:
                        entity.Transform.LocalPosition += (-entity.Transform.Right * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.S:
                        entity.Transform.LocalPosition += (-entity.Transform.Forward * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.D:
                        entity.Transform.LocalPosition += (entity.Transform.Right * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.Space:
                        entity.Transform.LocalPosition += (entity.Transform.Up * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.ShiftLeft:
                        entity.Transform.LocalPosition += (-entity.Transform.Up * Time.FrameDelta * 5.0f);
                        break;
                    case KeyboardKey.Keypad4:
                        entity.Transform.Rotate(Vector3f.UnitY, 20.0f * Time.FrameDelta);
                        break;
                    case KeyboardKey.Keypad6:
                        entity.Transform.Rotate(-Vector3f.UnitY, 20.0f * Time.FrameDelta);
                        break;
                    case KeyboardKey.Keypad8:
                        camera.Transform.LocalPosition += (camera.Transform.Forward * 20.0f * Time.FrameDelta);
                        break;
                    case KeyboardKey.Keypad2:
                        camera.Transform.LocalPosition += (-camera.Transform.Forward * 20.0f * Time.FrameDelta);
                        break;
                }
            }
        }
    }
}
