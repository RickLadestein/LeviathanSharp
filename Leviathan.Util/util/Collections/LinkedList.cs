using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.Util.Collections
{
    public class LinkedList<T> : IEnumerable<T>, ICollection<T> where T : IEquatable<T> 
    {
        private LinkedListItem<T> head;
        private LinkedListItem<T> tail;

        private int _itemcount;
        public int Count { get { return _itemcount; } }

        public bool IsReadOnly => false;

        public LinkedList()
        {
            _itemcount = 0;
            head = null;
            tail = null;
        }
        public bool Contains(T item)
        {
            var ret = FindFirstItemWithValue(item);
            return ret.Item2 != -1;
        }

        public void Clear()
        {
            this.head = null;
            this._itemcount = 0;
        }


        public void Add(T item)
        {
            if (head == null)
            {
                head = new LinkedListItem<T>(item);
                tail = head;
            } else
            {
                LinkedListItem<T> node = new LinkedListItem<T>(item, tail);
                tail.next = node;
                tail = tail.next;
            }
            _itemcount += 1;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection.Count() == 0) { return; }

            bool skip = false;
            if (head == null) { 
                head = new LinkedListItem<T>(collection.First());
                tail = head;
                skip = true;
                _itemcount += 1;
            }

            LinkedListItem<T> current = tail;
            foreach (T _item in collection)
            {
                if (skip) { skip = false; continue; }
                LinkedListItem<T> node = new LinkedListItem<T>(_item, current);
                tail.next = node;
                tail = tail.next;
                _itemcount += 1;
            }
        }

        public T this[int index]
        {
            get
            {
                return GetIndex(index);
            }
            set
            {
                SetIndex(index, value);
            }
        }

        public T GetIndex(int index)
        {
            if (index < 0 || index >= _itemcount) { throw new ArgumentException("Requested index was outside the bounds of the LinkedList"); }
            LinkedListItem<T> curr = head;
            for (int i = 0; i != index; i++)
            {
                curr = curr.next;
            }
            return curr.item;
        }

        public void SetIndex(int index, T value)
        {
            if (index < 0 || index >= _itemcount) { throw new ArgumentException("Requested index was outside the bounds of the LinkedList"); }
            LinkedListItem<T> curr = head;
            for (int i = 0; i != index; i++)
            {
                curr = curr.next;
            }
            curr.item = value;
        }


        #region Remove
        public bool Remove(T value)
        {
            if(head == null) { return false; }
            //TODO: fix null equals
            if(head.item.Equals(value))
            {
                if(head.next != null)
                {
                    head = head.next;
                } else
                {
                    head = null;
                }
                _itemcount -= 1;
                return true;
            }

            LinkedListItem<T> curr = head.next;
            while(curr != null)
            {
                if(curr.item.Equals(value))
                {
                    LinkedListItem<T> prev = curr.previous;
                    LinkedListItem<T> next = curr.next;
                    prev.next = next;
                    next.previous = prev;
                    _itemcount -= 1;
                    return true;
                } else
                {
                    curr = curr.next;
                }
            }
            return false;
        }

        public void RemoveIndex(int index)
        {
            if (this._itemcount <= index || index < 0) { throw new ArgumentException("Index was greater than itemcount or less than zero"); }

            if (index == 0 && head == null) { return; }
            if (index == 0 && head.next == null) { head = null; return; }

            if (index == 0)
            {
                head = head.next;
                head.previous = null;
                _itemcount -= 1;
                return;
            }

            int current_index = 0;
            LinkedListItem<T> current = head;
            while (current != null)
            {
                if (current_index == index)
                {
                    LinkedListItem<T> prev = current.previous;
                    LinkedListItem<T> next = current.next;
                    prev.next = next;
                    next.previous = prev;
                    _itemcount -= 1;
                    return;
                }
                current_index += 1;
                current = current.next;
            }
        }

        public void RemoveAll (Predicate<T> predicate)
        {
            if (head == null) { return; }

            //First look in table children
            LinkedListItem<T> curr = head.next;
            while (curr != null)
            {
                if (predicate.Invoke(curr.item))
                {
                    LinkedListItem<T> prev = curr.previous;
                    LinkedListItem<T> next = curr.next;
                    prev.next = next;
                    next.previous = prev;
                    _itemcount -= 1;
                }
                else
                {
                    curr = curr.next;
                }
            }

            //Last look at the head
            if (predicate.Invoke(head.item))
            {
                if (head.next != null)
                {
                    head = head.next;
                }
                else
                {
                    head = null;
                }
                _itemcount -= 1;
            }
        }
        #endregion

        public T FindFirst(Predicate<T> predicate, out bool found)
        {
            foreach (T item in this)
            {
                if (predicate.Invoke(item))
                {
                    found = true;
                    return item;
                }
            }

            found = false;
            return default;
        }

        public T[] FindAll(Predicate<T> predicate)
        {
            List<T> found = new List<T>();
            foreach (T item in this)
            {
                if (predicate.Invoke(item))
                {
                    found.Add(item);
                }
            }
            return found.ToArray();
        }

        public T[] ToArray()
        {
            T[] output = new T[this.Count];
            LinkedListItem<T> current = head;

            for (int i = 0; i < this.Count; i++)
            { 
                output[i] = current.item;
                current = current.next;
            }

            return output;
        }

        public T[] ToArray(int startIndex)
        {
            if (startIndex < 0 || startIndex >= _itemcount) { throw new ArgumentException("startIndex was outside the bounds of the LinkedList"); }

            List<T> output = new List<T>();
            int index = 0;
            foreach (T item in this)
            {
                if (index >= startIndex)
                {
                    output.Add(item);
                }
            }
            return output.ToArray();
        }

        public T[] ToArray(int startIndex, int endIndex)
        {
            if(startIndex < 0 || startIndex >= _itemcount) { throw new ArgumentException("startIndex was outside the bounds of the LinkedList"); }
            if(endIndex < startIndex) { throw new ArgumentException("endIndex cannot be smaller than startIndex"); }
            if (endIndex < 0 || endIndex >= _itemcount) { throw new ArgumentException("endIndex was outside the bounds of the LinkedList"); }

            List<T> output = new List<T>();
            int index = 0;
            foreach(T item in this)
            {
                if(index >= startIndex)
                {
                    output.Add(item);
                    if(index > endIndex) { break; }
                }
            }
            return output.ToArray();

        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            T[] arr = this.ToArray(arrayIndex);
            Array.Copy(arr, array, arr.Length);
        }

        private IEnumerator<LinkedListItem<T>> GetItemEnumerator()
        {
            if(head == null) { yield break; }
            LinkedListItem<T> current = head;
            while (current != null)
            {
                yield return current;
                current = current.next;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if(head == null) { yield break; }

            LinkedListItem<T> current = head;
            while(current != null)
            {
                yield return current.item;
                current = current.next;
            }
        }

        

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        } 
        
        private Tuple<LinkedListItem<T>, int> FindFirstItemWithValue(T value)
        {
            IEnumerator<LinkedListItem<T>> enumerator = this.GetItemEnumerator();
            int index = 0;
            while (enumerator.MoveNext())
            {
                LinkedListItem<T> current = enumerator.Current;
                if(current == null) { break; }
                
                if(ReferenceEquals(current.item, null))
                {
                    if(ReferenceEquals(value, null)) {
                        return new Tuple<LinkedListItem<T>, int>(current, index);
                    } else
                    {
                        continue;
                    }
                } else
                {
                    if(current.item.Equals(value))
                    {
                        return new Tuple<LinkedListItem<T>, int>(current, index);
                    }
                }


                index += 1;
            }
            return new Tuple<LinkedListItem<T>, int>(null, -1);

        }
    }

    internal class LinkedListItem<T>
    {
        public T item;
        public LinkedListItem<T> previous;
        public LinkedListItem<T> next;

        public LinkedListItem(T _item)
        {
            item = _item;
            previous = null;
            next = null;
        }

        public LinkedListItem(T item, LinkedListItem<T> previous) : this(item)
        {
            this.previous = previous;
        }

        public LinkedListItem(T item, LinkedListItem<T> previous, LinkedListItem<T> next) : this(item)
        {
            this.previous = previous;
            this.next = next;
        }
    }
}
