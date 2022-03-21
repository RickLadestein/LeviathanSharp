using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Leviathan.Util
{
    public class FreeList<T>
    {
        private struct FreeListItem<T>
        {
            public T item;
            public bool populated;

            public FreeListItem(T _item)
            {
                item = _item;
                populated = true;
            }

            public static FreeListItem<T> Empty()
            {
                return new FreeListItem<T>()
                {
                    item = default(T),
                    populated = false
                };
            }
        }

        private List<FreeListItem<T>> data;
        private Queue<int> priority_q;

        public FreeList()
        {
            data = new List<FreeListItem<T>>();
            priority_q = new Queue<int>();
        }

        public FreeList(uint reserved)
        {
            data = new List<FreeListItem<T>>((int)reserved);
            priority_q = new Queue<int>();
        }


        public T this[int index]
        {
            get
            {
                return data[index].item;
            }
        }
        public T Get(int index)
        {
            return this[index];
        }

        public void Insert(T item)
        {
            if (priority_q.Count != 0)
            {
                int index = priority_q.Dequeue();
                data[index] = new FreeListItem<T>(item);
            } else
            {
                data.Add(new FreeListItem<T>(item));
            }
        }

        public void Insert(IEnumerable<T> coll)
        {
            int index = 0;
            foreach(T item in coll)
            {
                Insert(item);
                index += 1;
            }
        }

        public void Remove(int index)
        {
            //data[index].populated = false;
        }

        public void Replace(int index, T item)
        {
            FreeListItem<T> _litem = new FreeListItem<T>(item);
            data[index] = _litem;
        }

        public bool IsPopulated(int index)
        {
            return data[index].populated;
        }
    }
}
