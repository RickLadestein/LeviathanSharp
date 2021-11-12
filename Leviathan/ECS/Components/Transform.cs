using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.ECS
{
    public class Transform : Component
    {

        public Transform()
        {
            Parent = null;
            LocalPosition = Vector3f.Zero;
            Orientation = Quaternion.Identity;
            LocalScale = Vector3f.One;
        }

        /// <summary>
        /// The current Position
        /// </summary>
        public Vector3f LocalPosition;

        /// <summary>
        /// The current Scale
        /// </summary>
        public Vector3f LocalScale;

        /// <summary>
        /// The current orientation quaternion
        /// </summary>
        public Quaternion Orientation;

        /// <summary>
        /// The current Rotation in degrees
        /// </summary>
        public Vector3f LocalRotation
        {
            get
            {
                Orientation.ToEulerAngles(out Vector3f _rot);
                return new Vector3f((float)_rot.X, (float)_rot.Y, (float)_rot.Z);
            }
        }

        public Vector3f Position
        {
            get
            {
                Mat4 modelmat = GetParentedTransformationMat();
                Vector3f newpos = (modelmat * new Vector4f(LocalPosition, 1.0f)).Xyz;
                return newpos;
            }
        }

        public Vector3f Rotation
        {
            get
            {
                Vector3f newrot = (GetParentedRotation() * new Vector4f(LocalRotation, 1.0f)).Xyz;
                return newrot;
            }
        }

        /// <summary>
        /// Gets the normalized Direction vector of the Transform
        /// </summary>
        public Vector3f Forward
        {
            get
            {
                Vector4f result = GetParentedRotation() * new Vector4f(Vector3f.Forward, 1.0f);
                return result.Xyz.Normalized();
            }
        }

        /// <summary>
        /// Gets the normalized Up vector of the Transform
        /// </summary>
        public Vector3f Up
        {
            get
            {
                Vector4f result = GetParentedRotation() * new Vector4f(Vector3f.Up, 1.0f);
                //Vector4f result = GetParentedTransformationMat() * new Vector4f(Vector3f.Up, 1.0f);
                return result.Xyz.Normalized();
            }
        }

        /// <summary>
        /// Gets the normalized Right vector of the Transform
        /// </summary>
        public Vector3f Right
        {
            get
            {
                Vector4f result = GetParentedRotation() * new Vector4f(Vector3f.Right, 1.0f);
                //Vector4f result = GetParentedTransformationMat() * new Vector4f(Vector3f.Up, 1.0f);
                //result.Normalize();
                return result.Xyz.Normalized();
            }
        }

        /// <summary>
        /// The matrix that describes the orientation, scale and position
        /// </summary>
        public Mat4 LocalModelMat
        {
            get
            {
                CalcModelMatrix();
                return _model;
            }
        }
        private Mat4 _model;

        public Mat4 ModelMat
        {
            get
            {
                return GetParentedModelMat();
            }
        }

        /// <summary>
        /// The matrix that describes the translation of the normal vector
        /// </summary>
        public Mat3 NormalMat
        {
            get
            {
                Mat4 mod = ModelMat;
                mod.Transpose();
                return new Mat3(mod.Inverted());
            }
        }

        /// <summary>
        /// Resets the transform rotation, position and scale to default values
        /// </summary>
        public void Reset()
        {
            this.LocalPosition = Vector3f.Zero;
            this.Orientation = Quaternion.Identity;
            this.LocalScale = new Vector3f(1.0f, 1.0f, 1.0f);
        }

        /// <summary>
        /// Set the current orientation
        /// </summary>
        /// <param name="roll">A float containing the roll of the orientation in degrees</param>
        /// <param name="pitch">A float containing the pitch of the orientation in degrees</param>
        /// <param name="yaw">A float containing the yaw of the orientation in degrees</param>
        public void SetOrientationEulerDeg(float roll, float pitch, float yaw)
        {
            float rad_x = MathL.DegreesToRadians(pitch);
            float rad_y = MathL.DegreesToRadians(yaw);
            float rad_z = MathL.DegreesToRadians(roll);
            SetOrientationEulerRad(rad_z, rad_x, rad_y);
        }

        public void SetOrientationEulerRad(float roll, float pitch, float yaw)
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
            Quaternion rot = Quaternion.FromAxisAngle(axis_angle, MathL.DegreesToRadians(degrees));
            this.Orientation *= rot;
        }

        private void CalcModelMatrix()
        {
            Mat4 rot = Mat4.CreateFromQuaternion(this.Orientation);
            Mat4 trans = Mat4.CreateTranslation(LocalPosition);
            Mat4 scl = Mat4.CreateScale(LocalScale);

            //SRT (Scale Rotate Translate)
            this._model = ((Mat4.Identity * scl) * rot) * trans;
            //this._model = ((Mat4.Identity * trans) * scl) * rot;
        }

        private Mat4 CalcTransformMatrix()
        {
            Mat4 rot = Mat4.CreateFromQuaternion(this.Orientation);
            Mat4 trans = Mat4.CreateTranslation(LocalPosition);
            return (Mat4.Identity * rot) * trans;
            //return (Mat4.Identity * trans) * rot;
        }

        /// <summary>
        /// Gets the concatinated model matrices from the parents and the current transform 
        /// </summary>
        /// <returns>Model matrix describing the translation/rotation/scale of the current entity relative to the world</returns>
        private Mat4 GetParentedModelMat()
        {
            Mat4 result = Mat4.Identity * LocalModelMat;
            if (Parent.Parent != null)
            {
                result *= Parent.Parent.Transform.GetParentedModelMat();
            }
            //result *= LocalModelMat;

            //Primal_matrix -> Child -> Child -> Child -> etc
            return result;
        }

        private Mat4 GetParentedTransformationMat()
        {
            Mat4 result = Mat4.Identity * CalcTransformMatrix();
            if (Parent.Parent != null)
            {
                result *= Parent.Parent.Transform.GetParentedTransformationMat();
            }
            //result *= CalcTransformMatrix();
            return result;
        }

        private Quaternion GetParentedRotation()
        {
            Quaternion result = Quaternion.Identity * Orientation;
            if(Parent.Parent != null)
            {
                result *= Parent.Parent.Transform.GetParentedRotation();
            }
            //result *= Orientation;
            result.Normalize();
            return result;
        }

        public override string ToString()
        {
            return $"Position: {LocalPosition.X}, {LocalPosition.Y}, {LocalPosition.Z}" +
                $"Rotation: {LocalRotation.X}, {LocalRotation.Y}, {LocalRotation.Z}" +
                $"Scale: {LocalScale.X}, {LocalScale.Y}, {LocalScale.Z}";
        }
    }
}
