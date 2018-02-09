using System;
using System.Windows;
using System.Windows.Controls;

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
            if (_graphController.GraphData.ShowPeaks)
            {
                Values.Content = "Hide";
            }
            else
            {
                Values.Content = "Show";
            }
        }

        private void MenuButton(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new MenuView(MainWindow));
        }

        protected override void SetReferences()
        {
            _graphController = GraphController.GetInstance();
        }

        private void SetGlobalPeak(object sender, RoutedEventArgs e)
        {
            _graphController.GraphData.GlobalPeak = true;
        }

        private void UnsetGlobalPeak(object sender, RoutedEventArgs e)
        {
            _graphController.GraphData.GlobalPeak = false;
        }

        private void SetShowValue(object sender, RoutedEventArgs e)
        {
            if (Value.Text != "")
            {
                _graphController.GraphData.Treshold = Int32.Parse(Value.Text);

                if (_graphController.GraphData.ShowValues)
                {
                    Values.Content = "Hide";
                }
                else
                {
                    Values.Content = "Show";
                }
                _graphController.GraphData.ShowPeaks = !_graphController.GraphData.ShowPeaks;
                _graphController.GraphData.ShowValues = !_graphController.GraphData.ShowValues;
            }
            else
            {
             
                if (_graphController.GraphData.ShowPeaks)
                {
                    Values.Content = "Show";
                }
                else
                {
                    Values.Content = "Hide";
                }
                _graphController.GraphData.ShowPeaks = !_graphController.GraphData.ShowPeaks;
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
    }
}
