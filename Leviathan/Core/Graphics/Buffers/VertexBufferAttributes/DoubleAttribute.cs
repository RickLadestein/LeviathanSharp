using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    /// <summary>
    /// Attribute class for storing Double values
    /// </summary>
    public class DoubleAttribute : Attribute<double>
    {
        /// <summary>
        /// Instantiates a new instance of DoubleAttribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Double valuetype spaces that are reserved</param>
        public DoubleAttribute(uint reserve = 0)
        {
            attribute_data = new List<double>((int)reserve);
            descriptor = new VertexAttributeDescriptor(AttributeDataType.DOUBLE, DataCollectionType.SINGULAR);
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttribute attrib = new VertexAttribute(this.descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<double>(attribute_data.ToArray());
            return attrib;
        }

        public static DoubleAttribute FromVertexAttribute()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Attribute class for storing Vector2d values
    /// </summary>
    public class Double2Attribute : Attribute<Vector2d>
    {
        /// <summary>
        /// Instantiates a new instance of Double2Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector2d spaces that are reserved</param>
        public Double2Attribute(uint reserve = 0)
        {
            attribute_data = new List<Vector2d>((int)reserve);
            descriptor = new VertexAttributeDescriptor(AttributeDataType.DOUBLE, DataCollectionType.VEC2);
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttribute attrib = new VertexAttribute(this.descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector2d>(attribute_data.ToArray());
            return attrib;
        }

        public static Double4Attribute FromVertexAttribute()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Attribute class for storing Vector3d values
    /// </summary>
    public class Double3Attribute : Attribute<Vector3d>
    {
        /// <summary>
        /// Instantiates a new instance of Double3Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector3d spaces that are reserved</param>
        public Double3Attribute(uint reserve = 0)
        {
            attribute_data = new List<Vector3d>((int)reserve);
            descriptor = new VertexAttributeDescriptor(AttributeDataType.DOUBLE, DataCollectionType.VEC3);
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttribute attrib = new VertexAttribute(this.descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector3d>(attribute_data.ToArray());
            return attrib;
        }

        public static Double4Attribute FromVertexAttribute()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Attribute class for storing Vector4d values
    /// </summary>
    public class Double4Attribute : Attribute<Vector4d>
    {
        /// <summary>
        /// Instantiates a new instance of Double4Attribute with specified segments reserved
        /// </summary>
        /// <param name="reserve">The amount of Vector4d spaces that are reserved</param>
        public Double4Attribute(uint reserve = 0)
        {
            attribute_data = new List<Vector4d>((int)reserve);
            descriptor = new VertexAttributeDescriptor(AttributeDataType.DOUBLE, DataCollectionType.VEC4);
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttribute attrib = new VertexAttribute(this.descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector4d>(attribute_data.ToArray());
            return attrib;
        }

        public static Double4Attribute FromVertexAttribute()
        {
            throw new NotImplementedException();
        }
    }
}
