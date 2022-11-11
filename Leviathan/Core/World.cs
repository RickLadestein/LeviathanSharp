using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Assimp;
using Leviathan.Core;
using Leviathan.Core.Graphics;

using Leviathan.Core.Input;
using Leviathan.Core.Windowing;
using Leviathan.ECS;
using Leviathan.ECS.Systems;
using Silk.NET.Assimp;

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

        private List<Entity> scripted_entities;
        private SystemRegistry system_registry;
        private EntityStore entity_store;


        public CameraComponent PrimaryCam { get; set; }

        private World()
        {
            ConstructRegistries();
            Context.ParentWindow.refresh += Parent_window_refresh;
        }

        private void ConstructRegistries()
        {
            scripted_entities = new List<Entity>();
            system_registry = new SystemRegistry();
            entity_store = new EntityStore();
            RenderSystem renderSystem = new RenderSystem();
            ScriptingSystem scriptingSystem = new ScriptingSystem();
            DelayedExecutionSystem dxsystem = new DelayedExecutionSystem();
            system_registry.AddSystems(renderSystem, scriptingSystem, dxsystem);
        }


        public void AddEntity(Entity entity)
        {
            entity_store.AddEntity(entity);
            entity.SetWorldEntityListener(this);
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
                //en.SetWorldEntityListener(this);
                AddEntity(en);
            }
        }

        public void Clear()
        {
            this.scripted_entities.Clear();
            this.entity_store.Clear();
        }
        private void Parent_window_refresh()
        {
            double start = Context.GLFWContext.GetTime();
            foreach (SystemPriority prio in (SystemPriority[])Enum.GetValues(typeof(SystemPriority)))
            {
                List<ECSystem> syslist = system_registry.GetSystemByPriority(prio);
                foreach (ECSystem s in syslist)
                {
                    if (s.Enabled)
                    {
                        s.Execute();
                    }
                }
            }
            double end = Context.GLFWContext.GetTime();
            double delta = end - start;
            double fps = 1.0d / delta;
            Context.ParentWindow.SetTitle($"{fps.ToString("0.##")} fps");
        }
        #region Interfaces
        public List<Entity> QueryEntitiesByIds(IEnumerable<Guid> ids)
        {
            List<Entity> retval = entity_store.FindAll(new Predicate<Entity>((e) => 
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
            List<Entity> retval = entity_store.FindAll(new Predicate<Entity>((e) =>
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
            Entity retval = entity_store.Find(new Predicate<Entity>((e) =>
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
            Entity retval = entity_store.Find(new Predicate<Entity>((e) =>
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

        public Entity[] QueryEntityByComponent<T>() where T : Component
        {
            string component_name = typeof(T).Name;
            Archetype found = entity_store.GetArchetypeMatchingType(typeof(T));
            return found.entities.ToArray();
        }

        public IEnumerator<Entity> GetEntityEnumerator()
        {
            return this.entity_store.GetEntityEnumerator();
        }

        public IEnumerator<Entity> GetScriptableEntityEnumerator()
        {
            return this.scripted_entities.GetEnumerator();
        }

        public bool HasSystem<T>() where T : ECSystem
        {
            return system_registry.HasSystem<T>();
        }

        public ECSystem GetSystem<T>() where T : ECSystem
        {
            return system_registry.GetSystem<T>();
        }

        public void OnComponentAdded(Entity caller, Component comp)
        {
            entity_store.OnEntitycomponentAdded(caller, comp);

        }

        public void OnComponentRemoved(Entity caller, Component comp)
        {
            entity_store.OnEntityComponentRemoved(caller, comp);
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

        public void OnEntityChildAdded(Entity caller, Entity child)
        {
            this.AddEntity(child);
            entity_store.AddEntity(child);
        }

        public void OnEntityChildRemoved(Entity caller, Entity child)
        {
            if(child.Scripts.Count != 0)
            {
                this.scripted_entities.Remove(child);
            }
            entity_store.RemoveEntity(child);
        }
        #endregion
    }

    public interface IWorldEntityProvider
    {
        public List<Entity> QueryEntitiesByIds(IEnumerable<Guid> ids);
        public List<Entity> QueryEntitiesByIds(IEnumerable<string> ids);
        public Entity QueryEntityById(Guid id);
        public Entity QueryEntityById(string id);

        public Entity[] QueryEntityByComponent<T>() where T : Component;

        public List<Entity> QueryEntityByPredicate(Predicate<Entity> predicate);
        public Entity QueryFirstEntityMatchByPredicate(Predicate<Entity> predicate);
    }

    public interface IWorldEntityListener
    {
        public void OnComponentAdded(Entity caller, Component comp);
        public void OnComponentRemoved(Entity caller, Component comp);

        public void OnScriptAdded(Entity caller, MonoScript script);

        public void OnScriptRemoved(Entity caller, MonoScript script);

        public void OnEntityChildAdded(Entity caller, Entity child);

        public void OnEntityChildRemoved(Entity caller, Entity child);
    }

    public class SystemRegistry
    {
        public HashSet<string> systemhashes;
        public Dictionary<SystemPriority, List<ECSystem>> systems;

        public SystemRegistry()
        {
            systemhashes = new HashSet<string>();
            systems = new Dictionary<SystemPriority, List<ECSystem>>();
        }

        public bool HasSystem<T>() where T : ECSystem
        {
            return systemhashes.Contains(typeof(T).Name);
        }

        public void AddSystems(params ECSystem[] systems)
        {
            foreach(ECSystem s in systems)
            {
                this.AddSystem(s);
            }
        }

        public void AddSystem(ECSystem system)
        {
            if(!systemhashes.Contains(system.GetType().Name))
            {
                systemhashes.Add(system.FriendlyName);
            } else
            {
                throw new Exception("System collision: cannot have more than 2 of the same systems");
            }

            if (!systems.ContainsKey(system.Priority)) {
                systems.Add(system.Priority, new List<ECSystem>());
            }
            systems[system.Priority].Add(system);
        }

        public ECSystem GetSystem<T>() where T : ECSystem
        {
            if(systemhashes.Contains(typeof(T).Name))
            {
                foreach(KeyValuePair<SystemPriority, List<ECSystem>> syslist in systems)
                {

                    ECSystem found = syslist.Value.Find(new Predicate<ECSystem>((e) => { return e is T; }));
                    if(found != null)
                    {
                        return found;
                    }
                }
            }
            return null;
        }

        public List<ECSystem> GetSystemByPriority(SystemPriority priority)
        {
            if(systems.ContainsKey(priority))
            {
                return systems[priority];
            } else
            {
                return new List<ECSystem>();
            }
        }

        public void RemoveSystem<T>() where T : ECSystem
        {
            if (systemhashes.Contains(typeof(T).Name))
            {
                foreach (KeyValuePair<SystemPriority, List<ECSystem>> syslist in systems)
                {
                    syslist.Value.RemoveAll(new Predicate<ECSystem>((e) => { return e is T; }));
                }
            }
        }
    }
}
