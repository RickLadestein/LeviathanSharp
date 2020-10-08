using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Leviathan.Util;
using OpenTK.Graphics.OpenGL;

namespace Leviathan.Core.Graphics
{
    class ShaderProgram
    {
        public int Handle { get; private set; }
        public ShaderProgram(VertexShader v_shadr, FragmentShader f_shadr)
        {
            this.Handle = -1;
            if (v_shadr == null || f_shadr == null) { return; }
            if (v_shadr.Handle == Shader.INVALID_HANDLE  || f_shadr.Handle == Shader.INVALID_HANDLE) { return; }

            this.Handle = GL.CreateProgram();
            GL.AttachShader(this.Handle, v_shadr.Handle);
            GL.AttachShader(this.Handle, f_shadr.Handle);
            GL.LinkProgram(this.Handle);

            string log = GL.GetProgramInfoLog(this.Handle);
            if (log != string.Empty)
            {
                this.Destroy();
                Logger.GetInstance().LogError(log, true);
            }
        }

        public ShaderProgram(VertexShader v_shadr, GeometryShader g_shadr, FragmentShader f_shadr)
        {
            this.Handle = -1;
            if (v_shadr == null || g_shadr == null || f_shadr == null) { return; }
            if (v_shadr.Handle == Shader.INVALID_HANDLE || g_shadr.Handle == Shader.INVALID_HANDLE || f_shadr.Handle == Shader.INVALID_HANDLE) { return; }

            this.Handle = GL.CreateProgram();
            GL.AttachShader(this.Handle, v_shadr.Handle);
            GL.AttachShader(this.Handle, g_shadr.Handle);
            GL.AttachShader(this.Handle, f_shadr.Handle);
            GL.LinkProgram(this.Handle);

            string log = GL.GetProgramInfoLog(this.Handle);
            if(log != string.Empty)
            {
                this.Destroy();
                Logger.GetInstance().LogError(log, true);
            }
        }

        ~ShaderProgram()
        {
            this.Destroy();
        }

        public void Destroy()
        {
            if(!(this.Handle < 0))
            {
                GL.DeleteProgram(this.Handle);
                this.Handle = -1;
            }
            return;
        }
    }

    class FragmentShader : Shader
    {
        public FragmentShader(string folder_id, string file) : base(ShaderType.FRAGMENT)
        {
            string src = FileManager.GetInstance().ReadTextFile(folder_id, file);
            if(src == string.Empty) { return; }

            this.Handle = GL.CreateShader(OpenTK.Graphics.OpenGL.ShaderType.FragmentShader);
            GL.ShaderSource(this.Handle, src);
            GL.CompileShader(this.Handle);
            string log = this.GetInfoLog();
            if (log != string.Empty)
            {
                Logger.GetInstance().LogError(log, true);
                this.Destroy();
            }
        }
    }

    class VertexShader : Shader
    {
        public VertexShader(string folder_id, string file) : base(ShaderType.VERTEX)
        {
            string src = FileManager.GetInstance().ReadTextFile(folder_id, file);
            if (src == string.Empty) { return; }

            this.Handle = GL.CreateShader(OpenTK.Graphics.OpenGL.ShaderType.VertexShader);
            GL.ShaderSource(this.Handle, src);
            GL.CompileShader(this.Handle);
            string log = this.GetInfoLog();
            if (log != string.Empty)
            {
                Logger.GetInstance().LogError(log, true);
                this.Destroy();
            }
        }
    }

    class GeometryShader : Shader
    {
        public GeometryShader(string folder_id, string file) : base(ShaderType.GEOMETRY)
        {
            string src = FileManager.GetInstance().ReadTextFile(folder_id, file);
            if (src == string.Empty) { return; }

            this.Handle = GL.CreateShader(OpenTK.Graphics.OpenGL.ShaderType.GeometryShader);
            GL.ShaderSource(this.Handle, src);
            GL.CompileShader(this.Handle);
            string log = this.GetInfoLog();
            if (log != string.Empty)
            {
                Logger.GetInstance().LogError(log, true);
                this.Destroy();
            }
        }
    }

    class ComputeShader : Shader
    {
        public ComputeShader(string folder_id, string file) : base(ShaderType.COMPUTE)
        {
            throw new NotImplementedException();
        }
    }

    abstract class Shader {
        public static readonly int INVALID_HANDLE = -1;
        public int Handle { get; protected set; }
        public ShaderType Type { get; protected set; }

        public Shader()
        {
            this.Handle = -1;
            this.Type = ShaderType.INVALID;
        }

        public Shader(ShaderType type)
        {
            this.Type = type;
            this.Handle = -1;
        }

        public Shader(ShaderType type, int id)
        {
            this.Type = type;
            this.Handle = id;
        }

        ~Shader()
        {
            this.Destroy();
        }

        public string GetInfoLog()
        {
            string output = GL.GetShaderInfoLog(this.Handle);
            string shader_type = string.Empty;
            if (output != string.Empty)
            {
                switch (this.Type)
                {
                    case ShaderType.VERTEX:
                        shader_type = "VERTEX";
                        break;
                    case ShaderType.FRAGMENT:
                        shader_type = "FRAGMENT";
                        break;
                    case ShaderType.GEOMETRY:
                        shader_type = "GEOMETRY";
                        break;
                    case ShaderType.COMPUTE:
                        shader_type = "COMPUTE";
                        break;
                }
                output = $"Error {shader_type} compilation failed: \n{output}";
                return output;
            }
            return string.Empty;
        }

        public void Destroy()
        {
            if (!(this.Handle < 0))
            {
                GL.DeleteShader(this.Handle);
                this.Handle = -1;
            }
        }
    }

    enum ShaderType
    {
        FRAGMENT,
        VERTEX,
        GEOMETRY,
        COMPUTE,
        INVALID
    }



}
