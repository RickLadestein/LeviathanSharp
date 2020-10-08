using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.ES11;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Leviathan.Util
{
    class Image
    {
        public List<byte> Pixel_data { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        private Image(in List<byte> pixels, int width, int height)
        {
            this.Pixel_data = new List<byte>(pixels);
            this.Width = width;
            this.Height = height;
        }

        public static Image Load(string path)
        {
            List<byte> pixels = new List<byte>();
            Leviathan.Util.Image image = null;

            try
            {
                Image<Rgba32> im = Image<Rgba32>.Load<Rgba32>(path);
                im.Mutate(x => x.Flip(FlipMode.Vertical));

                for (int y = 0; y < im.Height; y++)
                {
                    for (int x = 0; x < im.Width; x++)
                    {
                        Rgba32 pixel = im[x, y];
                        pixels.Add(pixel.R);
                        pixels.Add(pixel.G);
                        pixels.Add(pixel.B);
                        pixels.Add(pixel.A);
                    }
                }
                image = new Image(pixels, im.Width, im.Height);
            } catch(Exception e)
            {
                Logger.GetInstance().LogError(e.Message);
            }
            return image;
        }

    }
}
