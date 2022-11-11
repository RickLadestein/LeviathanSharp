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
using System.IO;


namespace Leviathan.Core.Graphics
{
    public class Mesh
    {
        public String Name { get; private set; }
        public VertexBuffer Vbuffer { get; set; }
        public Mesh(string name)
        {
            Name = name;
            //Vbuffer = new VertexBuffer();
        }
    }

    public class MeshLoader : IDisposable
    {
        public List<Mesh> Meshes;
        public List<int> material_bindings;

        public List<Material> materials;
        public List<Texture2D> textures;
        public List<Tuple<Mesh, Material>> materialbindings;

        public MeshLoader()
        {
            Meshes = new List<Mesh>();
            textures = new List<Texture2D>();
            materialbindings = new List<Tuple<Mesh, Material>>();
            material_bindings = new List<int>();
            materials = new List<Material>();
        }

        public void PushToRenderContext()
        {
            foreach(Mesh mesh in Meshes)
            {
                Context.MeshManager.AddResource(mesh.Name, mesh);
            }

            foreach(Material material in materials)
            {
                Context.MaterialManager.AddResource(material.Name, material);
            }
        }

        public void ImportMeshesFromScene(List<Assimp.Mesh> meshes)
        {
            foreach (Assimp.Mesh a_mesh in meshes)
            {
                ImportMesh(a_mesh);
            }
        }

        public void ImportMesh(Assimp.Mesh mesh)
        {
            Mesh current = new Mesh(mesh.Name);
            List<KeyValuePair<LAttributeType, VertexAttribute>> attrib_coll = new();
            ParseMeshData(attrib_coll, mesh);

            VertexBufferFactory factory = new VertexBufferFactory();
            factory.SetPrimitiveType(LPrimitiveType.TRIANGLES);
            foreach (KeyValuePair<LAttributeType, VertexAttribute> kvp in attrib_coll)
            {
                factory.AddVertexAttribute(kvp.Value, kvp.Key);
            }
            current.Vbuffer = factory.Build();
            Meshes.Add(current);
        }

        public void Import(string path, LPrimitiveType prim_type)
        {
            ImportAssimpMeshes(path, out List<Assimp.Mesh> loadedMeshes, out List<Assimp.Material> loadedMaterials);

            Material.FromAssimpMaterials(loadedMaterials, out materials);

            foreach(Material m in materials)
            {
                Context.MaterialManager.AddResource(m.Name, m);
                Material.ImportTexturesSynchronous(m);
            }


            foreach (Assimp.Mesh a_mesh in loadedMeshes)
            {
                Mesh current = new Mesh(a_mesh.Name);
                List<KeyValuePair<LAttributeType, VertexAttribute>> attrib_coll = new();
                ParseMeshData(attrib_coll, a_mesh);

                VertexBufferFactory factory = new VertexBufferFactory();
                factory.SetPrimitiveType(prim_type);
                foreach (KeyValuePair<LAttributeType, VertexAttribute> kvp in attrib_coll)
                {
                    factory.AddVertexAttribute(kvp.Value, kvp.Key);
                }
                current.Vbuffer = factory.Build();
                Material bound_material = materials[a_mesh.MaterialIndex];
                materialbindings.Add(new Tuple<Mesh, Material>(current, bound_material));
                Meshes.Add(current);
            }
        }

        public void Import(System.IO.Stream stream, LPrimitiveType prim_type)
        {
            List<Assimp.Mesh> loadedMeshes = ImportAssimpMeshesFromStream(stream);

            foreach(Assimp.Mesh a_mesh in loadedMeshes)
            {
                Mesh current = new Mesh(a_mesh.Name);
                List<KeyValuePair<LAttributeType, VertexAttribute>> attrib_coll = new();
                ParseMeshData(attrib_coll, a_mesh);

                VertexBufferFactory factory = new VertexBufferFactory();
                factory.SetPrimitiveType(prim_type);
                foreach (KeyValuePair<LAttributeType, VertexAttribute> kvp in attrib_coll)
                {
                    factory.AddVertexAttribute(kvp.Value, kvp.Key);
                }
                current.Vbuffer = factory.Build();
                Meshes.Add(current);
            }
            
        }

        public void ImportCustomMesh(string identifier, List<KeyValuePair<LAttributeType, VertexAttribute>> attrib_coll, LPrimitiveType prim_type)
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

        private void ImportAssimpMeshes(string file_path, out List<Assimp.Mesh> meshes, out List<Assimp.Material> materials)
        {
            meshes = new List<Assimp.Mesh>();
            materials = new List<Assimp.Material>();

            Assimp.AssimpContext ac = new AssimpContext();
            ac.SetConfig(new NormalSmoothingAngleConfig(66.0f));
            Assimp.Scene scene = ac.ImportFile(file_path, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.CalculateTangentSpace);
            meshes.AddRange(scene.Meshes);
            materials.AddRange(scene.Materials);
            ac.Dispose();
        }

        private List<Assimp.Mesh> ImportAssimpMeshesFromStream(Stream filestream)
        {
            List<Assimp.Mesh> output = new List<Assimp.Mesh>();

            Assimp.AssimpContext ac = new AssimpContext();
            ac.SetConfig(new NormalSmoothingAngleConfig(66.0f));
            Assimp.Scene scene = ac.ImportFileFromStream(filestream, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.CalculateTangentSpace);

            output.AddRange(scene.Meshes);

            scene.Clear();
            ac.Dispose();
            return output;
        }


        private void ParseMeshData(List<KeyValuePair<LAttributeType, VertexAttribute>> obj, Assimp.Mesh mesh)
        {
            List<Vector3f> vertex_data = new List<Vector3f>(mesh.VertexCount);
            List<Vector3f> normal_data = new List<Vector3f>(mesh.VertexCount);
            List<Vector3f> tangent_data = new List<Vector3f>(mesh.VertexCount);
            
            List<Vector3f>[] textures_data;
            bool hadnotexturechannels = false;
            if(mesh.TextureCoordinateChannelCount == 0)
            {
                textures_data = new List<Vector3f>[1];
                textures_data[0] = new List<Vector3f>(mesh.VertexCount) { Vector3f.Zero };
                hadnotexturechannels = true;
            } else
            {
                textures_data = new List<Vector3f>[mesh.TextureCoordinateChannelCount];
                for(int i = 0; i < mesh.TextureCoordinateChannelCount; i++)
                {
                    textures_data[i] = new List<Vector3f>(mesh.VertexCount);
                }
            }

            for (int i = 0; i < mesh.VertexCount; i++)
            {
                Vector3f v;
                v.X = mesh.Vertices[i].X;
                v.Y = mesh.Vertices[i].Y;
                v.Z = mesh.Vertices[i].Z;
                vertex_data.Add(v);

                Vector3f n;
                n.X = mesh.Normals[i].X;
                n.Y = mesh.Normals[i].Y;
                n.Z = mesh.Normals[i].Z;
                normal_data.Add(n);

                if (!hadnotexturechannels)
                {
                    Vector3f texcoord = Vector3f.Zero;
                    for (int j = 0; j < mesh.TextureCoordinateChannelCount; j++)
                    {
                        texcoord.X = mesh.TextureCoordinateChannels[j][i].X;
                        texcoord.Y = mesh.TextureCoordinateChannels[j][i].Y;
                        texcoord.Z = 0.0f;
                        textures_data[j].Add(texcoord);
                    }
                }

                Vector3f tangent = new Vector3f();
                if (mesh.HasTangentBasis)
                {
                    tangent.X = mesh.Tangents[i].X;
                    tangent.Y = mesh.Tangents[i].Y;
                    tangent.Z = mesh.Tangents[i].Z;
                }
                tangent_data.Add(tangent);
            }



            Float3Attribute vertex_attrib = new Float3Attribute();
            vertex_attrib.AddData(vertex_data.ToArray());

            Float3Attribute normal_attrib = new Float3Attribute();
            normal_attrib.AddData(normal_data.ToArray());

            Float3Attribute tangent_attrib = new Float3Attribute();
            tangent_attrib.AddData(tangent_data.ToArray());

            obj.Add(new KeyValuePair<LAttributeType, VertexAttribute>(LAttributeType.POSITION, vertex_attrib.CompileToVertexAttribute()));
            obj.Add(new KeyValuePair<LAttributeType, VertexAttribute>(LAttributeType.NORMAL, normal_attrib.CompileToVertexAttribute()));
            obj.Add(new KeyValuePair<LAttributeType, VertexAttribute>(LAttributeType.TANGENT, tangent_attrib.CompileToVertexAttribute()));

            int index = 0;
            foreach(List<Vector3f> tex_data in textures_data)
            {
                Float3Attribute texture_attrib = new Float3Attribute();
                texture_attrib.AddData(tex_data.ToArray());
                LAttributeType attrib_type = (LAttributeType)(((int)LAttributeType.TEXTURECOORD0) + index);
                obj.Add(new KeyValuePair<LAttributeType, VertexAttribute>(attrib_type, texture_attrib.CompileToVertexAttribute()));
                index += 1;
            }
            
        }

        public void Dispose()
        {
            Meshes.Clear();
            textures.Clear();
            this.materials.Clear();
            this.materialbindings.Clear();
        }
    }

    public enum LPrimitiveType
    {
        POINTS = Silk.NET.OpenGL.GLEnum.Points,
        LINES = Silk.NET.OpenGL.GLEnum.Lines,
        TRIANGLES = Silk.NET.OpenGL.GLEnum.Triangles
    }
}
