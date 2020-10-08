using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Core.Data;
using Leviathan.Util;
using OpenTK.Graphics.OpenGL;

namespace Leviathan.Core.Graphics
{
	public abstract class Texture
    {
        public TextureTarget tex_type { get; protected set; }
        public TextureUnit tex_unit { get; protected set; }
        public int handle { get; protected set; }
        public Texture(TextureTarget target)
        {
            this.tex_type = target;
            this.handle = -1;
        }

        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            if(this.tex_unit != TextureUnit.Texture0)
            {
                this.Unbind();
            }
            this.tex_unit = unit;
            GL.ActiveTexture(tex_unit);
            GL.BindTexture(this.tex_type, this.handle);
        }

        public void Unbind()
        {
            GL.ActiveTexture(this.tex_unit);
            GL.BindTexture(this.tex_type, 0);
            this.tex_unit = TextureUnit.Texture0;
        }
    }

    public class Texture2D : Texture
    {
        public Texture2D(string folder_id, string file, bool mipmap) : base(TextureTarget.Texture2D)
        {
            FileManager fm = FileManager.GetInstance();
            string path = fm.CombineFilepath(fm.GetDirectoryPath(folder_id), file);
            Image image = Image.Load(path);
            
            if(image == null)
            {
                return;
            }
            this.handle = GL.GenTexture();
            this.Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Pixel_data.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            this.Unbind();
        }
    }
}
