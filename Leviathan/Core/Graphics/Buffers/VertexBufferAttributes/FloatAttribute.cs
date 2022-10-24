using Leviathan.Math;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        public FloatAttribute(uint reserve = 0) : base(LAttributeDataType.FLOAT, LDataCollectionType.SINGULAR)
        {
            attribute_data = new List<float>((int)reserve);
        }

        public FloatAttribute(float[] data) : base(LAttributeDataType.FLOAT, LDataCollectionType.SINGULAR)
        {
            attribute_data = new List<float>(data);
            this.SegmentCount = (uint)attribute_data.Count;
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttributeDescriptor descriptor = this.GenerateDescriptor();
            VertexAttribute attrib = new VertexAttribute(descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<float>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static FloatAttribute FromVertexAttribute(VertexAttribute attribute)
        {
            if (!(attribute.Descriptor.value_type == LAttributeDataType.FLOAT
                && attribute.Descriptor.collection_type == LDataCollectionType.SINGULAR))
            {
                throw new Exception("Could not convert to specific Attribute: incorrect value or collection type");
            }

            float[] data = new float[attribute.SegmentCount];
            unsafe
            {
                fixed (void* dest_raw_ptr = &data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(attribute.data, 0, dest_ptr, attribute.data.Length);
                }
            }
            FloatAttribute output = new FloatAttribute(data);
            return output;
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
        public Float2Attribute(uint reserve = 0) : base(LAttributeDataType.FLOAT, LDataCollectionType.VEC2)
        {
            attribute_data = new List<Vector2f>((int)reserve);
        }

        public Float2Attribute(Vector2f[] data) : base(LAttributeDataType.FLOAT, LDataCollectionType.VEC2)
        {
            attribute_data = new List<Vector2f>(data);
            this.SegmentCount = (uint)attribute_data.Count;
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttributeDescriptor descriptor = this.GenerateDescriptor();
            VertexAttribute attrib = new VertexAttribute(descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector2f>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static Float2Attribute FromVertexAttribute(VertexAttribute attribute)
        {
            if (!(attribute.Descriptor.value_type == LAttributeDataType.FLOAT
                && attribute.Descriptor.collection_type == LDataCollectionType.VEC2))
            {
                throw new Exception("Could not convert to Attribute: incorrect value or collection type");
            }

            Vector2f[] data = new Vector2f[attribute.SegmentCount];
            unsafe
            {
                fixed (void* dest_raw_ptr = &data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(attribute.data, 0, dest_ptr, attribute.data.Length);
                }
            }
            Float2Attribute output = new Float2Attribute(data);
            return output;
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
        public Float3Attribute(uint reserve = 0) : base(LAttributeDataType.FLOAT, LDataCollectionType.VEC3)
        {
            attribute_data = new List<Vector3f>((int)reserve);
        }

        public Float3Attribute(Vector3f[] data) : base(LAttributeDataType.FLOAT, LDataCollectionType.VEC3)
        {
            attribute_data = new List<Vector3f>(data);
            this.SegmentCount = (uint)attribute_data.Count;
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttributeDescriptor descriptor = this.GenerateDescriptor();
            VertexAttribute attrib = new VertexAttribute(descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector3f>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static Float3Attribute FromVertexAttribute(VertexAttribute attribute)
        {
            if (!(attribute.Descriptor.value_type == LAttributeDataType.FLOAT
                && attribute.Descriptor.collection_type == LDataCollectionType.VEC3))
            {
                throw new Exception("Could not convert to Attribute: incorrect value or collection type");
            }

            Vector3f[] data = new Vector3f[attribute.SegmentCount];
            unsafe
            {
                fixed (void* dest_raw_ptr = &data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(attribute.data, 0, dest_ptr, attribute.data.Length);
                }
            }
            Float3Attribute output = new Float3Attribute(data);
            return output;
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
        public Float4Attribute(uint reserve = 0) : base(LAttributeDataType.FLOAT, LDataCollectionType.VEC4)
        {
            attribute_data = new List<Vector4f>((int)reserve);
        }

        public Float4Attribute(Vector4f[] data) : base(LAttributeDataType.FLOAT, LDataCollectionType.VEC4)
        {
            attribute_data = new List<Vector4f>(data);
            this.SegmentCount = (uint)attribute_data.Count;
        }

        public override VertexAttribute CompileToVertexAttribute()
        {
            VertexAttributeDescriptor descriptor = this.GenerateDescriptor();
            VertexAttribute attrib = new VertexAttribute(descriptor, (uint)attribute_data.Count);
            attrib.SetDataColl<Vector4f>(attribute_data.ToArray());
            attrib.RevalidateDescriptor();
            return attrib;
        }

        public static Float4Attribute FromVertexAttribute(VertexAttribute attribute)
        {
            if(!(attribute.Descriptor.value_type == LAttributeDataType.FLOAT 
                && attribute.Descriptor.collection_type == LDataCollectionType.VEC4))
            {
                throw new Exception("Could not convert to Attribute: incorrect value or collection type");
            }

            Vector4f[] data = new Vector4f[attribute.SegmentCount];
            unsafe
            {
                fixed (void* dest_raw_ptr = &data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(attribute.data, 0, dest_ptr, attribute.data.Length);
                }
            }
            Float4Attribute output = new Float4Attribute(data);
            return output;
        }
    }
}
