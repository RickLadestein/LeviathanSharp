using Leviathan.Core.ECS;
using Leviathan.Core.ECS.Components;
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

        public float Roll, Pitch, Yaw;

        public Vector3 Position { get; set; }

        public Vector3 Rotation_d
        {
            get
            {
                return new Vector3(MathHelper.RadiansToDegrees(_pitch), MathHelper.RadiansToDegrees(_yaw), MathHelper.RadiansToDegrees(_roll));
            }
            set
            {
                this._roll += MathHelper.DegreesToRadians(value.Z);
                this._pitch += MathHelper.DegreesToRadians(value.X);
                this._yaw += MathHelper.DegreesToRadians(value.Y);
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
                this._roll += value.Z;
                this._pitch += value.X;
                this._yaw += value.Y;
            }
        }

        public Vector3 Cam_target { get; private set; }
        public Vector3 Cam_up { get; private set; }
        public Vector3 Cam_right { get; private set; }

        public Matrix4 Projection { get; private set; }
        public Matrix4 View { get; private set; }

       

        
        public Camera()
        {

            this.Viewsettings = new ViewSettings
            {
                width = 100,
                height = 100,
                fov_deg = 45,
                z_near = 0.1f,
                z_far = 100f
            };
            this.Position = Vector3.Zero;
            this.Roll = 0;
            this.Pitch = 0;
            this.Yaw = 0;
            this.ConstructViewMatrix();
        }

        public void MoveForeward(float frametime, float step)
        {
            this.Position += (this.Cam_target - this.Position) * (frametime * step);
        }

        public void MoveBackward(float frametime, float step)
        {
            this.Position -= (this.Cam_target - this.Position) * (frametime * step);
        }

        public void MoveRight(float frametime, float step)
        {
            this.Position += this.Cam_right * (frametime * step);
        }

        public void MoveLeft(float frametime, float step)
        {
            this.Position -= this.Cam_right * (frametime * step);
        }

        public void MoveUp(float frametime, float step)
        {
            this.Position += this.Cam_up * (frametime * step);
        }

        public void MoveDown(float frametime, float step)
        {
            this.Position -= this.Cam_up * (frametime * step);
        }

        public void Rotate(float roll, float pitch, float yaw)
        {
            this.Roll += (roll % 360.0f);
            if(this.Roll >= 360.0f)
            {
                this.Roll = this.Roll % 360;
            } else if(this.Roll < 0.0f)
            {
                this.Roll = 360 +(this.Roll % 360.0f);
            }

            this.Pitch += pitch;
            this.Pitch = MathHelper.Clamp(Pitch, -89.9f, 89.9f);

            this.Yaw += (yaw % 360.0f);
            if (this.Yaw >= 360.0f)
            {
                this.Yaw = this.Yaw % 360;
            }
            else if (this.Yaw < 0.0f)
            {
                this.Yaw = 360 + (this.Yaw % 360.0f);
            }
            this.ConstructViewMatrix();
        }

        private void ConstructViewMatrix()
        {
            Quaternion QuatAroundX = new Quaternion(new Vector3(_pitch, 0f, 0f));
            Quaternion QuatAroundY = new Quaternion(new Vector3(0f, _yaw, 0f));
            Quaternion final = QuatAroundX * QuatAroundY;
            this.Cam_target = (final * Vector3.UnitZ) + this.Position;
            this.Cam_up = Vector3.UnitY;

            Vector3 direction = Vector3.Normalize(this.Position - this.Cam_target);
            this.Cam_right = Vector3.Normalize(Vector3.Cross(this.Cam_up, direction));
            this.Cam_up = Vector3.Normalize(Vector3.Cross(direction, Cam_right));

            this.Projection = Matrix4.LookAt(this.Position, Cam_target, this.Cam_up);
        }

        private void SetViewMatrix(ViewSettings settings)
        {
            float ratio = ((float)settings.width / (float)settings.height);
            float fov = MathHelper.DegreesToRadians(settings.fov_deg);
            this.View = Matrix4.CreatePerspectiveFieldOfView(fov, ratio, settings.z_near, settings.z_far);
            this._vsettings = settings;
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
