using Leviathan.Core;
using Leviathan.Core.Graphics;
using Leviathan.Core.Graphics.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.ECS
{
    public class MeshComponent : Component
    {
        public Mesh Mesh { get; private set; }

        public MeshComponent() : base()
        {
            SetMesh("Cube");
        }

        public void SetMesh(string identifier)
        {
            //if(Vbuffer.handle != 0)
            //{
            //    Vbuffer.PurgeBuffer();
            //}

            Mesh = Context.MeshManager.GetResource(identifier);
            if (Mesh == null)
            {
                throw new Exception($"Could not find mesh resource with identifier: {identifier}");
            }
        }


        public override string ToString()
        {
            return "MeshComponent";
        }
    }
}
