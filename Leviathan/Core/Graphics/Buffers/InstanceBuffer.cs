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
        /// N = switch to next instance of data every Nnd draw instance
        /// </summary>
        public uint mode;


        public InstanceBuffer()
        {
        }

        public static InstanceBuffer FromAttribute(VertexBufferAttributes.Attribute attribute, uint mode)
        {
            InstanceBuffer ret = new InstanceBuffer
            {
                data = attribute.data.ToArray(),
                valuetype = attribute.valuetype,
                coll_type = attribute.coll_type,
                value_size = attribute.value_size,
                mode = mode
            };
            return ret;
        }
    }
}
