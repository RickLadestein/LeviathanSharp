using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    /// <summary>
    /// Attribute class for storing int values
    /// </summary>
    public class IntAttribute : Attribute<int>
    {
        /// <summary>
        /// Instantiates a new instance of IntAttribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of int valuetype spaces that are reserved</param>
        public IntAttribute(uint reserve = 0)
        {
            attribute_data = new List<int>((int)reserve);
            descriptor = new VertexAttributeDescriptor(AttributeDataType.INT, DataCollectionType.SINGULAR);
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttribute attrib = new VertexAttribute(this.descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<int>(attribute_data.ToArray());
            return attrib;
        }

        public static DoubleAttribute FromVertexAttribute()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Attribute class for storing Vector2i values
    /// </summary>
    public class Int2Attribute : Attribute<Vector2i>
    {
        /// <summary>
        /// Instantiates a new instance of Int2Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector2i spaces that are reserved</param>
        public Int2Attribute(uint reserve = 0)
        {
            attribute_data = new List<Vector2i>((int)reserve);
            descriptor = new VertexAttributeDescriptor(AttributeDataType.INT, DataCollectionType.VEC2);
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttribute attrib = new VertexAttribute(this.descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector2i>(attribute_data.ToArray());
            return attrib;
        }

        public static DoubleAttribute FromVertexAttribute()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Attribute class for storing Vector3i values
    /// </summary>
    public class Int3Attribute : Attribute<Vector3i>
    {
        /// <summary>
        /// Instantiates a new instance of Int3Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector3i spaces that are reserved</param>
        public Int3Attribute(uint reserve = 0)
        {
            attribute_data = new List<Vector3i>((int)reserve);
            descriptor = new VertexAttributeDescriptor(AttributeDataType.INT, DataCollectionType.VEC3);
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttribute attrib = new VertexAttribute(this.descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector3i>(attribute_data.ToArray());
            return attrib;
        }

        public static DoubleAttribute FromVertexAttribute()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Attribute class for storing Vector4i values
    /// </summary>
    public class Int4Attribute : Attribute<Vector4i>
    {
        /// <summary>
        /// Instantiates a new instance of Int4Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector4i spaces that are reserved</param>
        public Int4Attribute(uint reserve = 0)
        {
            attribute_data = new List<Vector4i>((int)reserve);
            descriptor = new VertexAttributeDescriptor(AttributeDataType.INT, DataCollectionType.VEC4);
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttribute attrib = new VertexAttribute(this.descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector4i>(attribute_data.ToArray());
            return attrib;
        }

        public static DoubleAttribute FromVertexAttribute()
        {
            throw new NotImplementedException();
        }
    }
}
