using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Leviathan.Core;
using Leviathan.Core.Graphics;

using Leviathan.Core.Input;
using Leviathan.Core.Windowing;
using Leviathan.ECS;
using Leviathan.ECS.Systems;

namespace Leviathan.Core
{
    public class World : IWorldEntityProvider, IWorldEntityListener
    {
        private static World _instance;
        public static World Current
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new World();
                }
                return _instance;
            }
        }

        private List<Entity> all_entities;
        private List<Entity> scripted_entities;
        private ComponentRegistry component_registry;

        RenderSystem renderSystem;
        ScriptingSystem scriptingSystem;
        public CameraComponent PrimaryCam { get; set; }

        private World()
        {
            renderSystem = new RenderSystem();
            scriptingSystem = new ScriptingSystem();
            all_entities = new List<Entity>();
            ConstructRegistries();
            Context.ParentWindow.refresh += Parent_window_refresh;
        }

        private void ConstructRegistries()
        {

            scripted_entities = new List<Entity>();
            component_registry = new ComponentRegistry();

            if(all_entities.Count != 0)
            {
                foreach(Entity entity in all_entities)
                {
                    if(entity.Scripts.Count != 0)
                    {
                        scripted_entities.Add(entity);
                    }

                    foreach(Component comp in entity.Components)
                    {
                        component_registry.OnComponentAdded(entity, comp);
                    }
                }
            }
        }


        public void AddEntity(Entity entity)
        {
            all_entities.Add(entity);
            if (entity.Scripts.Count != 0)
            {
                scripted_entities.Add(entity);
            }

            foreach (Component comp in entity.Components)
            {
                component_registry.OnComponentAdded(entity, comp);
            }
        }

        public void LoadScene(Scene scene)
        {
            this.Clear();
            AddScene(scene);
        }

        public void AddScene(Scene scene)
        {
            foreach (Entity en in scene.Entities)
            {
                AddEntity(en);
            }
        }

        public void Clear()
        {
            this.all_entities.Clear();
            this.scripted_entities.Clear();
        }
        private void Parent_window_refresh()
        {
            scriptingSystem.Execute();
            renderSystem.Execute();
            
        }
        #region Interfaces
        public List<Entity> QueryEntitiesByIds(IEnumerable<Guid> ids)
        {
            List<Entity> retval = all_entities.FindAll(new Predicate<Entity>((e) => 
            { 
                foreach(Guid id in ids)
                {
                    if(e.Id.Equals(id))
                    {
                        return true;
                    }
                }
                return false;
            }));
            return retval;
        }

        public List<Entity> QueryEntitiesByIds(IEnumerable<string> ids)
        {
            List<Guid> tofind = new List<Guid>();
            foreach(string id in ids)
            {
                tofind.Add(new Guid(id));
            }
            List<Entity> retval = all_entities.FindAll(new Predicate<Entity>((e) =>
            {
                foreach (Guid id in tofind)
                {
                    if (e.Id.Equals(ids))
                    {
                        return true;
                    }
                }
                return false;
            }));
            return retval;
        }

        public Entity QueryEntityById(Guid id)
        {
            Entity retval = all_entities.Find(new Predicate<Entity>((e) =>
            {
                if (e.Id.Equals(id))
                {
                    return true;
                }
                return false;
            }));
            return retval;
        }

        public Entity QueryEntityById(string id)
        {
            Guid compare = new Guid(id);
            Entity retval = all_entities.Find(new Predicate<Entity>((e) =>
            {
                if (e.Id.Equals(compare))
                {
                    return true;
                }
                return false;
            }));
            return retval;
        }

        
        public List<Entity> QueryEntityByPredicate(Predicate<Entity> predicate)
        {
            throw new NotImplementedException();
        }

        public Entity QueryFirstEntityMatchByPredicate(Predicate<Entity> predicate)
        {
            throw new NotImplementedException();
        }

        public List<Entity> QueryEntityByComponent<T>() where T : Component
        {
            string component_name = typeof(T).Name;
            if(component_registry.componenthashes.Contains(component_name))
            {
                return new List<Entity>(component_registry.collected_entities[component_name].Values);
            } else
            {
                return new List<Entity>();
            }

        }

        public IEnumerator<Entity> GetEntityEnumerator()
        {
            return this.all_entities.GetEnumerator();
        }

        public IEnumerator<Entity> GetScriptableEntityEnumerator()
        {
            return this.scripted_entities.GetEnumerator();
        }

        public void OnComponentAdded(Entity caller, Component comp)
        {
            if(all_entities.Contains(caller))
            {
                component_registry.OnComponentAdded(caller, comp);
            }
        }

        public void OnComponentRemoved(Entity caller, Component comp)
        {
            if (all_entities.Contains(caller))
            {
                component_registry.OnComponentRemoved(caller, comp);
            }
        }

        public void OnScriptAdded(Entity caller, MonoScript script)
        {
            if(!scripted_entities.Contains(caller))
            {
                scripted_entities.Add(caller);
            }
        }

        public void OnScriptRemoved(Entity caller, MonoScript script)
        {
            if (scripted_entities.Contains(caller))
            {
                scripted_entities.Remove(caller);
            }
        }
        #endregion
    }

    public interface IWorldEntityProvider
    {
        public List<Entity> QueryEntitiesByIds(IEnumerable<Guid> ids);
        public List<Entity> QueryEntitiesByIds(IEnumerable<string> ids);
        public Entity QueryEntityById(Guid id);
        public Entity QueryEntityById(string id);

        public List<Entity> QueryEntityByComponent<T>() where T : Component;

        public List<Entity> QueryEntityByPredicate(Predicate<Entity> predicate);
        public Entity QueryFirstEntityMatchByPredicate(Predicate<Entity> predicate);
    }

    public interface IWorldEntityListener
    {
        public void OnComponentAdded(Entity caller, Component comp);
        public void OnComponentRemoved(Entity caller, Component comp);

        public void OnScriptAdded(Entity caller, MonoScript script);

        public void OnScriptRemoved(Entity caller, MonoScript script);
    }

    public class ComponentRegistry
    {
        public HashSet<string> componenthashes;
        public Dictionary<string, Dictionary<Guid, Entity>> collected_entities;

        public ComponentRegistry()
        {
            componenthashes = new HashSet<string>();
            collected_entities = new Dictionary<string, Dictionary<Guid, Entity>>();
        }

        public void OnComponentAdded(Entity caller, Component comp)
        {
            if(!componenthashes.Contains(comp.FriendlyName))
            {
                componenthashes.Add(comp.FriendlyName);

                Dictionary<Guid, Entity> toadd = new Dictionary<Guid, Entity>();
                toadd.Add(caller.Id, caller);
                collected_entities.Add(comp.FriendlyName, toadd);
            }
            else
            {
                collected_entities[comp.FriendlyName].Add(caller.Id, caller);
            }
        }

        public void OnComponentRemoved(Entity caller, Component comp)
        {
            if (componenthashes.Contains(comp.FriendlyName))
            {
                collected_entities[comp.FriendlyName].Remove(caller.Id);
            }
        }
    }

    public class SystemRegistry
    {
        public Dictionary<SystemPriority, List<ECSystem>> Systems;

        public void AddSystem()
        {

        }

        public ECSystem GetSystem<T>() where T : ECSystem
        {
            return default(ECSystem);
        }

        public void RemoveSystem()
        {

        }
    }
}
