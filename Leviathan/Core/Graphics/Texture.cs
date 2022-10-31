using Leviathan.Core.Graphics.Buffers;
using Leviathan.Math;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Leviathan.Core.Graphics
{
    public enum LTextureType
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
    public abstract class Texture : GraphicsResource
    {
        public LTextureType Type { get; private set; }

        public TextureWrapSetting Wrap { get; private set; }

        public MinMagSetting Filter { get; private set; }

        public Silk.NET.OpenGL.PixelType PixelType { get; protected set; }
        public Silk.NET.OpenGL.PixelFormat PixelFormat { get; protected set; }

        public Vector3i Size { get; protected set; }
        public Silk.NET.OpenGL.InternalFormat InternalPixelFormat { get; protected set; }

        public static readonly Texture Zero = new Texture2D() { Handle = 0, Type = LTextureType.TEXTURE_2D };
        private Texture oldtex;

        public Texture(LTextureType _type)
        {
            this.Handle = Context.GLContext.GenTexture();
            this.Type = _type;
            this.Wrap = TextureWrapSetting.REPEAT;
            this.Filter = MinMagSetting.NEAREST;

        }

        protected void StartModification()
        {
            Context.GLContext.ActiveTexture(Silk.NET.OpenGL.TextureUnit.Texture0);
            oldtex = TextureBuffer.Instance[0];
            Context.GLContext.BindTexture((Silk.NET.OpenGL.GLEnum)this.Type, this.Handle);
        }

        protected void EndModification()
        {
            Context.GLContext.ActiveTexture(Silk.NET.OpenGL.TextureUnit.Texture0);
            TextureBuffer.Instance[0] = oldtex;
        }

        protected unsafe void LoadImageIntoTexture(ImageResource im)
        {
            Context.GLContext.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)im.width, (uint)im.height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
            im.image.ProcessPixelRows(accessor =>
            {
                //ImageSharp 2 does not store images in contiguous memory by default, so we must send the image row by row
                for (int y = 0; y < accessor.Height; y++)
                {
                    fixed (void* data = accessor.GetRowSpan(y))
                    {
                        //Loading the actual image.
                        Context.GLContext.TexSubImage2D(TextureTarget.Texture2D, 0, 0, y, (uint)accessor.Width, 1, PixelFormat.Rgba, PixelType.UnsignedByte, data);
                    }
                }
            });
            //fixed (void* data = &MemoryMarshal.GetReference(im.image.GetPixelRowSpan(0)))
            //{
            //    Context.GLContext.TexImage2D(
            //        (Silk.NET.OpenGL.GLEnum)this.Type, 
            //        0, 
            //        (int)InternalPixelFormat, 
            //        im.width, 
            //        im.height, 
            //        0, 
            //        PixelFormat, 
            //        PixelType, 
            //        data);
            //}
        }

        public void SetFilterMode(MinMagSetting filter)
        {
            StartModification();
            this.Filter = filter;
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureMinFilter, (uint)filter);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureMagFilter, (uint)filter);
            EndModification();
        }

        public void SetWrapMode(TextureWrapSetting wrap)
        {
            StartModification();
            this.Wrap = wrap;
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureWrapS, (uint)wrap);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureWrapT, (uint)wrap);
            EndModification();
        }

        public override void Dispose()
        {
            if(this.Handle != GraphicsResource.EMPTY_HANDLE)
            {
                Context.GLContext.DeleteTexture(this.Handle);
            }
        }
    }

    public class MultiTexture
    {
        public Texture[] textures;

        public MultiTexture()
        {
            textures = new Texture[TextureBuffer.MAX_TEXTURES];
            for(int i = 0; i < textures.Length; i++)
            {
                textures[i] = Texture2D.Zero;
            }
        }
    }

    public class Texture2D : Texture
    {
        public Texture2D() : base(LTextureType.TEXTURE_2D)
        {

        }

        public Texture2D(ImageResource im) : base(LTextureType.TEXTURE_2D)
        {
            if(im == null)
            {
                throw new ArgumentNullException(nameof(im));
            }

            this.PixelType = im.GetPixelType();
            this.InternalPixelFormat = im.GetPixelInternalFormat();
            this.PixelFormat = im.GetPixelFormat();
            this.Size = new Vector3i((int)im.width, (int)im.height, 1);

            StartModification();
            Context.GLContext.BindTexture(Silk.NET.OpenGL.GLEnum.Texture2D, this.Handle);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureWrapS, (uint)Wrap);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureWrapT, (uint)Wrap);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureMinFilter, (uint)Filter);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureMagFilter, (uint)Filter);
            //LoadImageIntoTexture(im);
            unsafe
            {
                fixed (void* data = &MemoryMarshal.GetReference(im.image.GetPixelRowSpan(0)))
                {
                    Context.GLContext.TexImage2D(
                        (Silk.NET.OpenGL.GLEnum)this.Type,
                        0,
                        (int)InternalFormat.Rgba,
                        (uint)im.width,
                        (uint)im.height,
                        0,
                        PixelFormat.Rgba,
                        PixelType.UnsignedByte,
                        data);
                }
            }

            EndModification();
        }

        public Texture2D(Vector2i size, Silk.NET.OpenGL.PixelFormat pformat, Silk.NET.OpenGL.InternalFormat iformat, Silk.NET.OpenGL.PixelType ptype) 
            : base(LTextureType.TEXTURE_2D)
        {
            this.PixelType = ptype;
            this.InternalPixelFormat = iformat;
            this.PixelFormat = pformat;

            StartModification();
            Context.GLContext.BindTexture(Silk.NET.OpenGL.GLEnum.Texture2D, this.Handle);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureWrapS, (uint)Wrap);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureWrapT, (uint)Wrap);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureMinFilter, (uint)Filter);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureMagFilter, (uint)Filter);

            unsafe
            {
                Context.GLContext.TexImage2D(
                    (Silk.NET.OpenGL.GLEnum)this.Type,
                    0,
                    (int)InternalPixelFormat,
                    (uint)size.X,
                    (uint)size.Y,
                    0,
                    PixelFormat,
                    PixelType,
                    null);
            }

            EndModification();
        }

        public static Texture2D ImportTexture(String path)
        {
            ImageResource image = ImageResource.Load(path, true);
            if(image == null)
            {
                Console.WriteLine($"Could not find: {path}");
                return null;
            } else
            {
                Texture2D output = new Texture2D(image);
                Console.WriteLine($"load complete");
                return output;
            }
        }
    }

    public class Texture3D : Texture
    {
        public Texture3D(Vector3i size, Silk.NET.OpenGL.PixelFormat pformat, Silk.NET.OpenGL.InternalFormat iformat, Silk.NET.OpenGL.PixelType ptype) 
            : base(LTextureType.TEXTURE_3D)
        {
            this.PixelType = ptype;
            this.InternalPixelFormat = iformat;
            this.PixelFormat = pformat;
            this.Size = size;
            StartModification();
            Context.GLContext.BindTexture(Silk.NET.OpenGL.GLEnum.Texture3D, this.Handle);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureWrapS, (uint) Wrap);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureWrapT, (uint) Wrap);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureMinFilter, (uint) Filter);
            Context.GLContext.TextureParameterI(this.Handle, Silk.NET.OpenGL.GLEnum.TextureMagFilter, (uint) Filter);

            unsafe
            {
                Context.GLContext.TexImage3D(
                    (Silk.NET.OpenGL.GLEnum)this.Type,
                    0,
                    (int) InternalPixelFormat,
                    (uint) size.X,
                    (uint) size.Y,
                    (uint) size.Z,
                    0,
                    PixelFormat,
                    PixelType,
                    null);
        }

        EndModification();
    }
}
}
