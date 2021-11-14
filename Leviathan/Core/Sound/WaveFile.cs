using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Leviathan.Core.Sound
{
    public class WaveFile
    {
        public HeaderChunk Header;
        public FormatChunk Format;
        public DataChunk Data;

        public ListChunk List;
        public List<JunkChunk> Extra_data { get; private set; }

        private WaveFile()
        {
            Extra_data = new List<JunkChunk>();
        }

        public static WaveFile Import(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                throw new ArgumentException("Path cannot be empty");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Could not find sound file", path);
            }

            WaveFile output = new WaveFile();

            FileStream fs = null;
            BinaryReader reader = null;
            try
            {
                fs = File.OpenRead(path);
                reader = new BinaryReader(fs);

                while(reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    string chunk_id = ReadBlockId(reader).ToUpper();
                    switch(chunk_id)
                    {
                        case "RIFF":
                            output.Header = HeaderChunk.Parse(reader, chunk_id);
                            break;
                        case "FMT ":
                            output.Format = FormatChunk.Parse(reader, chunk_id);
                            break;
                        case "DATA":
                            output.Data = DataChunk.Parse(reader, chunk_id);
                            break;
                        case "LIST":
                            output.List = ListChunk.Parse(reader, chunk_id);
                            break;
                        case "FACT":
                            break;
                        default:
                            JunkChunk fc = JunkChunk.Parse(reader, chunk_id);
                            output.Extra_data.Add(fc);
                            break;
                    }
                }
            }
            finally
            {
                if(fs != null)
                {
                    fs.Close();
                }
            }


            if(output.Format.audio_format == AudioFormat.IEEE_FLOAT)
            {
                output.Data.ConvertIEEEToPCM();
                output.Format.bits_per_sample = 16;
                output.Format.audio_format = AudioFormat.PCM;
            }
            return output;
        }

        public void ConvertToMono()
        {
            this.Format.num_channels = 1;
            if(this.Format.bits_per_sample == 16)
            {
                this.Data.ConvertToMono16();
            } else if(this.Format.bits_per_sample == 8)
            {
                this.Data.ConvertToMono8();
            } else
            {
                throw new NotSupportedException($"{Format.bits_per_sample} bits per sample conversion is not supported");
            }
        }

        private static String ReadBlockId(BinaryReader reader)
        {
            if(reader.BaseStream.Position < (reader.BaseStream.Length - 4))
            {
                return new String(reader.ReadChars(4));
            }
            return String.Empty;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct HeaderChunk
    {
        /*
         0         4   ChunkID          Contains the letters "RIFF" in ASCII form
                                        (0x52494646 big-endian form).
         4         4   ChunkSize        36 + SubChunk2Size, or more precisely:
                                        4 + (8 + SubChunk1Size) + (8 + SubChunk2Size)
                                        This is the size of the rest of the chunk 
                                        following this number.  This is the size of the 
                                        entire file in bytes minus 8 bytes for the
                                        two fields not included in this count:
                                        ChunkID and ChunkSize.
         8         4   Format           Contains the letters "WAVE"
                                        (0x57415645 big-endian form).
         */

        public UInt32 file_size;
        public string format_type;

        public static HeaderChunk Parse(BinaryReader reader, string identifier)
        {
            if(identifier.ToUpper() == "RIFF")
            {
                HeaderChunk output = new HeaderChunk();
                output.file_size = reader.ReadUInt32();
                output.format_type = new string(reader.ReadChars(4));
                return output;
            } else
            {
                throw new NotSupportedException("Identifier was not supported for HeaderChunk");
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FormatChunk
    {

        /*
            12        4   Subchunk1ID      Contains the letters "fmt "
                                           (0x666d7420 big-endian form).
            16        4   Subchunk1Size    16 for PCM.  This is the size of the
                                           rest of the Subchunk which follows this number.
            20        2   AudioFormat      PCM = 1 (i.e. Linear quantization)
                                           Values other than 1 indicate some 
                                           form of compression.
            22        2   NumChannels      Mono = 1, Stereo = 2, etc.
            24        4   SampleRate       8000, 44100, etc.
            28        4   ByteRate         == SampleRate * NumChannels * BitsPerSample/8
            32        2   BlockAlign       == NumChannels * BitsPerSample/8
                                           The number of bytes for one sample including
                                           all channels. I wonder what happens when
                                           this number isn't an integer?
            34        2   BitsPerSample    8 bits = 8, 16 bits = 16, etc.
                      2   ExtraParamSize   if PCM, then doesn't exist
                      X   ExtraParams      space for extra parameters
         */
        public string chunk_id;
        public UInt32 block_length;
        public AudioFormat audio_format;
        public ushort num_channels;
        public UInt32 sample_rate;
        public UInt32 byte_rate;
        public ushort block_align;
        public ushort bits_per_sample;

        //Unconventional data for PCM
        public ushort extra_data_size;
        public byte[] extra_data;

        public static FormatChunk Parse(BinaryReader reader, string identifier)
        {
            if (identifier.ToUpper() == "FMT ")
            {
                FormatChunk output = new FormatChunk();
                output.chunk_id = identifier;
                output.block_length = reader.ReadUInt32();
                output.audio_format = (AudioFormat)(reader.ReadUInt16());
                if(output.audio_format == AudioFormat.UNSUPPORTED)
                {
                    throw new NotSupportedException("Extended audio format is not supported");
                }
                output.num_channels = reader.ReadUInt16();
                output.sample_rate = reader.ReadUInt32();
                output.byte_rate = reader.ReadUInt32();
                output.block_align = reader.ReadUInt16();
                output.bits_per_sample = reader.ReadUInt16();
                if(output.block_length > 16)
                {
                    output.extra_data_size = reader.ReadUInt16();
                    if(output.extra_data_size != 0)
                    {
                        output.extra_data = reader.ReadBytes(output.extra_data_size);
                    }
                }
                return output;
            }
            else
            {
                throw new NotSupportedException("Identifier was not supported for FormatChunk");
            }

        }

        public OpenTK.Audio.OpenAL.ALFormat GetSoundFormat()
        {
            if (num_channels == 0 || num_channels > 2)
            {
                throw new NotSupportedException($"{num_channels} channels not supported. Supported channels are (1 or 2)");
            }

            switch (audio_format)
            {
                case AudioFormat.PCM:
                    return GetSoundFormatPCM();
                default:
                    throw new NotSupportedException();
            }
        }

        private OpenTK.Audio.OpenAL.ALFormat GetSoundFormatPCM()
        {
            if(num_channels == 1)
            {
                switch(bits_per_sample)
                {
                    case 8:
                        return OpenTK.Audio.OpenAL.ALFormat.Mono8;
                    case 16:
                        return OpenTK.Audio.OpenAL.ALFormat.Mono16;
                    default:
                        throw new NotSupportedException("PCM format cannot contain more than 16 bits");
                }
            } else
            {
                switch (bits_per_sample)
                {
                    case 8:
                        return OpenTK.Audio.OpenAL.ALFormat.Stereo8;
                    case 16:
                        return OpenTK.Audio.OpenAL.ALFormat.Stereo16;
                    default:
                        throw new NotSupportedException("PCM format cannot contain more than 16 bits");
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DataChunk
    {
        /*
            36        4   Subchunk2ID      Contains the letters "data"
                               (0x64617461 big-endian form).
            40        4   Subchunk2Size    == NumSamples * NumChannels * BitsPerSample/8
                               This is the number of bytes in the data.
                               You can also think of this as the size
                               of the read of the subchunk following this 
                               number.
            44        *   Data             The actual sound data.
         
         */
        public string chunk_id;
        public UInt32 block_length;
        public byte[] data;

        public static DataChunk Parse(BinaryReader reader, string identifier)
        {
            if (identifier.ToUpper() == "DATA")
            {
                DataChunk output = new DataChunk();
                output.chunk_id = identifier;
                output.block_length = reader.ReadUInt32();
                output.data = reader.ReadBytes((int)output.block_length);
                return output;
            }
            else
            {
                throw new NotSupportedException("Identifier was not supported for DataChunk");
            }
        }

        public void ConvertIEEEToPCM()
        {
            int original_buffer_idx = 0;
            int new_buffer_idx = 0;
            byte[] output_data = new byte[data.Length / 2];
            while(original_buffer_idx != data.Length)
            {
                float original_data = BitConverter.ToSingle(new byte[] {
                    data[original_buffer_idx],
                    data[original_buffer_idx + 1] ,
                    data[original_buffer_idx + 2] ,
                    data[original_buffer_idx + 3] }, 
                    0);
                Int16 converted = (Int16)(original_data * Int16.MaxValue);
                byte[] converted_bytes = BitConverter.GetBytes(converted);
                output_data[new_buffer_idx] = converted_bytes[0];
                output_data[new_buffer_idx + 1] = converted_bytes[1];

                original_buffer_idx += 4;
                new_buffer_idx += 2;
            }
            block_length = (uint)output_data.Length;
            data = output_data;
        }

        public void ConvertToMono16()
        {
            int original_buffer_idx = 0;
            int new_buffer_idx = 0;
            byte[] output_data = new byte[data.Length / 2];
            while (original_buffer_idx != data.Length)
            {
                Int16 data_1 = BitConverter.ToInt16(new byte[] {
                    data[original_buffer_idx],
                    data[original_buffer_idx + 1] },
                    0);
                Int16 data_2 = BitConverter.ToInt16(new byte[] {
                    data[original_buffer_idx + 2],
                    data[original_buffer_idx + 3] },
                    0);

                Int16 converted = (Int16)((data_1 + data_2) / 2);
                byte[] converted_bytes = BitConverter.GetBytes(converted);
                output_data[new_buffer_idx] = converted_bytes[0];
                output_data[new_buffer_idx + 1] = converted_bytes[1];

                original_buffer_idx += 4;
                new_buffer_idx += 2;
            }
            block_length = (uint)output_data.Length;
            data = output_data;
        }

        public void ConvertToMono8()
        {
            int original_buffer_idx = 0;
            int new_buffer_idx = 0;
            byte[] output_data = new byte[data.Length / 2];
            while (original_buffer_idx != data.Length)
            {
                byte data_1 = data[original_buffer_idx];
                byte data_2 = data[original_buffer_idx + 1];
                Int16 avg = (Int16)(((Int16)data_1 + (Int16)data_2) / 2);

                byte converted = BitConverter.GetBytes(avg)[0];
                output_data[new_buffer_idx] = converted;

                original_buffer_idx += 2;
                new_buffer_idx += 1;
            }
            block_length = (uint)output_data.Length;
            data = output_data;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ListChunk
    {
        public string chunk_id;
        public UInt32 block_length;
        public string block_marker;
        public char[] data;

        public static ListChunk Parse(BinaryReader reader, string identifier)
        {
            if (identifier.ToUpper() == "LIST")
            {
                ListChunk output = new ListChunk();
                output.chunk_id = identifier;
                output.block_length = reader.ReadUInt32();
                output.block_marker = new string(reader.ReadChars(4));
                output.data = reader.ReadChars((int)output.block_length - 4);
                return output;
            }
            else
            {
                throw new NotSupportedException("Identifier was not supported for HeaderChunk");
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JunkChunk
    {
        public string chunk_id;
        public byte[] data;

        public static JunkChunk Parse(BinaryReader reader, string identifier)
        {
            JunkChunk output = new JunkChunk();
            output.chunk_id = identifier;
            long read_bytes = reader.BaseStream.Length - reader.BaseStream.Position;
            output.data = reader.ReadBytes((int)read_bytes);
            return output;

        }
    }

    public enum AudioFormat : ushort
    {
        PCM = 1,        //	PCM / uncompressed format
        ADPCM = 2,      //	Microsoft ADPCM
        IEEE_FLOAT = 3, //	IEEE float
        ITU_A_LAW = 6,  //  ITU G.711 a-law
        ITU_U_LAW = 7,  //  ITU G.711 μ-law
        GSM = 49,       //  GSM 6.10
        ITU_ADPCM = 64,  //  ITU G.721 ADPCM
        UNSUPPORTED = 65534 // UNSUPPORTED EXTENDED FORMAT
    }
}
