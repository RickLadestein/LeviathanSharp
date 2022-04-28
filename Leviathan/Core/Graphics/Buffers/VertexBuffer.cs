using Leviathan.Core.Graphics.Buffers.VertexBufferAttributes;
using Leviathan.Math;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers
{
    public class VertexBuffer : IDisposable, IVBODataListener
    {
        public static uint Bound_vbuffer;
        public uint handle;
        public uint current_attrib;
        public ElementType prim_type;
        public uint vertex_count;

        public bool validation_needed;

        public IBO[] ibos;

        public List<VertexBufferObject> Vbuffers;


        public VertexBuffer()
        {
            Vbuffers = new List<VertexBufferObject>();
            validation_needed = false;

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

        public void LoadDataBuffers(AttributeCollection attrib_col, ElementType primitive)
        {
            this.prim_type = primitive;

            int vcount = 0;
            bool first = true;

            Bind();
            foreach (KeyValuePair<Leviathan.Core.Graphics.Buffers.VertexBufferAttributes.AttributeType, VertexAttribute> kp in attrib_col)
            {
                if (first)
                {
                    vcount = kp.Value.SegmentCount;
                    first = false;
                    this.vertex_count = (uint)vcount;
                }

                if (kp.Value.SegmentCount != vcount)
                {
                    throw new Exception("Attribute buffer misalignment detected: please ensure that each attribute buffer has the same attribute count");
                }


                VertexBufferObject vbo = VertexBufferObject.Create(kp.Value, current_attrib, this);
                vbo.Bind();
                unsafe
                {
                    Context.GLContext.VertexAttribPointer(current_attrib, (int)vbo.Descriptor.collection_type, (GLEnum)vbo.Descriptor.value_type, false, 0, (void*)0);
                    Context.GLContext.EnableVertexAttribArray(current_attrib);
                    current_attrib += 1;
                }

                VertexBufferObject.Unbind();
                Vbuffers.Add(vbo);
            }
            Unbind();
            return;
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

        public void Destroy()
        {
            if(this.handle != 0)
            {
                Context.GLContext.DeleteVertexArray(handle);
                foreach (VertexBufferObject obj in Vbuffers)
                {
                    obj.Dispose();
                }
                Vbuffers.Clear();
            }
        }

        public void Dispose()
        {
            Destroy();
        }

        public void OnVBODataChanged()
        {
            throw new NotImplementedException();
        }
    }

    public class VertexBufferObject : VertexAttribute, IDisposable
    {
        public uint Handle;
        public uint Attribute_index { get; set; }

        private IVBODataListener datalistener;

        private VertexBufferObject(VertexAttribute attrib, uint vao_index, IVBODataListener listener) : base(attrib.Descriptor, (uint)attrib.SegmentCount)
        {
            Handle = Context.GLContext.GenBuffer();
            VertexAttribute.CopyTo(attrib, this);
            Attribute_index = vao_index;
            this.datalistener = listener;
        }

        public static VertexBufferObject Create(VertexAttribute attrib, uint vao_index, IVBODataListener listener ,VertexBufferObjectUsageHint usagehint = VertexBufferObjectUsageHint.STATIC)
        {
            VertexBufferObject tmp = new VertexBufferObject(attrib, vao_index, listener);

            tmp.data = new byte[attrib.data.Length];
            Array.Copy(attrib.data, tmp.data, attrib.data.Length);
            
            //Guard against not complete data in the BufferedData array
            if(!tmp.RevalidateDescriptor())
            {
                throw new Exception("Buffered data size does not fit with segment byte size in the Descriptor");
            }

            tmp.WriteDataToGPU(usagehint);

            return tmp;
        }

        public void LoadNewData(VertexAttribute attrib)
        {
            this.Descriptor = attrib.Descriptor;
            if(attrib.data.Length != data.Length)
            {
                data = new byte[attrib.data.Length];
            }
            Array.Copy(data, attrib.data, attrib.data.Length);
            datalistener.OnVBODataChanged();
        }

        public void WriteDataToGPU(VertexBufferObjectUsageHint usagehint = VertexBufferObjectUsageHint.STATIC)
        {
            Bind();
            unsafe
            {
                fixed (void* d_ptr = &this.data[0])
                {
                    Context.GLContext.BufferData(GLEnum.ArrayBuffer, (uint)data.Length, d_ptr, (GLEnum)usagehint);
                }
            };
            Unbind();
        }

        /// <summary>
        /// Loads data from the GPU in the local DataBuffer
        /// </summary>
        private void RetrieveDataFromGPU()
        {
            if(this.data.Length != (SegmentCount * Descriptor.segment_byte_size))
            {
                this.data = new byte[SegmentCount * Descriptor.segment_byte_size];
            }

            Bind();
            unsafe
            {
                fixed (void* d_ptr = &this.data[0])
                {
                    Context.GLContext.GetBufferSubData(GLEnum.ArrayBuffer, 0, (uint)this.data.Length, d_ptr);
                }
            }
            Unbind();
        }

        /// <summary>
        /// Clears the local CPU sided data buffer
        /// </summary>
        public void PurgeLocalBuffer()
        {
            this.data = Array.Empty<byte>();
        }

        /// <summary>
        /// Binds this VertexBufferObject as the selected buffer
        /// </summary>
        public void Bind()
        {
            Context.GLContext.BindBuffer(GLEnum.ArrayBuffer, Handle);
        }

        /// <summary>
        /// Binds the default buffer as the selected buffer
        /// </summary>
        public static void Unbind()
        {
            Context.GLContext.BindBuffer(GLEnum.ArrayBuffer, 0);
        }

        /// <summary>
        /// Destroys the VertexBufferObject CPU&GPU sided and releases all resources contained in it
        /// </summary>
        private void Destroy()
        {
            if (this.Handle != 0)
            {
                Context.GLContext.DeleteBuffer(this.Handle);
                this.Handle = 0;
            }

        }

        public void Dispose()
        {
            Destroy();
        }
    }

    public enum VertexBufferObjectUsageHint
    {
        /// <summary>
        /// The data store contents will be modified once and used at most a few times.
        /// </summary>
        STREAM = GLEnum.StreamDraw,
        /// <summary>
        /// The data store contents will be modified once and used many times
        /// </summary>
        STATIC = GLEnum.StaticDraw,
        /// <summary>
        /// The data store contents will be modified repeatedly and used many times.
        /// </summary>
        DYNAMIC = GLEnum.DynamicDraw
    }

    public interface IVBODataListener
    {
        void OnVBODataChanged();
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
