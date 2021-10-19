using Leviathan.ECS;
using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics
{
    public class Camera
    {
        public Vector3f Position;
        public Vector3f Orientation;


        public Vector3f Foreward { get; private set; }
        public Vector3f Up { get; private set; }
        public Vector3f Right { get; private set; }
        public Vector3f Target { get; private set; }

        public CameraViewSettings ViewSettings { get; private set; }
        public CameraMode Mode { get; private set; }
        public Mat4 ProjectionMatrix { get; private set; }
        public Mat4 ViewMatrix { get; private set; }

        public Camera()
        {
            CameraViewSettings tmp = new CameraViewSettings()
            {
                capture_size = new Vector2i(1080, 720),
                field_of_view = 40.0f,
                clipspace = new Vector2f(0.1f, 100.0f)
            };
            SetViewSettings(tmp);
            this.SetMode(CameraMode.FPS);
        }

        public void SetMode(CameraMode newmode)
        {
            this.Mode = newmode;
            this.Orientation = Vector3f.Zero;
            UpdateViewMatrix();
        }

        /// <summary>
        /// Rotates the camera in 3 dimensions in degrees
        /// </summary>
        /// <param name="pitch">The x-axis rotation in degrees</param>
        /// <param name="yaw">The y-axis rotation in degrees</param>
        /// <param name="roll">The z-axis rotation in degrees</param>
        public void Rotate3D(float pitch, float yaw, float roll)
        {
            Rotate3D(new Vector3f(pitch, yaw, roll));
        }

        /// <summary>
        /// Rotates the camera in 3 dimensions in degrees 
        /// </summary>
        /// <param name="rotation">The xyz-axis rotation vector in degrees</param>
        public void Rotate3D(Vector3f rotation)
        {
            this.Orientation += rotation;
            CorrectRotation();
        }

        /// <summary>
        /// Rotates the camera in 2 dimensions in degrees
        /// </summary>
        /// <param name="pitch">The x-axis rotation in degrees</param>
        /// <param name="yaw">The y-axis rotation in degrees</param>
        public void Rotate2D(float pitch, float yaw)
        {
            Rotate2D(new Vector2f(pitch, yaw));
        }

        /// <summary>
        /// Rotates the camera in 2 dimensions in degrees 
        /// </summary>
        /// <param name="rotation">The xy-axis rotation vector in degrees</param>
        public void Rotate2D(Vector2f rotation)
        {
            this.Orientation += new Vector3f(rotation.X, rotation.Y, 0.0f);
            CorrectRotation();
        }

        private void CorrectRotation()
        {
            if (Mode == CameraMode.FPS)
            {

                if(Orientation.X > 89.9f)
                {
                    Orientation.X = 89.9f;
                } else if(Orientation.X < -89.9f)
                {
                    Orientation.X = -89.9f;
                }

            }
        }

        public void Translate(Vector3f offset)
        {
            this.Position = Position + offset;
        }

        public void SetViewSettings(CameraViewSettings newsettings)
        {
            this.ViewSettings = newsettings;
            float aspect = ((float)ViewSettings.capture_size.X) / ((float)ViewSettings.capture_size.Y);
            this.ProjectionMatrix = Mat4.CreatePerspectiveFieldOfView(
                                            Math.Math.DegreesToRadians(ViewSettings.field_of_view),
                                            aspect,
                                            ViewSettings.clipspace.X,
                                            ViewSettings.clipspace.Y);
        }

        public void UpdateViewMatrix()
        {
            //Translate the orientation to looking point
            Vector3f rotation = Vector3f.Zero;
            rotation.X = Math.Math.Cos(Math.Math.DegreesToRadians(Orientation.Y)) * Math.Math.Cos(Math.Math.DegreesToRadians(Orientation.X));
            rotation.Y = Math.Math.Sin(Math.Math.DegreesToRadians(Orientation.X));
            rotation.Z = Math.Math.Sin(Math.Math.DegreesToRadians(Orientation.Y)) * Math.Math.Cos(Math.Math.DegreesToRadians(Orientation.X));

            this.Foreward = rotation;
            this.Target = this.Foreward + this.Position;

            //apply the current orientation and calculate right vector, up vector and lookat matrix
            Vector3f virt_cam_up = Vector3f.UnitY;
            Vector3f cam_dir = Vector3f.Normalize(this.Position - this.Target);
            this.Right = Vector3f.Normalize(Vector3f.Cross(virt_cam_up, cam_dir));
            this.Up = Vector3f.Normalize(Vector3f.Cross(cam_dir, this.Right));
            this.ViewMatrix = Mat4.LookAt(this.Position, this.Target, this.Up);
            //Console.WriteLine(Orientation);
        }


    }

    public enum CameraMode
    {
        OMNIDIRECTIONAL,
        FPS
    }

    public struct CameraViewSettings
    {
        /// <summary>
        /// The size of the capture window in pixels [width, height]
        /// </summary>
        public Vector2i capture_size;

        /// <summary>
        /// The camera field of view in degrees
        /// </summary>
        public float field_of_view;

        /// <summary>
        /// The camera viewable clipspace [min, max]
        /// </summary>
        public Vector2f clipspace;
    }
}
