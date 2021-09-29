using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Leviathan.Math;
using Leviathan.Util;

namespace Leviathan.Core.Graphics
{
    public class ShaderProgram
    {
        public uint Handle;
        public static uint BoundProgram { get; private set; }

        public ShaderProgram(VertexShader vshader, FragmentShader fshader)
        {
            if(vshader.HasError || fshader.HasError)
            {
                throw new Exception("Vertex/Fragment Shader compilation has failed: check individual error logs for detail");
            }
            this.Handle = Context.gl_context.CreateProgram();
            if (Handle == 0)
            {
                throw new Exception("OpenGL was not able to create ShaderProgram handle at this time");
            }
            Context.gl_context.AttachShader(this.Handle, vshader.Handle);
            Context.gl_context.AttachShader(this.Handle, fshader.Handle);
            Context.gl_context.LinkProgram(this.Handle);

            String log = GetProgramInfoLog();
            if (log.Length > 0)
            {
                throw new Exception($"Shader program linking has failed: {log}");
            }
        }

        public ShaderProgram(VertexShader vshader, GeometryShader gshader, FragmentShader fshader)
        {
            if (vshader.HasError || gshader.HasError || fshader.HasError)
            {
                throw new Exception("Vertex/Geometry/Fragment Shader compilation has failed: check individual error logs for detail");
            }
            this.Handle = Context.gl_context.CreateProgram();
            if (Handle == 0)
            {
                throw new Exception("OpenGL was not able to create ShaderProgram handle at this time");
            }
            Context.gl_context.AttachShader(this.Handle, vshader.Handle);
            Context.gl_context.AttachShader(this.Handle, gshader.Handle);
            Context.gl_context.AttachShader(this.Handle, fshader.Handle);
            Context.gl_context.LinkProgram(this.Handle);

            String log = GetProgramInfoLog();
            if (log.Length > 0)
            {
                throw new Exception($"Shader program linking has failed: {log}");
            }
        }

        public ShaderProgram(ComputeShader cshader)
        {
            if (cshader.HasError)
            {
                throw new Exception("Compute Shader compilation has failed: check individual error logs for detail");
            }
            this.Handle = Context.gl_context.CreateProgram();
            if (Handle == 0)
            {
                throw new Exception("OpenGL was not able to create ShaderProgram handle at this time");
            }

            Context.gl_context.AttachShader(this.Handle, cshader.Handle);
            Context.gl_context.LinkProgram(this.Handle);

            String log = GetProgramInfoLog();
            if (log.Length > 0)
            {
                throw new Exception($"Shader program linking has failed: {log}");
            }
        }

        public void Bind()
        {
            if(BoundProgram != Handle)
            {
                Context.gl_context.UseProgram(Handle);
                BoundProgram = Handle;
            }
        }

        public void Unbind()
        {
            if (BoundProgram == Handle)
            {
                Context.gl_context.UseProgram(0);
                BoundProgram = 0;
            } else
            {
                throw new Exception("Attempted to unbind not currently bound shader program");
            }
        }

        private String GetProgramInfoLog()
        {
            if (Handle == 0)
            {
                return "ShaderProgram handle was not initialised";
            }

            String log = Context.gl_context.GetProgramInfoLog(this.Handle);
            if (log.Length > 0)
            {
                return $"ShaderProgram linking failed: \n {log}";
            }
            return String.Empty;
        }

        public static void Import(ShaderFile sfile, string shader_identifier)
        {
            ShaderProgram sp;
            if(sfile.HasVertex && sfile.HasFragment)
            {
                VertexShader vshader = new VertexShader(sfile.Vertex_src);
                FragmentShader fshader = new FragmentShader(sfile.Fragment_src);
                if(sfile.HasGeometry)
                {
                    GeometryShader gshader = new GeometryShader(sfile.Geometry_src);
                    sp = new ShaderProgram(vshader, gshader, fshader);
                }
                sp = new ShaderProgram(vshader, fshader);
            } else if(sfile.HasCompute)
            {
                ComputeShader cshader = new ComputeShader(sfile.Compute_src);
                sp = new ShaderProgram(cshader);
            } else
            {
                throw new Exception("Shaderfile was not complete: please fix");
            }
            ShaderResourceManager.Instance.AddResource(shader_identifier, sp);
        }


        #region Uniforms

        /// <summary>
        /// Sets a shader specific global variable in the GPU
        /// </summary>
        /// <param name="location">The uniform name in the shader program</param>
        /// <param name="value">Boolean value</param>
        public void SetUniform(string location, bool value)
        {
            int loc = Context.gl_context.GetUniformLocation(this.Handle, location);
            if (loc != -1)
            {
                int rep = value ? 255 : 0;
                Context.gl_context.Uniform1(loc, rep);
            }
        }

        /// <summary>
        /// Sets a shader specific global variable in the GPU
        /// </summary>
        /// <param name="location">The uniform name in the shader program</param>
        /// <param name="value">Integer value</param>
        public void SetUniform(string location, int value)
        {
            int loc = Context.gl_context.GetUniformLocation(this.Handle, location);
            if (loc != -1)
            {
                Context.gl_context.Uniform1(loc, value);
            }
        }

        /// <summary>
        /// Sets a shader specific global variable in the GPU
        /// </summary>
        /// <param name="location">The uniform name in the shader program</param>
        /// <param name="value">Unsigned Integer value</param>
        public void SetUniform(string location, uint value)
        {
            int loc = Context.gl_context.GetUniformLocation(this.Handle, location);
            if (loc != -1)
            {
                Context.gl_context.Uniform1(loc, value);
            }
        }

        /// <summary>
        /// Sets a shader specific global variable in the GPU
        /// </summary>
        /// <param name="location">The uniform name in the shader program</param>
        /// <param name="value">Float value</param>
        public void SetUniform(string location, float value)
        {
            int loc = Context.gl_context.GetUniformLocation(this.Handle, location);
            if (loc != -1)
            {
                Context.gl_context.Uniform1(loc, value);
            }
        }

        /// <summary>
        /// Sets a shader specific global variable in the GPU
        /// </summary>
        /// <param name="location">The uniform name in the shader program</param>
        /// <param name="value">Double value</param>
        public void SetUniform(string location, double value)
        {
            int loc = Context.gl_context.GetUniformLocation(this.Handle, location);
            if (loc != -1)
            {
                Context.gl_context.Uniform1(loc, value);
            }
        }

        /// <summary>
        /// Sets a shader specific global variable in the GPU
        /// </summary>
        /// <param name="location">The uniform name in the shader program</param>
        /// <param name="vec">Vector2 value</param>
        public void SetUniform(string location, Vector2f vec)
        {
            int loc = Context.gl_context.GetUniformLocation(this.Handle, location);
            if (loc != -1)
            {
                Context.gl_context.Uniform2(loc, vec.X, vec.Y);
            }
        }

        /// <summary>
        /// Sets a shader specific global variable in the GPU
        /// </summary>
        /// <param name="location">The uniform name in the shader program</param>
        /// <param name="vec">Vector3 value</param>
        public void SetUniform(string location, Vector3f vec)
        {
            int loc = Context.gl_context.GetUniformLocation(this.Handle, location);
            if (loc != -1)
            {
                Context.gl_context.Uniform3(loc, vec.X, vec.Y, vec.Z);
            }
        }

        /// <summary>
        /// Sets a shader specific global variable in the GPU
        /// </summary>
        /// <param name="location">The uniform name in the shader program</param>
        /// <param name="vec">Vector4 value</param>
        public void SetUniform(string location, Vector4f vec)
        {
            int loc = Context.gl_context.GetUniformLocation(this.Handle, location);
            if (loc != -1)
            {
                Context.gl_context.Uniform4(loc, vec.X, vec.Y, vec.Z, vec.W);
            }
        }

        /// <summary>
        /// Sets a shader specific global variable in the GPU
        /// </summary>
        /// <param name="location">The uniform name in the shader program</param>
        /// <param name="matrix">Matrix3 value</param>
        public unsafe void SetUniform(string location, Mat2 matrix)
        {
            int loc = Context.gl_context.GetUniformLocation(this.Handle, location);
            if (loc != -1)
            {
                Context.gl_context.UniformMatrix3(loc, 1, false, (float*)&matrix);
            }
        }

        /// <summary>
        /// Sets a shader specific global variable in the GPU
        /// </summary>
        /// <param name="location">The uniform name in the shader program</param>
        /// <param name="matrix">Matrix3 value</param>
        public unsafe void SetUniform(string location, Mat3 matrix)
        {
            int loc = Context.gl_context.GetUniformLocation(this.Handle, location);
            if (loc != -1)
            {
                Context.gl_context.UniformMatrix3(loc, 1, false, (float*)&matrix);
            }
        }

        /// <summary>
        /// Sets a shader specific global variable in the GPU
        /// </summary>
        /// <param name="location">The uniform name in the shader program</param>
        /// <param name="matrix">Matrix4 value</param>
        public unsafe void SetUniform(string location, Mat4 matrix)
        {
            int loc = Context.gl_context.GetUniformLocation(this.Handle, location);
            if (loc != -1)
            {
                Context.gl_context.UniformMatrix4(loc, 1, false, (float*)&matrix);
            }
        }
        #endregion
    }

    public class VertexShader : Shader
    {
        public VertexShader(string vsrc) : base(vsrc, ShaderType.VERTEX) { }
    }

    public class GeometryShader : Shader
    {
        public GeometryShader(string gsrc) : base(gsrc, ShaderType.GEOMETRY) { }
    }

    public class FragmentShader : Shader
    {
        public FragmentShader(string fsrc) : base(fsrc, ShaderType.FRAGMENT) { }
    }

    public class ComputeShader : Shader
    {
        public ComputeShader(string csrc) : base(csrc, ShaderType.COMPUTE) { }
    }


    public abstract class Shader
    {
        public uint Handle { get; private set; }
        public ShaderType Type { get; private set; }
        public string Error { get; private set; }
        public bool HasError { get; private set; }

        public Shader(string shdr_src, ShaderType _type)
        {
            Type = _type;
            Error = string.Empty;
            HasError = false;
            Handle = Context.gl_context.CreateShader((Silk.NET.OpenGL.ShaderType)_type);
            if(Handle == 0)
            {
                throw new Exception($"Shader handle creation failed: {_type}");
            }
            CompileShader(shdr_src, Type);
        }

        private void CompileShader(string src, ShaderType _type)
        {
            if (this.Handle == 0)
            {
                Error = "Shader handle was not initialised";
                HasError = true;
                return;
            }
            if (src.Length == 0)
            {
                Error = "Shader source code was empty";
                HasError = true;
                return;
            }

            Context.gl_context.ShaderSource(Handle, src);
            Context.gl_context.CompileShader(Handle);

            string log = GetInfoLog();
            if (log.Length > 0)
            {
                Error = log;
                HasError = true;
            }
            return;
        }

        private String GetInfoLog()
        {
            if (Handle == 0)
            {
                return "Shader handle was not initialised";
            }

            String log = Context.gl_context.GetShaderInfoLog(Handle);
            if (log.Length > 0)
            {
                return $"{Type} Shader compilation failed: \n {log}";
            }
            return String.Empty;
        }
    }

    public class ShaderFile
    {
        private const string v_marker = "VERTEX";
        private const string g_marker = "GEOMETRY";
        private const string f_marker = "FRAGMENT";
        private const string c_marker = "COMPUTE";
        private const string cat_marker = "??";
        public string Vertex_src { get; private set; }
        public bool HasVertex { get; private set; }
        public string Geometry_src { get; private set; }
        public bool HasGeometry { get; private set; }
        public string Fragment_src { get; private set; }
        public bool HasFragment { get; private set; }
        public string Compute_src { get; private set; }
        public bool HasCompute { get; private set; }
        public string File_path { get; private set; }
        public ShaderFile(string path)
        {
            HasVertex = false;
            HasGeometry = false;
            HasFragment = false;
            HasCompute = false;
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                throw new ArgumentException("Path was empty", nameof(path));
            }

            if (!path.EndsWith(".glsl"))
            {
                throw new ArgumentException("Given path does not lead to valid shader file");
            }

            this.File_path = path;

            System.IO.StreamReader rd = new System.IO.StreamReader(path);
            string content = rd.ReadToEnd();

            string[] tokens = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string marker = "", sh_src = "";
            for(int i = 0; i < tokens.Length; i++)
            {
                
                string line = tokens[i].TrimEnd('\r');
                if (line.Length == 0)
                {
                    continue;
                }

                if (line.StartsWith(cat_marker))
                {
                    HandleSplits(marker, sh_src);
                    sh_src = "";
                    marker = line.Remove(0, 2);
                } else
                {
                    sh_src += line;
                    sh_src += '\n';
                }

                if(i == (tokens.Length - 1) )
                {
                    HandleSplits(marker, sh_src);
                }
            }
            rd.Close();
        }

        private void HandleSplits(string marker, string content)
        {
            switch(marker) {
                case v_marker:
                    Vertex_src = content;
                    HasVertex = true;
                    break;
                case g_marker:
                    Geometry_src = content;
                    HasGeometry = true;
                    break;
                case f_marker:
                    Fragment_src = content;
                    HasFragment = true;
                    break;
                case c_marker:
                    Compute_src = content;
                    HasCompute = true;
                    break;
                default:
                    break;
            }
        }

        public static void WriteShader(ShaderFile shader)
        {
            if(shader == null)
            {
                throw new ArgumentNullException(nameof(shader));
            }

            FileStream fs;
            if(File.Exists(shader.File_path))
            {
                fs = File.Open(shader.File_path, FileMode.Truncate);
            } else
            {
                fs = File.Open(shader.File_path, FileMode.Create);
            }
            StreamWriter writer = new StreamWriter(fs);
            
            if(shader.Vertex_src != null)
            {
                writer.Write(cat_marker + v_marker);
                writer.Write('\n');
                writer.Write(shader.Vertex_src);
                writer.Write('\n');
            }

            if(shader.Geometry_src != null)
            {
                writer.Write(cat_marker + g_marker);
                writer.Write('\n');
                writer.Write(shader.Geometry_src);
                writer.Write('\n');
            }

            if(shader.Fragment_src != null)
            {
                writer.Write(cat_marker + f_marker);
                writer.Write('\n');
                writer.Write(shader.Fragment_src);
                writer.Write('\n');
            }

            if (shader.Compute_src != null)
            {
                writer.Write(cat_marker + c_marker);
                writer.Write('\n');
                writer.Write(shader.Compute_src);
                writer.Write('\n');
            }
            writer.Flush();
            writer.Close();
        }
    }


    public enum ShaderType
    {
        VERTEX = Silk.NET.OpenGL.ShaderType.VertexShader,
        GEOMETRY = Silk.NET.OpenGL.ShaderType.GeometryShader,
        FRAGMENT = Silk.NET.OpenGL.ShaderType.FragmentShader,
        COMPUTE = Silk.NET.OpenGL.ShaderType.ComputeShader
    }
}
