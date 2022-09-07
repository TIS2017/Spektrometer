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

        private bool initializingOnly = false;
        public MeasurementView(MainWindow mainWindow) : base(mainWindow)
        {
            initializingOnly = true;
            InitializeComponent();
            initializingOnly = false;
            if (_graphController.GraphData.ShowValues)
            {
                Values.Content = "Hide";
            }
            else
            {
                Values.Content = "Show";
            }
            GlobalPeakChecked.IsChecked = _graphController.GraphData.GlobalPeak;
            Value.Text = _graphController.GraphData.Treshold.ToString();
            MinValeyHeight.Text = _graphController.GraphData.MinValeyHeight.ToString();
            XMinDist.Text = _graphController.GraphData.XMinDist.ToString();
            FilterChartList.SelectedIndex = (int)_graphController.GraphData.Filter;
            FillChartChecked.IsChecked = _graphController.GraphData.FillChart;
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

        private void SetFillChart(object sender, RoutedEventArgs e)
        {
            _graphController.GraphData.FillChart = true;
        }

        private void UnsetFillChart(object sender, RoutedEventArgs e)
        {
            _graphController.GraphData.FillChart = false;
        }

        private void SetShowValue(object sender, RoutedEventArgs e)
        {
            try
            {
                _graphController.GraphData.Treshold = Int32.Parse(Value.Text);
                _graphController.GraphData.XMinDist = Int32.Parse(XMinDist.Text);
                _graphController.GraphData.MinValeyHeight = Int32.Parse(MinValeyHeight.Text);
            }
            catch (Exception) { }

            if (_graphController.GraphData.ShowValues)
            {
                Values.Content = "Show";                
                _graphController.GraphData.ShowValues = false;
            }
            else
            {
                Values.Content = "Hide";                
                _graphController.GraphData.ShowValues = true;
            }           
        }

        private void ReferencePicture(object sender, RoutedEventArgs e)
        {
            if (_graphController.GraphData.ActualPicture.Count > 0)
                _graphController.GraphData.ReferencedPicture = _graphController.GraphData.ActualPicture;
            else
                MessageBox.Show("No data available for storing a reference picture.");
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
            if (initializingOnly) return;
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
                case 4:
                    _graphController.GraphData.Filter = Filter.MAX;
                    break;
                default:
                    _graphController.GraphData.Filter = Filter.RGB;
                    break;
            }
        }
    }
}
