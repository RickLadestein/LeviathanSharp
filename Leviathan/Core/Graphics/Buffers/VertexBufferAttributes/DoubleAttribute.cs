using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        public DoubleAttribute(uint reserve = 0) : base(LAttributeDataType.DOUBLE, LDataCollectionType.SINGULAR)
        {
            attribute_data = new List<double>((int)reserve);
        }

        public DoubleAttribute(double[] data) : base(LAttributeDataType.DOUBLE, LDataCollectionType.SINGULAR)
        {
            attribute_data = new List<double>(data);
            this.SegmentCount = (uint)attribute_data.Count;
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttributeDescriptor descriptor = this.GenerateDescriptor();
            VertexAttribute attrib = new VertexAttribute(descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<double>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static DoubleAttribute FromVertexAttribute(VertexAttribute attribute)
        {
            if (!(attribute.Descriptor.value_type == LAttributeDataType.DOUBLE
                && attribute.Descriptor.collection_type == LDataCollectionType.SINGULAR))
            {
                throw new Exception("Could not convert to Attribute: incorrect value or collection type");
            }

            double[] data = new double[attribute.SegmentCount];
            unsafe
            {
                fixed (void* dest_raw_ptr = &data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(attribute.data, 0, dest_ptr, attribute.data.Length);
                }
            }
            DoubleAttribute output = new DoubleAttribute(data);
            return output;
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
        public Double2Attribute(uint reserve = 0) : base(LAttributeDataType.DOUBLE, LDataCollectionType.VEC2)
        {
            attribute_data = new List<Vector2d>((int)reserve);
        }

        public Double2Attribute(Vector2d[] data) : base(LAttributeDataType.DOUBLE, LDataCollectionType.VEC2)
        {
            attribute_data = new List<Vector2d>(data);
            this.SegmentCount = (uint)attribute_data.Count;
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttributeDescriptor descriptor = this.GenerateDescriptor();
            VertexAttribute attrib = new VertexAttribute(descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector2d>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static Double2Attribute FromVertexAttribute(VertexAttribute attribute)
        {
            if (!(attribute.Descriptor.value_type == LAttributeDataType.DOUBLE
                && attribute.Descriptor.collection_type == LDataCollectionType.VEC2))
            {
                throw new Exception("Could not convert to Attribute: incorrect value or collection type");
            }

            Vector2d[] data = new Vector2d[attribute.SegmentCount];
            unsafe
            {
                fixed (void* dest_raw_ptr = &data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(attribute.data, 0, dest_ptr, attribute.data.Length);
                }
            }
            Double2Attribute output = new Double2Attribute(data);
            return output;
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
        public Double3Attribute(uint reserve = 0) : base(LAttributeDataType.DOUBLE, LDataCollectionType.VEC3)
        {
            attribute_data = new List<Vector3d>((int)reserve);
        }

        public Double3Attribute(Vector3d[] data) : base(LAttributeDataType.DOUBLE, LDataCollectionType.VEC3)
        {
            attribute_data = new List<Vector3d>(data);
            this.SegmentCount = (uint)attribute_data.Count;
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttributeDescriptor descriptor = this.GenerateDescriptor();
            VertexAttribute attrib = new VertexAttribute(descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector3d>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static Double3Attribute FromVertexAttribute(VertexAttribute attribute)
        {
            if (!(attribute.Descriptor.value_type == LAttributeDataType.DOUBLE
                && attribute.Descriptor.collection_type == LDataCollectionType.VEC3))
            {
                throw new Exception("Could not convert to Attribute: incorrect value or collection type");
            }

            Vector3d[] data = new Vector3d[attribute.SegmentCount];
            unsafe
            {
                fixed (void* dest_raw_ptr = &data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(attribute.data, 0, dest_ptr, attribute.data.Length);
                }
            }
            Double3Attribute output = new Double3Attribute(data);
            return output;
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
        public Double4Attribute(uint reserve = 0) : base(LAttributeDataType.DOUBLE, LDataCollectionType.VEC4)
        {
            attribute_data = new List<Vector4d>((int)reserve);
        }

        public Double4Attribute(Vector4d[] data) : base(LAttributeDataType.DOUBLE, LDataCollectionType.VEC4)
        {
            attribute_data = new List<Vector4d>(data);
            this.SegmentCount = (uint)attribute_data.Count;
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttributeDescriptor descriptor = this.GenerateDescriptor();
            VertexAttribute attrib = new VertexAttribute(descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector4d>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static Double4Attribute FromVertexAttribute(VertexAttribute attribute)
        {
            if (!(attribute.Descriptor.value_type == LAttributeDataType.DOUBLE
                && attribute.Descriptor.collection_type == LDataCollectionType.VEC4))
            {
                throw new Exception("Could not convert to Attribute: incorrect value or collection type");
            }

            Vector4d[] data = new Vector4d[attribute.SegmentCount];
            unsafe
            {
                fixed (void* dest_raw_ptr = &data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(attribute.data, 0, dest_ptr, attribute.data.Length);
                }
            }
            Double4Attribute output = new Double4Attribute(data);
            return output;
        }
    }
}
