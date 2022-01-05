using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Util
{
    public class RingBuffer<T>
    {
        private T[] buffer;
        private uint end;
        private uint start;
        private uint max_size;
        private readonly T default_val;

        /// <summary>
        /// The amount of items that the RingBuffer currently contains
        /// </summary>
        public uint Items { get; private set; }

        /// <summary>
        /// Instantiates a new instance of RingBuffer with specified size
        /// </summary>
        /// <param name="size">The size of the RingBuffer</param>
        public RingBuffer(uint size)
        {
            buffer = new T[size];
            max_size = size;
            Items = 0;
            end = 0;
            start = 0;
            default_val = default;
        }

        /// <summary>
        /// Instantiates a new instance of RingBuffer with specified size and value initialiser
        /// </summary>
        /// <param name="size">The size of the RingBuffer</param>
        /// <param name="def_val">The default value for each item in the buffer</param>
        public RingBuffer(uint size, T def_val)
        {
            buffer = new T[size];
            max_size = size;
            Items = 0;
            end = 0;
            start = 0;
            default_val = def_val;

            for (int i =0; i < size; i++)
            {
                buffer[i] = def_val;
            }
        }


        /// <summary>
        /// Stores an item into the RingBuffer
        /// </summary>
        /// <param name="value"></param>
        public void Push(T value)
        {
            buffer[end++] = value;
            end %= max_size;

            if(Items != max_size)
            {
                Items += 1;
            }
        }

        /// <summary>
        /// Retrieves the first item from the RingBuffer
        /// </summary>
        /// <returns>Stored item</returns>
        public T Pop()
        {
            if(Items == 0)
            {
                throw new RingbufferEmptyException();
            } else
            {
                T retval = buffer[start];
                buffer[start++] = default_val;
                start %= max_size;
                Items -= 1;
                return retval;
            }
        }

        /// <summary>
        /// Retrieves the RingBuffer as an array
        /// </summary>
        /// <returns>Array of items in the RingBuffer</returns>
        public T[] ToArray()
        {
            T[] ret = new T[buffer.Length];
            buffer.CopyTo(ret, 0);
            return ret;
        }

        /// <summary>
        /// Checks the RingBuffer for an item
        /// </summary>
        /// <param name="val">The item that the RingBuffer should contain</param>
        /// <returns></returns>
        public bool Contains(T val)
        {
            for(int i = 0; i < max_size; i++)
            {
                if(buffer[i].Equals(val))
                {
                    return true;
                }
            }
            return false;
        }


    }

    public class RingbufferEmptyException : Exception
    {
        public RingbufferEmptyException() : base("Could not retrieve value from RingBuffer: RingBuffer was empty") { }
    }
}
