using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core
{
    public class ImageResource : IDisposable
    {
        /// <summary>
        /// An image
        /// </summary>
        public Image<Rgba32> image;

        /// <summary>
        /// The width and height of an image
        /// </summary>
        public uint width, height;

        /// <summary>
        /// The constructor of an image
        /// </summary>
        /// <param name="im">The given image</param>
        public ImageResource(SixLabors.ImageSharp.Image<Rgba32> im)
        {
            if (im == null)
            {
                throw new ArgumentException("Image cannot be null");
            }

            this.image = im;
            width = (uint)im.Width;
            height = (uint)im.Height;
        }

        public Silk.NET.OpenGL.PixelFormat GetPixelFormat()
        {
            return Silk.NET.OpenGL.PixelFormat.Rgba;
        }

        public Silk.NET.OpenGL.InternalFormat GetPixelInternalFormat()
        {
            return Silk.NET.OpenGL.InternalFormat.Rgba;
        }

        public Silk.NET.OpenGL.PixelType GetPixelType()
        {
            int bits_per_pixel = image.PixelType.BitsPerPixel;
            switch(bits_per_pixel)
            {
                case 16:
                    return Silk.NET.OpenGL.PixelType.UnsignedShort;
                case 32:
                    return Silk.NET.OpenGL.PixelType.Float;
                default:
                    return Silk.NET.OpenGL.PixelType.Byte;
            }
        }

        /// <summary>
        /// A function which loads an image
        /// </summary>
        /// <param name="path">The path of the image</param>
        /// <param name="flip">A boolean which contains whether the image should be flipped</param>
        /// <returns></returns>
        public static ImageResource Load(string path, bool flip)
        {
            try
            {
                SixLabors.ImageSharp.Image<Rgba32> im = (SixLabors.ImageSharp.Image<Rgba32>)SixLabors.ImageSharp.Image.Load(path);
                if (flip)
                {
                    im.Mutate(x => x.Flip(FlipMode.Vertical));
                }
                return new ImageResource(im);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// A function which diposes of an image
        /// </summary>
        public void Dispose()
        {
            if (image != null)
            {
                image.Dispose();
            }
        }
    }
}
