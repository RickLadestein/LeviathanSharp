using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    public enum AttributeType
    {
        POSITION,
        NORMAL,
        TANGENT,
        TEXTURE
    }

    public enum AttributeDataType
    {
        INT = Silk.NET.OpenGL.GLEnum.Int,
        FLOAT = Silk.NET.OpenGL.GLEnum.Float,
        DOUBLE = Silk.NET.OpenGL.GLEnum.Double
    }

    public enum DataCollectionType : uint
    {
        SINGULAR = 1U,
        VEC2 = 2U,
        VEC3 = 3U,
        VEC4 = 4U,
    }

    
}
