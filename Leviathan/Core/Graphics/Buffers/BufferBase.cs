using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leviathan.Core.Graphics.Buffers
{
    public abstract class BufferBase : IDisposable
    {
        public uint Handle { get; protected set; }

        public abstract void CreateBuffer();

        public abstract void Dispose();
    }
}
