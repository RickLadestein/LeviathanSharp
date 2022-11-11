﻿using Leviathan.Core;
using Leviathan.Core.Graphics;
using Leviathan.Core.Graphics.Buffers;
using Leviathan.Math;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Leviathan.ECS
{
    public class RenderComponent : Component
    {
        public override string FriendlyName => "RenderComponent";

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

            for(int i = 0; i < matcomp.Material.maps.Count; i++)
            {
                var m = matcomp.Material.maps[i];
                
                if (m == null || !Context.TextureManager.HasResource(Path.GetFileName(m.Item2.FilePath)))
                {
                    tex.textures[i] = Context.TextureManager.GetResource("default");
                } else
                {
                    string id = Path.GetFileName(m.Item2.FilePath);
                    Core.Graphics.Texture t = Context.TextureManager.GetResource(id);
                    tex.textures[i] = t;
                }
            }

            TextureBuffer.Instance.UseMultitex(tex);

            for (int i = 0; i < TextureBuffer.MAX_TEXTURES; i++)
            {
                sh.SetUniform($"texture_{i}", i);
            }

            VertexBuffer vbuf = meshcomp.Mesh.Vbuffer;
            vbuf.Bind();
            TextureBuffer.Instance.Bind();
            //sh.SetUniform("model", parent.Transform.ModelMat);
            sh.SetUniform("model", Parent.Transform.ModelMat);
            //sh.SetUniform("normal_mat", parent.Transform.NormalMat);
            sh.SetUniform("projection", target.ProjectionMatrix);
            sh.SetUniform("view", target.ViewMatrix);
            sh.SetUniform("time", (float)Context.GLFWContext.GetTime());

            Context.GLContext.DrawArrays((GLEnum)vbuf.prim_type, 0, vbuf.vertex_count);
            vbuf.Unbind();
            sh.Unbind();
            TextureBuffer.UnbindAll();
        }

        public override string ToString()
        {
            return "RenderComponent";
        }
    }
}
