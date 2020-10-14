using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace Leviathan.Core.ECS.Components
{
    class Transform3DComponent
    {
        /// <summary>
        /// Position in cartesian float coordinates
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Rotation in floating point radians
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Scale in floating point units
        /// </summary>
        public Vector3 Scale { get; set; }
    }
}
