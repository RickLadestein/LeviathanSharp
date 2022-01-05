using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics
{
    public class GraphicsResource : IDisposable
    {
        public uint Handle;
        public static readonly uint EMPTY_HANDLE = 0;

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
