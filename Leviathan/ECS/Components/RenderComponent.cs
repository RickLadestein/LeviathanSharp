using Leviathan.Core;
using Leviathan.Core.Graphics;
using Leviathan.Core.Graphics.Buffers;
using Leviathan.Math;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.ECS
{
    public class RenderComponent : Component
    {

        protected override void AddDependencies()
        {
            base.AddDependencies();
            if(!Parent.HasComponent<MeshComponent>())
            {
                Console.WriteLine($"Entity[{Parent.Id}] was missing MeshComponent: adding default");
                Parent.AddComponent(new MeshComponent());
            }

            if(!Parent.HasComponent<MaterialComponent>())
            {
                Console.WriteLine($"Entity[{Parent.Id}] was missing MaterialComponent: adding default");
                Parent.AddComponent(new MaterialComponent());
            }
        }

        public override void Initialise()
        {
            base.Initialise();
            AddDependencies();
        }

        //public void Render(Camera target)
        //{
        //    if (!Parent.HasComponent<MeshComponent>())
        //    {
        //        throw new Exception("Tried to draw entity without a mesh on screen");
        //    }
        //
        //    MeshComponent meshcomp = Parent.GetComponent<MeshComponent>();
        //    MaterialComponent matcomp = Parent.GetComponent<MaterialComponent>();
        //    
        //    ShaderProgram sh = matcomp.Shader;
        //    sh.Bind();
        //
        //    MultiTexture tex = matcomp.Texture;
        //    TextureBuffer.Instance.UseMultitex(tex);
        //
        //    for(int i = 0; i < TextureBuffer.MAX_TEXTURES; i++)
        //    {
        //        sh.SetUniform($"texture_{i}", i);
        //    }
        //
        //    VertexBuffer vbuf = meshcomp.Vbuffer;
        //    vbuf.Bind();
        //
        //    //sh.SetUniform("model", parent.Transform.ModelMat);
        //    sh.SetUniform("model", Parent.Transform.ModelMat);
        //    //sh.SetUniform("normal_mat", parent.Transform.NormalMat);
        //    sh.SetUniform("projection", target.ProjectionMatrix);
        //    sh.SetUniform("view", target.ViewMatrix);
        //    sh.SetUniform("time", (float)Context.GLFWContext.GetTime());
        //
        //    Context.GLContext.DrawArrays((GLEnum)vbuf.prim_type, 0, vbuf.vertex_count);
        //    vbuf.Unbind();
        //    sh.Unbind();
        //    target.UpdateViewMatrix();
        //}

        public void Render(CameraComponent target)
        {
            if (!Parent.HasComponent<MeshComponent>())
            {
                throw new Exception("Tried to draw entity without a mesh on screen");
            }

            MeshComponent meshcomp = Parent.GetComponent<MeshComponent>();
            MaterialComponent matcomp = Parent.GetComponent<MaterialComponent>();

            ShaderProgram sh = matcomp.Shader;
            sh.Bind();

            MultiTexture tex = matcomp.Texture;
            TextureBuffer.Instance.UseMultitex(tex);

            for (int i = 0; i < TextureBuffer.MAX_TEXTURES; i++)
            {
                sh.SetUniform($"texture_{i}", i);
            }

            VertexBuffer vbuf = meshcomp.Vbuffer;
            vbuf.Bind();

            //sh.SetUniform("model", parent.Transform.ModelMat);
            sh.SetUniform("model", Parent.Transform.ModelMat);
            //sh.SetUniform("normal_mat", parent.Transform.NormalMat);
            sh.SetUniform("projection", target.ProjectionMatrix);
            sh.SetUniform("view", target.ViewMatrix);
            sh.SetUniform("time", (float)Context.GLFWContext.GetTime());

            Context.GLContext.DrawArrays((GLEnum)vbuf.prim_type, 0, vbuf.vertex_count);
            vbuf.Unbind();
            sh.Unbind();
        }

        //public void RenderInstanced(Camera target, uint instances)
        //{
        //    if (!Parent.HasComponent<MeshComponent>())
        //    {
        //        throw new Exception("Tried to draw entity without a mesh on screen");
        //    }
        //
        //    MeshComponent meshcomp = Parent.GetComponent<MeshComponent>();
        //    MaterialComponent matcomp = Parent.GetComponent<MaterialComponent>();
        //
        //    ShaderProgram sh = matcomp.Shader;
        //    sh.Bind();
        //
        //    VertexBuffer vbuf = meshcomp.Vbuffer;
        //    vbuf.Bind();
        //    sh.SetUniform("model", Parent.Transform.LocalModelMat);
        //    //sh.SetUniform("normal_mat", parent.Transform.NormalMat);
        //    sh.SetUniform("projection", target.ProjectionMatrix);
        //    sh.SetUniform("view", target.ViewMatrix);
        //    sh.SetUniform("time", (float)Context.GLFWContext.GetTime());
        //
        //    Context.GLContext.DrawArraysInstanced((GLEnum)vbuf.prim_type, 0, vbuf.vertex_count, instances);
        //    vbuf.Unbind();
        //    sh.Unbind();
        //    target.UpdateViewMatrix();
        //}

        public override string ToString()
        {
            return "RenderComponent";
        }
    }
}
