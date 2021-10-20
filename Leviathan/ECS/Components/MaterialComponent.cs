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
        public ShaderProgram Shader { get; private set; }

        public MultiTexture Texture { get; private set; }

        public MaterialComponent()
        {
            Shader = null;
            Texture = new MultiTexture();
            SetShader("default");
        }

        public void SetShader(string shader_id)
        {
            ShaderProgram sp = ShaderResourceManager.Instance.GetResource(shader_id);
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

        public void SetTextureUnit(String tex_id, int index)
        {
            Texture tex = TextureResourceManager.Instance.GetResource(tex_id);
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
