using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics.Buffers.VertexBufferAttributes
{
    public enum LAttributeType
    {
        POSITION,
        NORMAL,
        TANGENT,
        COLOR,
        TEXTURECOORD0,
        TEXTURECOORD1,
        TEXTURECOORD2,
        TEXTURECOORD3,
        TEXTURECOORD4,
        TEXTURECOORD5,
        TEXTURECOORD6,
        TEXTURECOORD7,
        CUSTOM
    }

    public enum LAttributeDataType
    {
        INT = Silk.NET.OpenGL.GLEnum.Int,
        UINT = Silk.NET.OpenGL.GLEnum.UnsignedInt,
        FLOAT = Silk.NET.OpenGL.GLEnum.Float,
        DOUBLE = Silk.NET.OpenGL.GLEnum.Double
    }

    public enum LDataCollectionType : uint
    {
        SINGULAR = 1U,
        VEC2 = 2U,
        VEC3 = 3U,
        VEC4 = 4U,
    }

    
}
