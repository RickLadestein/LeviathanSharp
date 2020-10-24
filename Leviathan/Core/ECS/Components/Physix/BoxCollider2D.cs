using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.ECS.Components.Physix
{
    class BoxCollider2D : Collider
    {
        public Vector2 Start { get; set; }
        public Vector2 Dimensions { get; set; }

        public float X_min { get { return Start.X; } private set { } }
        public float X_max { get { return Start.X + Dimensions.X; } private set { } }

        public float Y_min { get { return Start.Y; } private set { } }
        public float Y_max { get { return Start.Y + Dimensions.Y; } private set { } }


        public BoxCollider2D()
        {
            this.Start = new Vector2();
            this.Dimensions = new Vector2();
        }
    }
}
