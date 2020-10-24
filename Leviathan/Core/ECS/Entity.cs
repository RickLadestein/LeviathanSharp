using Leviathan.Core.ECS.Components;
using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Leviathan.Core.ECS
{
    public class Entity
    {
        public Guid Uuid { get; private set; }

        private List<Component> components;
        private uint component_ids;
        private Mutex mutex;
        public Entity()
        {
            this.Uuid = Guid.NewGuid();
            this.components = new List<Component>();
            this.mutex = new Mutex();
            this.component_ids = 0;
        }

        public bool HasComponent<T>()
        {
            this.mutex.WaitOne();
            bool output = false;
            foreach(Component c in components)
            {
                if(c is T)
                {
                    output = true;
                    break;
                }
            }
            mutex.ReleaseMutex();
            return output;
        }

        public Component GetComponent<T>()
        {
            this.mutex.WaitOne();
            foreach(Component c in components)
            {
                if(c is T)
                {
                    this.mutex.ReleaseMutex();
                    return c;
                }
            }
            this.mutex.ReleaseMutex();
            return null;
        }

        public void AddComponent<T>(Component c)
        {
            if(c is T)
            {
                if(HasComponent<T>())
                {
                    return;
                }
                mutex.WaitOne();
                this.components.Add(c);
                this.component_ids |= c.GetComponentId();
                mutex.ReleaseMutex();
            } else
            {
                throw new ArgumentException("Argument type did not match specifier type");
            }
            
        }

        public bool RemoveComponent<T>()
        {
            Component c = this.GetComponent<T>();
            if(c != null)
            {
                mutex.WaitOne();
                this.component_ids ^= c.GetComponentId();
                this.components.Remove(c);
                mutex.ReleaseMutex();
                return true;
            }
            return false;
        }
    }
}
