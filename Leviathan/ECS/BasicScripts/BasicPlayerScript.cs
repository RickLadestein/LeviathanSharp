using Leviathan.Core;
using Leviathan.Core.Graphics;
using Leviathan.Core.Input;
using Leviathan.Core.Sound;
using Leviathan.ECS;
using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.ECS.BasicScripts
{
    public class BasicPlayerScript : MonoScript
    {
        public Entity camera;
        public float walk_factor = 100.0f;

        private List<KeyboardKey> keys;
        private Mouse mouse;
        private Keyboard keyboard;

        private float xRotation;
        public BasicPlayerScript()
        {
            keys = new List<KeyboardKey>();
            mouse = Context.ParentWindow.Mouse;
            keyboard = Context.ParentWindow.Keyboard;
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

                xRotation += pitch;
                xRotation = System.Math.Clamp(xRotation, -90, 90);

                //camera.Transform.Orientation = Quaternion.FromEulerAngles(new Vector3f(MathL.DegreesToRadians(xRotation), 0.0f, 0.0f));
                camera.Transform.Orientation = Quaternion.Identity;
                camera.Transform.Rotate(camera.Transform.Right, xRotation);
                entity.Transform.Rotate(Vector3f.Up, -yaw);
            }
        }

        public override void Update()
        {
            base.Update();
            keys = Context.ParentWindow.Keyboard.GetPressedKeys();
            foreach (KeyboardKey k in keys)
            {
                switch (k)
                {
                    case KeyboardKey.W:
                        entity.Transform.LocalPosition += (entity.Transform.Forward * Time.FrameDelta * walk_factor);
                        break;
                    case KeyboardKey.A:
                        entity.Transform.LocalPosition += (-entity.Transform.Right * Time.FrameDelta * walk_factor);
                        break;
                    case KeyboardKey.S:
                        entity.Transform.LocalPosition += (-entity.Transform.Forward * Time.FrameDelta * walk_factor);
                        break;
                    case KeyboardKey.D:
                        entity.Transform.LocalPosition += (entity.Transform.Right * Time.FrameDelta * walk_factor);
                        break;
                    case KeyboardKey.Space:
                        entity.Transform.LocalPosition += (entity.Transform.Up * Time.FrameDelta * walk_factor);
                        break;
                    case KeyboardKey.ShiftLeft:
                        entity.Transform.LocalPosition += (-entity.Transform.Up * Time.FrameDelta * walk_factor);
                        break;
                    case KeyboardKey.Keypad8:
                        camera.Transform.LocalPosition += (Vector3f.Forward * 20.0f * Time.FrameDelta);
                        break;
                    case KeyboardKey.Keypad2:
                        camera.Transform.LocalPosition += (-Vector3f.Forward * 20.0f * Time.FrameDelta);
                        break;
                    case KeyboardKey.Keypad4:
                        entity.Transform.Rotate(Vector3f.UnitY, 50.0f * Time.FrameDelta);
                        break;
                    case KeyboardKey.Keypad6:
                        entity.Transform.Rotate(Vector3f.UnitY, -50.0f * Time.FrameDelta);
                        break;

                }
            }
        }
    }
}
