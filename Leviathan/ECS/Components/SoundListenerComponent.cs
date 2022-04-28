using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Leviathan.Core;
using Leviathan.Math;

namespace Leviathan.ECS
{
    public class SoundListenerComponent : Component
    {
        public override string FriendlyName => "SoundListenerComponent";
        public Vector3f Position
        {
            get
            {
                Context.ALApi.GetListenerProperty(Silk.NET.OpenAL.ListenerVector3.Position, out float X, out float Y, out float Z);
                return new Vector3f(X, Y, Z);
            }
            set
            {
                Context.ALApi.SetListenerProperty(Silk.NET.OpenAL.ListenerVector3.Position, value.X, value.Y, value.Z);
            }
        }

        public Vector3f Velocity
        {
            get
            {
                Context.ALApi.GetListenerProperty(Silk.NET.OpenAL.ListenerVector3.Velocity, out float X, out float Y, out float Z);
                return new Vector3f(X, Y, Z);
            }
            set
            {
                Context.ALApi.SetListenerProperty(Silk.NET.OpenAL.ListenerVector3.Velocity, value.X, value.Y, value.Z);
            }
        }

        public Tuple<Vector3f, Vector3f> Orientation
        {
            get
            {
                float[] orientation = new float[3 * 2];
                GCHandle pinnedArray = GCHandle.Alloc(orientation, GCHandleType.Pinned);
                IntPtr pointer = pinnedArray.AddrOfPinnedObject();
                unsafe
                {
                    Context.ALApi.GetListenerProperty(Silk.NET.OpenAL.ListenerFloatArray.Orientation, (float*)pointer.ToPointer());
                }
                pinnedArray.Free();
                return new Tuple<Vector3f, Vector3f>(new Vector3f(orientation[0], orientation[1], orientation[2]), new Vector3f(orientation[3], orientation[4], orientation[5]));
            }
            set
            {
                float[] orientation = new float[] { value.Item1.X, value.Item1.Y, value.Item1.Z, value.Item2.X, value.Item2.Y, value.Item2.Z };
                GCHandle pinnedArray = GCHandle.Alloc(orientation, GCHandleType.Pinned);
                IntPtr pointer = pinnedArray.AddrOfPinnedObject();
                unsafe
                {
                    Context.ALApi.SetListenerProperty(Silk.NET.OpenAL.ListenerFloatArray.Orientation, (float*)pointer.ToPointer());
                }
                pinnedArray.Free();
            }
        }

        public float Gain
        {
            get
            {
                Context.ALApi.GetListenerProperty(Silk.NET.OpenAL.ListenerFloat.Gain, out float value);
                return value;
            }
            set
            {
                if(value < 0.0f)
                {
                    throw new ArgumentException("Gain value cannot be below zero [0.0f <-> float.PositiveInfinity]");
                }
                Context.ALApi.SetListenerProperty(Silk.NET.OpenAL.ListenerFloat.Gain, value);
            }
        }
        public SoundListenerComponent()
        {

        }

        public override void Initialise()
        {
            base.Initialise();
            //Parent.AddScript(new SoundListenerScript(this));
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
