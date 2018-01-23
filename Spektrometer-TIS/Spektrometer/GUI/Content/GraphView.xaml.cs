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
using System.ComponentModel;
using System.Globalization;

using LiveCharts.Events;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts.Geared;

using Spektrometer.Logic;
using Spektrometer.GUI;


//    /// <summary>
//    /// Interaction logic for GraphView.xaml
//    /// </summary>
namespace Spektrometer.GUI
{
    public partial class GraphView
    {
        public GraphView()
        {
            InitializeComponent();

            graphController.forceUpdate += updateForce;

            mainAxisX.Sections.Add(
             new AxisSection
             {
                 Value = 100,
                 DataLabel = true,
                 StrokeThickness = 1,
                 Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0)),
                 StrokeDashArray = new DoubleCollection(new[] { 10d }),
                 DisableAnimations = true,
                 DataLabelForeground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255))
             }
            );
        }

        public void updateForce()
        {
            cartesianChart.Update(false, true); //force chart redraw
        }

        //private void ResetZoomOnClick(object sender, RoutedEventArgs e)
        //{
        //    //Use the axis MinValue/MaxValue properties to specify the values to display.
        //    //use double.Nan to clear it.

        //    X.MinValue = double.NaN;
        //    X.MaxValue = double.NaN;
        //    Y.MinValue = double.NaN;
        //    Y.MaxValue = double.NaN;
        //}

        private void Axis_OnPreviewRangeChanged(PreviewRangeChangedEventArgs e)
        {
            var vm = (GraphController)DataContext;

            vm.RangeValue = e.Range;


            //if less than -0.5, cancel
            if (e.PreviewMinValue < -0.5)
            {
                mainAxisX.MinValue = double.NaN;
                mainAxisX.MaxValue = double.NaN;
                e.Cancel = true;
            }

            //if greater than the number of items on our array plus a 0.5 offset, stay on max limit
            if (e.PreviewMaxValue > 1280 - 0.5) e.Cancel = true;

            //finally if the axis range is less than 1, cancel the event (maximalny zoom)
            if (e.PreviewMaxValue - e.PreviewMinValue < 10) e.Cancel = true;
        }
    }
}