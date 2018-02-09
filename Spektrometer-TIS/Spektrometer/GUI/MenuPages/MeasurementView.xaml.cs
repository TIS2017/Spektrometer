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
            if (_graphController.GraphData.ShowPeaks)
            {
                Peaks.Content = "Show";             
            } else
            {
                Peaks.Content = "Hide";
            }
            _graphController.GraphData.ShowPeaks = !_graphController.GraphData.ShowPeaks;
        }

        private void SetGlobalPeak(object sender, RoutedEventArgs e)
        {
            if (_graphController.GraphData.GlobalPeak)
            {
                GlobalPeakChecked.IsChecked = false;
            } else
            {
                GlobalPeakChecked.IsChecked = true;
            }
            _graphController.GraphData.GlobalPeak = !_graphController.GraphData.GlobalPeak;
        }

        private void SetShowValue(object sender, RoutedEventArgs e)
        {
            if (Value.Text != "")
            {
                _graphController.GraphData.Treshold = Int32.Parse(Value.Text);

                if (_graphController.GraphData.ShowValues)
                {
                    Values.Content = "Show";
                    _graphController.GraphData.ShowValues = !_graphController.GraphData.ShowValues;
                }
                else
                {
                    Values.Content = "Hide";
                    _graphController.GraphData.ShowValues = !_graphController.GraphData.ShowValues;
                }
            }


        }

        private void ReferencePicture(object sender, RoutedEventArgs e)
        {
            if (_graphController.GraphData.ActualPicture.Count > 0)
                _graphController.GraphData.ReferencedPicture = _graphController.GraphData.ActualPicture;
            else
                MessageBox.Show("Ziadne data pre ulozenie referencneho snimku.");
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

        private void FilterChart(object sender, SelectionChangedEventArgs e)
        {
            switch (FilterChartList.SelectedIndex)
            {
                case 0:
                    _graphController.GraphData.Filter = Filter.RGB;
                    break;
                case 1:
                    _graphController.GraphData.Filter = Filter.R;
                    break;
                case 2:
                    _graphController.GraphData.Filter = Filter.G;
                    break;
                case 3:
                    _graphController.GraphData.Filter = Filter.B;
                    break;
                default:
                    _graphController.GraphData.Filter = Filter.RGB;
                    break;
            }
        }

        private void FillChartChecked(object sender, RoutedEventArgs e)
        {
            // call method to start fillChart
        }

        private void FillChartUnChecked(object sender, RoutedEventArgs e)
        {
            // call method to stop fillChart
        }
    }
}
