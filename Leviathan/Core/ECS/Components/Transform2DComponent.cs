using OpenTK.Mathematics;
using SixLabors.ImageSharp.Formats.Tga;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.ECS.Components
{
    public class Transform2DComponent : Component
    {

        /// <summary>
        /// Position in cartesian float coordinates
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Rotation in floating point radians
        /// </summary>
        public Vector2 Rotation { get; set; }

        /// <summary>
        /// Rotation in floating point degrees
        /// </summary>
        public Vector2 Rotation_d
        {
            get
            {
                float x_comp = MathHelper.RadiansToDegrees(Rotation.X);
                float y_comp = MathHelper.RadiansToDegrees(Rotation.Y);
                return new Vector2(x_comp, y_comp);
            }

            set
            {
                float x_comp = MathHelper.DegreesToRadians(value.X);
                float y_comp = MathHelper.DegreesToRadians(value.Y);
                this.Rotation = new Vector2(x_comp, y_comp);
            }
        }

        /// <summary>
        /// Scale in floating point units
        /// </summary>
        public Vector2 Scale { get; set; }

        public Matrix4 GetModelMatrix
        {
            get
            {
                Quaternion quat = new Quaternion(new Vector3(this.Rotation.X, this.Rotation.Y, 0));
                Matrix4 rot = Matrix4.CreateFromQuaternion(quat);
                Matrix4 scl = Matrix4.CreateScale(this.Scale.X, this.Scale.Y, 0);
                Matrix4 trans = Matrix4.CreateTranslation(this.Position.X, this.Position.Y, 0);
                Matrix4 output = (rot * trans) * scl;
                return output;
            }
            private set { }
        }
    }
}
