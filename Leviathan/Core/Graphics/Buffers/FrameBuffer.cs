using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers
{
    public class FrameBuffer : IDisposable
    {
        private static FrameBuffer bound;
        public DepthBuffer DepthBuff { get; private set; }
        public MultiTexture RenderTextures { get; private set; }
        public uint Handle { get; private set; }
        public Vector2i BufferSize { get; private set; }

        private RenderBuffer rbuffer;

        private FrameBuffer()
        {
            Handle = Context.gl_context.GenFramebuffer();
        }

        public void Bind()
        {
            Context.gl_context.BindFramebuffer(Silk.NET.OpenGL.FramebufferTarget.Framebuffer, this.Handle);
            bound = this;
        }

        public static void UnbindAll()
        {
            Context.gl_context.BindFramebuffer(Silk.NET.OpenGL.FramebufferTarget.Framebuffer, 0);
            bound = null;
        }

        public void CopyDepthBufferToMainBuffer()
        {
            Context.gl_context.BlitFramebuffer(0, 0, BufferSize.X, BufferSize.Y, 0, 0, BufferSize.X, BufferSize.Y, Silk.NET.OpenGL.ClearBufferMask.DepthBufferBit, Silk.NET.OpenGL.BlitFramebufferFilter.Nearest);
        }

        public void Destroy()
        {
            if(this.Handle != 0)
            {
                rbuffer.Destroy();
                Context.gl_context.DeleteFramebuffer(this.Handle);
                this.Handle = 0;
            }
        }

        public static FrameBuffer Create(FrameBufferType type, Vector2i size)
        {
            FrameBuffer fb = new FrameBuffer();
            fb.Bind();
            fb.BufferSize = size;

            switch(type)
            {
                case FrameBufferType.DEFERRED:
                    CreateDeferredBuffer(size);
                    break;
                case FrameBufferType.FORWARD:
                    CreateForwardBuffer(size);
                    break;
            }
            FrameBuffer.UnbindAll();
            return fb;
        }

        public String GetFrameBufferStatus()
        {
            this.Bind();
            string output = string.Empty;
            Silk.NET.OpenGL.GLEnum status = Context.gl_context.CheckFramebufferStatus(Silk.NET.OpenGL.FramebufferTarget.Framebuffer);
            if (status != Silk.NET.OpenGL.GLEnum.FramebufferComplete)
            {
                output = $"Framebuffer is not complete: {status}";
            }
            FrameBuffer.UnbindAll();
            return output;
        }

        private static FrameBuffer CreateDeferredBuffer(Vector2i size)
        {
            //Texture2D albedoBuffer = new Texture2D(size, Silk.NET.OpenGL.PixelFormat.Rgba, Silk.NET.OpenGL.InternalFormat.Rgba, Silk.NET.OpenGL.PixelType.UnsignedByte);
            //albedoBuffer.SetFilterMode(MinMagSetting.NEAREST);
            //albedoBuffer.SetWrapMode(TextureWrapSetting.BORDER_CLAMP);
            //Context.gl_context.FramebufferTexture2D(Silk.NET.OpenGL.FramebufferTarget.Framebuffer,
            //    Silk.NET.OpenGL.FramebufferAttachment.ColorAttachment0,
            //    Silk.NET.OpenGL.GLEnum.Texture2D,
            //    albedoBuffer.Handle,
            //    0);
            //
            //Texture2D normal_specularBuffer = new Texture2D(size, Silk.NET.OpenGL.PixelFormat.Rgba, Silk.NET.OpenGL.InternalFormat.Rgba, Silk.NET.OpenGL.PixelType.UnsignedByte);
            //normal_specularBuffer.SetFilterMode(MinMagSetting.NEAREST);
            //normal_specularBuffer.SetWrapMode(TextureWrapSetting.BORDER_CLAMP);
            //Context.gl_context.FramebufferTexture2D(Silk.NET.OpenGL.FramebufferTarget.Framebuffer,
            //    Silk.NET.OpenGL.FramebufferAttachment.ColorAttachment1,
            //    Silk.NET.OpenGL.GLEnum.Texture2D,
            //    normal_specularBuffer.Handle,
            //    0);
            //
            //Texture2D positionBuffer = new Texture2D(size, Silk.NET.OpenGL.PixelFormat.Rgb, Silk.NET.OpenGL.InternalFormat.Rgb, Silk.NET.OpenGL.PixelType.UnsignedByte);
            //positionBuffer.SetFilterMode(MinMagSetting.NEAREST);
            //positionBuffer.SetWrapMode(TextureWrapSetting.BORDER_CLAMP);
            //Context.gl_context.FramebufferTexture2D(Silk.NET.OpenGL.FramebufferTarget.Framebuffer,
            //    Silk.NET.OpenGL.FramebufferAttachment.ColorAttachment2,
            //    Silk.NET.OpenGL.GLEnum.Texture2D,
            //    positionBuffer.Handle,
            //    0);

            FrameBuffer fb = new FrameBuffer();
            fb.Bind();
            fb.BufferSize = size;

            Texture3D deferredBuffer = new Texture3D(new Vector3i(size.X, size.Y, 3), 
                Silk.NET.OpenGL.PixelFormat.Rgba, 
                Silk.NET.OpenGL.InternalFormat.Rgba, 
                Silk.NET.OpenGL.PixelType.UnsignedByte);
            deferredBuffer.SetFilterMode(MinMagSetting.NEAREST);
            deferredBuffer.SetWrapMode(TextureWrapSetting.BORDER_CLAMP);
            
            //Albedo buffer
            Context.gl_context.FramebufferTexture3D(Silk.NET.OpenGL.FramebufferTarget.Framebuffer,
                Silk.NET.OpenGL.FramebufferAttachment.ColorAttachment0,
                Silk.NET.OpenGL.GLEnum.Texture3D,
                deferredBuffer.Handle,
                0,
                0);

            //Normal/Specular buffer
            Context.gl_context.FramebufferTexture3D(Silk.NET.OpenGL.FramebufferTarget.Framebuffer,
                Silk.NET.OpenGL.FramebufferAttachment.ColorAttachment1,
                Silk.NET.OpenGL.GLEnum.Texture3D,
                deferredBuffer.Handle,
                0,
                1);

            //Position buffer
            Context.gl_context.FramebufferTexture3D(Silk.NET.OpenGL.FramebufferTarget.Framebuffer,
                Silk.NET.OpenGL.FramebufferAttachment.ColorAttachment2,
                Silk.NET.OpenGL.GLEnum.Texture3D,
                deferredBuffer.Handle,
                0,
                2);

            fb.rbuffer = new RenderBuffer(size, Silk.NET.OpenGL.FramebufferAttachment.DepthAttachment, Silk.NET.OpenGL.InternalFormat.DepthComponent);
            FrameBuffer.UnbindAll();
            return fb;


        }

        private static FrameBuffer CreateForwardBuffer(Vector2i size)
        {
            FrameBuffer fb = new FrameBuffer();
            fb.Bind();
            fb.BufferSize = size;
            Texture2D albedoBuffer = new Texture2D(size, Silk.NET.OpenGL.PixelFormat.Rgb, Silk.NET.OpenGL.InternalFormat.Rgb, Silk.NET.OpenGL.PixelType.UnsignedByte);
            albedoBuffer.SetFilterMode(MinMagSetting.NEAREST);
            albedoBuffer.SetWrapMode(TextureWrapSetting.BORDER_CLAMP);
            Context.gl_context.FramebufferTexture2D(Silk.NET.OpenGL.FramebufferTarget.Framebuffer,
                Silk.NET.OpenGL.FramebufferAttachment.ColorAttachment0,
                Silk.NET.OpenGL.GLEnum.Texture2D,
                albedoBuffer.Handle,
                0);

            fb.RenderTextures.textures[0] = albedoBuffer;
            fb.rbuffer = new RenderBuffer(size, Silk.NET.OpenGL.FramebufferAttachment.DepthAttachment, Silk.NET.OpenGL.InternalFormat.DepthComponent);
            FrameBuffer.UnbindAll();
            return fb;
        }

        public void Dispose()
        {
            this.Destroy();
        }
    }

    public class RenderBuffer : IDisposable
    {
        public uint Handle { get; private set; }

        public static RenderBuffer Bound { get; private set; }

        public RenderBuffer(Vector2i buff_size, Silk.NET.OpenGL.FramebufferAttachment fAttachment, Silk.NET.OpenGL.InternalFormat iFormat)
        {
            this.Handle = Context.gl_context.GenRenderbuffer();
            Context.gl_context.RenderbufferStorage(Silk.NET.OpenGL.RenderbufferTarget.Renderbuffer, 
                iFormat, 
                (uint)buff_size.X, 
                (uint)buff_size.Y);
            
            Context.gl_context.FramebufferRenderbuffer(Silk.NET.OpenGL.FramebufferTarget.Framebuffer,
                fAttachment,
                Silk.NET.OpenGL.RenderbufferTarget.Renderbuffer,
                Handle);
        }

        public void Bind()
        {
            if(Handle != 0)
            {
                Context.gl_context.BindRenderbuffer(Silk.NET.OpenGL.GLEnum.Renderbuffer, Handle);
                Bound = this;
            } else
            {
                throw new Exception("Attempted to bind Invalid RenderBuffer");
            }
            
        }

        public static void UnbindAll()
        {
            Context.gl_context.BindRenderbuffer(Silk.NET.OpenGL.GLEnum.Renderbuffer, 0);
            Bound = null;
        }

        public void Destroy()
        {
            if(Handle != 0)
            {
                if(Bound == this)
                {

                }
                Context.gl_context.DeleteRenderbuffer(Handle);
                Handle = 0;
            }
        }

        public void Dispose()
        {
            
        }
    }

    public enum FrameBufferType
    {
        DEFERRED,
        FORWARD
    }
}
