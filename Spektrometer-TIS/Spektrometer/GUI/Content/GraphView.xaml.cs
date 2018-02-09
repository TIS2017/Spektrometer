using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using Spektrometer.Logic;


//    /// <summary>
//    /// Interaction logic for GraphView.xaml
//    /// </summary>
namespace Spektrometer.GUI
{
    public partial class GraphView
    {
        private GraphData _graphData;
        private CalibrationPoints _calibrationPoints;
        private GraphCalculator _graphCalculator;

        public const double margin = 30;

        double xmin;
        double xmax;
        double ymin;
        double ymax;
        
        double axisLineWidth = 10;

        double stepAxisX;
        double stepAxisY;
        
        private double _minValueRange;
        private double _maxValueRange;
        private bool _entered;
        private bool _dragging;
        private double _lastMouseXPosition;

        double step;

        List<Color> pixelData;
        Brush[] brushes;
        DisplayFormat? lastDiplayFormat;

        public GraphView()
        {
            InitializeComponent();

            var graphController = GraphController.GetInstance();
            graphController.RedrawChart += DrawGraph;

            _graphCalculator = new GraphCalculator();
            _graphData = graphController.GraphData;
            _calibrationPoints = graphController.CalibrationPoints;


        }
        
        public void DrawGraph()
        {
            canGraph.Children.Clear();


            xmin = margin;
            xmax = canGraph.Width - margin;
            ymin = margin;
            ymax = canGraph.Height - margin;
            if (_graphData.DisplayFormat == DisplayFormat.parabola)
            {
                if (lastDiplayFormat == null || !lastDiplayFormat.Equals(_graphData.DisplayFormat))
                {
                    _maxValueRange = _graphData.IntesityData.Count-1;
                    _minValueRange = 0;
                }
                lastDiplayFormat = _graphData.DisplayFormat;
                step = ((_maxValueRange - _minValueRange) / (canGraph.Width - margin));
                DrawAxisX();
                DrawAxisY();
                DrawCalibrationGraph();
            }
            else
            {
                if (_graphData.GraphDataInPixels.Count == 0)
                {
                    return;
                }
                this.pixelData = _graphData.GraphDataInPixels;

                if (lastDiplayFormat == null || !lastDiplayFormat.Equals(_graphData.DisplayFormat))
                {
                    _maxValueRange = pixelData.Count-1;
                    _minValueRange = 0;
                }
                lastDiplayFormat = _graphData.DisplayFormat;
                

                SetBrushes();


                step = ((_maxValueRange - _minValueRange) / (canGraph.Width - margin));
                DrawGraphData();
                DrawAxisX();
                DrawAxisY();

                if (_graphData.GlobalPeak)
                    ShowGlobalPeak();
                if (_graphData.ShowPeaks)
                    ShowPeaks();
            }
        }

        private void DrawCalibrationGraph()
        {
            var intensity = _graphData.IntesityData;
            var count = 0;
            PointCollection points = new PointCollection();
            var stepAxisY = (canGraph.Height - margin) / (intensity.Last() + 100);

            for (double x = _minValueRange; x <= _maxValueRange; x += step)
            {
                double yValue = ymax - intensity[(int)x] * (stepAxisY);
                points.Add(new Point(xmin + count, yValue));
                count++;
            }

            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 10;
            polyline.Stroke = Brushes.Black;
            polyline.Points = points;
            canGraph.Children.Add(polyline);

            foreach (var point in _calibrationPoints.CalibrationPointsList)
            {
                if (point.X < _minValueRange || point.X > _maxValueRange)
                    continue;
                var circle = new Ellipse();
                circle.Fill = Brushes.GreenYellow;
                circle.Height = circle.Width = 15;
                circle.Stroke = Brushes.Black;
                TranslateTransform translate = new TranslateTransform(xmin + (point.X - _minValueRange) * (1/step), ymax - point.Y * stepAxisY);
                circle.RenderTransform = translate;
                canGraph.Children.Add(circle);
            }
        }

        private void DrawAxisX()
        {
            var intensity = _graphData.IntesityData;
            var maxValueRange = 0.0;
            var minValueRange = 0.0;
            if (_graphData.DisplayFormat.Equals(DisplayFormat.pixel) || _graphData.DisplayFormat.Equals(DisplayFormat.parabola))
            {
                maxValueRange = _maxValueRange;
                minValueRange = _minValueRange;
            }
            if (_graphData.DisplayFormat.Equals(DisplayFormat.wavelength))
            {
                maxValueRange = intensity[(int)_maxValueRange];
                minValueRange = intensity[(int)_minValueRange];
            }
            stepAxisX = (maxValueRange - minValueRange) / (canGraph.Width - margin);
            if (stepAxisX == 0)
                return;
            // X AXIS
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(new Point(0, ymax), new Point(canGraph.Width, ymax))); //x-ova priamka

            double countAxisX = 0;
            var lengthBetweenTwoAxisXPoints = (maxValueRange - minValueRange) / 13;
            var counterForAxisX = minValueRange;
            for (double x = minValueRange; x <= maxValueRange; x += stepAxisX)
            {
                if (x > counterForAxisX)
                {
                    xaxis_geom.Children.Add(new LineGeometry(
                    new Point(countAxisX + margin, ymax - axisLineWidth / 2),
                    new Point(countAxisX + margin, ymax + axisLineWidth / 2)));

                    TextBlock txt = new TextBlock();
                    txt.Text = String.Format("{0:0}", counterForAxisX);
                    Canvas.SetTop(txt, ymax + 5);
                    Canvas.SetLeft(txt, countAxisX + 20);
                    canGraph.Children.Add(txt);
                    counterForAxisX += lengthBetweenTwoAxisXPoints;
                }
                countAxisX += 1;
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);
        }

        private void DrawAxisY()
        {  
            double countingAxisYPoints = 0;
            var finishY = _graphData.DisplayFormat.Equals(DisplayFormat.parabola) ? _graphData.IntesityData.Last() : 260;
            var lengthBetweenTwoAxisYPoints = finishY / 13;
            var counterForAxisY = ymax - (lengthBetweenTwoAxisYPoints * stepAxisY);
            stepAxisY = (canGraph.Height - (2 * margin)) / finishY;
            if (stepAxisY == 0)
                return;
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

        private void ShowGlobalPeak()
        {
            var globalPeakIndex = _graphCalculator.globalMax(pixelData);

            Line l = new Line();
            l.X1 = l.X2 = (globalPeakIndex - _minValueRange) * (1 / step) + margin;
            l.Y1 = ymax;
            l.Y2 = (ymax - 290 * stepAxisY) * 0.5;
            l.Stroke = Brushes.Black;
            l.StrokeThickness = 1;
            l.StrokeDashArray = new DoubleCollection(new[] { 5d });
            canGraph.Children.Add(l);

            TextBlock maxValueTxt = new TextBlock();
            maxValueTxt.Text = String.Format("{0:0}", globalPeakIndex);
            Canvas.SetTop(maxValueTxt, l.Y2 * 0.1);
            Canvas.SetLeft(maxValueTxt, l.X1 - 10);
            canGraph.Children.Add(maxValueTxt);
        }

        private void ShowPeaks()
        {
            bool showValues = _graphData.ShowValues;

            var treshold = _graphData.Treshold;
            var indexes = _graphCalculator.Peaks(pixelData, treshold);

            foreach (var maxIndex in indexes)
            {
                //MAXIMUM
                if (brushes.Contains(maxIndex.Key.Value) && (maxIndex.Key.Key > _minValueRange && maxIndex.Key.Key < _maxValueRange))
                {
                    Line l = new Line();
                    l.X1 = l.X2 = (maxIndex.Key.Key - _minValueRange) * (1/step) + margin;
                    l.Y1 = ymax;
                    l.Y2 = (ymax - 260 * stepAxisY) * 0.5;
                    l.Stroke = Brushes.Gray;
                    l.StrokeThickness = 0.5;
                    l.StrokeDashArray = new DoubleCollection(new[] { 5d });
                    canGraph.Children.Add(l);

                    if (showValues)
                    {
                        TextBlock maxValueTxt = new TextBlock();
                        maxValueTxt.Text = String.Format("{0:0}", maxIndex.Key.Key);
                        Canvas.SetTop(maxValueTxt, l.Y2 * 0.6);
                        Canvas.SetLeft(maxValueTxt, l.X1 - 10);
                        canGraph.Children.Add(maxValueTxt);
                    }
                }
            }
        }

        private void SetBrushes()
        {
            brushes = new Brush[]{ Brushes.Red, Brushes.Green, Brushes.Blue };
            if (_graphData.Filter.Equals(Filter.R))
            {
                brushes = new Brush[] { Brushes.Red };
            }
            else if (_graphData.Filter.Equals(Filter.G))
            {
                brushes = new Brush[] { Brushes.Green };
            }
            else if (_graphData.Filter.Equals(Filter.B))
            {
                brushes = new Brush[] { Brushes.Blue };
            }
        }

        private void DrawGraphData()
        {
            //==================== ADDING GRAPH DATA ======================
            

            foreach (var brush in brushes)
            {
                double count = 0;

                PointCollection points = new PointCollection();
                for (double x = _minValueRange; x <= _maxValueRange; x += step)
                {
                    double yValue = 0;
                    if (brush.Equals(Brushes.Red))
                    {
                        yValue = ymax - pixelData.ElementAt((int)x).R * stepAxisY;
                    }
                    else if (brush.Equals(Brushes.Green))
                    {
                        yValue = ymax - pixelData.ElementAt((int)x).G * stepAxisY;
                    }
                    else if (brush.Equals(Brushes.Blue))
                    {
                        yValue = ymax - pixelData.ElementAt((int)x).B * stepAxisY;
                    }
                    points.Add(new Point(xmin + count, yValue));
                    count++;

                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = brush;
                polyline.Points = points;
                canGraph.Children.Add(polyline);
            }
        }

        private void canGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawGraph();
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
            DrawGraph();
        }

        private void ZoomOutGraph(Point mouse)
        {
            var range = _maxValueRange - _minValueRange;
            var ratio = (mouse.X - margin) / (canGraph.Width - margin);
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
                _maxValueRange = pixelData.Count-1;
            }
            else
            {
                _maxValueRange += rightLenght;
            }
        }

        private void ZoomInGraph(Point mouse)
        {
            var range = _maxValueRange - _minValueRange;
            var ratio = (mouse.X - margin) / (canGraph.Width - margin);
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
                DrawGraph();
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