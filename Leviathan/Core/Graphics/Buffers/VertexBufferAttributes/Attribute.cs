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
    public abstract class Attribute : IDisposable
    {
        /// <summary>
        /// The type of segments stored in this attribute
        /// </summary>
        public AttributeDataType valuetype;

        /// <summary>
        /// The amount of native data types per segment (SINGLE = 1, VEC2 = 2, etc)
        /// </summary>
        public DataCollectionType coll_type;

        /// <summary>
        /// The byte size of a segment stored in this attribute
        /// </summary>
        public uint value_size;

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
        public Attribute(AttributeDataType _type, DataCollectionType _coll_type = DataCollectionType.SINGULAR, uint _reserve = 0)
        {
            this.valuetype = _type;
            this.coll_type = _coll_type;
            DecodeDataType();
            data = new List<byte>((int)(_reserve * value_size * ((uint)_coll_type)));
        }

        /// <summary>
        /// Adds amount of bytes to this attribute
        /// </summary>
        /// <param name="_data">Bytes to add to this attribute</param>
        protected void AddByteData(byte[] _data)
        {
            data.AddRange(_data);
        }

        /// <summary>
        /// Adds amount of bytes to this attribute
        /// </summary>
        /// <param name="_data">Bytes to add to this attribute</param>
        protected void AddByteData(List<byte> _data)
        {
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

        private void DecodeDataType()
        {
            switch (this.valuetype)
            {
                case AttributeDataType.DOUBLE:
                    value_size = 8;
                    break;
                default:
                    value_size = 4;
                    break;
            }
        }

        [DllImport("msvcrt.dll", SetLastError = false)]
        private static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        public void Dispose()
        {
            data.Clear();
        }
    }
}
