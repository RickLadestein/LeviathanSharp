using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        public IntAttribute(uint reserve = 0) : base(LAttributeDataType.INT, LDataCollectionType.SINGULAR)
        {
            attribute_data = new List<int>((int)reserve);
        }

        public IntAttribute(int[] data) : base(LAttributeDataType.INT, LDataCollectionType.SINGULAR)
        {
            attribute_data = new List<int>(data);
            this.SegmentCount = (uint)attribute_data.Count;
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttributeDescriptor descriptor = this.GenerateDescriptor();
            VertexAttribute attrib = new VertexAttribute(descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<int>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static IntAttribute FromVertexAttribute(VertexAttribute attribute)
        {
            if (!(attribute.Descriptor.value_type == LAttributeDataType.INT
                && attribute.Descriptor.collection_type == LDataCollectionType.SINGULAR))
            {
                throw new Exception("Could not convert to Attribute: incorrect value or collection type");
            }

            int[] data = new int[attribute.SegmentCount];
            unsafe
            {
                fixed (void* dest_raw_ptr = &data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(attribute.data, 0, dest_ptr, attribute.data.Length);
                }
            }
            IntAttribute output = new IntAttribute(data);
            return output;
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
        public Int2Attribute(uint reserve = 0) : base(LAttributeDataType.INT, LDataCollectionType.VEC2)
        {
            attribute_data = new List<Vector2i>((int)reserve);
        }

        public Int2Attribute(Vector2i[] data) : base(LAttributeDataType.INT, LDataCollectionType.VEC2)
        {
            attribute_data = new List<Vector2i>(data);
            this.SegmentCount = (uint)attribute_data.Count;
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttributeDescriptor descriptor = this.GenerateDescriptor();
            VertexAttribute attrib = new VertexAttribute(descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector2i>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static Int2Attribute FromVertexAttribute(VertexAttribute attribute)
        {
            if (!(attribute.Descriptor.value_type == LAttributeDataType.INT
                && attribute.Descriptor.collection_type == LDataCollectionType.VEC2))
            {
                throw new Exception("Could not convert to Attribute: incorrect value or collection type");
            }

            Vector2i[] data = new Vector2i[attribute.SegmentCount];
            unsafe
            {
                fixed (void* dest_raw_ptr = &data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(attribute.data, 0, dest_ptr, attribute.data.Length);
                }
            }
            Int2Attribute output = new Int2Attribute(data);
            return output;
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
        public Int3Attribute(uint reserve = 0) : base(LAttributeDataType.INT, LDataCollectionType.VEC3)
        {
            attribute_data = new List<Vector3i>((int)reserve);
        }

        public Int3Attribute(Vector3i[] data) : base(LAttributeDataType.INT, LDataCollectionType.VEC3)
        {
            attribute_data = new List<Vector3i>(data);
            this.SegmentCount = (uint)attribute_data.Count;
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttributeDescriptor descriptor = this.GenerateDescriptor();
            VertexAttribute attrib = new VertexAttribute(descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector3i>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static Int3Attribute FromVertexAttribute(VertexAttribute attribute)
        {
            if (!(attribute.Descriptor.value_type == LAttributeDataType.INT
                && attribute.Descriptor.collection_type == LDataCollectionType.VEC3))
            {
                throw new Exception("Could not convert to Attribute: incorrect value or collection type");
            }

            Vector3i[] data = new Vector3i[attribute.SegmentCount];
            unsafe
            {
                fixed (void* dest_raw_ptr = &data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(attribute.data, 0, dest_ptr, attribute.data.Length);
                }
            }
            Int3Attribute output = new Int3Attribute(data);
            return output;
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
        public Int4Attribute(uint reserve = 0) : base(LAttributeDataType.INT, LDataCollectionType.VEC4)
        {
            attribute_data = new List<Vector4i>((int)reserve);
        }

        public Int4Attribute(Vector4i[] data) : base(LAttributeDataType.INT, LDataCollectionType.VEC4)
        {
            attribute_data = new List<Vector4i>(data);
            this.SegmentCount = (uint)attribute_data.Count;
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttributeDescriptor descriptor = this.GenerateDescriptor();
            VertexAttribute attrib = new VertexAttribute(descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector4i>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static Int4Attribute FromVertexAttribute(VertexAttribute attribute)
        {
            if (!(attribute.Descriptor.value_type == LAttributeDataType.INT
                && attribute.Descriptor.collection_type == LDataCollectionType.VEC4))
            {
                throw new Exception("Could not convert to Attribute: incorrect value or collection type");
            }

            Vector4i[] data = new Vector4i[attribute.SegmentCount];
            unsafe
            {
                fixed (void* dest_raw_ptr = &data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(attribute.data, 0, dest_ptr, attribute.data.Length);
                }
            }
            Int4Attribute output = new Int4Attribute(data);
            return output;
        }
    }
}
