using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.ECS.Systems
{
    public class RenderSystem : ECSystem
    {
        public override SystemPriority Priority => SystemPriority.RENDER;
        public override string FriendlyName => "RenderSystem";


        public RenderSystem()
        {

        }
        protected override void SystemFunc()
        {
            List<Entity> found = Core.World.Current.QueryEntityByComponent<RenderComponent>();
            foreach(Entity en in found)
            {
                en.GetComponent<RenderComponent>().Render(Core.World.Current.PrimaryCam);
            }
        }
    }
}
