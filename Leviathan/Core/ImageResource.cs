using Leviathan.Math;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Leviathan.Core.Graphics.Buffers.VertexBufferAttributes;
using Leviathan.Util.util.Collections;
using System.Runtime.CompilerServices;

namespace Leviathan.Core
{
    public class ImageResource : IDisposable
    {
        /// <summary>
        /// An image
        /// </summary>
        public Image<Rgba32> image;

        public byte[] image_data;

        /// <summary>
        /// The width and height of an image
        /// </summary>
        public uint width, height;

        /// <summary>
        /// The constructor of an image
        /// </summary>
        /// <param name="im">The given image</param>
        public ImageResource(Image<Rgba32> im)
        {
            if (im == null)
            {
                throw new ArgumentException("Image cannot be null");
            }

            this.image = im;
            width = (uint)im.Width;
            height = (uint)im.Height;
        }

        ~ImageResource()
        {
            this.Dispose();
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
                case 64:
                    return Silk.NET.OpenGL.PixelType.UnsignedShort;
                case 128:
                    return Silk.NET.OpenGL.PixelType.UnsignedInt;
                default:
                    return Silk.NET.OpenGL.PixelType.UnsignedByte;
            }
        }

        /// <summary>
        /// A function which loads an image
        /// </summary>
        /// <param name="path">The path of the image</param>
        /// <param name="flip">A boolean which indicates whether the image should be flipped</param>
        /// <returns></returns>
        public static ImageResource Load(string path, bool flip)
        {
            try
            {
                var im = Image.Load<Rgba32>(path);
                if (flip)
                {
                    im.Mutate(x => x.Flip(FlipMode.Vertical));
                }
                ImageResource output = new ImageResource(im);
                //output.image_data = ImportRows(im);
                return output;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public static async ImageResource LoadAsync(string path, bool flip)
        //{
        //    try
        //    {
        //        var im = await Image.LoadAsync<Rgba32>(path);
        //        if (flip)
        //        {
        //            im.Mutate(x => x.Flip(FlipMode.Vertical));
        //        }
        //        
        //        ImageResource output = new ImageResource(im);
        //        //output.image_data = ImportRows(im);
        //        return output;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        private static unsafe byte[] ImportRows(Image<Rgba32> img)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ValueCollection<byte> image_data = new ValueCollection<byte>(img.Width * img.Height * 4);
            img.ProcessPixelRows(accessor =>
            {
                //ImageSharp 2 does not store images in contiguous memory by default, so we must send the image row by row
                for (int y = 0; y < accessor.Height; y++)
                {
                    Rgba32[] rowspan = accessor.GetRowSpan(y).ToArray();
                    image_data.AddRangeFromTypedBuffer<Rgba32>(rowspan);
                }
            });
            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"Loading took: {elapsedMs} milliseconds");
            return image_data.ToByteArray();
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
