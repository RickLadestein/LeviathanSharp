using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Windows.Markup;

namespace Leviathan.Core
{
    public class Camera : Entity
    {
        private static Camera _main;

        public static Camera Main
        {
            get { 
                if(_main == null) 
                    _main = new Camera(); 
                return _main; 
            }
            set { 
                if (value == null) 
                    throw new Exception("Main camera cannot be null"); 
                _main = value; 
            }
        }

        private ViewSettings _vsettings;
        public ViewSettings Viewsettings { 
            get { 
                return _vsettings; 
            }
            set { 
                this._vsettings = value;
                this.SetViewMatrix(this._vsettings);
            }
        }

        private float _roll, _pitch, _yaw;

        public Vector3 Position { get; set; }

        public Vector3 Rotation_d
        {
            get
            {
                return new Vector3(MathHelper.RadiansToDegrees(_pitch), MathHelper.RadiansToDegrees(_yaw), MathHelper.RadiansToDegrees(_roll));
            }
            set
            {
                this._roll = MathHelper.DegreesToRadians(value.Z);
                this._pitch = MathHelper.DegreesToRadians(value.X);
                this._yaw = MathHelper.DegreesToRadians(value.Y);
                this.ConstructProjectionMatrix();
            }
        }

        public Vector3 Rotation_r
        {
            get
            {
                return new Vector3(_pitch, _yaw, _roll);
            }
            set
            {
                this._roll = value.Z;
                this._pitch = value.X;
                this._yaw = value.Y;
                this.ConstructProjectionMatrix();
            }
        }

        public Vector3 Cam_target { get; private set; }
        public Vector3 Cam_up { get; private set; }
        public Vector3 Cam_right { get; private set; }

        public Matrix4 Projection { get; private set; }
        public Matrix4 View { get; private set; }

       

        
        public Camera()
        {
            this.Position = Vector3.Zero;
            this.Projection = Matrix4.Identity;
            this.View = Matrix4.Identity;
            this._roll = 0;
            this._pitch = 0;
            this._yaw = 0;
            this.Viewsettings = new ViewSettings
            {
                width = 100,
                height = 100,
                fov_deg = 45,
                z_near = 0.1f,
                z_far = 100f
            };
            
            this.ConstructProjectionMatrix();
        }

        public void MoveForeward(float frametime, float step)
        {
            this.Position += (this.Cam_target - this.Position) * (frametime * step);
            this.ConstructProjectionMatrix();
        }

        public void MoveBackward(float frametime, float step)
        {
            this.Position -= (this.Cam_target - this.Position) * (frametime * step);
            this.ConstructProjectionMatrix();
        }

        public void MoveRight(float frametime, float step)
        {
            this.Position += this.Cam_right * (frametime * step);
            this.ConstructProjectionMatrix();
        }

        public void MoveLeft(float frametime, float step)
        {
            this.Position -= this.Cam_right * (frametime * step);
            this.ConstructProjectionMatrix();
        }

        public void MoveUp(float frametime, float step)
        {
            this.Position += this.Cam_up * (frametime * step);
            this.ConstructProjectionMatrix();
        }

        public void MoveDown(float frametime, float step)
        {
            this.Position -= this.Cam_up * (frametime * step);
            this.ConstructProjectionMatrix();
        }

        public void Rotate(float roll, float pitch, float yaw)
        {
            Vector3 rotation = this.Rotation_d;
            rotation.Z += roll;
            if(rotation.Z < 0f)
            {
                rotation.Z = 360 - rotation.Z;
            } else if(rotation.Z > 360.0f)
            {
                rotation.Z = rotation.Z % 360.0f;
            }

            rotation.Y += yaw;
            if (rotation.Y < 0f)
            {
                rotation.Y = 360 - rotation.Y;
            }
            else if (rotation.Y > 360.0f)
            {
                rotation.Y %= 360.0f;
            }

            rotation.X += pitch;
            rotation.X = MathHelper.Clamp(rotation.X, -89.9f, 89.9f);

            this.Rotation_d = rotation;

            this.ConstructProjectionMatrix();
            Console.WriteLine($"Rotation [{rotation.X}, {rotation.Y}, {rotation.Z}]");
        }

        private void ConstructProjectionMatrix()
        {
            
            Quaternion QuatAroundX = new Quaternion(new Vector3(_pitch, 0f, 0f));
            Quaternion QuatAroundY = new Quaternion(new Vector3(0, _yaw, 0f));
            Quaternion QuatAroundZ = new Quaternion(new Vector3(0, 0f, _roll));
            Matrix4 final = Matrix4.CreateFromQuaternion(QuatAroundX * QuatAroundY * QuatAroundZ);

            this.Cam_target = (final * Vector4.UnitZ).Xyz + this.Position;
            this.Cam_up = Vector3.UnitY;
            Vector3 direction = Vector3.Normalize(this.Position - this.Cam_target);
            this.Cam_right = Vector3.Normalize(Vector3.Cross(this.Cam_up, direction));
            this.Cam_up = Vector3.Normalize(Vector3.Cross(direction, Cam_right));
            this.View = Matrix4.LookAt(this.Position, Cam_target, this.Cam_up);
        }

        private void SetViewMatrix(ViewSettings settings)
        {
            float aspect = ((float)settings.width / (float)settings.height);
            float fov = MathHelper.DegreesToRadians(settings.fov_deg);
            this.Projection = Matrix4.CreatePerspectiveFieldOfView(fov, aspect, settings.z_near, settings.z_far);
        }
    }

    public struct ViewSettings {
        public int width;
        public int height;
        public float z_near;
        public float z_far;
        public float fov_deg;
    }
}
