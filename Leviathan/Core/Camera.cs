using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace Leviathan.Core
{
    class Camera
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
        }

        public float Roll
        {
            get => MathHelper.RadiansToDegrees(Roll);
            set {
                    _roll = MathHelper.DegreesToRadians(value);
                    ConstructViewMatrix();
                }
        }

        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(Pitch);
            set
            {
                var angle = MathHelper.Clamp(Pitch, -89.9f, 89.9f);
                _pitch = MathHelper.DegreesToRadians(angle);
                ConstructViewMatrix();
            }
        }

        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(Yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                ConstructViewMatrix();
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

    struct ViewSettings {
        public int width;
        public int height;
        public float z_near;
        public float z_far;
        public float fov_deg;
    }
}
