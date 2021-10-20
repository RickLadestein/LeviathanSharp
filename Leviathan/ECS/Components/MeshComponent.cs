using Leviathan.Core.Graphics;
using Leviathan.Core.Graphics.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.ECS
{
    public class MeshComponent : Component
    {
        public VertexBuffer Vbuffer;

        public MeshComponent() : base()
        {
            Vbuffer = new VertexBuffer();
            SetMesh("Cube");
        }

        public void SetMesh(string identifier)
        {
            if(Vbuffer.handle != 0)
            {
                Vbuffer.PurgeBuffer();
            }

            Mesh mesh = Util.MeshResourceManager.Instance.GetResource(identifier);
            if (mesh == null)
            {
                throw new Exception($"Could not find mesh resource with identifier: {identifier}");
            } else
            {
                Vbuffer.LoadDataBuffers(mesh);
            }
        }


        public override string ToString()
        {
            return "MeshComponent";
        }
    }
}
