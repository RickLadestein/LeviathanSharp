using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public static void InitDefaultResources()
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

}
