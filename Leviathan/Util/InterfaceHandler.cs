using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Leviathan.Util
{
    public class InterfaceHandler<T>
    {
        private List<T> events;
        private Queue<T> deletion_coll;
        private Queue<T> addition_coll;

        public InterfaceHandler()
        {
            events = new List<T>();
            deletion_coll = new Queue<T>();
            addition_coll = new Queue<T>();
        }

        public void AddSubscriber(T sub)
        {
            addition_coll.Enqueue(sub);
        }

        public void RemoveSubscriber(T sub)
        {
            deletion_coll.Enqueue(sub);
        }

        public void Invoke(Action<T> action)
        {
            while (addition_coll.Count != 0)
            {
                events.Add(addition_coll.Dequeue());
            }

            while (deletion_coll.Count != 0)
            {
                events.Remove(deletion_coll.Dequeue());
            }

            foreach (T item in events)
            {
                if(item == null)
                {
                    deletion_coll.Enqueue(item);
                } else
                {
                    action?.Invoke(item);
                }
            }

            
        }
    }
}
