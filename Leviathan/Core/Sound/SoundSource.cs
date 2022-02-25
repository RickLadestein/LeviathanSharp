using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Sound
{
    public class SoundSource : IDisposable
    {
        public uint Handle { get; private set; }

        #region floats
        /// <summary>
        /// Directional Source, inner cone angle, in degrees. Range: [0-360] Default: 360.
        /// </summary>
        public float ConeInnerAngle
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.ConeInnerAngle, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f || value > 360)
                {
                    throw new ArgumentException("ConeInnerAngle value must be within the [0 <-> 360] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.ConeInnerAngle, value);
            }
        }

        /// <summary>
        /// Directional Source, outer cone angle, in degrees. Range: [0-360] Default: 360.
        /// </summary>
        public float ConeOuterAngle
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.ConeInnerAngle, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f || value > 360)
                {
                    throw new ArgumentException("ConeOuterAngle value must be within the [0 <-> 360] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.ConeOuterAngle, value);
            }
        }

        /// <summary>
        /// Directional Source, outer cone gain. Default: 0.0f Range: [0.0f - 1.0] (Logarithmic).
        /// </summary>
        public float ConeOuterGain
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.ConeOuterGain, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f || value > 1.0f)
                {
                    throw new ArgumentException("ConeOuterGain value must be within the [0.0 <-> 1.0] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.ConeOuterGain, value);
            }
        }

        /// <summary>
        /// Indicate the gain (volume amplification) applied. Type: float. Range: [0.0f -
        /// ? ] A value of 1.0 means un-attenuated/unchanged. Each division by 2 equals an
        /// attenuation of -6dB. Each multiplicaton with 2 equals an amplification of +6dB.
        /// A value of 0.0f is meaningless with respect to a logarithmic scale; it is interpreted
        /// as zero volume - the channel is effectively disabled.
        /// </summary>
        public float Gain
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.Gain, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("Gain value must be within the [0.0 <-> float.PositiveInfinity] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.Gain, value);
            }
        }

        /// <summary>
        /// Indicate distance above which Sources are not attenuated using the inverse clamped
        /// distance model. Default: float.PositiveInfinity Type: float Range: [0.0f - float.PositiveInfinity].
        /// </summary>
        public float MaxDistance
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.MaxDistance, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("MaxDistance value must be within the [0.0 <-> float.PositiveInfinity] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.MaxDistance, value);
            }
        }

        /// <summary>
        /// Indicate maximum Source attenuation. Type: float Range: [0.0f - 1.0f] (Logarthmic).
        /// </summary>
        public float MaxGain
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.MaxGain, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f || value > 1.0f)
                {
                    throw new ArgumentException("MaxGain value must be within the [0.0 <-> 1.0] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.MaxGain, value);
            }
        }

        /// <summary>
        /// Indicate minimum Source attenuation. Type: float Range: [0.0f - 1.0f] (Logarthmic).
        /// </summary>
        public float MinGain
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.MinGain, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f || value > 1.0f)
                {
                    throw new ArgumentException("MinGain value must be within the [0.0 <-> 1.0] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.MinGain, value);
            }
        }

        /// <summary>
        /// Specify the pitch to be applied, either at Source, or on mixer results, at Listener.
        /// Range: [0.5f - 2.0f] Default: 1.0f. 
        /// </summary>
        public float Pitch
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.Pitch, out float value);
                return value;
            }
            set
            {
                if (value < 0.5f || value > 2.0f)
                {
                    throw new ArgumentException("Pitch value must be within the [0.5 <-> 2.0] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.Pitch, value);
            }
        }

        /// <summary>
        /// Source specific reference distance. Type: float Range: [0.0f - float.PositiveInfinity].
        /// At 0.0f, no distance attenuation occurs. Type: float Default: 1.0f.
        /// </summary>
        public float ReferenceDistance
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.ReferenceDistance, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("ReferenceDistance value must be within the [0.0 <-> float.PositiveInfinity] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.ReferenceDistance, value);
            }
        }

        /// <summary>
        /// Source specific rolloff factor. Type: float Range: [0.0f - float.PositiveInfinity].
        /// </summary>
        public float RolloffFactor
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.RolloffFactor, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("RolloffFactor value must be within the [0.0 <-> float.PositiveInfinity] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.RolloffFactor, value);
            }
        }

        /// <summary>
        /// The offset in seconds that the sound effect should start playing
        /// </summary>
        public float SecOffset
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.SecOffset, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("SecOffset value must be within the [0.0 <-> sample_duration] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceFloat.SecOffset, value);
            }
        }
        #endregion

        #region Ints
        /// <summary>
        /// The current byte position in the audio buffer
        /// </summary>
        public int ByteOffset
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.GetSourceInteger.ByteOffset, out int value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("ByteOffset value must be within the [0.0 <-> amount_of_sample_bytes] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceInteger.ByteOffset, value);
            }
        }

        /// <summary>
        /// The current sample position in the audio buffer
        /// </summary>
        public int SampleOffset
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.GetSourceInteger.SampleOffset, out int value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("SampleOffset value must be within the [0.0 <-> amount_of_sample_bytes] range");
                }
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceInteger.SampleOffset, value);
            }
        }
        #endregion

        #region Bools

        /// <summary>
        /// Indicates that the sound sample is playing on repeat
        /// </summary>
        public bool Looping
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceBoolean.Looping, out bool value);
                return value;
            }
            set
            {
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceBoolean.Looping, value);
            }
        }


        /// <summary>
        /// Indication that the sound source is relative to the listener's position
        /// </summary>
        public bool SourceRelative
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceBoolean.SourceRelative, out bool value);
                return value;
            }
            set
            {
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceBoolean.SourceRelative, value);
            }
        }
        #endregion

        #region Vector
        /// <summary>
        /// Specify the current location in three dimensional space. OpenAL, like OpenGL,
        /// uses a right handed coordinate system, where in a frontal default view X (thumb)
        /// points right, Y points up (index finger), and Z points towards the viewer/camera
        /// (middle finger). To switch from a left handed coordinate system, flip the sign
        /// on the Z coordinate. Listener position is always in the world coordinate system.
        /// </summary>
        public Vector3f Position
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceVector3.Position, out float X, out float Y, out float Z);
                return new Vector3f(X, Y, Z);
            }

            set
            {
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceVector3.Position, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Specify the current direction vector.
        /// </summary>
        public Vector3f Direction
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceVector3.Direction, out float X, out float Y, out float Z);
                return new Vector3f(X, Y, Z);
            }

            set
            {
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceVector3.Direction, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Specify the current velocity in three dimensional space.
        /// </summary>
        public Vector3f Velocity
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.SourceVector3.Velocity, out float X, out float Y, out float Z);
                return new Vector3f(X, Y, Z);
            }

            set
            {
                Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceVector3.Velocity, value.X, value.Y, value.Z);
            }
        }

        #endregion

        public Silk.NET.OpenAL.SourceState State
        {
            get
            {
                Context.ALApi.GetSourceProperty(Handle, Silk.NET.OpenAL.GetSourceInteger.SourceState, out int value);
                //OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALGetSourcei.SourceState, out int value);
                return (Silk.NET.OpenAL.SourceState)value;
            }
        }

        public SoundSource()
        {
            this.Handle = Context.ALApi.GenSource();
            //this.Handle = OpenTK.Audio.OpenAL.AL.GenSource();
        }

        public void Play(AudioSample sample)
        {
            if(sample == null)
            {
                throw new ArgumentNullException(nameof(sample));
            }

            this.Stop();
            Context.ALApi.SetSourceProperty(Handle, Silk.NET.OpenAL.SourceInteger.Buffer, sample.Handle);
            Context.ALApi.SourcePlay(Handle);
            //OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcei.Buffer, sample.Handle);
            //OpenTK.Audio.OpenAL.AL.SourcePlay(Handle);
        }

        public void Pause()
        {
            var state = this.State;
            if (state == Silk.NET.OpenAL.SourceState.Playing)
            {
                Context.ALApi.SourcePause(Handle);
                //OpenTK.Audio.OpenAL.AL.SourcePause(Handle);
            }
        }

        public void Stop()
        {
            var state = this.State;
            if (state == Silk.NET.OpenAL.SourceState.Playing || state == Silk.NET.OpenAL.SourceState.Paused)
            {
                Context.ALApi.SourceStop(Handle);
                //OpenTK.Audio.OpenAL.AL.SourceStop(Handle);
            }
        }

        public void Destroy()
        {
            if(this.Handle != 0)
            {
                Context.ALApi.DeleteSource(Handle);
                //OpenTK.Audio.OpenAL.AL.DeleteSource(this.Handle);
                this.Handle = 0;
            }
        }
        public void Dispose()
        {
            Destroy();
        }
    }
}
