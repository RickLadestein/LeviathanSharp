using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.ECS
{
    public class Entity
    {
        private List<Component> components;

        /// <summary>
        /// The scripts that are bound to the entity
        /// </summary>
        public List<MonoScript> Scripts { get; private set; }

        /// <summary>
        /// The identifier of this Entity
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The custom name of this Entity
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The current transform of this entity
        /// </summary>
        public Transform Transform;

        /// <summary>
        /// The children this entity has
        /// </summary>
        public List<Entity> Children { get; private set; }

        public Entity Parent { get; private set; }

        /// <summary>
        /// Creates a new instance of Entity with default parameters
        /// </summary>
        public Entity()
        {
            components = new List<Component>();
            Scripts = new List<MonoScript>();
            Id = Guid.NewGuid();
            Name = "Entity";
            Transform = new Transform();
            Children = new List<Entity>();
            Transform.Reset();
        }

        /// <summary>
        /// Creates a new instance of Entity with custom name and default parameters
        /// </summary>
        /// <param name="name">The name that is to be assigned to the Entity</param>
        public Entity(String name)
        {
            if (name.Length == 0)
            {
                throw new ArgumentException("Entity name cannot be empty");
            }
            else
            {
                components = new List<Component>();
                Id = Guid.NewGuid();
                Name = name;
                Transform = new Transform();
                Children = new List<Entity>();
                Scripts = new List<MonoScript>();
                Transform.Reset();
            }
        }

        /// <summary>
        /// Gets the concatinated model matrices from the parents and the current transform 
        /// </summary>
        /// <returns>Model matrix describing the translation/rotation/scale of the current entity relative to the world</returns>
        public Mat4 GetParentedModelMat()
        {
            Mat4 result = Mat4.Identity;
            result *= Transform.ModelMat;
            if(Parent != null)
            {
                result *= Parent.GetParentedModelMat();
            }
            return result;
        }

        /// <summary>
        /// Adds a child Entity to this Entity
        /// </summary>
        /// <param name="child">A to be child Entity</param>
        public void AddChild(Entity child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        /// <summary>
        /// Adds a collection of children Entities to this Entity
        /// </summary>
        /// <param name="children">A collection of to be children entities</param>
        public void AddChildren(List<Entity> children)
        {
            if (children != null)
            {
                foreach (Entity child in children)
                {
                    this.AddChild(child);
                }
            }
        }

        /// <summary>
        /// Sets the parent Entity of this Entity
        /// </summary>
        /// <param name="_newparent">The Entity that this Entity is the child of</param>
        public void SetParent(Entity _newparent)
        {
            this.Parent = _newparent;
        }

        public void SetTransform(Transform _newtrans)
        {
            this.Transform = _newtrans;
            this.Transform.Parent = this;
        }
        

        /// <summary>
        /// Adds a component to this Entity
        /// </summary>
        /// <param name="c">The component to add</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddComponent(Component c)
        {
            if (c == null)
            {
                throw new ArgumentNullException("Component cannot be null");
            }
            else
            {
                c.parent = this;
                c.Initialise();
                components.Add(c);
            }
        }

        /// <summary>
        /// Adds a collection of components to this Entity
        /// </summary>
        /// <param name="_components">The components to add</param>
        public void AddComponents(List<Component> _components)
        {
            if(_components != null)
            {
                foreach(Component c in _components)
                {
                    this.AddComponent(c);
                }
            }
        }

        /// <summary>
        /// Retrieves the first found component that matches the search parameter.
        /// </summary>
        /// <typeparam name="T">The component that is to be found</typeparam>
        /// <returns>Found component, if not found returns <c>null</c></returns>
        public T GetComponent<T>() where T : Component
        {
            foreach (Component c in components)
            {
                if (c != null && c is T t)
                {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves all components that match the search parameter
        /// </summary>
        /// <typeparam name="T">The type of component that is to be found</typeparam>
        /// <param name="comps">A list where found c</param>
        public void GetComponents<T>(ref List<T> comps) where T : Component
        {
            foreach (Component c in components)
            {
                if (c != null && c is T t)
                {
                    comps.Add(t);
                }
            }
        }

        /// <summary>
        /// A function which checks whether the entity has components
        /// </summary>
        /// <typeparam name="T">The type of the component which should be checked</typeparam>
        /// <returns>A boolean depending on the result</returns>
        public bool HasComponent<T>() where T : Component
        {
            foreach (Component c in components)
            {
                if (c != null && c is T t)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Adds a script to this Entity
        /// </summary>
        /// <param name="c">The script to add</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddScript(MonoScript c)
        {
            if (c == null)
            {
                throw new ArgumentNullException("Component cannot be null");
            }
            else
            {
                c.entity = this;
                Scripts.Add(c);
            }
        }

        /// <summary>
        /// Retrieves the first found script that matches the search parameter.
        /// </summary>
        /// <typeparam name="T">The script that is to be found</typeparam>
        /// <returns>Found script, if not found returns <c>null</c></returns>
        public T GetScript<T>() where T : MonoScript
        {
            foreach (MonoScript c in Scripts)
            {
                if (c != null && c is T t)
                {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves all scripts that match the search parameter
        /// </summary>
        /// <typeparam name="T">The type of script that is to be found</typeparam>
        /// <param name="comps">A list where found scripts are stored</param>
        public void GetScripts<T>(ref List<T> comps) where T : MonoScript
        {
            foreach (Component c in components)
            {
                if (c != null && c is T t)
                {
                    comps.Add(t);
                }
            }
        }

        /// <summary>
        /// A function which checks whether the entity has a script
        /// </summary>
        /// <typeparam name="T">The type of the script which should be checked</typeparam>
        /// <returns>A boolean depending on the result</returns>
        public bool HasScript<T>() where T : MonoScript
        {
            foreach (MonoScript m in Scripts)
            {
                if (m != null && m is T t)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class EntityFactory
    {
        private Entity target;

        public EntityFactory()
        {
            target = new Entity();
        }

        public EntityFactory SetEntityName(String _name)
        {
            target.Name = _name;
            return this;
        }

        public EntityFactory SetEntityTransform(Transform _transform)
        {
            target.Transform = _transform;
            return this;
        }

        public EntityFactory SetEntityParent(Entity _parent)
        {
            target.SetParent(_parent);
            return this;
        }

        public EntityFactory AddEntityChildren(List<Entity> _children)
        {
            target.AddChildren(_children);
            return this;
        }

        public EntityFactory AddEntityChild(Entity _child)
        {
            target.AddChild(_child);
            return this;
        }

        public EntityFactory AddEntityComponents(List<Component> _components)
        {
            //target.AddComponent(_components)
            return this;
        }
    }
}
