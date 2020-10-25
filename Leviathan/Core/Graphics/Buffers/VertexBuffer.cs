using Leviathan.Core.Data;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using Leviathan.Util;

namespace Leviathan.Core.Graphics.Buffers
{
    public class VertexBuffer
    {
        public Vao vao;
        private List<Vbo> v_buffers;
        public VertexBuffer()
        {
            this.vao = new Vao()
            {
                handle = GL.GenVertexArray(),
                segments = 0,
                current_attrib = 0
            };
            this.v_buffers = new List<Vbo>();
        }

        ~VertexBuffer()
        {
            this.Destroy();
        }

        public void BufferData(in List<Primitive> primitives)
        {
            if(this.vao.handle == -1)
            {
                this.vao = new Vao()
                {
                    handle = GL.GenVertexArray(),
                    segments = 0,
                    current_attrib = 0
                };
            }
            if(this.vao.segments != 0 && this.vao.segments != (primitives.Count * 3))
            {
                Logger.GetInstance().LogError("Could not buffer primitives: component size does not match with loaded components");
                return;
            }

            Vbo vbo = new Vbo()
            {
                handle = GL.GenBuffer(),
                segments = primitives.Count * 3,
                segment_size = Marshal.SizeOf<Primitive>()
            };

            this.vao.segments = primitives.Count * 3;
            GL.BindVertexArray(this.vao.handle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.handle);

            GL.BufferData(BufferTarget.ArrayBuffer, vbo.segment_size * primitives.Count, primitives.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexArrayAttrib(this.vao.handle, this.vao.current_attrib);
            GL.VertexAttribPointer(this.vao.current_attrib, 3, VertexAttribPointerType.Float, false, VertexData.GetSize(), 0);
            this.vao.current_attrib += 1;

            GL.EnableVertexArrayAttrib(this.vao.handle, this.vao.current_attrib);
            GL.VertexAttribPointer(this.vao.current_attrib, 3, VertexAttribPointerType.Float, false, VertexData.GetSize(), 3 * sizeof(float));
            this.vao.current_attrib += 1;

            GL.EnableVertexArrayAttrib(this.vao.handle, this.vao.current_attrib);
            GL.VertexAttribPointer(this.vao.current_attrib, 3, VertexAttribPointerType.Float, false, VertexData.GetSize(), 6 * sizeof(float));
            this.vao.current_attrib += 1;

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            this.v_buffers.Add(vbo);
        }

        public void Bind()
        {
            GL.BindVertexArray(this.vao.handle);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Destroy()
        {
            this.Unbind();
            for(int i = 0; i < v_buffers.Count; i++)
            {
                if(this.v_buffers[i].handle > 0)
                {
                    GL.DeleteBuffer(this.v_buffers[i].handle);
                }
            }
            this.v_buffers.Clear();

            if(this.vao.handle > 0)
            {
                GL.DeleteVertexArray(this.vao.handle);
                this.vao.handle = -1;
                this.vao.current_attrib = 0;
                this.vao.segments = 0;
            }
        }
    }

    public struct Vbo
    {
        public int handle;
        public int segments;
        public int segment_size;
    }

    public struct Vao
    {
        public int handle;
        public int segments;
        public int current_attrib;
    }
}
