using Leviathan.Core;
using Leviathan.Util.util.Collections;
using Silk.NET.SDL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Assimp.Metadata;

namespace Leviathan.ECS
{
    internal class EntityStore
    {
        private List<Entity> all_entities;
        private List<Archetype> archetypes;
        public EntityStore()
        {
            all_entities = new List<Entity>();
            archetypes = new List<Archetype>();
        }

        public void Clear()
        {
            all_entities.Clear();
            archetypes.Clear();
        }

        public List<Entity> FindAll(Predicate<Entity> predicate)
        {
            return this.all_entities.FindAll(predicate);
        }

        public Entity Find(Predicate<Entity> predicate)
        {
            return this.all_entities.Find(predicate);
        }

        public Archetype GetPrimalArchetypeMatchingType(Type component_type)
        {
            foreach (Archetype ab in archetypes)
            {
                if (ab.HasArchetype(component_type))
                {
                    return ab;
                }
            }
            return null;
        }

        public Archetype[] GetAllArchetypesContainingType(params Type[] component_types)
        {
            List<Archetype> found = new List<Archetype>();
            foreach (Archetype ab in archetypes)
            {
                if (ab.HasArchetype(component_types))
                {
                    found.Add(ab);
                }
            }
            return found.ToArray();
        }
        public Archetype[] GetAllArchetypesContainingType<a1>()
            where a1 : Component
        {
            List<Archetype> found = new List<Archetype>();
            foreach (Archetype ab in archetypes)
            {
                if (ab.HasArchetype<a1>())
                {
                    found.Add(ab);
                }
            }
            return found.ToArray();
        }

        public Archetype GetArchetypeMatchingType(params Type[] component_types)
        {
            Archetype found = null;
            if(component_types.Length == 0) { throw new ArgumentException("Supplied types arguments cannot be empty"); }
            foreach(Archetype at in archetypes)
            {
                if(at.MatchesArchetype(component_types))
                {
                    found = at;
                    return found;
                }
            }
            return null;
        }

        public IEnumerator<Entity> GetEntityEnumerator()
        {
            return all_entities.GetEnumerator();
        }

        public Archetype GetArchetypeMatchingType<a1>()
            where a1 : Component
        {
            return GetArchetypeMatchingType(typeof(a1));
        }

        public Archetype GetArchetypeMatchingType<a1, a2>()
            where a1 : Component
            where a2 : Component
        {
            return GetArchetypeMatchingType(typeof(a1), typeof(a2));
        }

        public Archetype GetArchetypeMatchingType<a1, a2, a3>()
            where a1 : Component
            where a2 : Component
            where a3 : Component
        {
            return GetArchetypeMatchingType(typeof(a1), typeof(a2), typeof(a3));
        }

        public Archetype GetArchetypeMatchingType<a1, a2, a3, a4>()
            where a1 : Component
            where a2 : Component
            where a3 : Component
            where a4 : Component
        {
            return GetArchetypeMatchingType(typeof(a1), typeof(a2), typeof(a3), typeof(a4));
        }

        public bool CreateArchetype(params Type[] component_types)
        {
            if(GetArchetypeMatchingType(component_types) != null) { return false; }
            this.archetypes.Add(new Archetype(component_types));

            //primal
            bool created_new_archetype = false;
            foreach(Type t in component_types)
            {
                if(GetArchetypeMatchingType(t) == null)
                {
                    this.archetypes.Add(new Archetype(t));
                    created_new_archetype = true;
                }
            }

            if(component_types.Length > 1 && !created_new_archetype)
            {
                List<Entity> matched_entities = new List<Entity>();
                matched_entities.AddRange(GetArchetypeMatchingType(component_types[0]).entities);
                foreach (Type t in component_types)
                {
                    Archetype prim = GetArchetypeMatchingType(t);
                    List<Entity> found_intersections = new List<Entity>();
                    found_intersections.AddRange(matched_entities.Intersect(prim.entities));
                    matched_entities = found_intersections;
                }
            }
            return true;
        }

        public void AddEntity(Entity entity)
        {
            if(all_entities.Contains(entity)) { return; }
            
            this.all_entities.Add(entity);
            foreach(Component c in entity.Components)
            {
                CreateArchetype(c.GetType());
                Archetype[] found = GetAllArchetypesContainingType(c.GetType());
                foreach(Archetype at in found)
                {
                    at.AddEntity(entity);
                }
            }
        }

        public void RemoveEntity(Entity entity)
        {
            this.all_entities.Remove(entity);
            foreach(Component c in entity.Components)
            {
                Archetype found = GetArchetypeMatchingType(c.GetType());
                if(found != null)
                {
                    found.RemoveEntity(entity);
                }
            }
        }
        public void OnEntitycomponentAdded(Entity entity, Component comp)
        {
            Type component_type = comp.GetType();
            CreateArchetype(comp.GetType());
            Archetype[] found = GetAllArchetypesContainingType(component_type);
            foreach(Archetype ac in found)
            {
                ac.AddEntity(entity);
            }
        }

        public void OnEntityComponentRemoved(Entity entity, Component comp)
        {
            Type component_type = comp.GetType();
            Archetype[] found = GetAllArchetypesContainingType(component_type);
            foreach (Archetype ac in found)
            {
                ac.RemoveEntity(entity);
            }
        }
    }


    internal class Archetype
    {
        private const int MAX_SIGNATURES_IN_ARCHETYPE = 256;
        private BitArray contained_signatures;
        public UniqueList<Entity> entities;
        public bool IsPrimal { get; private set; }
        public int TypesCount
        {
            get; private set;
        }

        public Archetype(params Type[] types)
        {
            if(types.Length == 0) { throw new ArgumentException("types array cannot be empty"); }
            contained_signatures = new BitArray(new bool[MAX_SIGNATURES_IN_ARCHETYPE]);
            entities = new UniqueList<Entity>();
            IsPrimal = types.Length == 1 ? true : false;
            AddArchetypes(types);
        }

        public void AddEntity(Entity entity)
        {
            this.entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            this.entities.Remove(entity);
        }

        public bool HasArchetype(params Type[] types)
        {
            Type basetype = typeof(Component);
            foreach (Type curr in types)
            {
                if (curr.BaseType != basetype)
                {
                    throw new Exception("Archetype to search for must be a child class of Component");
                }
                if(!HasArchetype(curr))
                {
                    return false;
                }
                
            }
            return true;
        }

        public bool HasArchetype(Type component_type)
        {
            Type correct_base_type = typeof(Component);
            if(component_type.BaseType == correct_base_type)
            {
                int signature = ComponentRegistry.GetInstance().GetSignature(component_type);
                int index = (int)System.Math.Log2(signature);
                return contained_signatures[index];
            }
            return false;
        }

        public bool HasArchetype<T>()
        {
            return HasArchetype(typeof(T));
        }

        public bool HasArchetype<a1, a2>()
        {
            bool result1 = HasArchetype(typeof(a1));
            bool result2 = HasArchetype(typeof(a2));
            return result1 && result2;
        }

        public bool HasArchetype<a1, a2, a3>()
        {
            bool result1 = HasArchetype(typeof(a1));
            bool result2 = HasArchetype(typeof(a2));
            bool result3 = HasArchetype(typeof(a3));
            return result1 && result2 && result3;
        }

        public bool MatchesArchetype(params Type[] matching_types)
        {
            if(matching_types.Length != TypesCount) { return false; }
            return HasArchetype(matching_types);
        }

        public static Archetype Create<a1>()
            where a1 : Component
        {
            Archetype at = new();
            at.AddArchetype<a1>();
            return at;
        }

        public static Archetype Create<a1, a2>()
            where a1 : Component
            where a2 : Component
        {
            Archetype at = new();
            at.AddArchetype<a1>();
            at.AddArchetype<a2>();
            return at;
        }

        public static Archetype Create<a1, a2, a3>()
            where a1 : Component
            where a2 : Component
            where a3 : Component
        {
            Archetype at = new();
            at.AddArchetype<a1>();
            at.AddArchetype<a2>();
            at.AddArchetype<a3>();
            return at;
        }

        

        private void AddArchetype(Type type)
        {
            Type basetype = typeof(Component);
            if (type.BaseType != basetype)
            {
                throw new ArgumentException($"Argument type does not have Component as base");
            }
            int signature = ComponentRegistry.GetInstance().GetSignature(type);
            int index = (int)System.Math.Log2(signature);
            this.contained_signatures[index] = true;

            TypesCount += 1;
            this.IsPrimal = TypesCount == 1 ? true : false;
        }

        private void AddArchetype<at>() where at : Component
        {
            Type t = typeof(at);
            this.AddArchetype(typeof(at));

        }

        private void AddArchetypes(Type[] types)
        {
            Type basetype = typeof(Component);
            for (int i = 0; i < types.Length; i++)
            {
                AddArchetype(types[i]);
            }
        }

        
    }
}
