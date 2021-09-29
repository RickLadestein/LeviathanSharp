using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.ECS
{
    public struct Transform
    {
        /// <summary>
        /// The current Position
        /// </summary>
        public Vector3f Position;

        /// <summary>
        /// The current Rotation in degrees
        /// </summary>
        public Vector3f Rotation
        {
            get
            {
                Orientation.ToEulerAngles(out Vector3f _rot);
                return new Vector3f((float)_rot.X, (float)_rot.Y, (float)_rot.Z);
            }
        }


        /// <summary>
        /// The current orientation
        /// </summary>
        public Quaternion Orientation;

        /// <summary>
        /// The current Scale
        /// </summary>
        public Vector3f Scale;

        /// <summary>
        /// The matrix that describes the orientation, scale and position
        /// </summary>
        public Mat4 ModelMat
        {
            get
            {
                CalcModelMatrix();
                return _model;
            }
        }
        private Mat4 _model;

        /// <summary>
        /// The matrix that describes the translation of the normal vector
        /// </summary>
        public Mat3 NormalMat
        {
            get
            {
                CalcModelMatrix();
                Mat4 mod = _model;
                mod.Transpose();
                return new Mat3(mod.Inverted());
            }
        }

        /// <summary>
        /// Resets the transform rotation, position and scale to default values
        /// </summary>
        public void Reset()
        {
            this.Position = Vector3f.Zero;
            this.Orientation = Quaternion.Identity;
            this.Scale = new Vector3f(1.0f, 1.0f, 1.0f);
        }

        /// <summary>
        /// Set the current orientation
        /// </summary>
        /// <param name="roll">A float containing the roll of the orientation in degrees</param>
        /// <param name="pitch">A float containing the pitch of the orientation in degrees</param>
        /// <param name="yaw">A float containing the yaw of the orientation in degrees</param>
        public void SetOrientationDeg(float roll, float pitch, float yaw)
        {
            float rad_x = Math.Math.DegreesToRadians(pitch);
            float rad_y = Math.Math.DegreesToRadians(yaw);
            float rad_z = Math.Math.DegreesToRadians(roll);
            SetOrientationRad(rad_z, rad_x, rad_y);
        }

        public void SetOrientationRad(float roll, float pitch, float yaw)
        {
            Quaternion Quaternion_x = Quaternion.FromAxisAngle(new Vector3f(1.0f, 0.0f, 0.0f), pitch);
            Quaternion Quaternion_y = Quaternion.FromAxisAngle(new Vector3f(0.0f, 1.0f, 0.0f), yaw);
            Quaternion Quaternion_z = Quaternion.FromAxisAngle(new Vector3f(0.0f, 0.0f, 1.0f), roll);
            Quaternion Quaternion_fin = Quaternion_y * Quaternion_z * Quaternion_x;
            this.Orientation = Quaternion_fin;
        }

        /// <summary>
        /// Rotate the object
        /// </summary>
        /// <param name="rotation_Quaternion">The Quaternion used for the rotation</param>
        public void Rotate(Quaternion rotation_Quaternion)
        {
            this.Orientation *= rotation_Quaternion;
        }

        /// <summary>
        /// Rotate the object
        /// </summary>
        /// <param name="axis_angle">A Vector3f which contains the axis angle over which the object should be rotated</param>
        /// <param name="degrees">A float which contains the degrees of the rotation</param>
        public void Rotate(Vector3f axis_angle, float degrees)
        {
            Quaternion rot = Quaternion.FromAxisAngle(axis_angle, Math.Math.DegreesToRadians(degrees));
            this.Orientation = Orientation * rot;
            //this.Orientation = this.Orientation.Rotated(Math.Math.DegreesToRadians(degrees), axis_angle);
        }

        private void CalcModelMatrix()
        {
            Mat4 rot = Mat4.CreateFromQuaternion(this.Orientation);
            Mat4 trans = Mat4.CreateTranslation(Position);
            Mat4 scl = Mat4.CreateScale(Scale);
            this._model = Mat4.Identity * trans * scl * rot;
            //this._model = Mat4.Identity * trans ;
        }

        public override string ToString()
        {
            return $"Position: {Position.X}, {Position.Y}, {Position.Z}" +
                $"Rotation: {Rotation.X}, {Rotation.Y}, {Rotation.Z}" +
                $"Scale: {Scale.X}, {Scale.Y}, {Scale.Z}";
        }
    }
}
