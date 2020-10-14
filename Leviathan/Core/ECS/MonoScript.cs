using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.ECS
{
    public abstract class MonoScript
    {
        private bool enabled;
        public bool Enabled { 
            get { 
                return enabled; 
            } 
            set { 
                if (!this.enabled && value) { this.enabled = true; this.OnEnable(); }
                else if (this.enabled && !value) { this.enabled = false; this.OnDisable(); }
            }
        }

        public ScriptPriority Priority { get; set; }

        public MonoScript()
        {
            this.OnCreate();
            this.Priority = ScriptPriority.DEFAULT;
        }

        public MonoScript(ScriptPriority priority)
        {
            this.OnCreate();
            this.Priority = priority;
        }

        ~MonoScript()
        {
            this.OnDestroy();
        }

        public abstract void OnUpdate(float delta, Entity parent);

        public abstract void OnCreate();
        public abstract void OnDestroy();

        public abstract void OnEnable();
        public abstract void OnDisable();
    }

    public enum ScriptPriority
    {
        HIGH,
        DEFAULT,
        LOW
    }
}
