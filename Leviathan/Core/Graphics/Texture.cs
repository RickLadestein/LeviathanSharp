using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Leviathan.Core.Data;
using Leviathan.Util;
using OpenTK.Graphics.OpenGL;

namespace Leviathan.Core.Graphics
{
    public abstract class Texture
    {
        public TextureTarget Tex_Type { get; protected set; }

        public TextureUnit Tex_unit { get; set; }
        public int Handle { get; protected set; }
        public bool Bound { get; protected set; }

        public static int INVALID_TEXTURE_ID { get; private set; } = -1;
        public Texture(TextureTarget target)
        {
            this.Tex_Type = target;
            this.Handle = INVALID_TEXTURE_ID;
            this.Bound = false;
        }

        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            if(this.Handle == -1) { return; }
            this.Tex_unit = unit;
            GL.ActiveTexture(this.Tex_unit);
            GL.BindTexture(this.Tex_Type, this.Handle);
            GL.ActiveTexture(TextureUnit.Texture0);
            this.Bound = true;
        }

        public void Unbind()
        {
            GL.ActiveTexture(this.Tex_unit);
            GL.BindTexture(this.Tex_Type, 0);
            this.Tex_unit = TextureUnit.Texture0;
            this.Bound = false;
        }

        private static Dictionary<String, Texture> textures = new Dictionary<string, Texture>();
        private static Mutex tex_mutex = new Mutex();

        public static bool AddTextureToStorage(string folder_id, string file, bool mipmap, TextureType type, string identifier)
        {
            tex_mutex.WaitOne();
            try
            {
                if (textures.ContainsKey(identifier))
                {
                    return false;
                }
                Texture t = null;
                switch (type)
                {
                    case TextureType.TEXTURE_2D:
                        t = new Texture2D(folder_id, file, mipmap);
                        break;
                    default:
                        throw new NotImplementedException();
                        break;
                }
                if (t == null || t.Handle == Texture.INVALID_TEXTURE_ID)
                {
                    return false;
                }
                textures.Add(identifier, t);
                return true;
            }
            catch(Exception)
            {
                return false;
            } finally {
                tex_mutex.ReleaseMutex();
            }
        }

        public static Texture GetTextureFromStorage(string texture_identifier)
        {
            tex_mutex.WaitOne();
            try
            {
                if(textures.ContainsKey(texture_identifier))
                {
                    return textures[texture_identifier];
                }
                return null;
            } catch(Exception)
            {
                return null;
            } finally
            {
                tex_mutex.ReleaseMutex();
            }
        }
    }

    public class MultiTexture
    {
        private Dictionary<TextureUnit, Texture> textures;
        private Mutex tex_mutex;
        public bool Bound { get; private set; }

        /// <summary>
        /// Creates a new instance of multitexture
        /// </summary>
        public MultiTexture()
        {
            textures = new Dictionary<TextureUnit, Texture>();
            tex_mutex = new Mutex();
            this.Bound = false;
        }


        ~MultiTexture()
        {
            if(this.Bound)
            {
                this.Unbind();
            }
        }

        /// <summary>
        /// Adds the texture to the multitexture at given texture layer
        /// </summary>
        /// <param name="tex">The texture to be added</param>
        /// <param name="unit">The texture layer where the texture should be loaded in</param>
        /// <returns></returns>
        public bool SetTexture(Texture tex, TextureUnit unit)
        {
            if(tex == null || tex.Handle == Texture.INVALID_TEXTURE_ID)
            {
                Logger.GetInstance().LogError("Could not add texture to multitex: Texture was null or invalid");
                return false;
            }
            tex_mutex.WaitOne();
            try
            {
                if(textures.ContainsKey(unit))
                {
                    textures[unit] = tex;
                } else
                {
                    textures.Add(unit, tex);
                }
                return true;
            }catch(Exception)
            {
                return false;
            }finally
            {
                tex_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Removes texture layer from the multitexture
        /// </summary>
        /// <param name="unit">The texture layer to remove</param>
        /// <returns></returns>
        public bool RemoveTexture(TextureUnit unit)
        {
            tex_mutex.WaitOne();
            try
            {
                if (textures.ContainsKey(unit))
                {
                    Texture t = textures[unit];
                    if(t.Bound) { t.Unbind(); }
                    textures.Remove(unit);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                tex_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Binds the multitexture textures in the gpu
        /// </summary>
        public void Bind()
        {
            if (!this.Bound)
            {
                tex_mutex.WaitOne();
                foreach (KeyValuePair<TextureUnit, Texture> t in textures)
                {
                    t.Value.Tex_unit = t.Key;
                    t.Value.Bind();
                }
                this.Bound = true;
                tex_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Unbinds the multitexture textures
        /// </summary>
        public void Unbind()
        {
            if (this.Bound)
            {
                tex_mutex.WaitOne();
                foreach (KeyValuePair<TextureUnit, Texture> t in textures)
                {
                    t.Value.Unbind();
                }
                this.Bound = false;
                tex_mutex.ReleaseMutex();
            } else
            {
                Logger.GetInstance().LogWarning("Tried to unbind not bound texture");
            }
        }
    }

    public class Texture2D : Texture
    {
        public Texture2D(string folder_id, string file, bool mipmap) : base(TextureTarget.Texture2D)
        {
            FileManager fm = FileManager.GetInstance();
            string path = fm.CombineFilepath(folder_id, file);
            Image image = Image.Load(path);
            
            if(image == null)
            {
                return;
            }
            this.Handle = GL.GenTexture();
            this.Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Pixel_data.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            this.Unbind();
        }
    }

    public enum TextureType
    {
        TEXTURE_1D,
        TEXTURE_2D,
        TEXTURE_3D
    }
}
