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
                Vector3f newpos = (new Vector4f(LocalPosition, 1.0f) * modelmat).Xyz;
                return newpos;
            }
        }

        public Vector3f Rotation
        {
            get
            {
                Vector3f newrot = (GetParentedRotationMatrix() * new Vector4f(LocalRotation, 1.0f)).Xyz;
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
                Vector4f result =  GetParentedRotationMatrix() * Vector4f.Forward;
                return new Vector3f(result.X, result.Y, result.Z).Normalized();
            }
        }

        /// <summary>
        /// Gets the normalized Up vector of the Transform
        /// </summary>
        public Vector3f Up
        {
            get
            {
                //Vector4f result = Orientation * Vector4f.Up;
                //Vector4f result = Vector4f.Up * GetParentedRotationMatrix();
                Vector4f result = GetParentedRotationMatrix() * Vector4f.Up;
                return new Vector3f(result.X, result.Y, result.Z).Normalized();
            }
        }

        /// <summary>
        /// Gets the normalized Right vector of the Transform
        /// </summary>
        public Vector3f Right
        {
            get
            {
                //Vector4f result = Orientation * Vector4f.Right;
                //Vector4f result = Vector4f.Right * GetParentedRotationMatrix();
                Vector4f result = GetParentedRotationMatrix() * Vector4f.Right;
                return new Vector3f(result.X, result.Y, result.Z).Normalized();
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
            this.Orientation = Orientation * rot;
        }

        private void CalcModelMatrix()
        {
            Mat4 rot = Mat4.CreateFromQuaternion(this.Orientation);
            Mat4 trans = Mat4.CreateTranslation(LocalPosition);
            Mat4 scl = Mat4.CreateScale(LocalScale);
            this._model = Mat4.Identity * scl * rot * trans;
        }

        private Mat4 CalcTransformMatrix()
        {
            Mat4 rot = Mat4.CreateFromQuaternion(this.Orientation);
            Mat4 trans = Mat4.CreateTranslation(LocalPosition);
            return Mat4.Identity * rot * trans;
        }

        private Mat4 CalcRotationMatrix()
        {
            Mat4 rot = Mat4.CreateFromQuaternion(this.Orientation);
            return Mat4.Identity * rot;
        }

        /// <summary>
        /// Gets the concatinated model matrices from the parents and the current transform 
        /// </summary>
        /// <returns>Model matrix describing the translation/rotation/scale of the current entity relative to the world</returns>
        private Mat4 GetParentedModelMat()
        {
            Mat4 result = Mat4.Identity * LocalModelMat;
            //result *= ModelMat;
            if (Parent.Parent != null)
            {
                result *= Parent.Parent.Transform.GetParentedModelMat();
            }
            return result;
        }

        private Mat4 GetParentedTransformationMat()
        {
            Mat4 result = Mat4.Identity * CalcTransformMatrix();
            //result *= ModelMat;
            if (Parent.Parent != null)
            {
                result *= Parent.Parent.Transform.GetParentedTransformationMat();
            }
            return result;
        }

        //private Mat4 GetParentedRotationMatrix()
        //{
        //    //result *= ModelMat;
        //    Mat4 result = Mat4.Identity * CalcRotationMatrix();
        //    if (Parent.Parent != null)
        //    {
        //        result *= Parent.Parent.Transform.GetParentedRotationMatrix();
        //    }
        //    return result;
        //
        //    //Quaternion result = Quaternion.Identity * Orientation;
        //    //if(Parent.Parent != null)
        //    //{
        //    //    result *= Parent.Parent.Transform.Orientation;
        //    //}
        //    //return Mat4.CreateFromQuaternion(result);
        //}

        private Quaternion GetParentedRotationMatrix()
        {
            Quaternion result = Quaternion.Identity;
            if(Parent.Parent != null)
            {
                result *= Parent.Parent.Transform.Orientation;
            }
            result *= Orientation;
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
