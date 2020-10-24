using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.ECS.Components.Physix
{
    class SphereCollider3D : Collider
    {
        public Vector3 Position { get; set; }
        public float Radius { get; set; }

        public float X_min { get { return this.Position.X - Radius; } private set { } }
        public float X_max { get { return this.Position.X + Radius; } private set { } }

        public float Y_min { get { return this.Position.Y - Radius; } private set { } }
        public float Y_max { get { return this.Position.X + Radius; } private set { } }

        public float Z_min { get { return this.Position.Z - Radius; } private set { } }
        public float Z_max { get { return this.Position.Z + Radius; } private set { } }

        public SphereCollider3D()
        {
            this.Position = new Vector3();
            this.Radius = 0;
        }
    }
}
