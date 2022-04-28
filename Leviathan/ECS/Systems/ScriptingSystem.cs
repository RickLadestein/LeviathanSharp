using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.ECS.Systems
{
    public class ScriptingSystem : ECSystem
    {
        public override SystemPriority Priority => SystemPriority.PRE_RENDER;
        public override string FriendlyName => "ScriptingSystem";

        public ScriptingSystem()
        {
        }

        

        protected override void SystemFunc()
        {
            IEnumerator<Entity> enumerator = Core.World.Current.GetScriptableEntityEnumerator();
            do
            {
                Entity en = enumerator.Current;
                if(en == null) { continue; }
                foreach(MonoScript ms in en.Scripts)
                {
                    ms.Update();
                }
            } while(enumerator.MoveNext());
        }
    }
}
