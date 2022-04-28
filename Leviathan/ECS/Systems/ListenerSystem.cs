using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.ECS.Systems
{
    public class ListenerSystem : ECSystem
    {
        public override string FriendlyName => "ListenerSystem";

        public override SystemPriority Priority => SystemPriority.PRE_RENDER;

        protected override void SystemFunc()
        {
            List<Entity> found = Core.World.Current.QueryEntityByComponent<SoundListenerComponent>();
            Entity parent_entity = found[0];
            SoundListenerComponent sound_listener_component = parent_entity.GetComponent<SoundListenerComponent>();

            if (sound_listener_component != null)
            {
                sound_listener_component.Position = parent_entity.Transform.Position;
                sound_listener_component.Orientation = new Tuple<Vector3f, Vector3f>(parent_entity.Transform.Forward, parent_entity.Transform.Up);
            }
        }
    }
}
