using Leviathan.Core;
using Leviathan.Core.Graphics;
using Leviathan.Core.Graphics.Buffers;
using Leviathan.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.ECS
{
    public class MaterialComponent : Component
    {
        public override string FriendlyName => "MaterialComponent";
        public ShaderProgram Shader { get; private set; }

        public MultiTexture Texture { get; private set; }

        public Material Material { get; private set; }

        public MaterialComponent()
        {
            Shader = null;
            Material = Material.Default;
            Texture = new MultiTexture();
            SetShader("default");
        }

        public void SetShader(string shader_id)
        {
            ShaderProgram sp = Context.Shadermanager.GetResource(shader_id);

            if (sp == null)
            {
                throw new Exception($"ShaderProgram resource was not found for identifier: {shader_id}");
            } else
            {
                SetShader(sp);
            }
        }

        public void SetShader(ShaderProgram prg) 
        {
            if (prg == null)
            {
                throw new ArgumentNullException(nameof(prg));
            }
            else
            {
                this.Shader = prg;
            }
        }

        public void SetShaderById(string shaderId)
        {
            ShaderProgram prog = Context.Shadermanager.GetResource(shaderId);
            if(prog == null)
            {
                throw new Exception($"Cannot locate shader resource named: {shaderId}");
            } else
            {
                this.Shader = prog;
            }
        }

        public void SetMaterial(string materialId)
        {
            Material mat = Context.MaterialManager.GetResource(materialId);
            if(mat == null)
            {
                throw new Exception($"Cannot locate material resource named: {materialId}");
            } else
            {
                this.Material = mat;
            }
        }

        public void SetTextureUnit(String tex_id, int index)
        {
            Texture tex = Context.TextureManager.GetResource(tex_id);
            if(tex == null)
            {
                throw new Exception($"Texture resource was not found for identifier: {tex_id}");
            } else
            {
                this.SetTextureUnit(tex, index);
            }
        }

        public void SetTextureUnit(Texture tex, int index)
        {
            if(tex == null)
            {
                throw new ArgumentNullException(nameof(tex));
            } else
            {
                Texture.textures[index] = tex;
            }
        }

        public override string ToString()
        {
            return "MaterialComponent";
        }
    }
}
