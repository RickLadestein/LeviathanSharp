using System;
using System.Collections.Generic;
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
    /// Interaction logic for ProjectViewer.xaml
    /// </summary>
    public partial class ProjectViewer : UserControl
    {
        public ProjectViewer()
        {
            InitializeComponent();
            FileViewer.Focused = true;
            FileViewer.Initialize(Environment.CurrentDirectory);

            FolderControl.Initialize(Environment.CurrentDirectory, 0, FileViewer);
        }

        private void TabControl_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            return;
        }

        private void TabControl_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            return;
        }
    }
}
