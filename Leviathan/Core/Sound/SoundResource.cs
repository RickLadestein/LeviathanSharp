using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Sound
{
    public class SoundResource : IDisposable
    {
        public uint Handle;
        public static readonly uint EMPTY_HANDLE = 0;

        public SoundResource()
        {
            Handle = EMPTY_HANDLE;
        }

        public virtual void Dispose()
        {
            if (Handle != EMPTY_HANDLE)
            {
                throw new SoundResourceDisposeException("Resource still has an active handle to a resource in the Sound card and cannot " +
                    "be destroyed with the default Dispose operation");
            }
        }
    }

    public class SoundResourceDisposeException : Exception
    {
        public SoundResourceDisposeException(string msg) : base($"Graphics resource dispose error: {msg}") { }
    }
}
