using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Sound
{
    public class SoundResource : IDisposable
    {
        public uint Handle;
        public static readonly uint EMPTY_HANDLE = 0;

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
