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
    public abstract class VertexAttribute : IDisposable
    {
        public VertexAttributeDescriptor Descriptor { get; private set; }

        /// <summary>
        /// The byte data stored in this attribute
        /// </summary>
        public List<byte> data;

        /// <summary>
        /// The amount of data segments stored in the attribute
        /// </summary>
        public int SegmentCount { get; protected set; }

        /// <summary>
        /// Instantiates a new instance of Attribute with specified parameters
        /// </summary>
        /// <param name="_type">The native data type that is stored in this attribute</param>
        /// <param name="_coll_type">The amount of native data types per segment</param>
        /// <param name="_reserve">The amount of segments to reserve in the internal storage</param>
        public VertexAttribute(AttributeDataType _type, DataCollectionType _coll_type = DataCollectionType.SINGULAR, uint _reserve = 0)
        {
            Descriptor = new VertexAttributeDescriptor(_type, _coll_type);
            data = new List<byte>((int)_reserve * Descriptor.segment_byte_size);
        }

        /// <summary>
        /// Adds amount of bytes to this attribute
        /// </summary>
        /// <param name="_data">Bytes to add to this attribute</param>
        protected void AddByteData(byte[] _data)
        {
            if(_data.Length % Descriptor.segment_byte_size != 0)
            {
                throw new Exception("data to be pushed in the VertexAttribute does not match data segment properties described in the Attribute Descriptor.");
            }
            data.AddRange(_data);
        }

        /// <summary>
        /// Adds amount of bytes to this attribute
        /// </summary>
        /// <param name="_data">Bytes to add to this attribute</param>
        protected void AddByteData(List<byte> _data)
        {
            if (_data.Count % Descriptor.segment_byte_size != 0)
            {
                throw new Exception("data to be pushed in the VertexAttribute does not match data segment properties described in the Attribute Descriptor.");
            }
            data.AddRange(_data);
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
        protected void AddDataColl<T>(T[] datapoints) where T : unmanaged
        {
            byte[] result = new byte[datapoints.Length * Marshal.SizeOf<T>()];
            unsafe
            {
                fixed (void* dp = &datapoints[0])
                {
                    IntPtr ptr = new IntPtr(dp);
                    Marshal.Copy(ptr, result, 0, result.Length);
                    this.data.AddRange(result);
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

        [DllImport("msvcrt.dll", SetLastError = false)]
        private static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        public void Dispose()
        {
            data.Clear();
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
        public AttributeDataType value_type;

        /// <summary>
        /// The collection type that the data in the vertex attribute contains
        /// </summary>
        public DataCollectionType collection_type;

        /// <summary>
        /// The amount of bytes of a single value in a segment
        /// </summary>
        public int value_byte_size;

        /// <summary>
        /// The amount of bytes stored in a single segment i.e(VEC2, VEC3, VEC4 etc)
        /// </summary>
        public int segment_byte_size;

        /// <summary>
        /// Instantiates a new instance of VertexAttributeDescriptor given specified AttributeDataType and DataCollectionType
        /// </summary>
        /// <param name="value_type">The value type stored inside a single segment</param>
        /// <param name="collection_type">The collection type that the data in the vertex attribute contains</param>
        public VertexAttributeDescriptor(AttributeDataType value_type, DataCollectionType collection_type) : this()
        {
            this.value_type = value_type;
            this.collection_type = collection_type;

            switch (value_type)
            {
                case AttributeDataType.INT:
                case AttributeDataType.FLOAT:
                    value_byte_size = 4;
                    break;
                case AttributeDataType.DOUBLE:
                    value_byte_size = 8;
                    break;
                default:
                    throw new NotSupportedException($"AttributeDataType: {value_type} not yet supported!");
            }
            segment_byte_size = ((int)collection_type) * value_byte_size;
        }
    }
}
