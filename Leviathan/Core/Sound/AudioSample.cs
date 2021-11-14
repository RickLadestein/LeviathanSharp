﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Leviathan.Core.Sound
{
    public class AudioSample : IDisposable
    {
        public int Handle { get; private set; }
        public float Duration { get; private set; }
        public int Frequency { get; private set; }


        public unsafe AudioSample(WaveFile file)
        {
            this.Handle = OpenTK.Audio.OpenAL.AL.GenBuffer();
            OpenTK.Audio.OpenAL.ALFormat bformat = file.Format.GetSoundFormat();
            Frequency = (int)file.Format.sample_rate;
            Duration = (float)file.Data.data.Length / (float)(Frequency * (file.Format.bits_per_sample / 8) * file.Format.num_channels);
            GCHandle pinnedArray = GCHandle.Alloc(file.Data.data, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            OpenTK.Audio.OpenAL.AL.BufferData(this.Handle, bformat, pointer.ToPointer(), file.Data.data.Length, Frequency);
            pinnedArray.Free();
        }

        public void Destroy()
        {
            if(this.Handle != 0)
            {
                OpenTK.Audio.OpenAL.AL.DeleteBuffer(Handle);
                this.Handle = 0;
            }
        }

        public void Dispose()
        {
            this.Destroy();
        }
    }
}
