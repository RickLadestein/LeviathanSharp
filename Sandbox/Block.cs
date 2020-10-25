using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Core;
using Leviathan.Core.Graphics;
using OpenTK.Mathematics;

namespace Sandbox
{
    public class Block : RenderableEntity
    {
        public Block()
        {
            this.Position = Vector3.UnitZ * 3;
        }
    }
}
