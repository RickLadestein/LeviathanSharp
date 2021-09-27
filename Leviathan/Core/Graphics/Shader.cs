using System;
using System.Collections.Generic;
using System.Text;

namespace Leviathan.Core.Graphics
{
    public class ShaderProgram
    {

    }

    public class VertexShader : Shader
    {
        public VertexShader(string vpath) : base(vpath, ShaderType.VERTEX) { }

        protected override bool IsPathValid(string path)
        {
            return path.EndsWith(".vsh");
        }
    }

    public class GeometryShader : Shader
    {
        public GeometryShader(string gpath) : base(gpath, ShaderType.GEOMETRY) { }

        protected override bool IsPathValid(string path)
        {
            return path.EndsWith(".gsh");
        }
    }

    public class FragmentShader : Shader
    {
        public FragmentShader(string fpath) : base(fpath, ShaderType.FRAGMENT) { }

        protected override bool IsPathValid(string path)
        {
            return path.EndsWith(".fsh");
        }
    }

    public class ComputeShader : Shader
    {
        public ComputeShader(string cpath) : base(cpath, ShaderType.COMPUTE) { }

        protected override bool IsPathValid(string path)
        {
            return path.EndsWith(".csh");
        }
    }


    public abstract class Shader
    {
        protected uint handle;
        protected ShaderType type;
        protected string sh_path;

        public Shader(string _shder_path, ShaderType _type)
        {
            this.sh_path = _shder_path;
            type = _type;
            handle = Context.gl_context.CreateShader((Silk.NET.OpenGL.ShaderType)_type);
            if(handle == 0)
            {
                throw new Exception($"Shader creation failed: {_type}");
            }
        }

        protected abstract bool IsPathValid(string path);

        public void CompileShader()
        {

        }
    }

    public class ShaderFile
    {
        private const string v_marker = "?[VERTEX";
        private const string g_marker = "?[GEOMETRY";
        private const string f_marker = "?[FRAGMENT";
        private const string c_marker = "?[COMPUTE";
        public string Vertex_src { get; private set; }
        public string Geometry_src { get; private set; }
        public string Fragment_src { get; private set; }
        public string Compute_src { get; private set; }
        public ShaderFile(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                throw new ArgumentException("Path was empty", nameof(path));
            }

            if (!path.EndsWith(".sh"))
            {
                throw new ArgumentException("Given path does not lead to valid shader file");
            }

            System.IO.FileStream fs = System.IO.File.OpenRead(path);
            System.IO.StreamReader rd = new System.IO.StreamReader(fs);
            string content = rd.ReadToEnd();

            string[] tokens = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string marker = "", sh_src = "";
            bool reading_src = false;
            foreach(string line in tokens)
            {
                
                if(line.StartsWith("?["))
                {
                    if (reading_src)
                    {
                        HandleSplits(marker, sh_src);
                    }
                    marker = line.Substring(0, 2);
                } else
                {
                    sh_src += line;
                    sh_src += '\n';
                }
            }
        }

        private void HandleSplits(string marker, string content)
        {
            switch(marker) {
                case v_marker:
                    Vertex_src = content;
                    break;
                case g_marker:
                    Geometry_src = content;
                    break;
                case f_marker:
                    Fragment_src = content;
                    break;
                case c_marker:
                    Compute_src = content;
                    break;
            }
        }

        public static void CreateNew()
        {

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
