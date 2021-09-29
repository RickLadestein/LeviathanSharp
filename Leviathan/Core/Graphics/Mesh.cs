using Leviathan.Core.Graphics.Buffers.VertexBufferAttributes;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Leviathan.Util;
using Leviathan.Math;
using Assimp;
using Assimp.Configs;

namespace Leviathan.Core.Graphics
{
    public class Mesh
    {
        public String Name { get; private set; }
        public ElementType PrimitiveType { get; private set; }
        public uint VertexCount { get; private set; }
        public AttributeCollection Attribs { get; private set; }
        public VBO[] vbos;
        public Mesh(string name)
        {
            Name = name;
            Attribs = new AttributeCollection();
            vbos = new VBO[0];
            this.PrimitiveType = ElementType.POINTS;
        }

        public void BuildBuffers()
        {
            if(vbos.Length > 0)
            {
                foreach(VBO vbo in vbos)
                {
                    vbo.Dispose();
                }
            }
            vbos = new VBO[Attribs.Count];
            int index = 0;
            foreach (KeyValuePair<Buffers.VertexBufferAttributes.AttributeType, Buffers.VertexBufferAttributes.Attribute> at in Attribs)
            {
                VBO tmp = VBO.Create(at.Key, at.Value);
                vbos[index] = tmp;
                index++;
            }
        }

        public static void Import(string identifier, string path, ElementType prim_type)
        {
            Mesh mesh = new Mesh(identifier);
            mesh.PrimitiveType = prim_type;
            String error = ImportMeshFromFile(path, out Assimp.Mesh a_mesh);
            if(error.Length > 0)
            {
                throw new Exception($"Mesh import failed: {error}");
            }

            ParseMeshData(mesh, a_mesh);
            mesh.BuildBuffers();
            MeshResourceManager.Instance.AddResource(identifier, mesh);
        }

        private static String ImportMeshFromFile(string path, out Assimp.Mesh mesh)
        {
            mesh = null;
            try
            {
                Assimp.AssimpContext ac = new AssimpContext();
                ac.SetConfig(new NormalSmoothingAngleConfig(66.0f));
                Assimp.Scene s = ac.ImportFile(path, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.CalculateTangentSpace);

                if (s.MeshCount == 0)
                {
                    return "No meshes were found in the file";
                }

                if (s.MeshCount > 1)
                {
                    return "Scene detected: please only load single mesh files";
                }

                mesh = s.Meshes[0];
                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static void ParseMeshData(Mesh obj, Assimp.Mesh mesh)
        {
            obj.Attribs.ClearAttributes();
            List<Vector3f> vertex_data =    new List<Vector3f>();
            List<Vector3f> normal_data =    new List<Vector3f>();
            List<Vector3f> texture_data =   new List<Vector3f>();
            List<Vector3f> tangent_data =   new List<Vector3f>();

            bool hastex = mesh.HasTextureCoords(0);

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                Vector3f v;
                v.X = mesh.Vertices[i].X;
                v.Y = mesh.Vertices[i].Y;
                v.Z = mesh.Vertices[i].Z;

                Vector3f n;
                n.X = mesh.Normals[i].X;
                n.Y = mesh.Normals[i].Y;
                n.Z = mesh.Normals[i].Z;

                Vector3f t;
                if (hastex)
                {
                    t.X = mesh.TextureCoordinateChannels[0][i].X;
                    t.Y = mesh.TextureCoordinateChannels[0][i].Y;
                    t.Z = 0.0f;
                }
                else
                {
                    t.X = 0.0f;
                    t.Y = 0.0f;
                    t.Z = 0.0f;
                }

                Vector3f tangent = new Vector3f();

                if (mesh.HasTangentBasis)
                {
                    tangent.X = mesh.Tangents[i].X;
                    tangent.Y = mesh.Tangents[i].Y;
                    tangent.Z = mesh.Tangents[i].Z;
                }

                vertex_data.Add(v);
                normal_data.Add(n);
                texture_data.Add(t);
                tangent_data.Add(tangent);

            }
            obj.VertexCount = (uint)vertex_data.Count;

            FloatAttribute vertex_attrib = new FloatAttribute(DataCollectionType.VEC3);
            vertex_attrib.AddData(vertex_data.ToArray());

            FloatAttribute normal_attrib = new FloatAttribute(DataCollectionType.VEC3);
            normal_attrib.AddData(normal_data.ToArray());

            FloatAttribute texture_attrib = new FloatAttribute(DataCollectionType.VEC3);
            texture_attrib.AddData(texture_data.ToArray());

            FloatAttribute tangent_attrib = new FloatAttribute(DataCollectionType.VEC3);
            tangent_attrib.AddData(tangent_data.ToArray());


            obj.Attribs.AddAttribute(vertex_attrib, Buffers.VertexBufferAttributes.AttributeType.POSITION);
            obj.Attribs.AddAttribute(normal_attrib, Buffers.VertexBufferAttributes.AttributeType.NORMAL);
            obj.Attribs.AddAttribute(texture_attrib, Buffers.VertexBufferAttributes.AttributeType.TEXTURE);
            obj.Attribs.AddAttribute(tangent_attrib, Buffers.VertexBufferAttributes.AttributeType.TANGENT);
        }
    }

    public class VBO : IDisposable
    {
        public uint handle;
        public uint value_size;
        public Buffers.VertexBufferAttributes.AttributeDataType value_type;
        public Buffers.VertexBufferAttributes.AttributeType attrib_type;
        public DataCollectionType coll_type;

        public static VBO Create(Buffers.VertexBufferAttributes.AttributeType attribtype, Buffers.VertexBufferAttributes.Attribute attrib)
        {
            byte[] vbobuff = attrib.data.ToArray();
            VBO tmp = new VBO()
            {
                handle = Context.gl_context.GenBuffer(),
                attrib_type = attribtype,
                value_type = attrib.valuetype,
                coll_type = attrib.coll_type,
                value_size = attrib.value_size
            };


            Context.gl_context.BindBuffer(GLEnum.ArrayBuffer, tmp.handle);
            unsafe
            {
                fixed (void* d_ptr = &vbobuff[0])
                {
                    Context.gl_context.BufferData(GLEnum.ArrayBuffer, (uint)vbobuff.Length, d_ptr, GLEnum.StaticDraw);
                }
            };
            Context.gl_context.BindBuffer(GLEnum.ArrayBuffer, 0);
            return tmp;
        }
        private void Destroy()
        {
            if(this.handle != 0)
            {
                Context.gl_context.DeleteBuffer(this.handle);
                this.handle = 0;
            }
            
        }

        public void Dispose()
        {
            Destroy();
        }
    }

    public enum ElementType
    {
        POINTS = Silk.NET.OpenGL.GLEnum.Points,
        LINES = Silk.NET.OpenGL.GLEnum.Lines,
        TRIANGLES = Silk.NET.OpenGL.GLEnum.Triangles
    }
}
