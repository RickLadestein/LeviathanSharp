using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics
{
    public class GraphicsResource : IDisposable
    {
        public uint Handle { get; protected set; }
        
        public static readonly uint EMPTY_HANDLE = 0;

        public GraphicsResource()
        {
            Handle = EMPTY_HANDLE;
        }

        public virtual void Dispose()
        {
            if(Handle != EMPTY_HANDLE)
            {
                throw new GraphicsResourceDisposeException("Resource still has an active handle to a resource in the Graphics card and cannot " +
                    "be destroyed with the default Dispose operation");
            }
        }
    }

    public class GraphicsResourceDisposeException : Exception
    {
        public GraphicsResourceDisposeException(string msg) : base($"Graphics resource dispose error: {msg}") { }
    }
}
