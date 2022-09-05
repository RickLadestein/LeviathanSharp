using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    /// <summary>
    /// Storage class for storing attribute data
    /// </summary>
    public class VertexAttribute : IDisposable
    {
        public VertexAttributeDescriptor Descriptor { get; protected set; }

        /// <summary>
        /// The byte data stored in this attribute
        /// </summary>
        public byte[] data;

        /// <summary>
        /// The amount of data segments stored in the attribute
        /// </summary>
        public int SegmentCount { get; private set; }

        /// <summary>
        /// Instantiates a new instance of Attribute with specified parameters
        /// </summary>
        public VertexAttribute(VertexAttributeDescriptor descriptor, uint _reserve = 0)
        {
            Descriptor = descriptor;
            data = new byte[((int)_reserve * Descriptor.segment_byte_size)];
        }

        public static void CopyTo(VertexAttribute attrib_src, VertexAttribute attrib_dst)
        {
            attrib_dst.Descriptor = attrib_src.Descriptor;
            attrib_dst.data = new byte[attrib_src.data.Length];
            Array.Copy(attrib_src.data, attrib_dst.data, attrib_src.data.Length);
        }

        /// <summary>
        /// Converts a value to a byte array
        /// </summary>
        /// <param name="value">The value that needs to be converted</param>
        /// <returns></returns>
        protected byte[] ToByteArray(object value)
        {
            int rawsize = Marshal.SizeOf(value);
            byte[] rawdata = new byte[rawsize];
            GCHandle handle =
                GCHandle.Alloc(rawdata,
                GCHandleType.Pinned);
            Marshal.StructureToPtr(value,
                handle.AddrOfPinnedObject(),
                false);
            handle.Free();
            return rawdata;
        }

        /// <summary>
        /// Adds datapoint segments to this attribute
        /// </summary>
        /// <typeparam name="T">The struct/valuetype to add to this attribute (only unmanaged types!)</typeparam>
        /// <param name="datapoints">The datapoint segments to add to this attribute</param>
        public void SetDataColl<T>(T[] datapoints) where T : unmanaged
        {
            byte[] result = new byte[datapoints.Length * Marshal.SizeOf<T>()];
            unsafe
            {
                fixed (void* dp = &datapoints[0])
                {
                    IntPtr ptr = new IntPtr(dp);
                    Marshal.Copy(ptr, result, 0, result.Length);
                    this.data = result;
                }
            }
        }

        /// <summary>
        /// Converts a byte array to given type
        /// </summary>
        /// <typeparam name="T">The struct/valuetype to convert to (only unmanaged types!)</typeparam>
        /// <param name="rawValue">The byte representation of requested type</param>
        /// <returns></returns>
        protected T FromByteArray<T>(byte[] rawValue) where T : unmanaged
        {
            GCHandle handle = GCHandle.Alloc(rawValue, GCHandleType.Pinned);
            T structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return structure;
        }

        /// <summary>
        /// Validates and reloads the VertexAttributeDescriptor to adjust to new data contained in the VertexAttribute
        /// </summary>
        /// <returns>True if revalidation was succesfull, False if the descriptor did not match contained data</returns>
        public bool RevalidateDescriptor()
        {
            this.Descriptor.Reload();
            bool succes = (this.data.Length % this.Descriptor.segment_byte_size) == 0;
            if(succes)
            {
                this.SegmentCount = this.data.Length / this.Descriptor.segment_byte_size;
            }
            return succes;
        }

        public void Dispose()
        {
            //data.Clear();
        }
    }

    /// <summary>
    /// Descriptor that provides information on what kind of data is contained in the VertexAttribute
    /// </summary>
    public struct VertexAttributeDescriptor
    {
        /// <summary>
        /// The name of the VertexAttribute in the shader program
        /// </summary>
        public String name;

        /// <summary>
        /// The value type stored inside a single segment
        /// </summary>
        public LAttributeDataType value_type;

        /// <summary>
        /// The collection type that the data in the vertex attribute contains
        /// </summary>
        public LDataCollectionType collection_type;

        /// <summary>
        /// The amount of bytes of a single value in a segment
        /// </summary>
        public int value_byte_size;

        /// <summary>
        /// The amount of bytes stored in a single segment i.e(VEC2, VEC3, VEC4 etc)
        /// </summary>
        public int segment_byte_size;

        public void Reload()
        {
            switch (this.value_type)
            {
                case LAttributeDataType.INT:
                case LAttributeDataType.FLOAT:
                    value_byte_size = 4;
                    break;
                case LAttributeDataType.DOUBLE:
                    value_byte_size = 8;
                    break;
                default:
                    throw new NotSupportedException($"AttributeDataType: {value_type} not yet supported!");
            }
            segment_byte_size = ((int)collection_type) * value_byte_size;
        }
    }
}
