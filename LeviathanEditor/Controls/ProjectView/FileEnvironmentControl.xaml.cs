using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeviathanEditor.Controls.ProjectView
{
    /// <summary>
    /// Interaction logic for FileEnvironmentControl.xaml
    /// </summary>
    public partial class FileEnvironmentControl : UserControl, IFileEnvironmentListener
    {
        public bool Focused { get; set; }
        private string currentfolderpath;
        public FileEnvironmentControl()
        {
            InitializeComponent();
            Focused = true;
            currentfolderpath = Environment.CurrentDirectory;
        }

        public void Initialize(string path)
        {
            currentfolderpath = path;
            Load();
        }

        public void Load()
        {
            
            IEnumerable<string> files = Directory.GetFiles(currentfolderpath);
            foreach(string file in files)
            {
                FileControl control = new FileControl(file);
                control.Visibility = Visibility.Visible;
                control.IsEnabled = true;
                control.Width = 96;
                control.Height = 96;
                this.FileViewCollection.Children.Add(control);
            }
        }

        public void UnLoad()
        {
            this.FileViewCollection.Children.Clear();
        }

        public void OnFocusPathChanged(string newpath)
        {
            currentfolderpath = newpath;

            if (Focused)
            {
                this.UnLoad();
                this.Load();
            }
            
        }

        public void OnFocusChanged(bool hasfocus)
        {
            this.Focused = hasfocus;
        }
    }

    public interface IFileEnvironmentListener
    {
        void OnFocusPathChanged(string newpath);
        void OnFocusChanged(bool hasfocus);
    }
}
