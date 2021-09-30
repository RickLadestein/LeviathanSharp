﻿using Leviathan.ECS;
using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics
{
    public class Camera
    {
        public Vector3f Position;
        public Quaternion Orientation;
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
            this.Orientation = Quaternion.Identity;
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
            this.Orientation = Quaternion.Rotate(Orientation, rotation);
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
            if(Mode == CameraMode.FPS && !CheckRotation(rotation))
            {
                return;
            }
            Orientation = Quaternion.Rotate(Orientation, rotation);
        }

        private bool CheckRotation(Vector2f rotation)
        {
            
            Quaternion neworient = Quaternion.Rotate(Orientation, rotation);
            Vector3f eulerangles_rad = neworient.ToEulerAngles();
            Vector3f eulerangles = Math.Math.RadiansToDegrees(eulerangles_rad);
            Console.WriteLine(eulerangles);
            if (eulerangles.X > 89.0f || eulerangles.X < -89.0f)
            {
                return false;
            } else
            {
                return true;
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
            Vector4f _foreward = Orientation * Vector4f.UnitZ;
            this.Foreward = _foreward.Xyz;
            this.Target = this.Foreward + this.Position;

            //apply the current orientation and calculate right vector, up vector and lookat matrix
            Vector3f virt_cam_up = Vector3f.UnitY;
            Vector3f cam_dir = Vector3f.Normalize(this.Position - this.Target);
            this.Right = Vector3f.Normalize(Vector3f.Cross(virt_cam_up, cam_dir));
            this.Up = Vector3f.Normalize(Vector3f.Cross(cam_dir, this.Right));
            this.ViewMatrix = Mat4.LookAt(this.Position, this.Target, this.Up);
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
