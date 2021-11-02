using Leviathan.Core.Graphics;
using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

using MathL = Leviathan.Math.Math;

namespace Leviathan.ECS.Components
{
    public class CameraComponent : Component
    {
        public CameraComponent()
        {
            _projection = Mat4.Identity;
            _view = Mat4.Identity;
            _projection_s = new ProjectionSettings();
        }

        private bool projection_changed;
        private ProjectionSettings _projection_s;
        public ProjectionSettings Projection { 
            get {
                return _projection_s;
            } 
            set {
                _projection_s = value;
                projection_changed = true;
            } 
        }
        
        private Mat4 _projection;
        public Mat4 ProjectionMatrix { 
            get {
                if(projection_changed)
                {
                    UpdateProjectionMatrix();
                    projection_changed = false;
                }
                return _projection; 
            } 
            private set {
                projection_changed = true;
                _projection = value;
            } 
        }

        private Mat4 _view;
        public Mat4 ViewMatrix {
            get
            {
                return _view;
            }
            private set
            {
                _view = value;
            }
        }

        private void UpdateViewmatrix()
        {
            //Translate the orientation to looking point
            //Vector3f rotation = Vector3f.Zero;
            //rotation.X = Math.Math.Cos(Math.Math.DegreesToRadians(Orientation.Y)) * Math.Math.Cos(Math.Math.DegreesToRadians(Orientation.X));
            //rotation.Y = Math.Math.Sin(Math.Math.DegreesToRadians(Orientation.X));
            //rotation.Z = Math.Math.Sin(Math.Math.DegreesToRadians(Orientation.Y)) * Math.Math.Cos(Math.Math.DegreesToRadians(Orientation.X));
            //
            //this.Foreward = rotation;
            //
            ////apply the current orientation and calculate right vector, up vector and lookat matrix
            //Vector3f virt_cam_up = Vector3f.UnitY;
            
            
            Vector3f Target = parent.Transform.Direction + parent.Transform.Position;
            Vector3f cam_dir = Vector3f.Normalize(parent.Transform.Position - Target);
            Right = Vector3f.Normalize(Vector3f.Cross(virt_cam_up, cam_dir));
            Up = Vector3f.Normalize(Vector3f.Cross(cam_dir, this.Right));
            
            Vector3f 
            this.ViewMatrix = Mat4.LookAt(parent.Transform.Position, Target, this.Up);
            //Console.WriteLine(Orientation);
        }

        private void UpdateProjectionMatrix()
        {
            if(_projection_s.perspective.fov < 0.0f)
            {
                _projection_s.perspective.fov = 0.0f;
            }

            if (_projection_s.perspective.fov > 180.0f)
            {
                _projection_s.perspective.fov = 180.0f;
            }

            Vector2d window_size = Context.parent_window.nativeWindow.w_size;
            Vector2f clipspace = _projection_s.clipspace.GetClipspace();
            if(_projection_s.mode == ProjectionMode.PERSPECTIVE)
            {
                float aspect = (float)(window_size.X / window_size.Y);
                float fov_rad = MathL.DegreesToRadians(_projection_s.perspective.fov);
                ProjectionMatrix = Mat4.CreatePerspectiveFieldOfView(fov_rad, aspect, clipspace.X, clipspace.Y);
            } else
            {
                ProjectionMatrix = Mat4.CreateOrthographic((float)window_size.X, (float)window_size.Y, clipspace.X, clipspace.Y);
            }
        }


        public override string ToString()
        {
            return string.Empty;
        }
    }

    public class ProjectionSettings
    {
        public ProjectionMode mode;
        public PerspectiveSettings perspective;
        public OrthographicSettings orthographic;
        public ClipspaceSettings clipspace;

        public ProjectionSettings(PerspectiveSettings _perspective)
        {
            Init();
            mode = ProjectionMode.PERSPECTIVE;
            perspective = _perspective;
        }

        public ProjectionSettings(OrthographicSettings _orthographic)
        {
            Init();
            mode = ProjectionMode.PERSPECTIVE;
            orthographic = _orthographic;
        }

        public ProjectionSettings()
        {
            Init();
        }

        private void Init()
        {
            perspective = new PerspectiveSettings();
            orthographic = new OrthographicSettings();
            clipspace = new ClipspaceSettings();

            Reset();
        }

        public void Reset()
        {
            mode = ProjectionMode.PERSPECTIVE;
            perspective.Reset();
            orthographic.Reset();
            clipspace.Reset();
        }
    }

    public struct PerspectiveSettings
    {
        /// <summary>
        /// The field of view in degrees;
        /// </summary>
        public float fov;

        public void Reset()
        {
            fov = 60.0f;
        }
    }

    public struct OrthographicSettings
    {
        /// <summary>
        /// The size of the aperture
        /// </summary>
        public float size;

        public void Reset()
        {
            size = 5.0f;
        }
    }

    public struct ClipspaceSettings
    {
        /// <summary>
        /// The minimal clip space distance
        /// </summary>
        public float zMin;

        /// <summary>
        /// The maximum clip space distance
        /// </summary>
        public float zMax;

        public Vector2f GetClipspace()
        {
            return new Vector2f(zMin, zMax);
        }

        public void Reset()
        {
            zMin = 0.1f;
            zMax = 100.0f;
        }
    }

    public enum ProjectionMode
    {
        PERSPECTIVE,
        ORTHOGRAPHIC
    }
}
