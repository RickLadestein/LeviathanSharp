using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Leviathan.Util
{
    public class FileManager
    {
        private static FileManager instance;
        public static FileManager GetInstance()
        {
            if(instance == null) { instance = new FileManager(); }
            return instance;
        }

        private Dictionary<string, string> directories;

        private FileManager()
        {
            this.directories = new Dictionary<string, string>();
        }

        public string GetDirectoryPath(string identifier)
        {
            if(directories.ContainsKey(identifier))
            {
                return directories[identifier];
            }
            return string.Empty;
        }

        public bool AddDirectoryPath(string folder_path, string identifier, bool replace = false)
        {
            if(directories.ContainsKey(identifier))
            {
                if(replace)
                {
                    directories[identifier] = folder_path;
                } else
                {
                    return false;
                }
            }
            directories.Add(identifier, folder_path);
            return true;
        }

        public bool RemoveDirectoryPath(string identifier)
        {
            if(directories.ContainsKey(identifier))
            {
                directories.Remove(identifier);
                return true;
            }
            return false;
        }

        public string CombineFilepath(string folder_identifier, string file)
        {
            string directory = GetDirectoryPath(folder_identifier);
            return Path.Combine(directory, file);
        }

        public bool CheckIfFileExists(string path)
        {
            try
            {
                return File.Exists(path);
            } catch(Exception)
            {
                return false;
            }
        }

        public string ReadTextFile(string folder_identifier, string file_name)
        {
            return ReadTextFile(CombineFilepath(folder_identifier, file_name));
        }

        public string ReadTextFile(string path)
        {
            if(!CheckIfFileExists(path)) { return string.Empty; }
            FileStream fs = null;
            string output = string.Empty;
            try
            {
                fs = File.OpenRead(path);
                StreamReader reader = new StreamReader(fs);
                output = reader.ReadToEnd();
            } catch(Exception)
            {
                output = string.Empty;
            } finally
            {
                fs?.Flush();
                fs?.Close();
            }
            return output;
        }

        public bool WriteTextFile(string folder_identifier, string file_name, string content, bool truncate = false)
        {
            return WriteTextFile(CombineFilepath(folder_identifier, file_name), content, truncate);
        }

        public bool WriteTextFile(string path, string content, bool truncate)
        {
            FileMode mode;
            if (!CheckIfFileExists(path))
            {
                mode = FileMode.Create;
            } else if(truncate)
            {
                mode = FileMode.Truncate;
            } else
            {
                mode = FileMode.Open;
            }

            FileStream fs = null;
            bool succes = false;
            try
            {
                fs = File.Open(path, mode, FileAccess.Write);
                StreamWriter writer = new StreamWriter(fs);
                writer.Write(content);
                succes = true;
            }
            catch (Exception)
            {
                succes = false;
            }
            finally
            {
                fs?.Flush();
                fs?.Close();
            }
            return succes;
        }

    }
}
