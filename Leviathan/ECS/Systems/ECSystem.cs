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

        public ECSystem()
        {
            Enabled = true;
        }

        public void Execute()
        {
            if(Enabled)
            {
                SystemFunc();
            }
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
