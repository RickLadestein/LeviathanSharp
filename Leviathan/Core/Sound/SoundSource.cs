using Leviathan.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Sound
{
    public class SoundSource : IDisposable
    {
        public int Handle { get; private set; }

        #region floats
        /// <summary>
        /// Directional Source, inner cone angle, in degrees. Range: [0-360] Default: 360.
        /// </summary>
        public float ConeInnerAngle
        {
            get
            {
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourcef.ConeInnerAngle, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f || value > 360)
                {
                    throw new ArgumentException("ConeInnerAngle value must be within the [0 <-> 360] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcef.ConeInnerAngle, value);
            }
        }

        /// <summary>
        /// Directional Source, outer cone angle, in degrees. Range: [0-360] Default: 360.
        /// </summary>
        public float ConeOuterAngle
        {
            get
            {
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourcef.ConeOuterAngle, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f || value > 360)
                {
                    throw new ArgumentException("ConeOuterAngle value must be within the [0 <-> 360] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcef.ConeOuterAngle, value);
            }
        }

        /// <summary>
        /// Directional Source, outer cone gain. Default: 0.0f Range: [0.0f - 1.0] (Logarithmic).
        /// </summary>
        public float ConeOuterGain
        {
            get
            {
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourcef.ConeOuterGain, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f || value > 1.0f)
                {
                    throw new ArgumentException("ConeOuterGain value must be within the [0.0 <-> 1.0] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcef.ConeOuterGain, value);
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
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourcef.Gain, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("Gain value must be within the [0.0 <-> float.PositiveInfinity] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcef.Gain, value);
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
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourcef.MaxDistance, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("MaxDistance value must be within the [0.0 <-> float.PositiveInfinity] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcef.MaxDistance, value);
            }
        }

        /// <summary>
        /// Indicate maximum Source attenuation. Type: float Range: [0.0f - 1.0f] (Logarthmic).
        /// </summary>
        public float MaxGain
        {
            get
            {
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourcef.MaxGain, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f || value > 1.0f)
                {
                    throw new ArgumentException("MaxGain value must be within the [0.0 <-> 1.0] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcef.MaxGain, value);
            }
        }

        /// <summary>
        /// Indicate minimum Source attenuation. Type: float Range: [0.0f - 1.0f] (Logarthmic).
        /// </summary>
        public float MinGain
        {
            get
            {
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourcef.MinGain, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f || value > 1.0f)
                {
                    throw new ArgumentException("MinGain value must be within the [0.0 <-> 1.0] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcef.MinGain, value);
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
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourcef.Pitch, out float value);
                return value;
            }
            set
            {
                if (value < 0.5f || value > 2.0f)
                {
                    throw new ArgumentException("Pitch value must be within the [0.5 <-> 2.0] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcef.Pitch, value);
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
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourcef.ReferenceDistance, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("ReferenceDistance value must be within the [0.0 <-> float.PositiveInfinity] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcef.ReferenceDistance, value);
            }
        }

        /// <summary>
        /// Source specific rolloff factor. Type: float Range: [0.0f - float.PositiveInfinity].
        /// </summary>
        public float RolloffFactor
        {
            get
            {
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourcef.RolloffFactor, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("RolloffFactor value must be within the [0.0 <-> float.PositiveInfinity] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcef.RolloffFactor, value);
            }
        }

        /// <summary>
        /// The offset in seconds that the sound effect should start playing
        /// </summary>
        public float SecOffset
        {
            get
            {
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourcef.SecOffset, out float value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("SecOffset value must be within the [0.0 <-> sample_duration] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcef.SecOffset, value);
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
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALGetSourcei.ByteOffset, out int value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("ByteOffset value must be within the [0.0 <-> amount_of_sample_bytes] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcei.ByteOffset, value);
            }
        }

        /// <summary>
        /// The current sample position in the audio buffer
        /// </summary>
        public int SampleOffset
        {
            get
            {
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALGetSourcei.SampleOffset, out int value);
                return value;
            }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("SampleOffset value must be within the [0.0 <-> amount_of_sample_bytes] range");
                }
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcei.SampleOffset, value);
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
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourceb.Looping, out bool value);
                return value;
            }
            set
            {
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourceb.Looping, value);
            }
        }


        /// <summary>
        /// Indication that the sound source is relative to the listener's position
        /// </summary>
        public bool SourceRelative
        {
            get
            {
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSourceb.SourceRelative, out bool value);
                return value;
            }
            set
            {
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourceb.SourceRelative, value);
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
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSource3f.Position, out float X, out float Y, out float Z);
                return new Vector3f(X, Y, Z);
            }

            set
            {
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSource3f.Position, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Specify the current direction vector.
        /// </summary>
        public Vector3f Direction
        {
            get
            {
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSource3f.Direction, out float X, out float Y, out float Z);
                return new Vector3f(X, Y, Z);
            }

            set
            {
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSource3f.Direction, value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// Specify the current velocity in three dimensional space.
        /// </summary>
        public Vector3f Velocity
        {
            get
            {
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALSource3f.Velocity, out float X, out float Y, out float Z);
                return new Vector3f(X, Y, Z);
            }

            set
            {
                OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSource3f.Velocity, value.X, value.Y, value.Z);
            }
        }

        #endregion

        public Silk.NET.OpenAL.SourceState State
        {
            get
            {
                OpenTK.Audio.OpenAL.AL.GetSource(Handle, OpenTK.Audio.OpenAL.ALGetSourcei.SourceState, out int value);
                return (Silk.NET.OpenAL.SourceState)value;
            }
        }

        public SoundSource()
        {
            this.Handle = OpenTK.Audio.OpenAL.AL.GenSource();
        }

        public void Play(AudioSample sample)
        {
            if(sample == null)
            {
                throw new ArgumentNullException(nameof(sample));
            }

            this.Stop();
            OpenTK.Audio.OpenAL.AL.Source(Handle, OpenTK.Audio.OpenAL.ALSourcei.Buffer, sample.Handle);
            OpenTK.Audio.OpenAL.AL.SourcePlay(Handle);
        }

        public void Pause()
        {
            var state = this.State;
            if (state == Silk.NET.OpenAL.SourceState.Playing)
            {
                OpenTK.Audio.OpenAL.AL.SourcePause(Handle);
            }
        }

        public void Stop()
        {
            var state = this.State;
            if (state == Silk.NET.OpenAL.SourceState.Playing || state == Silk.NET.OpenAL.SourceState.Paused)
            {
                OpenTK.Audio.OpenAL.AL.SourceStop(Handle);
            }
        }

        public void Destroy()
        {
            if(this.Handle != 0)
            {
                OpenTK.Audio.OpenAL.AL.DeleteSource(this.Handle);
                this.Handle = 0;
            }
        }
        public void Dispose()
        {
            Destroy();
        }
    }
}
