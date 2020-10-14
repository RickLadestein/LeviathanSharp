using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.ECS
{
    public abstract class Component
    {
        public Component()
        {
            ComponentRegister.GetInstance().RegisterComponent(this);
        }

        public uint GetComponentId()
        {
            return ComponentRegister.GetInstance().GetComponentId(this);
        }
    }

    public class ComponentRegister
    {
        private static ComponentRegister instance;
        public static readonly int MAX_COMPONENTS = 32;
        public static ComponentRegister GetInstance()
        {
            if (instance == null)
            {
                instance = new ComponentRegister();
            }
            return instance;
        }

        private Dictionary<string, uint> components;
        private ComponentRegister()
        {
            components = new Dictionary<string, uint>();
        }

        public bool RegisterComponent(Component c)
        {
            if (this.components.Count == MAX_COMPONENTS)
            {
                throw new Exception("Could not register component: maximum registered components reached");
            }
            string type = c.GetType().Name;
            if (this.components.ContainsKey(type))
            {
                return false;
            }
            int next_id = 0x01 << this.components.Count;
            this.components.Add(type, (uint)next_id);
            return true;
        }

        public uint GetComponentId(Component c)
        {
            uint output = 0;
            bool result = this.components.TryGetValue(c.GetType().Name, out output);
            if (!result) { return 0; }
            return output;
        }
    }
}
