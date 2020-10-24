using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.ECS.Components.Physix
{
    class SphereCollider2D : Collider
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }

        public float X_min { get { return this.Position.X - Radius; } private set { } }
        public float X_max { get { return this.Position.X + Radius; } private set { } }

        public float Y_min { get { return this.Position.Y - Radius; } private set { } }
        public float Y_max { get { return this.Position.Y + Radius; } private set { } }

        public SphereCollider2D()
        {
            this.Position = new Vector2();
            this.Radius = 0;
        }

        /// <summary>
        /// Checks intersection with other SphereCollider2D and returns the smallest exit vector on intersection
        /// </summary>
        /// <param name="other">The other SphereCollider2D to check intersection against</param>
        /// <returns>Tuple containing collision boolean and closest exit vector</returns>
        public Tuple<bool, Vector2> Intersects(SphereCollider2D other)
        {
            double distance = Vector2.Distance(this.Position, other.Position);
            double combined_radius = (this.Radius + other.Radius);

            bool intersect = distance <= combined_radius;
            Vector2 exit = Vector2.Zero;
            if(intersect)
            {
                float exit_ln = (float)(combined_radius - distance);
                exit = Vector2.Normalize(this.Position - other.Position) * exit_ln;
            }
            return new Tuple<bool, Vector2>(intersect, exit);
        }

        public Tuple<bool, Vector2> Intersects(BoxCollider2D other)
        {
            return new Tuple<bool, Vector2>(false, Vector2.Zero);
        }


    }
}
