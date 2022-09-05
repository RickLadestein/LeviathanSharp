using Leviathan.Core.Graphics.Buffers.VertexBufferAttributes;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Leviathan.Util;
using Leviathan.Math;
using Leviathan.Core.Graphics.Buffers;
using Assimp;
using Assimp.Configs;

namespace Leviathan.Core.Graphics
{
    public class Mesh
    {
        public String Name { get; private set; }
        public VertexBuffer Vbuffer { get; private set; }
        public Mesh(string name)
        {
            Name = name;
            //Vbuffer = new VertexBuffer();
        }

        public static void Import(string identifier, string path, LPrimitiveType prim_type)
        {
            Mesh mesh = new Mesh(identifier);
            String error = ImportMeshFromFile(path, out Assimp.Mesh a_mesh);
            if(error.Length > 0)
            {
                throw new Exception($"Mesh import failed: {error}");
            }

            List<KeyValuePair<LAttributeType, VertexAttribute>> attrib_coll = new();
            ParseMeshData(attrib_coll, a_mesh);

            VertexBufferFactory factory = new VertexBufferFactory();
            factory.SetPrimitiveType(prim_type);
            foreach(KeyValuePair<LAttributeType, VertexAttribute> kvp in attrib_coll)
            {
                factory.AddVertexAttribute(kvp.Value, kvp.Key);
            }
            mesh.Vbuffer = factory.Build();
            Context.MeshManager.AddResource(identifier, mesh);
        }

        public static void Import(string identifier, string path, LPrimitiveType prim_type, out Mesh mesh)
        {
            mesh = new Mesh(identifier);
            String error = ImportMeshFromFile(path, out Assimp.Mesh a_mesh);
            if (error.Length > 0)
            {
                throw new Exception($"Mesh import failed: {error}");
            }
            List<KeyValuePair<LAttributeType, VertexAttribute>> attrib_coll = new();
            ParseMeshData(attrib_coll, a_mesh);
            
            VertexBufferFactory factory = new VertexBufferFactory();
            factory.SetPrimitiveType(prim_type);
            foreach (KeyValuePair<LAttributeType, VertexAttribute> kvp in attrib_coll)
            {
                factory.AddVertexAttribute(kvp.Value, kvp.Key);
            }
            mesh.Vbuffer = factory.Build();
            Context.MeshManager.AddResource(identifier, mesh);
        }

        public static void Import(string identifier, System.IO.Stream stream, LPrimitiveType prim_type)
        {
            Mesh mesh = new Mesh(identifier);

            String error = ImportMeshFromStream(stream, out Assimp.Mesh a_mesh);
            if (error.Length > 0)
            {
                throw new Exception($"Mesh import failed: {error}");
            }

            List<KeyValuePair<LAttributeType, VertexAttribute>> attrib_coll = new();
            ParseMeshData(attrib_coll, a_mesh);
            
            VertexBufferFactory factory = new VertexBufferFactory();
            factory.SetPrimitiveType(prim_type);
            foreach (KeyValuePair<LAttributeType, VertexAttribute> kvp in attrib_coll)
            {
                factory.AddVertexAttribute(kvp.Value, kvp.Key);
            }
            mesh.Vbuffer = factory.Build();
            Context.MeshManager.AddResource(identifier, mesh);
        }

        public static void Import(string identifier, List<KeyValuePair<LAttributeType, VertexAttribute>> attrib_coll, LPrimitiveType prim_type)
        {
            Mesh mesh = new Mesh(identifier);
            VertexBufferFactory factory = new VertexBufferFactory();
            factory.SetPrimitiveType(prim_type);
            foreach (KeyValuePair<LAttributeType, VertexAttribute> kvp in attrib_coll)
            {
                factory.AddVertexAttribute(kvp.Value, kvp.Key);
            }
            mesh.Vbuffer = factory.Build();
            Context.MeshManager.AddResource(identifier, mesh);
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

        private static String ImportMeshFromStream(System.IO.Stream filestream, out Assimp.Mesh mesh)
        {
            mesh = null;
            try
            {
                Assimp.AssimpContext ac = new AssimpContext();
                ac.SetConfig(new NormalSmoothingAngleConfig(66.0f));
                Assimp.Scene s = ac.ImportFileFromStream(filestream, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.CalculateTangentSpace);

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

        private static void ParseMeshData(List<KeyValuePair<LAttributeType, VertexAttribute>> obj, Assimp.Mesh mesh)
        {
            List<Vector3f> vertex_data = new List<Vector3f>();
            List<Vector3f> normal_data = new List<Vector3f>();
            List<Vector3f> texture_data = new List<Vector3f>();
            List<Vector3f> tangent_data = new List<Vector3f>();

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

            

            Float3Attribute vertex_attrib = new Float3Attribute();
            vertex_attrib.AddData(vertex_data.ToArray());

            Float3Attribute normal_attrib = new Float3Attribute();
            normal_attrib.AddData(normal_data.ToArray());

            Float3Attribute texture_attrib = new Float3Attribute();
            texture_attrib.AddData(texture_data.ToArray());

            Float3Attribute tangent_attrib = new Float3Attribute();
            tangent_attrib.AddData(tangent_data.ToArray());


            obj.Add(new KeyValuePair<LAttributeType, VertexAttribute>(LAttributeType.POSITION, vertex_attrib.CompileToVertexAttribute()));
            obj.Add(new KeyValuePair<LAttributeType, VertexAttribute>(LAttributeType.NORMAL, normal_attrib.CompileToVertexAttribute()));
            obj.Add(new KeyValuePair<LAttributeType, VertexAttribute>(LAttributeType.TEXTURECOORD1, texture_attrib.CompileToVertexAttribute()));
            obj.Add(new KeyValuePair<LAttributeType, VertexAttribute>(LAttributeType.TANGENT, tangent_attrib.CompileToVertexAttribute()));
        }
    }

    public enum LPrimitiveType
    {
        POINTS = Silk.NET.OpenGL.GLEnum.Points,
        LINES = Silk.NET.OpenGL.GLEnum.Lines,
        TRIANGLES = Silk.NET.OpenGL.GLEnum.Triangles
    }
}
