using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for FileControl.xaml
    /// </summary>
    public partial class FileControl : UserControl
    {
        public string path;
        private string file_name;
        private string[] extensions;
        public FileControl()
        {
            InitializeComponent();
        }

        public FileControl(string file_path)
        {
            InitializeComponent();

            this.path = file_path;
            
            string[] tokens = path.Split('\\');
            string file = tokens[tokens.Length - 1];
            
            string[] parts = file.Split('.');
            file_name = parts[0];
            if(parts.Length == 2)
            {
                extensions = new string[1] { parts[parts.Length - 1] };
            } else
            {
                extensions = new string[parts.Length - 1];
                for(int i = 1; i < parts.Length; i++)
                {
                    extensions[i - 1] = parts[i];
                }
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(this.file_name);
            foreach(string s in extensions)
            {
                builder.Append('.');
                builder.Append(s);
            }
            this.FileNameLabel.Text = builder.ToString();
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "explorer";
            process.StartInfo.Arguments = $"{this.path}";
            process.Start();
        }
    }
}
