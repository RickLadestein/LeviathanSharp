using Leviathan.Core.Graphics;
using Leviathan.Util;
using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.ECS.Components.Physix
{
    class TextureComponent : Component
    {
        public MultiTexture Texture { get; private set; }

        TextureComponent()
        {
            this.Texture = new MultiTexture();
        }
        TextureComponent(params String[] texture_ids)
        {
            this.Texture = new MultiTexture();
            int index = 0;
            foreach(String s in texture_ids)
            {
                if(s == null || s == String.Empty)
                {
                    Logger.GetInstance().LogWarning("Tried to add empty texture: skipping");
                    continue;
                }
                TextureUnit unit = (TextureUnit)((int)TextureUnit.Texture0 + index);
                Texture tex;
                index++;
            }
        }
    }
}
