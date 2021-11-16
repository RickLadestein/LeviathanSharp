using Leviathan.Core.Sound;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;

namespace Leviathan.ECS
{
    public class SoundSourceComponent : Component
    {
        public List<SoundSource> sources;
        public Mutex sources_mutex;
        public SoundSourceComponent()
        {
            sources_mutex = new Mutex();
            sources = new List<SoundSource>();
        }

        ~SoundSourceComponent()
        {
        }

        public override void Initialise()

        {
            base.Initialise();
            Parent.AddScript(new SoundSourceScript(this));
        }

        public void Play(AudioSample sample)
        {
            try
            {
                sources_mutex.WaitOne();
                SoundSource src = new SoundSource();
                src.Play(sample);
                sources.Add(src);
            } finally
            {
                sources_mutex.ReleaseMutex();
            }
            
        }

        public void Play(SoundSource source, AudioSample sample)
        {
            try
            {
                sources_mutex.WaitOne();
                source.Play(sample);
                sources.Add(source);
            }
            finally
            {
                sources_mutex.ReleaseMutex();
            }
        }

        public override string ToString()
        {
            return "SoundSourceComponent";
        }
    }

    public class SoundSourceScript : MonoScript
    {
        private SoundSourceComponent scomp;
        

        public SoundSourceScript(SoundSourceComponent comp)
        {
            scomp = comp;
        }

        ~SoundSourceScript()
        {
            scomp = null;
        }

        public override void Update()
        {
            base.Update();
            if (scomp != null)
            {
                try
                {
                    scomp.sources_mutex.WaitOne();
                    if (scomp.sources.Count != 0)
                    {
                        scomp.sources.RemoveAll(
                            new Predicate<SoundSource>((e) => { return e.State == Silk.NET.OpenAL.SourceState.Stopped; })
                            );

                        foreach (SoundSource src in scomp.sources)
                        {
                            src.Position = entity.Transform.Position;
                            src.Direction = entity.Transform.Forward;
                        }
                    }
                }
                finally
                {
                    scomp.sources_mutex.ReleaseMutex();
                }
            }
        }
    }
}
