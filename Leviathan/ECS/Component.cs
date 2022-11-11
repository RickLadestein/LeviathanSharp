using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Silk.NET.OpenGL.Extensions.ImGui;
namespace Leviathan.ECS
{
    public abstract class Component
    {
        /// <summary>
        /// The component GUID that identifies the component
        /// </summary>
        public Guid id { get; private set; }

        public int Signature { get; private set; }

        /// <summary>
        /// The entity this component is bound to
        /// </summary>
        public Entity Parent { get; set; }

        public abstract string FriendlyName { get; }

        /// <summary>
        /// Creates a new instance of Component with default parameters
        /// </summary>
        public Component()
        {
            id = Guid.NewGuid();
            Signature = ComponentRegistry.GetInstance().GetSignature(GetType());
        }

        protected virtual void AddDependencies()
        {
            return;
        }

        /// <summary>
        /// Initialises the component to default values after entity has aggregated the component 
        /// </summary>
        public virtual void Initialise() { }

        public abstract new String ToString();
    }


    public class ComponentRegistry
    {
        private int next_signature;
        private object lockable;
        private Dictionary<Type, int> signature_bindings;

        private static ComponentRegistry instance;
        public static ComponentRegistry GetInstance()
        {
            if(instance == null) { instance = new ComponentRegistry(); }
            return instance;
        }
        private ComponentRegistry()
        {
            next_signature = 1;
            lockable = new();
            signature_bindings = new Dictionary<Type, int>();
        }

        public Type GetSignatureBinding(int signature)
        {
            foreach(KeyValuePair<Type, int> kvp in signature_bindings)
            {
                if(kvp.Value == signature)
                {
                    return kvp.Key;
                }
            }
            return null;
        }

        public int GetSignature(Type componentType)
        {
            if(componentType == null)
            {
                throw new ArgumentNullException(nameof(componentType));
            }
            if(componentType.BaseType != typeof(Component))
            {
                throw new ArgumentException("Signatures can only be given to component types");
            }

            lock (lockable)
            {
                bool found = signature_bindings.TryGetValue(componentType, out int collectedsignature);
                if (!found)
                {
                    int output = next_signature;
                    signature_bindings.Add(componentType, output);
                    Console.WriteLine($"Registered {componentType.FullName} to componentID:{output}");
                    next_signature *= 2;
                    return output;
                } else
                {
                    return collectedsignature;
                }
                
            }
        }
    }
}
