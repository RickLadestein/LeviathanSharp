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

        public List<VertexBufferObject> Vbuffers;


        public VertexBuffer()
        {
            Vbuffers = new List<VertexBufferObject>();

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


                VertexBufferObject vbo = VertexBufferObject.Create(kp.Value);
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

        public void PurgeBuffer()
        {
            Context.GLContext.DeleteVertexArray(handle);
            this.handle = Context.GLContext.GenVertexArray();
            this.current_attrib = 0;
        }
    }

    public class VertexBufferObject : IDisposable
    {
        public uint handle;
        public byte[] BufferedData { get; private set; }
        public int Vertices { get; private set; }
        public VertexAttributeDescriptor Descriptor { get; private set; }
        
        public int attribute_index;

        private VertexBufferObjectListener listener;

        public static VertexBufferObject Create(VertexAttribute attrib, VertexBufferObjectUsageHint usagehint = VertexBufferObjectUsageHint.STATIC)
        {
            VertexBufferObject tmp = new VertexBufferObject()
            {
                handle = Context.GLContext.GenBuffer(),
                BufferedData = attrib.data.ToArray(),
                Descriptor = attrib.Descriptor
            };
            
            //Guard against not complete data in the BufferedData array
            if((tmp.BufferedData.Length % tmp.Descriptor.segment_byte_size) != 0)
            {
                throw new Exception("Buffered data size does not fit with segment byte size in the Descriptor");
            }
            tmp.Vertices = tmp.BufferedData.Length / tmp.Descriptor.segment_byte_size;

            tmp.WriteDataToGPU(tmp.BufferedData, usagehint);

            return tmp;
        }

        /// <summary>
        /// Loads byte data into the buffer located on the GPU
        /// </summary>
        /// <param name="data"></param>
        public void WriteDataToGPU(byte[] data, VertexBufferObjectUsageHint usagehint)
        {
            BufferedData = data;
            Bind();
            unsafe
            {
                fixed (void* d_ptr = &BufferedData[0])
                {
                    Context.GLContext.BufferData(GLEnum.ArrayBuffer, (uint)BufferedData.Length, d_ptr, (GLEnum)usagehint);
                    //Context.GLContext.BufferData(GLEnum.ArrayBuffer, (uint)BufferedData.Length, d_ptr, GLEnum.StaticDraw);
                }
            };
            Unbind();
        }

        /// <summary>
        /// Loads data from the GPU in the local DataBuffer
        /// </summary>
        public void RetrieveDataFromGPU()
        {
            this.BufferedData = new byte[Vertices * Descriptor.segment_byte_size];
            Bind();
            unsafe
            {
                fixed (void* d_ptr = &BufferedData[0])
                {
                    Context.GLContext.GetBufferSubData(GLEnum.ArrayBuffer, 0, (uint)BufferedData.Length, d_ptr);
                }
            }
        }

        /// <summary>
        /// Clears the local CPU sided data buffer
        /// </summary>
        public void PurgeLocalBuffer()
        {
            BufferedData = Array.Empty<byte>();
        }

        /// <summary>
        /// Binds this VertexBufferObject as the selected buffer
        /// </summary>
        public void Bind()
        {
            Context.GLContext.BindBuffer(GLEnum.ArrayBuffer, handle);
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
            if (this.handle != 0)
            {
                Context.GLContext.DeleteBuffer(this.handle);
                this.handle = 0;
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

    public interface VertexBufferObjectListener
    {
        void OnBufferDataChanged(VertexBufferObject obj);
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
