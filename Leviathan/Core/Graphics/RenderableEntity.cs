using Leviathan.Core.Graphics.Buffers;
using Leviathan.Util;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics
{
    public class RenderableEntity : Entity
    {
        /// <summary>
        /// Position in cartesian float coordinates
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Rotation in floating point radians
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Rotation in floating point degrees
        /// </summary>
        public Vector3 Rotation_d
        {
            get
            {
                float x_comp = MathHelper.RadiansToDegrees(Rotation.X);
                float y_comp = MathHelper.RadiansToDegrees(Rotation.Y);
                float z_comp = MathHelper.RadiansToDegrees(Rotation.Z);
                return new Vector3(x_comp, y_comp, z_comp);
            }

            set
            {
                float x_comp = MathHelper.DegreesToRadians(value.X);
                float y_comp = MathHelper.DegreesToRadians(value.Y);
                float z_comp = MathHelper.DegreesToRadians(value.Z);
                this.Rotation = new Vector3(x_comp, y_comp, z_comp);
            }
        }

        /// <summary>
        /// Scale in floating point units
        /// </summary>
        public Vector3 Scale { get; set; }

        public Matrix4 Model
        {
            get
            {
                Quaternion quat = new Quaternion(Rotation);
                Matrix4 rot = Matrix4.Identity * Matrix4.CreateFromQuaternion(quat);
                Matrix4 scl = Matrix4.Identity * Matrix4.CreateScale(Scale);
                Matrix4 trans = Matrix4.Identity * Matrix4.CreateTranslation(Position);
                Matrix4 output = (rot * trans) * scl;
                return output;
            }
            private set { }
        }

        public MultiTexture Texture { get; private set; }

        public ShaderProgram Shader { get; set; }
        public VertexBuffer Buffer { get; private set; }

        public RenderableEntity() : base()
        {
            this.Buffer = new VertexBuffer();
            this.Texture = new MultiTexture();
            this.Shader = null;
            this.Scale = new Vector3(1.0f);
        }

        /// <summary>
        /// Draw to the screen relative to the camera view
        /// </summary>
        /// <param name="cam">The camera that observes</param>
        public void Draw(Camera cam)
        {
            
            if (Shader != null)
            {
                if (!Texture.Bound)
                {
                    Texture.Bind();
                }
                this.Buffer.Bind();
                Shader.Use();

                Shader.UniformMat4("projection", cam.Projection);
                Shader.UniformMat4("view", cam.View);
                Shader.UniformMat4("model", this.Model);
                GL.DrawArrays(PrimitiveType.Triangles, 0, this.Buffer.vao.segments);

                Buffer.Unbind();
                Shader.Unbind();
                Texture.Unbind();
            } 
        }

        public void SetTransform(Vector3 position, Vector3 rotation, bool radians, Vector3 scale) {
            this.Position = position;
            this.Scale = scale;
            if(radians)
            {
                this.Rotation = rotation;
            } else
            {
                this.Rotation_d = rotation;
            }
        }


        public bool SetMesh(string mesh_id)
        {
            Mesh mesh = Mesh.GetMeshFromLoadedMeshes(mesh_id);
            return this.SetMesh(mesh);
        }

        public bool SetMesh(Mesh mesh)
        {
            if(mesh == null || !mesh.Valid)
            {
                Logger.GetInstance().LogWarning("Could not load mesh: mesh was not found or not valid");
                return false;
            }
            if(this.Buffer.vao.handle > 0)
            {
                this.Buffer.Destroy();
            }
            this.Buffer.BufferData(mesh.Object_data);
            return true;
        }
        

        public bool SetShader(string shader_id)
        {
            ShaderProgram prog = ShaderProgram.GetShaderPorgramFromStorage(shader_id);
            if(prog != null)
            {
                this.Shader = prog;
                return true;
            }
            return false;
        }
    }
}
