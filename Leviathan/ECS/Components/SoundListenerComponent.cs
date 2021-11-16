using Leviathan.Core;
using Leviathan.Math;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.ECS
{
    public class SoundListenerComponent : Component
    {
        public Vector3f Position
        {
            get
            {
                AL.GetListener(ALListener3f.Position, out float X, out float Y, out float Z);
                return new Vector3f(X, Y, Z);
            }
            set
            {
                AL.Listener(ALListener3f.Position, value.X, value.Y, value.Z);
            }
        }

        public Vector3f Velocity
        {
            get
            {
                AL.GetListener(ALListener3f.Velocity, out float X, out float Y, out float Z);
                return new Vector3f(X, Y, Z);
            }
            set
            {
                AL.Listener(ALListener3f.Velocity, value.X, value.Y, value.Z);
            }
        }

        public Tuple<Vector3f, Vector3f> Orientation
        {
            get
            {
                AL.GetListener(ALListenerfv.Orientation, out OpenTK.Mathematics.Vector3 at, out OpenTK.Mathematics.Vector3 up);
                return new Tuple<Vector3f, Vector3f>(new Vector3f(at.X, at.Y, at.Z), new Vector3f(up.X, up.Y, up.Z));
            }
            set
            {
                float[] orientation = new float[] { value.Item1.X, value.Item1.Y, value.Item1.Z, value.Item2.X, value.Item2.Y, value.Item2.Z };
                AL.Listener(ALListenerfv.Orientation, orientation);
            }
        }

        public float Gain
        {
            get
            {
                AL.GetListener(ALListenerf.Gain, out float value);
                return value;
            }
            set
            {
                if(value < 0.0f)
                {
                    throw new ArgumentException("Gain value cannot be below zero [0.0f <-> float.PositiveInfinity]");
                }
                AL.Listener(ALListenerf.Gain, value);
            }
        }

        public float EFXMetersPerUnit
        {
            get
            {
                AL.GetListener(ALListenerf.EfxMetersPerUnit, out float value);
                return value;
            }
            set
            {
                AL.Listener(ALListenerf.EfxMetersPerUnit, value);
            }
        }
        public SoundListenerComponent()
        {

        }

        public override void Initialise()
        {
            base.Initialise();
            Parent.AddScript(new SoundListenerScript(this));
        }

        public override string ToString()
        {
            return "SoundListenerComponent";
        }
    }

    public class SoundListenerScript : MonoScript
    {
        private SoundListenerComponent listener_comp;
        public SoundListenerScript(SoundListenerComponent lcomp)
        {
            listener_comp = lcomp;
        }

        ~SoundListenerScript()
        {
            listener_comp = null;
        }

        public override void Update()
        {
            base.Update();
            if (listener_comp != null)
            {
                listener_comp.Position = entity.Transform.Position;
                listener_comp.Orientation = new Tuple<Vector3f, Vector3f>(entity.Transform.Forward, entity.Transform.Up);
            }
        }
    }
}
