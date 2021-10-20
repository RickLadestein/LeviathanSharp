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

        public void CopyToMainBuffer()
        {

        }

        public void Destroy()
        {
            if(this.Handle != 0)
            {
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

        private static void CreateDeferredBuffer(Vector2i size)
        {
            Texture2D colorBuffer = new Texture2D(size, Silk.NET.OpenGL.PixelFormat.Rgba, Silk.NET.OpenGL.InternalFormat.Rgba, Silk.NET.OpenGL.PixelType.UnsignedByte);
            colorBuffer.SetFilterMode(MinMagSetting.NEAREST);
            colorBuffer.SetWrapMode(TextureWrapSetting.BORDER_CLAMP);
            Context.gl_context.FramebufferTexture2D(Silk.NET.OpenGL.FramebufferTarget.Framebuffer,
                Silk.NET.OpenGL.FramebufferAttachment.ColorAttachment0,
                Silk.NET.OpenGL.GLEnum.Texture2D,
                colorBuffer.Handle,
                0);

            Texture2D normalBuffer = new Texture2D(size, Silk.NET.OpenGL.PixelFormat.Rgb, Silk.NET.OpenGL.InternalFormat.Rgb, Silk.NET.OpenGL.PixelType.UnsignedByte);
            normalBuffer.SetFilterMode(MinMagSetting.NEAREST);
            normalBuffer.SetWrapMode(TextureWrapSetting.BORDER_CLAMP);
            Context.gl_context.FramebufferTexture2D(Silk.NET.OpenGL.FramebufferTarget.Framebuffer,
                Silk.NET.OpenGL.FramebufferAttachment.ColorAttachment1,
                Silk.NET.OpenGL.GLEnum.Texture2D,
                normalBuffer.Handle,
                0);




        }

        private static void CreateForwardBuffer(Vector2i size)
        {

        }

        public void Dispose()
        {
            this.Destroy();
        }
    }

    public enum FrameBufferType
    {
        DEFERRED,
        FORWARD
    }
}
