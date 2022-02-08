using Leviathan.Core.Graphics.Buffers.VertexBufferAttributes;
using Leviathan.Math;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers
{
    public class VertexBuffer
    {
        public static uint Bound_vbuffer;
        public uint handle;
        public uint current_attrib;
        public ElementType prim_type;
        public uint vertex_count;

        public IBO[] ibos;

        public VertexBuffer()
        {
            handle = Context.GLContext.GenVertexArray();
            current_attrib = 0;
            ibos = new IBO[0];
        }

        public void Bind()
        {
            if(Bound_vbuffer != handle)
            {
                Context.GLContext.BindVertexArray(handle);
                Bound_vbuffer = handle;
            }
        }

        public void Unbind()
        {
            if (Bound_vbuffer == handle)
            {
                Context.GLContext.BindVertexArray(0);
                Bound_vbuffer = 0;
            } else
            {
                throw new Exception("Attempted to unbind not currently bound Vertex Buffer");
            }
        }

        public void LoadDataBuffers(Mesh mesh)
        {
            Context.GLContext.BindVertexArray(handle);
            this.prim_type = mesh.PrimitiveType;
            this.vertex_count = mesh.VertexCount;
            int index = 0;
            foreach(VBO vbo in mesh.vbos)
            {
                unsafe
                {
                    Context.GLContext.BindBuffer(GLEnum.ArrayBuffer, vbo.handle);
                    Context.GLContext.VertexAttribPointer(current_attrib, (int)vbo.coll_type, (GLEnum)vbo.value_type, false, 0, (void*)0);
                    Context.GLContext.EnableVertexAttribArray(current_attrib);
                }
                this.current_attrib += 1;
                index++;
            }
            Context.GLContext.BindBuffer(GLEnum.ArrayBuffer, 0);
            Context.GLContext.BindVertexArray(0);
        }

        public void LoadInstanceBuffers(InstanceBuffer[] ibufs)
        {
            Context.GLContext.BindVertexArray(handle);
            ibos = new IBO[ibufs.Length];
            int index = 0;
            foreach (InstanceBuffer ib in ibufs)
            {
                IBO tmp = IBO.Create(ib, current_attrib);
                
                this.current_attrib += 1;
                ibos[index] = tmp;
                index++;
            }
        }

        public void PurgeBuffer()
        {
            Context.GLContext.DeleteVertexArray(handle);
            this.handle = Context.GLContext.GenVertexArray();
            this.current_attrib = 0;
        }
    }

    

    public class IBO : IDisposable
    {
        public uint handle;
        public uint mode;
        public uint vao_index;
        public uint value_size;
        public VertexBufferAttributes.AttributeDataType value_type;
        public DataCollectionType coll_type;

        public static IBO Create(InstanceBuffer ibuf, uint current_attrib)
        {
            IBO tmp = new IBO()
            {
                handle = Context.GLContext.GenBuffer(),
                vao_index = current_attrib,
                value_type = ibuf.valuetype,
                coll_type = ibuf.coll_type,
                value_size = ibuf.value_size
            };


            Context.GLContext.BindBuffer(GLEnum.ArrayBuffer, tmp.handle);
            unsafe
            {
                fixed (void* d_ptr = &ibuf.data[0])
                {
                    Context.GLContext.BufferData(GLEnum.ArrayBuffer, (uint)ibuf.data.Length, d_ptr, GLEnum.StaticDraw);
                }

                Context.GLContext.VertexAttribPointer(tmp.vao_index, (int)tmp.coll_type, (GLEnum)tmp.value_type, false, 0, (void*)0);
                Context.GLContext.EnableVertexAttribArray(tmp.vao_index);
                Context.GLContext.VertexAttribDivisor(current_attrib, ibuf.mode);
            };
            Context.GLContext.BindBuffer(GLEnum.ArrayBuffer, 0);
            return tmp;
        }

        private void Destroy()
        {
            Context.GLContext.DeleteBuffer(this.handle);
            this.handle = 0;
        }

        public void Dispose()
        {
            Destroy();
        }
    }
}
