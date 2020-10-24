using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Leviathan.Util;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Leviathan.Core.Graphics
{
    public class ShaderProgram
    {
        public int Handle { get; private set; }

        /// <summary>
        /// Creates a shaderprogram with given vertex and fragment shader
        /// </summary>
        /// <param name="v_shadr">Vertex shader to link to the program</param>
        /// <param name="f_shadr">Fragment shader to link to the program</param>
        public ShaderProgram(VertexShader v_shadr, FragmentShader f_shadr)
        {
            this.Handle = Shader.INVALID_HANDLE;
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

        /// <summary>
        /// Creates a shaderprogram with given vertex, geometry and fragment shader
        /// </summary>
        /// <param name="v_shadr">Vertex shader to link to the program</param>
        /// <param name="g_shadr">Geometry shader to link to the program</param>
        /// <param name="f_shadr">Fragment shader to link to the program</param>
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


        /// <summary>
        /// Creates a shaderprogram with given compute shader
        /// </summary>
        /// <param name="c_shadr">Compute shader to link to the program</param>
        public ShaderProgram(ComputeShader c_shadr)
        {
            this.Handle = -1;
            if (c_shadr == null) { return; }
            if (c_shadr.Handle == Shader.INVALID_HANDLE) { return; }

            this.Handle = GL.CreateProgram();
            GL.AttachShader(this.Handle, c_shadr.Handle);
            GL.LinkProgram(this.Handle);

            string log = GL.GetProgramInfoLog(this.Handle);
            if (log != string.Empty)
            {
                this.Destroy();
                Logger.GetInstance().LogError(log, true);
            }
        }

        /// <summary>
        /// Destructor to be called at the end of object lifetime
        /// </summary>
        ~ShaderProgram()
        {
            this.Destroy();
        }

        /// <summary>
        /// Destroys the Shaderprogram's unmanaged resources
        /// </summary>
        public void Destroy()
        {
            if(!(this.Handle < 0))
            {
                GL.DeleteProgram(this.Handle);
                this.Handle = Shader.INVALID_HANDLE;
            }
            return;
        }
        #region Uniforms
        /// <summary>
        /// Sets the signed integer uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The signed integer to be loaded in the shaderprogram</param>
        public void Uniform1i(string name, int value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.Uniform1(loc, value);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the float uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The float to be loaded in the shaderprogram</param>
        public void Uniform1f(string name, float value)
        {
            if(this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.Uniform1(loc, value);
            } else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the double uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The double to be loaded in the shaderprogram</param>
        public void Uniform1d(string name, double value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.Uniform1(loc, value);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the Vector2i uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The Uniform2i to be loaded in the shaderprogram</param>
        public void Uniform2i(string name, Vector2i value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.Uniform2(loc, value.X, value.Y);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the Vector2f uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The Uniform2f to be loaded in the shaderprogram</param>
        public void Uniform2f(string name, Vector2 value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.Uniform2(loc, value.X, value.Y);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the Vector2d uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The Uniform2d to be loaded in the shaderprogram</param>
        public void Uniform2d(string name, Vector2d value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.Uniform2(loc, value.X, value.Y);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the Vector3i uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The Uniform3i to be loaded in the shaderprogram</param>
        public void Uniform3i(string name, Vector3i value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.Uniform3(loc, value.X, value.Y, value.Z);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the Vector3f uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The Uniform3f to be loaded in the shaderprogram</param>
        public void Uniform3f(string name, Vector3 value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.Uniform3(loc, value.X, value.Y, value.Z);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the Vector3d uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The Uniform3d to be loaded in the shaderprogram</param>
        public void Uniform3d(string name, Vector3d value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.Uniform3(loc, value.X, value.Y, value.Z);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the Vector4i uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The Uniform4i to be loaded in the shaderprogram</param>
        public void Uniform4i(string name, Vector4i value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.Uniform4(loc, value.X, value.Y, value.Z, value.W);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the Vector4f uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The Uniform4f to be loaded in the shaderprogram</param>
        public void Uniform4f(string name, Vector4 value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.Uniform4(loc, value.X, value.Y, value.Z, value.W);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the Vector4d uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The Uniform4d to be loaded in the shaderprogram</param>
        public void Uniform4d(string name, Vector4d value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.Uniform4(loc, value.X, value.Y, value.Z, value.W);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the matrix2 uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The matrix2 to be loaded in the shaderprogram</param>
        public void UniformMat2(string name, Matrix2 value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.UniformMatrix2(loc, false, ref value);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the matrix3 uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The matrix3 to be loaded in the shaderprogram</param>
        public void UniformMat3(string name, Matrix3 value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.UniformMatrix3(loc, false, ref value);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }

        /// <summary>
        /// Sets the matrix4 uniform in the shaderprogram
        /// </summary>
        /// <param name="name">Uniform name in the shaderprogram</param>
        /// <param name="value">The matrix4 to be loaded in the shaderprogram</param>
        public void UniformMat4(string name, Matrix4 value)
        {
            if (this.Handle != Shader.INVALID_HANDLE)
            {
                int loc = GL.GetUniformLocation(this.Handle, name);
                if (loc == -1) //No Location found
                {
                    Logger.GetInstance().LogError($"Could not set uniform: shaderprogram does not contain a uniform called [{name}]");
                    return;
                }
                GL.UniformMatrix4(loc, false, ref value);
            }
            else
            {
                Logger.GetInstance().LogError($"Could not set uniform: shaderprogram handle is invalid");
                return;
            }
        }


        #endregion

        #region ShaderStore
        private static Dictionary<String, ShaderProgram> programs = new Dictionary<string, ShaderProgram>();
        private static Mutex program_mutex = new Mutex();

        /// <summary>
        /// Adds the shaderprogram containing the fragment and vertex to the local storage
        /// </summary>
        /// <param name="folder_id">The folder id where the shader files are located</param>
        /// <param name="f_shadr_file">The fragment shader file <c>{shader_file_name}.glsl</param>
        /// <param name="v_shadr_file">The vertex shader file <c>{shader_file_name}.glsl</param>
        /// <param name="identifier">The identifier under which the shaderprogram is stored</param>
        /// <returns></returns>
        public static bool AddShaderProgramToStorage(string folder_id, string f_shadr_file, string v_shadr_file, string identifier)
        {
            VertexShader v_shadr = new VertexShader(folder_id, v_shadr_file);
            FragmentShader f_shadr = new FragmentShader(folder_id, f_shadr_file);
            ShaderProgram prog = new ShaderProgram(v_shadr, f_shadr);
            if(prog.Handle == Shader.INVALID_HANDLE) //Prog load fail
            {
                return false;
            }
            return ShaderProgram.AddShaderProgramToStorage(prog, identifier);
        }

        /// <summary>
        /// Adds the shaderprogram containing the fragment, vertex and geometry shader to the local storage
        /// </summary>
        /// <param name="folder_id">The folder id where the shader files are located</param>
        /// <param name="f_shadr_file">The fragment shader file <c>{shader_file_name}.glsl</param>
        /// <param name="g_shadr_file">The geometry shader file <c>{shader_file_name}.glsl</param>
        /// <param name="v_shadr_file">The vertex shader file <c>{shader_file_name}.glsl</param>
        /// <param name="identifier">The identifier under which the shaderprogram is stored</param>
        /// <returns></returns>
        public static bool AddShaderProgramToStorage(string folder_id, string f_shadr_file, string g_shadr_file, string v_shadr_file, string identifier)
        {
            VertexShader v_shadr = new VertexShader(folder_id, v_shadr_file);
            FragmentShader f_shadr = new FragmentShader(folder_id, f_shadr_file);
            GeometryShader g_shadr = new GeometryShader(folder_id, g_shadr_file);
            ShaderProgram prog = new ShaderProgram(v_shadr, g_shadr, f_shadr);
            if (prog.Handle == Shader.INVALID_HANDLE) //Prog load fail
            {
                return false;
            }
            return ShaderProgram.AddShaderProgramToStorage(prog, identifier);
        }

        /// <summary>
        /// Adds the shaderprogram containing the compute shader to the local storage
        /// </summary>
        /// <param name="folder_id">The folder id where the compute shader is located</param>
        /// <param name="c_shadr_file">The compute shader file <c>{shader_file_name}.glsl</c></param>
        /// <param name="identifier">The identifier under which the shaderprogram is stored</param>
        /// <returns></returns>
        public static bool AddShaderProgramToStorage(string folder_id, string c_shadr_file, string identifier)
        {
            ComputeShader c_shadr = new ComputeShader(folder_id, c_shadr_file);
            ShaderProgram prog = new ShaderProgram(c_shadr);
            if (prog.Handle == Shader.INVALID_HANDLE) //Prog load fail
            {
                return false;
            }
            return ShaderProgram.AddShaderProgramToStorage(prog, identifier);
        }


        /// <summary>
        /// Adds the shaderprogram to the local storage
        /// </summary>
        /// <param name="program">The shaderprogram to store</param>
        /// <param name="identifier">The identifier under which the shaderprogram is stored</param>
        /// <returns></returns>
        public static bool AddShaderProgramToStorage(ShaderProgram program, string identifier)
        {
            program_mutex.WaitOne();
            try
            {
                if(!programs.ContainsKey(identifier))
                {
                    programs.Add(identifier, program);
                    return true;
                }
                return false;
            } catch(Exception)
            {
                return false;
            } finally
            {
                program_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Removes the specified shaderprogram from the local storage
        /// </summary>
        /// <param name="identifier">The shaderprogram identifier under which the shaderprogram is stored locally</param>
        /// <returns></returns>
        public static bool RemoveShaderProgramFromStorage(string identifier)
        {
            program_mutex.WaitOne();
            try
            {
                if(programs.ContainsKey(identifier))
                {
                    programs.Remove(identifier);
                    return true;
                }
                return false;
            } catch(Exception)
            {
                return false;
            } finally
            {
                program_mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Replaces the shaderprogram in local storage with another
        /// </summary>
        /// <param name="identifier">The shaderprogram identifier under which the shaderprogram is stored locally</param>
        /// <param name="prog">The value that is suposed to be written </param>
        /// <returns></returns>
        public static bool ReplaceShaderProgramInStorage(string identifier, ShaderProgram prog)
        {
            program_mutex.WaitOne();
            try
            {
                if (!programs.ContainsKey(identifier))
                {
                    programs[identifier] = prog;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                program_mutex.ReleaseMutex();
            }
        }


        /// <summary>
        /// Gets the specified shaderprogram from storage
        /// </summary>
        /// <param name="identifier">The shaderprogram identifier under which the shaderprogram is stored locally</param>
        /// <returns></returns>
        public static ShaderProgram GetShaderPorgramFromStorage(string identifier)
        {
            program_mutex.WaitOne();
            try
            {
                if(programs.ContainsKey(identifier))
                {
                    return programs[identifier];
                }
                return null;
            } catch(Exception)
            {
                return null;
            } finally
            {
                program_mutex.ReleaseMutex();
            }
        }

        #endregion
    }

    public class FragmentShader : Shader
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

    public class VertexShader : Shader
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

    public class GeometryShader : Shader
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

    public class ComputeShader : Shader
    {
        public ComputeShader(string folder_id, string file) : base(ShaderType.COMPUTE)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class Shader {
        public static readonly int INVALID_HANDLE = -1;
        public int Handle { get; protected set; }
        public ShaderType Type { get; protected set; }

        public Shader()
        {
            this.Handle = INVALID_HANDLE;
            this.Type = ShaderType.INVALID;
        }

        public Shader(ShaderType type)
        {
            this.Type = type;
            this.Handle = INVALID_HANDLE;
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
                this.Handle = INVALID_HANDLE;
            }
        }
    }

    public enum ShaderType
    {
        FRAGMENT,
        VERTEX,
        GEOMETRY,
        COMPUTE,
        INVALID
    }



}
