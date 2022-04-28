using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    /// <summary>
    /// Attribute class for storing float values
    /// </summary>
    public class FloatAttribute : Attribute<float>
    {
        /// <summary>
        /// Instantiates a new instance of FloatAttribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of float valuetype spaces that are reserved</param>
        public FloatAttribute(uint reserve = 0){
            attribute_data = new List<float>((int)reserve);
            descriptor = new VertexAttributeDescriptor(AttributeDataType.FLOAT, DataCollectionType.SINGULAR);
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttribute attrib = new VertexAttribute(this.descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<float>(attribute_data.ToArray());
            return attrib;
        }

        public static FloatAttribute FromVertexAttribute()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Attribute class for storing Vector2f values
    /// </summary>
    public class Float2Attribute : Attribute<Vector2f>
    {
        /// <summary>
        /// Instantiates a new instance of Float2Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector2f spaces that are reserved</param>
        public Float2Attribute(uint reserve = 0)
        {
            attribute_data = new List<Vector2f>((int)reserve);
            descriptor = new VertexAttributeDescriptor(AttributeDataType.FLOAT, DataCollectionType.VEC2);
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttribute attrib = new VertexAttribute(this.descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector2f>(attribute_data.ToArray());
            return attrib;
        }

        public static Float4Attribute FromVertexAttribute()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Attribute class for storing Vector3f values
    /// </summary>
    public class Float3Attribute : Attribute<Vector3f>
    {
        /// <summary>
        /// Instantiates a new instance of Float3Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector3f spaces that are reserved</param>
        public Float3Attribute(uint reserve = 0)
        {
            attribute_data = new List<Vector3f>((int)reserve);
            descriptor = new VertexAttributeDescriptor(AttributeDataType.FLOAT, DataCollectionType.VEC3);
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttribute attrib = new VertexAttribute(this.descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector3f>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static Float4Attribute FromVertexAttribute()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Attribute class for storing Vector4f values
    /// </summary>
    public class Float4Attribute : Attribute<Vector4f>
    {
        /// <summary>
        /// Instantiates a new instance of Float4Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector4f spaces that are reserved</param>
        public Float4Attribute(uint reserve = 0)
        {
            attribute_data = new List<Vector4f>((int)reserve);
            descriptor = new VertexAttributeDescriptor(AttributeDataType.FLOAT, DataCollectionType.VEC4);
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttribute attrib = new VertexAttribute(this.descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector4f>(attribute_data.ToArray());
            return attrib;
        }

        public static Float4Attribute FromVertexAttribute()
        {
            throw new NotImplementedException();
        }
    }
}
