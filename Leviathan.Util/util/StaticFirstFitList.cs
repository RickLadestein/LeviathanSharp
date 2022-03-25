using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.Util.util
{
    public class StaticFirstFitList<T>
    {
        public int Count { get; private set; }
        public int MaxCount { get; private set; }
        
        private StaticFirstFirtListItem<T>[] data;
        
        private Queue<int> priority_q;

        public struct StaticFirstFirtListItem<T>
        {
            public T item;
            public bool populated;

            public StaticFirstFirtListItem(T _item)
            {
                item = _item;
                populated = true;
            }
        }

        private StaticFirstFitList() { }

        public StaticFirstFitList(int capacity)
        {
            data = new StaticFirstFirtListItem<T>[capacity];
            priority_q = new Queue<int>(capacity);
            Count = 0;
            MaxCount = capacity;

            for(int i = 0; i < data.Length; i++)
            {
                data[i] = new StaticFirstFirtListItem<T>() { item = default, populated = false };
                priority_q.Enqueue(i);
            }
        }

        public StaticFirstFirtListItem<T> this[int index]
        {
            get
            {
                return AtIndex(index); 
            }
            set
            {
                Replace(index, value.item);
            }
        }

        public void Insert(T _item)
        {
            if(Count >= MaxCount)
            {
                throw new Exception("Could not insert item into StaticFirstFitList: List is full!");
            }
            int index = priority_q.Dequeue();
            while(data[index].populated)
            {
                if(priority_q.Count == 0)
                {
                    throw new Exception("FIX ME: Index was replaced but was also in the priority queue");
                }
                index = priority_q.Dequeue();
            }

            data[index] = new StaticFirstFirtListItem<T>() { item = _item, populated = true };
            Count += 1;
        }

        public void Delete(int index)
        {
            if(Count == 0)
            {
                throw new IndexOutOfRangeException("StaticFirstFitList cannot delete from a list that is already empty");
            }

            if(index > (MaxCount - 1) || index < 0)
            {
                throw new IndexOutOfRangeException("StaticFirstFitList cannot delete index outside of the allocated space");
            }

            data[index].populated = false;
            priority_q.Enqueue(index);
            Count -= 1;
        }

        public void Replace(int index, T _item)
        {
            if (index > (MaxCount - 1) || index < 0)
            {
                throw new IndexOutOfRangeException("StaticFirstFitList cannot replace index outside of the allocated space");
            }

            bool waspopulated = data[index].populated;
            data[index] = new StaticFirstFirtListItem<T>() { item = _item, populated = true };
            if(!waspopulated)
            {
                Count += 1;
            }
        }

        public T[] GetPopulatedItems()
        {
            if(Count == 0)
            {
                return Array.Empty<T>();
            }

            T[] output = new T[Count];
            int current_index = 0;
            for(int i = 0; i < data.Length; i++)
            {
                if(data[i].populated)
                {
                    output[current_index] = data[i].item;
                    current_index += 1;
                }    
            }
            return output;
        }

        public StaticFirstFirtListItem<T> AtIndex(int index)
        {
            if (index > (MaxCount - 1) || index < 0)
            {
                throw new IndexOutOfRangeException("StaticFirstFitList cannot retrieve index outside of the allocated space");
            }
            return data[index];
        }

        public bool Contains(Predicate<T> predicate)
        {
            for(int i = 0; i < data.Length; i++)
            {
                if(predicate.Invoke(data[i].item) && data[i].populated)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsFull()
        {
            return Count == MaxCount;
        }
    }
}
