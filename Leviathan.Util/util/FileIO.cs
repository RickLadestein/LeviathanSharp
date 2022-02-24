using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Leviathan.Util.util
{
    public abstract class FileHandle : IDisposable
    {
        protected FileStream fs;
        protected FileSystemWatcher watcher;

        protected string folder;
        protected string file;

        protected string path;
        protected FileAccess access_mode;
        protected bool IsOpen { get; private set; }

        public FileHandle(string folder, string file, FileAccess acc_mode, bool append)
        {
            this.folder = folder;
            this.file = file;
            this.access_mode = acc_mode;

            if(folder.Length == 0)
            {
                throw new ArgumentException("Folder path cannot be empty");
            }

            if (!Directory.Exists(folder))
            {
                throw new Exception($"Directory [{folder}] is not valid");
            }

            if (file.Length == 0)
            {
                throw new ArgumentException("File cannot be empty");
            }

            if (folder.EndsWith(@"\") || folder.EndsWith(@"/")) {
                this.path = $"{folder}{file}";
            }
            else
            {
                this.path = $"{folder}{Path.DirectorySeparatorChar}{file}";
            }

            if(!File.Exists(path))
            {
                throw new Exception($"File {file} was not found in directory");
            }

            if(append)
            {
                fs = File.Open(path, FileMode.Append, acc_mode);
            } else
            {
                fs = File.Open(path, FileMode.Open, acc_mode);
            }
            
            IsOpen = true;
        }

        public void SubscribeToChanges(FileSystemEventHandler callback)
        {
            if(watcher == null)
            {
                CreateWatcher(NotifyFilters.LastWrite);
            }
            watcher.Changed += callback;
        }

        private void CreateWatcher(NotifyFilters filters)
        {
            watcher = new FileSystemWatcher(folder, file)
            {
                NotifyFilter = filters
            };
        }

        public void Dispose()
        {
            if(IsOpen)
            {
                fs.Flush();
                fs.Close();
            }

            if(watcher != null)
            {
                watcher.Dispose();
            }
        }
    }

    public class BinaryFileHandle : FileHandle
    {
        private BinaryReader reader;
        private BinaryWriter writer;
        public BinaryFileHandle(string folder, string file, FileAccess mode, bool append) : base(folder, file, mode, append)
        {
            if(mode == FileAccess.Read)
            {
                reader = new BinaryReader(fs);
            } else if(mode == FileAccess.Write)
            {
                writer = new BinaryWriter(fs);
            } else
            {
                writer = new BinaryWriter(fs);
                reader = new BinaryReader(fs);
            }
        }
    }

    public class RegularFileHandle : FileHandle
    {
        private StreamReader reader;
        private StreamWriter writer;

        public RegularFileHandle(string folder, string file, FileAccess mode, bool append) : base(folder, file, mode, append)
        {
            if (mode == FileAccess.Read)
            {
                reader = new StreamReader(fs);
            }
            else if (mode == FileAccess.Write)
            {
                writer = new StreamWriter(fs);
            }
            else
            {
                writer = new StreamWriter(fs);
                reader = new StreamReader(fs);
            }
        }
    }
}
