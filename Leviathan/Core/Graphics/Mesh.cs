using Leviathan.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Leviathan.Util;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using Assimp;
using Assimp.Configs;

namespace Leviathan.Core.Graphics
{
    public class Mesh
    {
        public List<Primitive> object_data { get; private set; }
        public string name { get; set; }
        public bool valid { get; private set; }

        public Mesh()
        {
            object_data = new List<Primitive>();
            name = "default";
        }

        public Mesh(string name)
        {
            this.object_data = new List<Primitive>();
            this.name = name;
        }

        public Mesh(in List<Primitive> obj, string name)
        {
            this.object_data = new List<Primitive>(obj);
            this.name = name;
        }

        public Mesh(string name, List<Vector3D> vertices, List<Vector3D> normals, List<Vector3D> textures, List<Face> faces)
        {
            this.name = name;
            this.object_data = new List<Primitive>();
            List<VertexData> v_data = new List<VertexData>();
            for (int i = 0; i < faces.Count; i++)
            {
                for(int index = 0; index < 3; index++)
                {
                    Vector3D vert = vertices[faces[i].Indices[index]];
                    float vx = vert.X;
                    float vy = vert.Y;
                    float vz = vert.Z;
                    Vector3 vertex = new Vector3(vx, vy, vz);

                    Vector3D norm = normals[faces[i].Indices[index]];
                    float nx = norm.X;
                    float ny = norm.Y;
                    float nz = norm.Z;
                    Vector3 normal = new Vector3(nx, ny, nz);

                    Vector3D tex = textures[faces[i].Indices[index]];
                    float tx = tex.X;
                    float ty = tex.Y;
                    float tz = tex.Z;
                    Vector3 texture = new Vector3(tx, ty, tz);

                    VertexData data = new VertexData(vertex, normal, texture);
                    v_data.Add(data);
                }
                
            }

            if(v_data.Count % 3 != 0)
            {
                valid = false;
                Logger.GetInstance().LogError($"Model {this.name} failed: vertex data was not triangulated");
                return;
            }
            for(int i = 0; i < v_data.Count; i+=3)
            {
                Primitive p = new Primitive(v_data[i], v_data[i + 1], v_data[i + 2]);
                this.object_data.Add(p);
            }
        }

        public static Mesh[] Load(string folder_id, string file)
        {
            string path = FileManager.GetInstance().CombineFilepath(folder_id, file);
            if(!FileManager.GetInstance().CheckIfFileExists(path))
            {
                Logger.GetInstance().LogError($"Could not load {path}: invalid file path");
                return null;
            }
            AssimpContext importer = new AssimpContext();
            var scene = importer.ImportFile(path, 
                PostProcessSteps.Triangulate |
                PostProcessSteps.SortByPrimitiveType |
                PostProcessSteps.GenerateSmoothNormals);
            if(scene == null)
            {
                Logger.GetInstance().LogError("Mesh loading failed");
                return new Mesh[0]; 
            }
            List<Mesh> meshes = new List<Mesh>();
            foreach(var mesh in scene.Meshes)
            {
                if(mesh.PrimitiveType != PrimitiveType.Triangle)
                {
                    Logger.GetInstance().LogWarning($"Detected non triangle primitive {mesh.Name}: skipping");
                } else
                {
                    Mesh md = new Mesh(mesh.Name, mesh.Vertices, mesh.Normals, mesh.TextureCoordinateChannels[0], mesh.Faces);
                    meshes.Add(md);
                }
                
            }
            return meshes.ToArray();
        }
    }
}
