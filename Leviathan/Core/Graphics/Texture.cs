﻿using Leviathan.Core.Graphics.Buffers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Leviathan.Core.Graphics
{
    public enum TextureType
    {
        TEXTURE_1D = Silk.NET.OpenGL.TextureTarget.Texture1D,
        TEXTURE_2D = Silk.NET.OpenGL.TextureTarget.Texture2D,
        TEXTURE_3D = Silk.NET.OpenGL.TextureTarget.Texture3D,
        TEXTURE_CUBEMAP = Silk.NET.OpenGL.TextureTarget.TextureCubeMap
    }

    /// <summary>
	/// Enum specifying the texture wrapping settings
	/// </summary>
	public enum TextureWrapSetting
    {
        /// <summary>
        /// Use repeat for outside mode of bounds texture mapping
        /// </summary>
        REPEAT = Silk.NET.OpenGL.GLEnum.Repeat,

        /// <summary>
        /// Use mirrored repeat mode for outside of bounds texture mapping
        /// </summary>
        MIRRORED_REPEAT = Silk.NET.OpenGL.GLEnum.MirroredRepeat,

        /// <summary>
        /// Use edge clamp mode for outside of bounds texture mapping
        /// </summary>
        EDGE_CLAMP = Silk.NET.OpenGL.GLEnum.ClampToEdge,

        /// <summary>
        /// Use border clamp mode for outside of bounds texture mapping
        /// </summary>
        BORDER_CLAMP = Silk.NET.OpenGL.GLEnum.ClampToBorder
    };

    /// <summary>
    /// Enum specifying the mini/magnification setting
    /// </summary>
    public enum MinMagSetting
    {
        /// <summary>
        /// Use nearest neighbour pixel filtering for mini/magnification 
        /// </summary>
        NEAREST = Silk.NET.OpenGL.GLEnum.Nearest,

        /// <summary>
        /// Use linear pixel filtering for mini/magnification 
        /// </summary>
        LINEAR = Silk.NET.OpenGL.GLEnum.Linear
    };
    public abstract class Texture
    {
        public uint Handle { get; private set; }
        public TextureType Type { get; private set; }

        public TextureWrapSetting Wrap { get; private set; }

        public MinMagSetting Filter { get; private set; }

        public static Texture Zero = new Texture2D() { Handle = 0, Type = TextureType.TEXTURE_2D };
        private Texture oldtex;

        public Texture(TextureType _type)
        {
            this.Handle = Context.gl_context.GenTexture();
            this.Type = _type;
            this.Wrap = TextureWrapSetting.REPEAT;
            this.Filter = MinMagSetting.NEAREST;

        }

        protected void StartModification()
        {
            Context.gl_context.ActiveTexture(Silk.NET.OpenGL.TextureUnit.Texture0);
            oldtex = TextureBuffer.Instance[0];
            Context.gl_context.BindTexture((Silk.NET.OpenGL.GLEnum)this.Type, this.Handle);
        }

        protected void EndModification()
        {
            Context.gl_context.ActiveTexture(Silk.NET.OpenGL.TextureUnit.Texture0);
            TextureBuffer.Instance[0] = oldtex;
        }

        protected unsafe void LoadImageIntoTexture(ImageResource im)
        {
            fixed (void* data = &MemoryMarshal.GetReference(im.image.GetPixelRowSpan(0)))
            {
                Context.gl_context.TexImage2D(
                    (Silk.NET.OpenGL.GLEnum)this.Type, 
                    0, 
                    (int)im.GetPixelInternalFormat(), 
                    im.width, 
                    im.height, 
                    0, 
                    im.GetPixelFormat(), 
                    im.GetPixelType(), 
                    data);
            }
        }

        public void SetFilterMode(MinMagSetting filter)
        {
            StartModification();
            this.Filter = filter;
            Context.gl_context.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureMinFilter, (uint)filter);
            Context.gl_context.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureMagFilter, (uint)filter);
            EndModification();
        }

        public void SetWrapMode(TextureWrapSetting wrap)
        {
            StartModification();
            this.Wrap = wrap;
            Context.gl_context.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureWrapS, (uint)wrap);
            Context.gl_context.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureWrapT, (uint)wrap);
            EndModification();
        }
    }

    public class MultiTexture
    {
        public Texture[] textures;

        public MultiTexture()
        {
            textures = new Texture[TextureBuffer.MAX_TEXTURES];
        }
    }

    public class Texture2D : Texture
    {
        public Texture2D() : base(TextureType.TEXTURE_2D)
        {

        }

        public Texture2D(ImageResource im) : base(TextureType.TEXTURE_2D)
        {
            if(im == null)
            {
                throw new ArgumentNullException(nameof(im));
            }

            StartModification();
            Context.gl_context.BindTexture(Silk.NET.OpenGL.GLEnum.Texture2D, this.Handle);
            Context.gl_context.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureWrapS, (uint)Wrap);
            Context.gl_context.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureWrapT, (uint)Wrap);
            Context.gl_context.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureMinFilter, (uint)Filter);
            Context.gl_context.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureMagFilter, (uint)Filter);
            LoadImageIntoTexture(im);

            EndModification();
        }

        public static Texture2D ImportTexture(String path)
        {
            ImageResource image = ImageResource.Load(path, true);
            Texture2D output = new Texture2D(image);
            return output;
        }
    }
}
