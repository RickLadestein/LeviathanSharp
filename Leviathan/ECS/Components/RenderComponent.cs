using Leviathan.Core.Graphics;
using Leviathan.Core.Graphics.Buffers;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.ECS
{
    public class RenderComponent : Component
    {
        public void Render(Camera target)
        {
            if (!parent.HasComponent<MeshComponent>())
            {
                throw new Exception("Tried to draw entity without a mesh on screen");
            }

            MeshComponent meshcomp = parent.GetComponent<MeshComponent>();
            MaterialComponent matcomp = parent.GetComponent<MaterialComponent>();
            
            ShaderProgram sh = matcomp.Shader;
            sh.Bind();

            VertexBuffer vbuf = meshcomp.Vbuffer;
            vbuf.Bind();
            sh.SetUniform("model", parent.Transform.ModelMat);
            //sh.SetUniform("normal_mat", parent.Transform.NormalMat);
            sh.SetUniform("projection", target.ProjectionMatrix);
            sh.SetUniform("view", target.ViewMatrix);
            sh.SetUniform("time", (float)Context.glfw_context.GetTime());

            Context.gl_context.DrawArrays((GLEnum)vbuf.prim_type, 0, vbuf.vertex_count);
            vbuf.Unbind();
            sh.Unbind();
            target.UpdateViewMatrix();
        }

        public override string ToString()
        {
            return "RenderComponent";
        }
    }
}
