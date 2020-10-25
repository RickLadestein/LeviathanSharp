using System;

namespace Leviathan.Core
{
    public class Entity
    {
        public Guid Uuid { get; private set; }
        public Entity()
        {
            this.Uuid = Guid.NewGuid();
        }
    }
}
