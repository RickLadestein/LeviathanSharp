using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.ECS.Components.Physix
{
    class BoxCollider3D : Collider
    {
        public Vector3 Start { get; set; }
        public Vector3 Dimensions { get; set; }

        public float X_min { get { return Start.X; } private set { } }
        public float X_max { get { return Start.X + Dimensions.X; } private set { } }

        public float Y_min { get { return Start.Y; } private set { } }
        public float Y_max { get { return Start.Y + Dimensions.Y; } private set { } }

        public float Z_min { get { return Start.Z; } private set { } }
        public float Z_max { get { return Start.Z + Dimensions.Z; } private set { } }

        public BoxCollider3D()
        {
            this.Start = new Vector3();
            this.Dimensions = new Vector3();
        }
    }
}
