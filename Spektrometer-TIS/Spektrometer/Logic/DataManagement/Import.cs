using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Spektrometer.Logic
{
    public class Import
    {
        private GraphController _graphController;
        private ImageController _imageController;
        internal string loadPath = Directory.GetCurrentDirectory();
        private static Import self;

        private Import()
        {
            _graphController = GraphController.GetInstance();
            _imageController = ImageController.GetInstance();
        }

        public static Import GetInstance()
        {
            if (self == null)
            {
                self = new Import();
            }
            return self;
        }

        /**
         Importuje dvojstĺpcový .txt súbor s kalibračnými bodmi.
         Stĺpce sú oddelené medzerami.
         Prvý stĺpec je x-ová súradnica
         Druhý stĺpec vlnová dĺžka.
            */
        public void ImportCalibrationFile(string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("Súbor sa nenašiel !");
                return;
            }
            string[] riadok;
            double x = -999;
            double y = -999;

            StreamReader calibFile = new StreamReader(path);
            while (!calibFile.EndOfStream)
            {
                riadok = calibFile.ReadLine().Split(' ');
                    try
                        {
                            x = Convert.ToDouble(riadok[0]);
                            y = Convert.ToDouble(riadok[1]);
                        } 
                    catch
                    {
                    }

                if(x!= -999 && y!= -999){
                    var point = new System.Windows.Point(x,y);

                    _graphController.CalibrationPoints.AddPoint(point);
                }
            }
           calibFile.Close();
        }

        internal void LoadConfig()
        {
            if (File.Exists("Config.xml"))
            {
                var doc = XDocument.Load("Config.xml");
                var spektrometer = doc.Element("Spektrometer");
                var measurement = spektrometer.Element("Measurement");
                LoadBasicConfiguration(measurement.ElementsBeforeSelf());
                LoadMeasurementConfiguration(measurement);
                LoadCamera(spektrometer.Element("Camera"));
            }
        }

        private void LoadCamera(XElement xElement)
        {
            if (xElement == null)
                return;

            var cameraController = CameraController.GetInstance();
            var calibPoint = GraphController.GetInstance().CalibrationPoints;

            var cameraID = xElement.Attribute("id").Value;
            var cameraIndex = cameraController.GetIndexOfCameraByID(cameraID);
            if (cameraIndex >= 0)
            {
                cameraController.SelectCamera(cameraIndex);
                var calibrationPoints = xElement.Elements();

                foreach (var point in calibrationPoints)
                {
                    var calibrationPoint = new Point
                    {
                        X = Double.Parse(point.Attribute("x").Value),
                        Y = Double.Parse(point.Attribute("y").Value)
                    };
                    calibPoint.AddPoint(calibrationPoint);
                }
            }
        }

        private void LoadMeasurementConfiguration(XElement measurement)
        {
            var graphData = GraphController.GetInstance().GraphData;

            var showPeaks = measurement.Element("showPeaks").Value;
            graphData.ShowPeaks = showPeaks.Equals("false") ? false : true;

            var globalPeak = measurement.Element("globalPeak").Value;
            graphData.GlobalPeak = globalPeak.Equals("false") ? false : true;

            var showValues = measurement.Element("showValues");
            var show = showValues.Element("show").Value;
            graphData.ShowValues = show.Equals("false") ? false : true;

            var offset = showValues.Element("offset").Value;
            graphData.Treshold = Int32.Parse(offset);

            var fillChart = measurement.Element("fillChart").Value;
            graphData.Filter = (Filter)Enum.Parse(typeof(Filter), fillChart);
        }

        private void LoadBasicConfiguration(IEnumerable<XElement> xElement)
        {
            var imageController = ImageController.GetInstance();
            var export = Export.GetInstance();
            foreach (var element in xElement)
            {
                if (element.Name.LocalName.Equals("rowIndex"))
                {
                    var value = Int32.Parse(element.Value);
                    imageController.SetRowIndex(value);
                }
                else if (element.Name.LocalName.Equals("rowCount"))
                {
                    var value = Int32.Parse(element.Value);
                    imageController.SetRowCount(value);
                }
                else if (element.Name.LocalName.Equals("imageCount"))
                {
                    var value = Int32.Parse(element.Value);
                    imageController.SetImageCount(value);
                }
                else if (element.Name.LocalName.Equals("saveLocation"))
                {
                    export.savePath = element.Value;
                }
                else if (element.Name.LocalName.Equals("loadLocation"))
                {
                    loadPath = element.Value;
                }
                else if (element.Name.LocalName.Equals("automaticSaveLocation"))
                {
                    export.automaticSavePath = element.Value;
                }
            }
        }

        /** 
         Importuje dáta z grafu ako .txt súbor,
         kde prvý stĺpec sú hodnoty červenej zložky,
         druhý stĺpec oddelený medzerou sú hodnoty zelenej zložky,
         tretí modrej, ako List sa potom posiela GraphControlleru.
            */
        public void ImportChartData(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show("Súbor sa nenašiel !");
                    return;
                }

                List<Color> graphData = new List<Color>();
                using (var chart = new StreamReader(path))
                {
                    while (!chart.EndOfStream)
                    {
                        var line = chart.ReadLine().Split(' ');
                        var red = Convert.ToInt32(line[0]);
                        var green = Convert.ToInt32(line[1]);
                        var blue = Convert.ToInt32(line[2]);
                        graphData.Add(Color.FromRgb(Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue)));
                    }
                }
                
                _graphController.GraphData.ActualPicture = graphData;
            }
            catch(Exception)
            {
                MessageBox.Show("Nepodarilo sa načítať súbor!");
            }
        }

        /**
         Importuje obrázok z kamery, ako .png a potom uloží ImageController,
         ako new Image...
         */
        public void ImportCameraImage(string path)
        {
            try
            {
                _imageController.SetImageCount(1);
                Stream imageStreamSource = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                PngBitmapDecoder decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                BitmapSource bitmapSource = decoder.Frames[0];
                _imageController.NewImage(bitmapSource);
            } catch(Exception e)
            {
                MessageBox.Show("Chyba pri načítavaní. " + e.Message);
            }
        }
    }
}
