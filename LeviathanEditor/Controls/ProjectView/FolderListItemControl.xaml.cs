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
    /// Interaction logic for FolderListItemControl.xaml
    /// </summary>
    public partial class FolderListItemControl : UserControl
    {
        private string relativeURI;
        private string folderuri;
        private uint indentlevel;
        private bool expanded;
        private bool isroot;

        private List<FolderListItemControl> folder_list;

        public static FolderListItemControl focussed;

        public readonly int INDENT_SIZE_PER_LEVEL = 10;

        public IFileEnvironmentListener FileEnvironmentListener { get; private set; }
        public FolderListItemControl()
        {
            InitializeComponent();
            folderuri = "";
            this.RelativeUri = "";
            this.IndentLevel = 0;
            this.expanded = false;
            folder_list = new List<FolderListItemControl>();
        }

        public void Initialize(String folder_url, uint indent_level, IFileEnvironmentListener fileEnvironmentListener, bool isroot = false)
        {
            folderuri = "";
            this.RelativeUri = folder_url;
            this.IndentLevel = indent_level;
            this.expanded = false;
            this.isroot = isroot;
            folder_list = new List<FolderListItemControl>();
            this.FileEnvironmentListener = fileEnvironmentListener;
            if (IsFolderEmpty())
            {
                this.ExpandButton.IsEnabled = false;
                this.ExpandButton.Visibility = Visibility.Hidden;
            }
        }

        public string RelativeUri
        {
            get
            {
                return relativeURI;
            }
            set
            {
                relativeURI = value;

                string[] tokens = relativeURI.Split('\\');
                folderuri = tokens[tokens.Length - 1];
                FolderNameTextField.Content = folderuri;
            }
        }

        public uint IndentLevel
        {
            get
            {
                return indentlevel;
            }
            set
            {
                indentlevel = value;
                this.IndentBox.Width = INDENT_SIZE_PER_LEVEL * indentlevel;
            }
        }

        private bool IsFolderEmpty()
        {
            IEnumerable<string> found = Directory.EnumerateDirectories(this.relativeURI);
            if(found.Count() == 0)
            {
                return true;
            }
            return false;
        }

        public void Expand()
        {
            if(!this.expanded)
            {
                IEnumerable<string> found = Directory.EnumerateDirectories(this.relativeURI);
                foreach(string s in found)
                {
                    FolderListItemControl control = new FolderListItemControl();
                    control.Initialize(s, this.indentlevel + 1, this.FileEnvironmentListener);
                    this.SubFolderCollection.Children.Add(control);
                    this.folder_list.Add(control);
                }
                this.expanded = true;
            }
        }

        public void Collapse()
        {
            if (this.expanded)
            {
                folder_list.Clear();
                this.SubFolderCollection.Children.Clear();
                this.expanded = false;
            }
        }

        public void Focus()
        {
            focussed = this;
            this.ContentGrid.Background = (SolidColorBrush)Application.Current.Resources["ControlBackground2"];
            if(FileEnvironmentListener != null)
            {
                FileEnvironmentListener.OnFocusPathChanged(this.RelativeUri);
            }
        }

        public void UnFocus()
        {
            focussed = null;
            this.ContentGrid.Background = (SolidColorBrush) Application.Current.Resources["ControlForeground"];
        }

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.expanded)
            {
                this.Collapse();
                RotateTransform rotateTransform = new RotateTransform(0);
                ExpandButton.RenderTransform = rotateTransform;
            } else
            {
                this.Expand();
                RotateTransform rotateTransform = new RotateTransform(90);
                ExpandButton.RenderTransform = rotateTransform;
            }
        }

        private void WrapPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(focussed != this)
            {
                if(focussed != null)
                {
                    focussed.UnFocus();
                }
                this.Focus();
            }
        }
    }
}
