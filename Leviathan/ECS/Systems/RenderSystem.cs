using Leviathan.Core;
using Leviathan.Core.Graphics.Buffers;
using Leviathan.Core.Graphics;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.ECS.Systems
{
    public class RenderSystem : ECSystem
    {
        public override SystemPriority Priority => SystemPriority.RENDER;
        public override string FriendlyName => "RenderSystem";


        public RenderSystem()
        {
            this.AddRequirementType(typeof(MeshComponent));
            this.AddRequirementType(typeof(MaterialComponent));
        }

        protected override void SystemFunc()
        {
            Entity[] found = Core.World.Current.QueryEntityByComponent<RenderComponent>();
            foreach(Entity en in found)
            {
                //en.GetComponent<RenderComponent>().Render(Core.World.Current.PrimaryCam);
                //TODO: Add support for different cams
                Render(en, World.Current.PrimaryCam);
            }
        }

        private void Render(Entity Parent, CameraComponent target)
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

            for (int i = 0; i < matcomp.Material.maps.Count; i++)
            {
                var m = matcomp.Material.maps[i];

                if (m == null || !Context.TextureManager.HasResource(Path.GetFileName(m.Item2.FilePath)))
                {
                    tex.textures[i] = Context.TextureManager.GetResource("default");
                }
                else
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
    }
}
