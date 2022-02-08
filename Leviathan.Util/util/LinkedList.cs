using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Util
{
    public class LinkedList<T> : IEnumerable<T>
    {
        private class LinkedListItem<T>
        {
            public LinkedListItem()
            {
                data = default;
                previous = null;
                next = null;
            }

            public T data;
            public LinkedListItem<T> previous;
            public LinkedListItem<T> next;
        }

        private LinkedListItem<T> first;
        public uint Length { get; private set; }
        public LinkedList()
        {
            first = null;
        }

        /// <summary>
        /// Gets an item from the array list at specified index
        /// </summary>
        /// <param name="index">The index that needs to be retrieved</param>
        /// <returns>Item at the given index</returns>
        public T Get(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException("index cannot be smaller than 0");
            }

            if (index == Length)
            {
                throw new IndexOutOfRangeException("index cannot be the same as the list size");
            }

            if(index > Length)
            {
                throw new IndexOutOfRangeException("index cannot be larger then the list size");
            }

            LinkedListItem<T> current = first;
            int idx = index;
            while (idx != 0)
            {
                idx -= 1;
                current = current.next;
            }
            return current.data;
        }

        /// <summary>
        /// Adds an item to the LinkedList
        /// </summary>
        /// <param name="value">The item that needs to be added to the list</param>
        /// <returns>True if succes, False if not</returns>
        public void Add(T value)
        {
            LinkedListItem<T> current = first;
            LinkedListItem<T> item = new LinkedListItem<T>();
            item.data = value;


            if (current == null)
            {
                first = item;
            }
            else
            {
                while (current.next != null)
                {
                    current = current.next;
                }
                item.previous = current;
                current.next = item;
            }
            Length += 1;
;
        }

        /// <summary>
        /// Removes an item from the LinkedList at given index
        /// </summary>
        /// <param name="index">The index that needs to be removed</param>
        /// <returns>True if deleted, False if not</returns>
        public void Remove(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException("index cannot be smaller than 0");
            }

            if (index == Length)
            {
                throw new IndexOutOfRangeException("index cannot be the same as the list size");
            }

            if (index > Length)
            {
                throw new IndexOutOfRangeException("index cannot be larger then the list size");
            }

            
            
            if (index == 0)
            {
                if (first.next != null)
                {
                    first = first.next;
                }
            }
            else
            {
                LinkedListItem<T> item = GetListItemAtIndex(index);
                LinkedListItem<T> previous = item.previous;
                LinkedListItem<T> next = item.next;

                if (previous != null && next != null)
                {
                    previous.next = next;
                    next.previous = previous;
                }
                else if (previous != null && next == null)
                {
                    previous.next = null;
                }
                else if (previous == null && next != null)
                {
                    next.previous = null;
                }
            }
            Length -= 1;
        }

        public void Swap(int index_1, int index_2)
        {
            LinkedListItem<T> item1 = GetListItemAtIndex(index_1);
            T item1_data = item1.data;
            LinkedListItem<T> item2 = GetListItemAtIndex(index_2);
            T item2_data = item2.data;

            item1.data = item2_data;
            item2.data = item1_data;

        }


        /// <summary>
        /// Converts the LinkedList to an Array
        /// </summary>
        /// <returns>Array with all the items of the LinkedList</returns>
        public T[] ToArray()
        {
            if(Length == 0)
            {
                return new T[0];
            }

            T[] output = new T[this.Length];
            LinkedListItem<T> current = first;
            for(int i = 0; i < Length; i++)
            {
                output[i] = current.data;
                current = current.next;
            }
            return output;
        }

        private LinkedListItem<T> GetListItemAtIndex(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException("index cannot be smaller than 0");
            }

            if (index == Length)
            {
                throw new IndexOutOfRangeException("index cannot be the same as the list size");
            }

            if (index > Length)
            {
                throw new IndexOutOfRangeException("index cannot be larger then the list size");
            }

            LinkedListItem<T> current = first;
            int idx = index;
            while (idx != 0)
            {
                idx -= 1;
                current = current.next;
            }
            return current;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if(Length == 0)
            {
                throw new Exception("Cannot enumerate on an empty LinkedList");
            }
            LinkedListItem<T> item = first;
            while(item != null)
            {
                yield return item.data;
                item = item.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }


    
}
