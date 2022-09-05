using Leviathan.Core.Graphics.Buffers.VertexBufferAttributes;
using Leviathan.Math;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers
{
    public class VertexBuffer : GraphicsResource, IDisposable
    {
        public uint DataHandle { get; private set; }

        public LPrimitiveType prim_type;
        public uint vertex_count;

        public bool validation_needed;

        public IBO[] ibos;

        public List<VertexBufferObject> Vbuffers;

        private static uint Bound_vbuffer;

        public VertexBuffer()
        {
            Vbuffers = new List<VertexBufferObject>();
            validation_needed = false;

            Handle = Context.GLContext.GenVertexArray();
            DataHandle = Context.GLContext.GenBuffer();
        }

        public void Bind()
        {
            if(Bound_vbuffer != Handle)
            {
                Context.GLContext.BindVertexArray(Handle);
                Bound_vbuffer = Handle;
            }
        }

        public void Unbind()
        {
            if (Bound_vbuffer == Handle)
            {
                Context.GLContext.BindVertexArray(0);
                Bound_vbuffer = 0;
            } else
            {
                throw new Exception("Attempted to unbind not currently bound Vertex Buffer");
            }
        }

        public void PurgeLocalVBOS()
        {
            foreach(VertexBufferObject vbo in this.Vbuffers)
            {
                vbo.PurgeLocalBuffer();
            }
        }

        public new void Dispose()
        {
            if (this.Handle != 0)
            {
                Context.GLContext.DeleteVertexArray(Handle);
                foreach (VertexBufferObject obj in Vbuffers)
                {
                    obj.Dispose();
                }
                Vbuffers.Clear();
            }

            if (this.DataHandle != 0)
            {
                Context.GLContext.DeleteBuffer(this.DataHandle);
            }
        }
    }

    public class VertexBufferObject : VertexAttribute, IDisposable
    {
        public uint Attribute_index { get; private set; }

        public uint Data_Offset { get; private set; }

        private VertexBufferObject(VertexAttribute attrib, uint vao_index, uint byte_offset) : base(attrib.Descriptor, (uint)attrib.SegmentCount)
        {
            VertexAttribute.CopyTo(attrib, this);
            Attribute_index = vao_index;
            Data_Offset = byte_offset;
        }

        public static VertexBufferObject Create(VertexAttribute attrib, uint vao_index, uint byte_offset)
        {
            VertexBufferObject tmp = new VertexBufferObject(attrib, vao_index, byte_offset);
            
            //Guard against not complete data in the BufferedData array
            if(!tmp.RevalidateDescriptor())
            {
                throw new Exception("Buffered data size does not fit with segment byte size in the Descriptor");
            }
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
            //datalistener.OnVBODataChanged();
        }

        public void WriteDataToGPU(VertexBufferObjectUsageHint usagehint = VertexBufferObjectUsageHint.STATIC)
        {
            //Bind();
            unsafe
            {
                fixed (void* d_ptr = &this.data[0])
                {
                    Context.GLContext.BufferData(GLEnum.ArrayBuffer, (uint)data.Length, d_ptr, (GLEnum)usagehint);
                }
            };
            //Unbind();
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

            //Bind();
            unsafe
            {
                fixed (void* d_ptr = &this.data[0])
                {
                    Context.GLContext.GetBufferSubData(GLEnum.ArrayBuffer, 0, (uint)this.data.Length, d_ptr);
                }
            }
            //Unbind();
        }

        /// <summary>
        /// Clears the local CPU sided data buffer
        /// </summary>
        public void PurgeLocalBuffer()
        {
            this.data = Array.Empty<byte>();
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

    public class IBO : IDisposable
    {
        public uint handle;
        public uint mode;
        public uint vao_index;
        public uint value_size;
        public VertexBufferAttributes.LAttributeDataType value_type;
        public LDataCollectionType coll_type;

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


    public class VertexBufferFactory
    {
        private VertexBuffer outputbuffer;
        private LPrimitiveType ptype;
        private VertexBufferObjectUsageHint usage;
        private List<KeyValuePair<LAttributeType, VertexAttribute>> buildlist;
        private List<uint> byte_offsets;
        private uint total_byte_size;


        public VertexBufferFactory()
        {
            ptype = LPrimitiveType.POINTS;
            usage = VertexBufferObjectUsageHint.STATIC;
            byte_offsets = new List<uint>();
            buildlist = new List<KeyValuePair<LAttributeType, VertexAttribute>>();
            total_byte_size = 0;
        }
        public VertexBuffer Build()
        {
            outputbuffer = new VertexBuffer
            {
                prim_type = ptype
            };

            int vcount = 0;
            bool first = true;
            uint current_attrib = 0;
            outputbuffer.Bind();
           
            Context.GLContext.BindBuffer(GLEnum.ArrayBuffer, outputbuffer.DataHandle);
            
            uint current_byte_offset = 0;
            byte[] total_data = new byte[total_byte_size];
            for(int i = 0; i < buildlist.Count; i++)
            {
                VertexAttribute kp = buildlist[i].Value;
                Array.Copy(kp.data, 0, total_data, current_byte_offset, kp.data.Length);
                current_byte_offset += byte_offsets[i];
            }

            unsafe
            {
                fixed (void* d_ptr = &total_data[0])
                {
                    Context.GLContext.BufferData(GLEnum.ArrayBuffer, (uint)total_data.Length, d_ptr, (GLEnum)usage);
                }
            };


            int index = 0;
            current_byte_offset = 0;
            foreach (KeyValuePair<LAttributeType, VertexAttribute> kp in buildlist)
            {
                if (first)
                {
                    vcount = kp.Value.SegmentCount;
                    first = false;
                    outputbuffer.vertex_count = (uint)vcount;
                }

                if (kp.Value.SegmentCount != vcount)
                {
                    throw new Exception("Attribute buffer misalignment detected: please ensure that each attribute buffer has the same attribute count");
                }

                VertexBufferObject vbo = VertexBufferObject.Create(kp.Value, current_attrib, byte_offsets[index]);
                unsafe
                {
                    Context.GLContext.VertexAttribPointer(current_attrib, (int)vbo.Descriptor.collection_type, (GLEnum)vbo.Descriptor.value_type, false, 0, (void*)current_byte_offset);
                    Context.GLContext.EnableVertexAttribArray(current_attrib);
                }
                current_byte_offset += byte_offsets[index];
                current_attrib += 1;
                index += 1;
            }

            Context.GLContext.BindBuffer(GLEnum.ArrayBuffer, 0);
            outputbuffer.Unbind();
            outputbuffer.PurgeLocalVBOS();
            return outputbuffer;
        }

        public VertexBufferFactory SetPrimitiveType(LPrimitiveType _elementType)
        {
            this.ptype = _elementType;
            return this;
        }

        public VertexBufferFactory SetUsageHint(VertexBufferObjectUsageHint usagehint)
        {
            this.usage = usagehint;
            return this;
        }

        public VertexBufferFactory AddVertexAttribute(VertexAttribute attrib, LAttributeType type)
        {
            //Guard against misaligned buffers
            if(buildlist.Count != 0)
            {
                if(buildlist[0].Value.SegmentCount != attrib.SegmentCount)
                {
                    throw new Exception("Buffer segment count misaligned with the segment count that was required by the first added buffer");
                }
            }

            //Guard against double assigned data
            foreach(KeyValuePair<LAttributeType, VertexAttribute> kvp in buildlist)
            {
                if(kvp.Key == type)
                {
                    throw new Exception("VertexAttribute type is already contained in the factory");
                }
            }
            uint offset = (uint)(attrib.SegmentCount * attrib.Descriptor.segment_byte_size);
            total_byte_size += offset;
            byte_offsets.Add(offset);
            buildlist.Add(new KeyValuePair<LAttributeType, VertexAttribute>(type, attrib));
            return this;
        }

        public VertexBufferFactory AddIndexBuffer()
        {
            throw new NotImplementedException();
        }
    }

    
}
