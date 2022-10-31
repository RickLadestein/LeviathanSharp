using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Leviathan.Util.util.Collections
{
    public class ValueCollection<_type> : List<_type> where _type : unmanaged
    {
        public ValueCollection() : base() {}

        public ValueCollection(IEnumerable<_type> enumerable) : base(enumerable) { }

        public ValueCollection(int capacity) : base(capacity) { } 
        public byte[] ToByteArray()
        {
            byte[] result = new byte[Count * Marshal.SizeOf<_type>()];
            _type[] array = this.ToArray();
            unsafe
            {
                fixed (void* dp = &array[0])
                {
                    IntPtr ptr = new IntPtr(dp);
                    Marshal.Copy(ptr, result, 0, result.Length);
                }
            }
            return result;
        }

        public int AddRangeFromByteBuffer(byte[] buffer)
        {
            int type_byte_size = Marshal.SizeOf<_type>();
            if (buffer.Length % type_byte_size != 0)
            {
                throw new ArgumentException("Buffer byte size does fit equal whole type byte size (bufflength % type_size != 0)");
            }

            int segment_count = buffer.Length / type_byte_size;

            _type[] converted_data = new _type[segment_count];
            unsafe
            {
                fixed (void* dest_raw_ptr = &converted_data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(buffer, 0, dest_ptr, buffer.Length);
                }
            }
            this.AddRange(converted_data);
            return segment_count;
        }

        public int AddRangeFromTypedBuffer<A>(A[] buffer) where A : unmanaged
        {
            int type_byte_size = Marshal.SizeOf<_type>();
            int buffer_type_byte_size = Marshal.SizeOf<A>();
            int buffer_byte_size = buffer.Length * buffer_type_byte_size;
            int remainder = buffer_byte_size % type_byte_size;
            if (remainder != 0)
            {
                throw new ArgumentException("Buffer byte size does fit equal whole type byte size (bufflength % type_size != 0)");
            }

            _type[] converted_data = new _type[buffer_byte_size];
            unsafe
            {
                fixed (void* dest_raw_ptr = &converted_data[0])
                {
                    fixed(void* src_raw_ptr = &buffer[0])
                    {
                        Buffer.MemoryCopy(src_raw_ptr, dest_raw_ptr, buffer_byte_size, buffer_byte_size);
                    }
                    
                }
            }
            this.AddRange(converted_data);
            return converted_data.Length;
        }

        public static ValueCollection<T> FromByteArray<T>(byte[] buffer) where T : unmanaged
        {
            int type_byte_size = Marshal.SizeOf<T>();
            if(buffer.Length % type_byte_size != 0)
            {
                throw new ArgumentException("Buffer size does fit equal whole type byte size (bufflength % type_size != 0)");
            }

            int segment_count = buffer.Length / type_byte_size;

            T[] converted_data = new T[segment_count];
            unsafe
            {
                fixed (void* dest_raw_ptr = &converted_data[0])
                {
                    IntPtr dest_ptr = new IntPtr(dest_raw_ptr);
                    Marshal.Copy(buffer, 0, dest_ptr, buffer.Length);
                }
            }
            return new ValueCollection<T>(converted_data);
        }
    }
}
