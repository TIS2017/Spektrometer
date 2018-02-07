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
        double initialX = 0;
        double initialY = 0;
        double finalX = 0;
        double finalY = 0;

        double minValueRange = 0;
        double maxValueRange = 0;

        double step;

        double graphWidth;

        double graphMargin;
        Boolean isDown;
        Boolean start = true;

        List<double> dataXcoords;
        List<Color> pixelData; 

        public GraphView()
        {
            InitializeComponent();

            GraphController.GetInstance().RedrawChart += DrawGraph;
        }

        public void DrawGraph(List<Color> pixelData)
        {
            if (pixelData == null)
            {
                return;
            }
            this.pixelData = pixelData;
            canGraph.Children.Clear();

            

            const double margin = 30;

            double xmin = margin;
            double xmax = canGraph.Width - margin;
            double ymin = margin;
            double ymax = canGraph.Height - margin;
            double axisLineWidth = 10;
            double lengthBetweenTwoAxisXPoints = 100;
            double lengthBetweenTwoAxisYPoints = 20;
            double countingAxisYPoints = 0;
            double stepAxisY = (canGraph.Height - (2 * margin)) / 300;

            double counterForAxisX = lengthBetweenTwoAxisXPoints;
            double counterForAxisY = ymax-(lengthBetweenTwoAxisYPoints*stepAxisY);


            if (start)
            {
                maxValueRange = pixelData.Count;
                graphMargin = margin;
                graphWidth = canGraph.Width;
                start = false;
            }


            Console.Write("TEST");

            //if (maxValueRange == pixelData.Count)
            //{
                step = ((maxValueRange - minValueRange) / (canGraph.Width - margin));
            //} 
           
            //MessageBox.Show("GRAPH WIDTH: " + graphWidth);

            //MAXIMUM
            Line l = new Line();
            l.X1 = margin + 100 + step;
            l.X2 = margin + 100 + step;
            l.Y1 = ymax;
            l.Y2 = 0;
            l.Stroke = Brushes.Black;
            l.StrokeThickness = 1;
            l.StrokeDashArray = new DoubleCollection(new[] { 10d });
            canGraph.Children.Add(l);



            //==================== ADDING GRAPH DATA ======================
            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };

            dataXcoords = new List<double>();
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
                            yValue = ymax - pixelData.ElementAt((int)x).R*stepAxisY;
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

                    //if (data_set == 0)
                    //{
                    //    dataXcoords.Add(xmin + count);
                    //}
                }

                //MessageBox.Show("" + count);
                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = brushes[data_set];
                polyline.Points = points;
                canGraph.Children.Add(polyline);
            }

            // X AXIS
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(new Point(0, ymax), new Point(canGraph.Width, ymax))); //x-ova priamka

            double countAxisX = 0;
            for (double x = minValueRange; x <= maxValueRange; x += step)
            {
                if (x > counterForAxisX)
                {
                    counterForAxisX += lengthBetweenTwoAxisXPoints;
                    xaxis_geom.Children.Add(new LineGeometry(
                    new Point(countAxisX + margin, ymax - axisLineWidth / 2),
                    new Point(countAxisX + margin, ymax + axisLineWidth / 2)));

                    TextBlock txt = new TextBlock();
                    txt.Text = String.Format("{0:0}", counterForAxisX-100);
                    Canvas.SetTop(txt, ymax + 5);
                    Canvas.SetLeft(txt, countAxisX + 20);
                    canGraph.Children.Add(txt);
                }
                countAxisX += 1;
            }
            //MessageBox.Show(""+ countingAxisXPoints);
            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);

            // Y AXIS
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(new Point(xmin, 0), new Point(xmin, canGraph.Height)));
            for (double y = ymax; y >= ymin; y -= stepAxisY)
            {
                if (y < counterForAxisY)
                {
                    counterForAxisY -= lengthBetweenTwoAxisYPoints*stepAxisY;
                    countingAxisYPoints++;

                    yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - axisLineWidth / 2, y),
                    new Point(xmin + axisLineWidth / 2, y)));

                    TextBlock txt = new TextBlock();
                    txt.Text = String.Format("{0:0}", lengthBetweenTwoAxisYPoints*countingAxisYPoints);
                    //txt.Text = txt.Text.Substring(0, txt.Text.Length - 1) + "0";
                    Canvas.SetTop(txt, y-10);
                    canGraph.Children.Add(txt);
                                        
                }
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);






            

            //MessageBox.Show("STEP_DATA: " + stepForData + " X: " + initialX + " Y: " + finalX + " STEP: " + step + " WIDTH: " + canGraph.Width);

        }

        private void canGraph_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDown = true;
            Point point = e.GetPosition(this);
            initialX = point.X;
            initialY = point.Y;
        }

        private void canGraph_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(this);
            finalX = point.X;
            finalY = point.Y;

            minValueRange = initialX - graphMargin;
            maxValueRange = finalX - graphMargin;

            step = 1;
            graphWidth = finalX - initialX;
        }

        private void canGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawGraph(pixelData);
        }
    }
}