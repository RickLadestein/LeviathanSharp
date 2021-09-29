using Leviathan.Core.Graphics;
using Leviathan.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.ECS
{
    public class MaterialComponent : Component
    {
        public ShaderProgram Shader { get; private set; }

        public MaterialComponent()
        {
            Shader = null;
        }

        public void SetShader(string shader_id)
        {
            ShaderProgram sp = ShaderResourceManager.Instance.GetResource(shader_id);
            if(sp == null)
            {
                throw new Exception($"ShaderProgram resource was not found for identifier: {shader_id}");
            } else
            {
                this.Shader = sp;
            }
        }

        public override string ToString()
        {
            return "MaterialComponent";
        }
    }
}
