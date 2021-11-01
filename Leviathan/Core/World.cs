using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Leviathan.Core;
using Leviathan.Core.Graphics;

using Leviathan.Core.Input;
using Leviathan.Core.Windowing;
using Leviathan.ECS;

namespace Leviathan.Core
{
    public class World
    {
        private static World _instance;
        public static World Instance
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

        private Mutex entitymutex;
        private List<Entity> entities;

        public Camera PrimaryCam { get; private set; }

        private World()
        {
            entitymutex = new Mutex();
            entities = new List<Entity>();
            PrimaryCam = new Camera();
            Context.parent_window.refresh += Parent_window_refresh;
        }

        public bool AddEntity(Entity entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entitymutex.WaitOne();
            try
            {
                entities.Add(entity);
                return true;
            }
            catch (Exception ex) { 
                Console.WriteLine($"An error ocurred while removing an entity: {ex}");
                return false;
            }
            finally
            {
                entitymutex.ReleaseMutex();
            }
        }

        public bool RemoveEntity(Predicate<Entity> predicate)
        {
            entitymutex.WaitOne();
            try
            {
                int removed = entities.RemoveAll(predicate);
                return removed != 0 ? true : false;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"An error ocurred while removing an entity: {ex}");
                return false;
            }
            finally
            {
                entitymutex.ReleaseMutex();
            }
        }

        public List<Entity> FindEntity(Predicate<Entity> predicate)
        {
            entitymutex.WaitOne();
            List<Entity> results = new List<Entity>();
            try
            {
                foreach (Entity en in entities)
                {
                    if (predicate.Invoke(en))
                    {
                        results.Add(en);
                    }
                }
                return results;
            } catch (Exception ex) 
            { 
                Console.WriteLine($"An error ocurred while finding an entity: {ex}");
                return results;
            }
            finally
            {
                entitymutex.ReleaseMutex();
            }
            
        }
        private void Parent_window_refresh()
        {
            entitymutex.WaitOne();
            try
            {
                foreach(Entity en in entities)
                {
                    if(en.HasComponent<RenderComponent>())
                    {
                        en.GetComponent<RenderComponent>().Render(PrimaryCam);
                    }
                }
            } finally
            {
                entitymutex.ReleaseMutex();
            }
            
        }
    }
}
