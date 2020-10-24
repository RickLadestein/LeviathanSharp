using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.ECS.Components
{
    public class Velocity3DComponent : Component
    {
        public Vector3 Velocity { get; set; }
    }
}
