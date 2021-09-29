using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    

    public abstract class Attribute : IDisposable
    {
        public AttributeDataType valuetype;
        public DataCollectionType coll_type;
        public uint value_size;

        public List<byte> data;


        public Attribute(AttributeDataType _type, DataCollectionType _coll_type = DataCollectionType.SINGULAR, uint _reserve = 0)
        {
            this.valuetype = _type;
            this.coll_type = _coll_type;
            DecodeDataType();
            data = new List<byte>((int)(_reserve * value_size * ((uint)_coll_type)));
        }

        protected void AddByteData(byte[] _data)
        {
            data.AddRange(_data);
        }

        protected void AddByteData(List<byte> _data)
        {
            data.AddRange(_data);
        }

        protected T FromByteArray<T>(byte[] rawValue)
        {
            GCHandle handle = GCHandle.Alloc(rawValue, GCHandleType.Pinned);
            T structure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return structure;
        }

        protected byte[] ToByteArray(object value, int maxLength = 0)
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
