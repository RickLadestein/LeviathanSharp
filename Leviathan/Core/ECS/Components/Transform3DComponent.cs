using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace Leviathan.Core.ECS.Components
{
    public class Transform3DComponent : Component
    {
        /// <summary>
        /// Position in cartesian float coordinates
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Rotation in floating point radians
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Rotation in floating point degrees
        /// </summary>
        public Vector3 Rotation_d
        {
            get
            {
                float x_comp = MathHelper.RadiansToDegrees(Rotation.X);
                float y_comp = MathHelper.RadiansToDegrees(Rotation.Y);
                float z_comp = MathHelper.RadiansToDegrees(Rotation.Z);
                return new Vector3(x_comp, y_comp, z_comp);
            }

            set
            {
                float x_comp = MathHelper.DegreesToRadians(value.X);
                float y_comp = MathHelper.DegreesToRadians(value.Y);
                float z_comp = MathHelper.DegreesToRadians(value.Z);
                this.Rotation = new Vector3(x_comp, y_comp, z_comp);
            }
        }

        /// <summary>
        /// Scale in floating point units
        /// </summary>
        public Vector3 Scale { get; set; }

        public Matrix4 GetModelMatrix
        {
            get 
            {
                Quaternion quat = new Quaternion(Rotation);
                Matrix4 rot = Matrix4.CreateFromQuaternion(quat);
                Matrix4 scl = Matrix4.CreateScale(Scale);
                Matrix4 trans = Matrix4.CreateTranslation(Position);
                Matrix4 output = (rot * trans) * scl;
                return output;
            }
            private set { }
        }
    }
}
