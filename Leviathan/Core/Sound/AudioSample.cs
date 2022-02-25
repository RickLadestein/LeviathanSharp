using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Leviathan.Core.Sound
{
    public class AudioSample : SoundResource
    {
        public float Duration { get; private set; }
        public int Frequency { get; private set; }

        public unsafe AudioSample(WaveFile file)
        {
            this.Handle = Context.ALApi.GenBuffer();
            Silk.NET.OpenAL.BufferFormat bformat = (Silk.NET.OpenAL.BufferFormat)file.Format.GetSoundFormat();
            Frequency = (int)file.Format.sample_rate;
            Duration = (float)file.Data.data.Length / (float)(Frequency * (file.Format.bits_per_sample / 8) * file.Format.num_channels);
            GCHandle pinnedArray = GCHandle.Alloc(file.Data.data, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            Context.ALApi.BufferData(Handle, bformat, pointer.ToPointer(), file.Data.data.Length, Frequency);
            pinnedArray.Free();

        }

        public override void Dispose()
        {
            if (this.Handle != 0)
            {
                Context.ALApi.DeleteBuffer(Handle);
                this.Handle = 0;
            }
        }
    }
}
