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

using Spektrometer.Logic;
using Spektrometer.GUI;


//    /// <summary>
//    /// Interaction logic for GraphView.xaml
//    /// </summary>
namespace Spektrometer.GUI
{
    public partial class GraphView
    {
        GraphController _graphController;
        GraphData _graphData;

        public const double margin = 30;

        double xmin;
        double xmax;
        double ymin;
        double ymax;

        double lengthBetweenTwoAxisYPoints = 20;
        double lengthBetweenTwoAxisXPoints = 100;
        double axisLineWidth = 10;

        double stepAxisY;

        double counterForAxisX;
        double counterForAxisY;

        double initialX = 0;
        double initialY = 0;
        double finalX = 0;
        double finalY = 0;

        private double _minValueRange;
        private double _maxValueRange;
        private bool _entered;
        private bool _dragging;
        private double _lastMouseXPosition;

        double step;
        double graphWidth;
        double graphMargin;
        Boolean start = true;

        List<Color> pixelData; 

        public GraphView()
        {
            InitializeComponent();
            _graphController = GraphController.GetInstance();
            _graphData = _graphController.GraphData;
            _graphController.RedrawChart += DrawGraph;

        }
        
        public void DrawGraph(List<Color> pixelData)
        {
            if (pixelData == null)
            {
                return;
            }
            this.pixelData = pixelData;

            canGraph.Children.Clear();

            xmin = margin;
            xmax = canGraph.Width - margin;
            ymin = margin;
            ymax = canGraph.Height - margin;


            if (start)
            {
                _maxValueRange = pixelData.Count;
                _minValueRange = 0;
                graphMargin = margin;
                graphWidth = canGraph.Width;
                start = false;
            }

            stepAxisY = (canGraph.Height - (2 * margin)) / 300;
            step = ((maxValueRange - minValueRange) / (canGraph.Width - margin));

            counterForAxisX = lengthBetweenTwoAxisXPoints;
            counterForAxisY = ymax - (lengthBetweenTwoAxisYPoints * stepAxisY);

            DrawGraphData(pixelData, new List<double>());
            DrawAxisX();
            DrawAxisY();
            ShowPeaks(_graphData.ShowPeaks);

        }

        public void DrawAxisX()
        {
            // X AXIS
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(new Point(0, ymax), new Point(canGraph.Width, ymax))); //x-ova priamka

            double countAxisX = 0;
            for (double x = _minValueRange; x <= _maxValueRange; x += step)
            {
                if (x > counterForAxisX)
                {
                    counterForAxisX += lengthBetweenTwoAxisXPoints;
                    xaxis_geom.Children.Add(new LineGeometry(
                    new Point(countAxisX + margin, ymax - axisLineWidth / 2),
                    new Point(countAxisX + margin, ymax + axisLineWidth / 2)));

                    TextBlock txt = new TextBlock();
                    txt.Text = String.Format("{0:0}", counterForAxisX - 100);
                    Canvas.SetTop(txt, ymax + 5);
                    Canvas.SetLeft(txt, countAxisX + 20);
                    canGraph.Children.Add(txt);
                }
                countAxisX += 1;
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);
        }

        public void DrawAxisY()
        {  
            double countingAxisYPoints = 0;

            // Y AXIS
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(new Point(xmin, 0), new Point(xmin, canGraph.Height)));
            for (double y = ymax; y >= ymin; y -= stepAxisY)
            {
                if (y < counterForAxisY)
                {
                    counterForAxisY -= lengthBetweenTwoAxisYPoints * stepAxisY;
                    countingAxisYPoints++;

                    yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - axisLineWidth / 2, y),
                    new Point(xmin + axisLineWidth / 2, y)));

                    TextBlock txt = new TextBlock();
                    txt.Text = String.Format("{0:0}", lengthBetweenTwoAxisYPoints * countingAxisYPoints);
                    Canvas.SetTop(txt, y - 10);
                    canGraph.Children.Add(txt);

                }
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);
        }

        public void ShowPeaks(bool show)
        {
            if (show)
            {
                List<int> indexes = _graphController.GetPeakIndexes(_graphData.ActualPicture, _graphData.Treshold); //TREBA VYRIESIT KEDY ACTUAL PICTURE A KEDY REFERENCED PICTURE

                for (int x = 0; x < indexes.Count; x++)
                {
                    //MAXIMUM
                    Line l = new Line();
                    l.X1 = indexes[x] + margin;
                    l.X2 = indexes[x] + margin;
                    l.Y1 = ymax;
                    l.Y2 = 0;
                    l.Stroke = Brushes.Black;                  
                    l.StrokeThickness = 1;
                    l.StrokeDashArray = new DoubleCollection(new[] { 10d });
                    canGraph.Children.Add(l);
                }
            }


        }

        public void ShowValues()
        {
            throw new NotImplementedException();
        }

        public void DrawGraphData(List<Color> pixelData, List<double> calibrationData)
        {
            //==================== ADDING GRAPH DATA ======================
            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };

            for (int data_set = 0; data_set < 3; data_set++)
            {
                double count = 0;

                PointCollection points = new PointCollection();
                for (double x = minValueRange; x <= maxValueRange; x += step)
                {
                    double yValue = 0;
                    switch (data_set)
                    {
                        case 0:
                            yValue = ymax - pixelData.ElementAt((int)x).R * stepAxisY;
                            break;
                        case 1:
                            yValue = ymax - pixelData.ElementAt((int)x).G * stepAxisY;
                            break;
                        case 2:
                            yValue = ymax - pixelData.ElementAt((int)x).B * stepAxisY;
                            break;
                        default:
                            throw new Exception("");

                    }
                    points.Add(new Point(xmin + count, yValue));
                    count++;

                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = brushes[data_set];
                polyline.Points = points;
                canGraph.Children.Add(polyline);
            }
        }

        private void canGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawGraph(pixelData);
        }

        private void canGraph_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (pixelData == null)
                return;
            var point = e.GetPosition(canGraph);
            if (e.Delta > 0)
                ZoomInGraph(point);
            else
                ZoomOutGraph(point);
            DrawGraph(pixelData);
        }

        private void ZoomOutGraph(Point mouse)
        {
            var range = _maxValueRange - _minValueRange;
            var ratio = (mouse.X - graphMargin) / (canGraph.Width - graphMargin);
            var leftLenght = range * ratio;
            var rightLenght = range - leftLenght;
            if (_minValueRange - leftLenght < 0)
            {
                _minValueRange = 0;
            }
            else
            {
                _minValueRange -= leftLenght;
            }

            if (_maxValueRange + rightLenght > pixelData.Count)
            {
                _maxValueRange = pixelData.Count;
            }
            else
            {
                _maxValueRange += rightLenght;
            }
        }

        private void ZoomInGraph(Point mouse)
        {
            var range = _maxValueRange - _minValueRange;
            var ratio = (mouse.X - graphMargin) / (canGraph.Width - graphMargin);
            var leftLenght = range * ratio;
            var rightLenght = range - leftLenght;
            leftLenght *= 0.5;
            rightLenght *= 0.5;
            _minValueRange += leftLenght;
            _maxValueRange -= rightLenght;
            if (_maxValueRange <= _minValueRange)
            {
                _minValueRange -= 10;
                _maxValueRange += 10;
            }
        }

        private void canGraph_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _dragging = false;
        }

        private void canGraph_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_entered)
                _dragging = true;
        }

        private void canGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                var deltaX = _lastMouseXPosition - e.GetPosition(canGraph).X;
                if (_minValueRange + deltaX < 0 || _maxValueRange + deltaX > pixelData.Count)
                    return;
                _minValueRange += deltaX;
                _maxValueRange += deltaX;
                DrawGraph(pixelData);
            }
            _lastMouseXPosition = e.GetPosition(canGraph).X;
        }

        private void canGraph_MouseEnter(object sender, MouseEventArgs e)
        {
            _entered = true;
        }

        private void canGraph_MouseLeave(object sender, MouseEventArgs e)
        {
            _entered = false;
            _dragging = false;
        }
    }
}