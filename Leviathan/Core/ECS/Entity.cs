using Leviathan.Core.ECS.Components;
using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.ECS
{
    public class Entity : Transform2DComponent
    {
        public Guid Uuid { get; private set; }
        public Entity()
        {
            this.Uuid = Guid.NewGuid();
        }
    }
}
