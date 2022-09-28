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

namespace LeviathanEditor.Controls.GameWindow.Graphs
{
    /// <summary>
    /// Interaction logic for ShaderGraph.xaml
    /// </summary>
    public partial class ShaderGraph : UserControl
    {

        public NodeGraph.ViewModel.FlowChartViewModel FlowChartViewModel
        {
            get { return (NodeGraph.ViewModel.FlowChartViewModel)GetValue(FlowChartViewModelProperty); }
            set { SetValue(FlowChartViewModelProperty, value); }
        }
        public static readonly DependencyProperty FlowChartViewModelProperty =
            DependencyProperty.Register("FlowChartViewModel", typeof(NodeGraph.ViewModel.FlowChartViewModel), typeof(MainWindow), new PropertyMetadata(null));


        public ShaderGraph()
        {
            InitializeComponent();
        }
    }
}
