using Leviathan.Core;
using Leviathan.Util.util.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.ECS.Systems
{
    public abstract class ECSystem
    {
        public bool Enabled { get; private set; }
        public abstract string FriendlyName { get; }
        public abstract SystemPriority Priority { get; }

        public UniqueList<Type> Required_types;
        public ECSystem()
        {
            Enabled = true;
            Required_types = new UniqueList<Type>();
        }

        public void Execute()
        {
            if(Enabled)
            {
                SystemFunc();
            }
        }

        protected void AddRequirementType(Type type)
        {
            Type basetype = typeof(Component);
            if(type.BaseType != basetype)
            {
                throw new ArgumentException("Cannot add requiredtype because the specified type is not a derivative of Component");
            }
            this.Required_types.Add(type);
        }

        protected abstract void SystemFunc();
    }

    public enum SystemPriority
    {
        PRE_RENDER,
        RENDER,
        POST_RENDER
    }
}
