using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.Util.util.Collections
{
    public class UniqueList<T> : IEnumerable<T>
    {
        private HashSet<T> values;
        private List<T> data;

        public UniqueList()
        {
            values = new HashSet<T>();
            data = new List<T>();
        }

        public int Count
        {
            get
            {
                return data.Count;
            }
        }

        public T this[int index]
        {
            get
            {
                return data[index];
            }
            set
            {
                Swap(index, value);
            }

        }

        public T Find(Predicate<T> predicate)
        {
            return data.Find(predicate);
        } 

        public bool Contains(T value)
        {
            return values.Contains(value);
        }

        public bool Add(T value)
        {
            if(values.Contains(value))
            {
                return false; 
            }
            this.data.Add(value);
            return true;
        }

        public int AddRange(IEnumerable<T> coll)
        {
            int count = 0;
            foreach(T item in coll)
            {
                bool succes = Add(item);
                if (succes)
                {
                    count += 1;
                }
            }
            return count;
        }

        public bool Remove(T value)
        {
            if(!values.Contains(value))
            {
                return false;
            }
            values.Remove(value);
            data.Remove(value);
            return true;
        }

        public void Swap(int index, T value)
        {
            T item = data[index];
            values.Remove(item);

            data[index] = value;
            values.Add(value);
        }

        public T[] ToArray()
        {
            return data.ToArray();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
