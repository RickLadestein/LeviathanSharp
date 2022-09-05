using System;
using System.Collections.Generic;
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
}
