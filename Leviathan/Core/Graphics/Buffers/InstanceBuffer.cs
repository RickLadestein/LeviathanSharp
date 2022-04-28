using Leviathan.Core.Graphics.Buffers.VertexBufferAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers
{
    public class InstanceBuffer
    {
        public AttributeDataType valuetype;
        public DataCollectionType coll_type;
        public uint value_size;

        public byte[] data;


        /// <summary>
        /// Indicates to OpenGL how this instancebuffer should be used
        /// 0 = switch to next instance of data every vertex
        /// 1 = switch to next instance of data every new draw instance
        /// 2 = switch to next instance of data every 2nd draw instance
        /// N = switch to next instance of data every Nth draw instance
        /// </summary>
        public uint mode;


        public InstanceBuffer()
        {
        }

        public static InstanceBuffer FromAttribute(VertexBufferAttributes.VertexAttribute attribute, uint mode)
        {
            InstanceBuffer ret = new InstanceBuffer
            {
                data = attribute.data,
                valuetype = attribute.Descriptor.value_type,
                coll_type = attribute.Descriptor.collection_type,
                value_size = (uint)attribute.Descriptor.value_byte_size,
                mode = mode
            };
            return ret;
        }
    }
}
