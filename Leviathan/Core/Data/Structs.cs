using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Leviathan.Core.Data
{
    struct VertexData
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector3 texture;

        public VertexData(Vector3 pos, Vector3 norm, Vector3 tex)
        {
            this.position = pos;
            this.normal = norm;
            this.texture = tex;
        }

        public static int GetSize()
        {
            return Marshal.SizeOf<VertexData>();
        }
    }

    struct Primitive
    {
        public VertexData v1;
        public VertexData v2;
        public VertexData v3;

        public Primitive(VertexData v1, VertexData v2, VertexData v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }
}
