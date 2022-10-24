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
            tmp.RevalidateDescriptor();
            return tmp;
        }

        public void LoadDataFromVertexAttribute(VertexAttribute attrib)
        {
            if (this.Descriptor.Equals(attrib.Descriptor))
            {
                throw new Exception("Could not load Attribute data, descriptor does not match");
            }

            if (attrib.data.Length != data.Length)
            {
                throw new Exception("Cannot modify the size of the buffer, only modify buffer contents");
            }

            Array.Copy(attrib.data, data, attrib.data.Length);
        }

        public void WriteDataToGPU(VertexBufferObjectUsageHint usagehint = VertexBufferObjectUsageHint.STATIC)
        {
            unsafe
            {
                fixed (void* d_ptr = &this.data[0])
                {
                    Context.GLContext.BufferSubData(GLEnum.ArrayBuffer, (int)this.Data_Offset, (uint)data.Length, d_ptr);
                }
            };
        }

        /// <summary>
        /// Loads data from the GPU in the local DataBuffer
        /// </summary>
        private void RetrieveDataFromGPU()
        {
            int total_buffer_size = (SegmentCount * Descriptor.segment_byte_size);
            if (this.data.Length != total_buffer_size)
            {
                this.data = new byte[total_buffer_size];
            }

            unsafe
            {
                fixed (void* d_ptr = &data[0])
                {
                    Context.GLContext.GetBufferSubData(GLEnum.ArrayBuffer, (int)Data_Offset, (uint)data.Length, d_ptr);
                }
            }
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
            this.SetPrimitiveType(this.ptype);
            outputbuffer = new VertexBuffer
            {
                prim_type = ptype
            };

            
            outputbuffer.Bind();
           
            Context.GLContext.BindBuffer(GLEnum.ArrayBuffer, outputbuffer.DataHandle);
            
            //Merge all seperate data containers into one buffer
            uint current_byte_offset = 0;
            byte[] total_data = new byte[total_byte_size];
            for(int i = 0; i < buildlist.Count; i++)
            {
                VertexAttribute kp = buildlist[i].Value;
                Array.Copy(kp.data, 0, total_data, current_byte_offset, kp.data.Length);
                current_byte_offset += byte_offsets[i];
            }

            //Copy all the data to the buffer on the GPU
            unsafe
            {
                fixed (void* d_ptr = &total_data[0])
                {
                    Context.GLContext.BufferData(GLEnum.ArrayBuffer, (uint)total_data.Length, d_ptr, (GLEnum)usage);
                }
            };

            int vcount = 0;
            uint current_attrib = 0;
            bool first = true;
            current_byte_offset = 0;
            foreach (KeyValuePair<LAttributeType, VertexAttribute> kp in buildlist)
            {
                if (first)
                {
                    vcount = kp.Value.SegmentCount;
                    first = false;
                    outputbuffer.vertex_count = (uint)vcount;
                }

                VertexBufferObject vbo = VertexBufferObject.Create(kp.Value, current_attrib, byte_offsets[(int)current_attrib]);
                unsafe
                {
                    Context.GLContext.VertexAttribPointer(current_attrib, (int)vbo.Descriptor.collection_type, (GLEnum)vbo.Descriptor.value_type, false, 0, (void*)current_byte_offset);
                    Context.GLContext.EnableVertexAttribArray(current_attrib);
                }
                current_byte_offset += byte_offsets[(int)current_attrib];
                current_attrib += 1;
            }

            Context.GLContext.BindBuffer(GLEnum.ArrayBuffer, 0);
            outputbuffer.Unbind();
            outputbuffer.PurgeLocalVBOS();
            return outputbuffer;
        }

        public VertexBufferFactory SetPrimitiveType(LPrimitiveType _elementType)
        {
            this.ptype = _elementType;
            switch(_elementType)
            {
                case LPrimitiveType.LINES:
                    if(this.total_byte_size % 2 != 0)
                    {
                        throw new Exception($"Cannot apply LPrimitiveType:Lines, buffer element count is not divisible by line element count:2");
                    }
                    break;
                case LPrimitiveType.TRIANGLES:
                    if (this.total_byte_size % 3 != 0)
                    {
                        throw new Exception($"Cannot apply LPrimitiveType:Triangles, buffer element count is not divisible by triangle element count:3");
                    }
                    break;
            }
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
            if(buildlist.FindIndex(new Predicate<KeyValuePair<LAttributeType, VertexAttribute>>((e) => { return e.Key == type; })) != -1)
            {
                throw new Exception("VertexAttribute type is already contained in the factory");
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
