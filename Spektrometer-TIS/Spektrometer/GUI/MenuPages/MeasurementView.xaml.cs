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

using Spektrometer.Logic;

namespace Spektrometer.GUI
{
    /// <summary>
    /// Interaction logic for MeasurementView.xaml
    /// </summary>
    public partial class MeasurementView : MenuComponent
    {
        private GraphController _graphController;

        public GraphController GraphController
        {
            get
            {
                return _graphController;
            }
            set
            {
                _graphController = value;
            }
        }
        public MeasurementView(MainWindow mainWindow) : base(mainWindow)
        {
            InitializeComponent();
        }

        private void MenuButton(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new MenuView(MainWindow));
        }

        protected override void SetReferences()
        {
            _graphController = GraphController.GetInstance();
        }

        private void ShowPeaks(object sender, RoutedEventArgs e)
        {
            _graphController.GraphData.ShowPeaks = !_graphController.GraphData.ShowPeaks;
            if (_graphController.GraphData.ShowPeaks)
            {
                Peaks.Content = "Hide";
            } else
            {
                Peaks.Content = "Show";
            }
        }

        private void SetGlobalPeak(object sender, RoutedEventArgs e)
        {
            _graphController.GraphData.GlobalPeak = GlobalPeakChecked.IsChecked.GetValueOrDefault();
        }

        private void SetShowValue(object sender, RoutedEventArgs e)
        {
            if (Value.Text != "")
            {
                _graphController.GraphData.Treshold = Int32.Parse(Value.Text);
            }
        }

        private void ReferencePicture(object sender, RoutedEventArgs e)
        {
            _graphController.GraphData.ReferencedPicture = _graphController.GraphData.ActualPicture;
        
        }

        private void Subtraction(object sender, RoutedEventArgs e)
        {
            _graphController.GraphData.Subtraction = !_graphController.GraphData.Subtraction;
            BtnSubtraction.Content = _graphController.GraphData.Subtraction ? "Unset" : "Set";
        }

        private void Division(object sender, RoutedEventArgs e)
        {
            _graphController.GraphData.Division = !_graphController.GraphData.Division;
            BtnDivision.Content = _graphController.GraphData.Division ? "Unset" : "Set";
        }

        private void FillChart(object sender, SelectionChangedEventArgs e)
        {
            switch (FillChartList.SelectedItem)
            {
                case "all":
                    _graphController.GraphData.Filter = Filter.RGB;
                    break;
                case "red":
                    _graphController.GraphData.Filter = Filter.R;
                    break;
                case "green":
                    _graphController.GraphData.Filter = Filter.G;
                    break;
                case "blue":
                    _graphController.GraphData.Filter = Filter.B;
                    break;
                default:
                    _graphController.GraphData.Filter = Filter.RGB;
                    break;
            }
        }
    }
}
