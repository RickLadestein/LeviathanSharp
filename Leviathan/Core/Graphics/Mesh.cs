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
using Microsoft.VisualBasic.CompilerServices;
using System.Threading;

namespace Leviathan.Core.Graphics
{
    public class Mesh
    {
        public List<Primitive> Object_data { get; private set; }
        public bool Valid { get; private set; }

        public Mesh()
        {
            Object_data = new List<Primitive>();
            Valid = true;
        }

        public Mesh(in List<Primitive> obj)
        {
            this.Object_data = new List<Primitive>(obj);
        }

        public Mesh(List<Vector3D> vertices, List<Vector3D> normals, List<Vector3D> textures, List<Face> faces)
        {
            this.Object_data = new List<Primitive>();
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
                Valid = false;
                Logger.GetInstance().LogError($"Model loading failed: vertex data was not triangulated");
                return;
            }
            for(int i = 0; i < v_data.Count; i+=3)
            {
                Primitive p = new Primitive(v_data[i], v_data[i + 1], v_data[i + 2]);
                this.Object_data.Add(p);
            }
        }

        private static Dictionary<String, Mesh> meshes = new Dictionary<string, Mesh>();
        private static Mutex meshMutex = new Mutex();
        public static bool LoadFromFile(string folder_id, string file)
        {
            string path = FileManager.GetInstance().CombineFilepath(folder_id, file);
            if(!FileManager.GetInstance().CheckIfFileExists(path))
            {
                Logger.GetInstance().LogError($"Could not load {path}: invalid file path");
                return false;
            }
            AssimpContext importer = new AssimpContext();
            var scene = importer.ImportFile(path, 
                PostProcessSteps.Triangulate |
                PostProcessSteps.SortByPrimitiveType |
                PostProcessSteps.GenerateSmoothNormals);
            if(scene == null)
            {
                Logger.GetInstance().LogError("Mesh loading failed");
                return false;
            }

            foreach(var mesh in scene.Meshes)
            {
                if(mesh.PrimitiveType != PrimitiveType.Triangle)
                {
                    Logger.GetInstance().LogWarning($"Detected non triangle primitive {mesh.Name}: skipping");
                } else
                {
                    Mesh md = new Mesh(mesh.Vertices, mesh.Normals, mesh.TextureCoordinateChannels[0], mesh.Faces);
                    Mesh.AddMeshToLoadedMeshes(md, mesh.Name);
                }
            }
            return true;
        }
        public static Mesh GetMeshFromLoadedMeshes(String mesh_identifier)
        {
            meshMutex.WaitOne();
            try
            {
                if (meshes.ContainsKey(mesh_identifier))
                {
                    return meshes[mesh_identifier];
                }
                return null;
            } catch(Exception) {
                return null;
            } finally
            {
                meshMutex.ReleaseMutex();
            }
        }

        public static bool AddMeshToLoadedMeshes(Mesh mesh, string identifier)
        {
            meshMutex.WaitOne();
            try
            {
                if (!meshes.ContainsKey(identifier))
                {
                    meshes.Add(identifier, mesh);
                    return true;
                }
                meshMutex.ReleaseMutex();
                return false;
            } catch (Exception) {
                return false;
            } finally {
                meshMutex.ReleaseMutex();
            }
        }

        public static bool RemoveMeshFromLoadedMeshes(string identifier)
        {
            meshMutex.WaitOne();
            try
            {
                return meshes.Remove(identifier);
            } catch(Exception)
            {
                return false;
            } finally
            {
                meshMutex.ReleaseMutex();
            }
        }

        public static bool ReplaceMeshInLoadedMeshes(string identifier, Mesh newmesh)
        {
            meshMutex.WaitOne();
            try
            {
                if (meshes.ContainsKey(identifier))
                {
                    meshes[identifier] = newmesh;
                    return true;
                }
                return false;
            } catch(Exception e)
            {
                return false;
            } finally
            {
                meshMutex.ReleaseMutex();
            }
        }
    }
}
