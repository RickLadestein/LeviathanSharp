using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers
{
    public class TextureBuffer
    {
        public const uint MAX_TEXTURES = 10;
        private Texture[] textures;
        private bool[] changemap;

        private static TextureBuffer _instance;
        public static TextureBuffer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TextureBuffer();
                }
                return _instance;
            }
        }

        private TextureBuffer()
        {
            textures = new Texture[MAX_TEXTURES];
            changemap = new bool[MAX_TEXTURES];
            for (int i = 0; i < MAX_TEXTURES; i++)
            {
                textures[i] = Texture.Zero;
                changemap[i] = false;
            }
        }

        public Texture this[int index] {
            get
            {
                if(index > (MAX_TEXTURES -1) || index < 0)
                {
                    throw new IndexOutOfRangeException();
                } else
                {
                    return textures[index];
                }
            }
            set
            {
                if (index > (MAX_TEXTURES - 1) || index < 0)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    if(value == null)
                    {
                        throw new ArgumentNullException("Texture cannot be null");
                    }

                    if(value != Texture.Zero && value.Handle == 0)
                    {
                        throw new ArgumentException("Texture handle cannot be zero except for Texture.Zero");
                    }

                    textures[index] = value;
                    changemap[index] = true;
                }
            }
        }

        public void UseMultitex(MultiTexture mtex)
        {
            for(int i = 0; i < MAX_TEXTURES; i++)
            {
                uint h1 = textures[i].Handle;
                uint h2 = mtex.textures[i].Handle;

                if(h1 != h2)
                {
                    textures[i] = mtex.textures[i];
                    changemap[i] = true;
                }
            }
        }

        public void Bind()
        {
            for(int i = 0; i < MAX_TEXTURES; i++)
            {
                if(changemap[i])
                {
                    Context.gl_context.ActiveTexture(Silk.NET.OpenGL.TextureUnit.Texture0 + i);
                    Context.gl_context.BindTexture((Silk.NET.OpenGL.GLEnum)textures[i].Type, textures[i].Handle);
                    changemap[i] = false;
                }
            }
        }

        public static void UnbindAll()
        {
            for(int i = 0; i < MAX_TEXTURES; i++)
            {
                Context.gl_context.ActiveTexture(Silk.NET.OpenGL.TextureUnit.Texture0 + i);
                Context.gl_context.BindTexture(Silk.NET.OpenGL.TextureTarget.Texture2D, 0);
            }
        }
    }
}
