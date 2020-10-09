using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace Leviathan.Core
{
    public class Camera
    {
        public Vector3 position;
        public ViewSettings Settings { get; private set; }
        public Matrix4 Projection { get; private set; }
        public Matrix4 View { get; private set; }

        public Vector3 Cam_target { get; private set; }
        public Vector3 Cam_up { get; private set; }
        public Vector3 Cam_right { get; private set; }

        private float _roll, _pitch, _yaw;
        public Camera()
        {
            this.Settings = new ViewSettings
            {
                width = 100,
                height = 100,
                fov_deg = 45,
                z_near = 0.1f,
                z_far = 100f
            };
            this.position = Vector3.Zero;
            this.Roll = 0;
            this.Pitch = 0;
            this.Yaw = 0;
            this.ConstructViewMatrix();
        }

        public float Roll
        {
            get; set;
        }

        public float Pitch
        {
            get; set;
        }

        public float Yaw
        {
            get; set;
        }

        public void MoveForeward(float frametime, float step)
        {
            this.position += (this.Cam_target - this.position) * (frametime * step);
        }

        public void MoveBackward(float frametime, float step)
        {
            this.position -= (this.Cam_target - this.position) * (frametime * step);
        }

        public void MoveRight(float frametime, float step)
        {
            this.position += this.Cam_right * (frametime * step);
        }

        public void MoveLeft(float frametime, float step)
        {
            this.position -= this.Cam_right * (frametime * step);
        }

        public void MoveUp(float frametime, float step)
        {
            this.position += this.Cam_up * (frametime * step);
        }

        public void MoveDown(float frametime, float step)
        {
            this.position -= this.Cam_up * (frametime * step);
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
        }

        private void ConstructViewMatrix()
        {
            Quaternion QuatAroundX = new Quaternion(new Vector3(_pitch, 0f, 0f));
            Quaternion QuatAroundY = new Quaternion(new Vector3(0f, _yaw, 0f));
            Quaternion final = QuatAroundX * QuatAroundY;
            this.Cam_target = (final * Vector3.UnitZ) + this.position;
            this.Cam_up = Vector3.UnitY;

            Vector3 direction = Vector3.Normalize(this.position - this.Cam_target);
            this.Cam_right = Vector3.Normalize(Vector3.Cross(this.Cam_up, direction));
            this.Cam_up = Vector3.Normalize(Vector3.Cross(direction, Cam_right));

            this.Projection = Matrix4.LookAt(this.position, Cam_target, this.Cam_up);
        }

        public void SetViewMatrix(ViewSettings settings)
        {
            float ratio = ((float)settings.width / (float)settings.height);
            float fov = MathHelper.DegreesToRadians(settings.fov_deg);
            this.View = Matrix4.CreatePerspectiveFieldOfView(fov, ratio, settings.z_near, settings.z_far);
            this.Settings = settings;
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
