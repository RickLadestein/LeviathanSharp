﻿using Leviathan.Core.Graphics;
using Leviathan.Core.Graphics.Buffers;
using Leviathan.Core.Sound;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Leviathan.Util
{
    public abstract class ResourceManager<T> where T : class
    {
        private Dictionary<String, T> resources;
        private Mutex access_mutex;

        /// <summary>
        /// Instantiates a new instance of ResourceManager with default attributes
        /// </summary>
        public ResourceManager()
        {
            resources = new Dictionary<string, T>();
            access_mutex = new Mutex();
        }

        public static void Init()
        {
            if (!System.IO.Directory.Exists("./assets"))
            {
                System.IO.Directory.CreateDirectory("./assets");
            }

            if (!System.IO.Directory.Exists("./assets/default"))
            {
                System.IO.Directory.CreateDirectory("./assets/default");

                System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
                string[] names = ass.GetManifestResourceNames();
                System.IO.Stream stream;
                foreach (String filename in names)
                {
                    stream = ass.GetManifestResourceStream(filename);
                    if (stream == null)
                    {
                        throw new Exception("Manifest stream error!");
                    }
                    else
                    {
                        String[] tokens = filename.Split(".");
                        System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                        System.IO.StreamWriter writer = new System.IO.StreamWriter($"./assets/default/{tokens[tokens.Length - 2]}.{tokens[tokens.Length - 1]}", false);
                        writer.Write(reader.ReadToEnd());
                        reader.Close();
                        writer.Flush();
                        writer.Close();
                    }
                    stream.Close();
                }
            }

            

            TextureResourceManager trm = TextureResourceManager.Instance;
            MeshResourceManager mrm = MeshResourceManager.Instance;
            ShaderResourceManager srm = ShaderResourceManager.Instance;
        }

        /// <summary>
        /// Checks if the collection contains a resource with given identifier
        /// </summary>
        /// <param name="identifier">The resource identifier</param>
        /// <returns></returns>
        public bool HasResource(string identifier)
        {
            access_mutex.WaitOne();
            bool found = resources.ContainsKey(identifier);
            access_mutex.ReleaseMutex();
            return found;
        }

        /// <summary>
        /// Adds a new resource with matching identifier to the collection
        /// </summary>
        /// <param name="identifier">The identifier for the resource</param>
        /// <param name="resource">The resource</param>
        /// <returns>true if resource was added, false if resource already existed under that specific identifier</returns>
        public bool AddResource(String identifier, T resource)
        {
            if (resource == null)
            {
                throw new ArgumentException("Resource cannot be null");
            }

            if (identifier == null || identifier.Length == 0)
            {
                throw new ArgumentException("Identifier cannot be null or empty");
            }

            access_mutex.WaitOne();
            bool succes = resources.TryAdd(identifier, resource);
            access_mutex.ReleaseMutex();
            if (succes)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Gets a resource at specified identifier
        /// </summary>
        /// <param name="identifier">The resource identifier string</param>
        /// <returns>Resource if found, returns null if not found</returns>
        public T GetResource(String identifier)
        {
            if (identifier == null || identifier.Length == 0)
            {
                throw new ArgumentException("Identifier cannot be null or empty");
            }

            access_mutex.WaitOne();
            bool succes = resources.TryGetValue(identifier, out T res);
            if (succes)
            {
                access_mutex.ReleaseMutex();
                return res;
            }
            else
            {
                access_mutex.ReleaseMutex();
                return null;
            }
        }

        /// <summary>
        /// Sets a the resource at specified identifier if the collection already contains the identifier
        /// </summary>
        /// <param name="identifier">The resource identifier string</param>
        /// <param name="resource">The resource to be set at identifier</param>
        /// <returns>true if resource was set, false if the identifier is foreign to the collection</returns>
        public bool SetResource(String identifier, T resource)
        {
            if (resource == null)
            {
                throw new ArgumentException("Resource cannot be null");
            }

            if (identifier == null || identifier.Length == 0)
            {
                throw new ArgumentException("Identifier cannot be null or empty");
            }

            access_mutex.WaitOne();
            bool found = resources.ContainsKey(identifier);
            if (found)
            {
                access_mutex.ReleaseMutex();
                resources[identifier] = resource;
                return true;
            }
            else
            {
                access_mutex.ReleaseMutex();
                throw new Exception("Could not add resource to identifier because manager does not contain identifier");
            }
        }

        /// <summary>
        /// Deletes resource with specified identifier from the collection
        /// </summary>
        /// <param name="identifier">The resource identifier string</param>
        /// <returns>true if resource was deleted, false if the resource was not found within the list</returns>
        public bool DeleteResource(String identifier)
        {
            if (identifier == null || identifier.Length == 0)
            {
                throw new ArgumentException("Identifier cannot be null or empty");
            }
            access_mutex.WaitOne();
            bool success = resources.Remove(identifier);
            access_mutex.ReleaseMutex();
            return success;
        }
    }



    public class MeshResourceManager : ResourceManager<Mesh>
    {
        private static MeshResourceManager instance;
        private static object _lck = new object();

        /// <summary>
        /// The current instance of MeshResourceManager
        /// </summary>
        public static MeshResourceManager Instance
        {
            get
            {
                lock (_lck)
                {
                    if (instance == null)
                    {
                        instance = new MeshResourceManager();
                    }
                }
                return instance;
            }
        }

        private MeshResourceManager() : base()
        {
            IEnumerable<string> eFiles = System.IO.Directory.EnumerateFiles(".\\assets\\default");
            foreach(string file in eFiles)
            {
                if(file.EndsWith(".obj"))
                {
                    String[] tokens = file.Split("\\");
                    string full_name = tokens[tokens.Length - 1];
                    string name = full_name.Remove(full_name.Length - 4);
                    Mesh.Import(name, file, ElementType.TRIANGLES, out Mesh mesh);
                    this.AddResource(name, mesh);
                }
            }

        }

    }

    public class ShaderResourceManager : ResourceManager<ShaderProgram>
    {
        private static ShaderResourceManager instance;
        private static object _lck = new object();

        /// <summary>
        /// The current instance of ShaderResourceManager
        /// </summary>
        public static ShaderResourceManager Instance
        {
            get
            {
                lock (_lck)
                {
                    if (instance == null)
                    {
                        instance = new ShaderResourceManager();
                    }
                }
                return instance;
            }
        }

        private ShaderResourceManager() : base()
        {
            IEnumerable<string> eFiles = System.IO.Directory.EnumerateFiles(".\\assets\\default");
            foreach (string file in eFiles)
            {
                if (file.EndsWith(".glsl"))
                {
                    String[] tokens = file.Split("\\");
                    string full_name = tokens[tokens.Length - 1];
                    string name = full_name.Remove(full_name.Length - 5);
                    ShaderFile sfile = ShaderFile.Import(file);
                    ShaderProgram sp = null;

                    if (sfile.HasVertex && sfile.HasFragment)
                    {
                        VertexShader vshader = new VertexShader(sfile.Vertex_src);
                        FragmentShader fshader = new FragmentShader(sfile.Fragment_src);
                        if (sfile.HasGeometry)
                        {
                            GeometryShader gshader = new GeometryShader(sfile.Geometry_src);
                            sp = new ShaderProgram(vshader, gshader, fshader);
                        }
                        sp = new ShaderProgram(vshader, fshader);
                    }
                    else if (sfile.HasCompute)
                    {
                        ComputeShader cshader = new ComputeShader(sfile.Compute_src);
                        sp = new ShaderProgram(cshader);
                    }
                    else
                    {
                        throw new Exception("Shaderfile was not complete: please fix");
                    }
                    AddResource(name, sp);
                }
            }
        }
    }

    public class TextureResourceManager : ResourceManager<Texture>
    {
        private static TextureResourceManager instance;
        private static object _lck = new object();

        /// <summary>
        /// The current instance of ShaderResourceManager
        /// </summary>
        public static TextureResourceManager Instance
        {
            get
            {
                lock (_lck)
                {
                    if (instance == null)
                    {
                        instance = new TextureResourceManager();
                    }
                }
                return instance;
            }
        }

        public TextureResourceManager() : base()
        {
            IEnumerable<string> eFiles = System.IO.Directory.EnumerateFiles(".\\assets\\default");
            foreach (string file in eFiles)
            {
                if (file.EndsWith(".jpeg"))
                {
                    String[] tokens = file.Split("\\");
                    string full_name = tokens[tokens.Length - 1];
                    string name = full_name.Remove(full_name.Length - 4);
                    Leviathan.Core.ImageResource image = Leviathan.Core.ImageResource.Load(file, true);
                    Texture2D tex = new Texture2D(image);
                    AddResource(name, tex);
                }
            }
        }
    }

    public class AudioResourceManager : ResourceManager<AudioSample>
    {

    } 
}
