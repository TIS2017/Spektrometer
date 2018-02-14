using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace Spektrometer.Logic
{
    public class Export
    {
        private GraphController _graphController;
        private ImageController _imageController;
        internal string savePath;
        internal string automaticSavePath;
        private double _time;
        private bool _saving = false;
        private static Export self;


        private Thread automaticImageSaving;

        private Export()
        {
            _graphController = GraphController.GetInstance();
            _imageController = ImageController.GetInstance();
            savePath = Directory.GetCurrentDirectory();
            automaticSavePath = Directory.GetCurrentDirectory();
        }

        public static Export GetInstance()
        {
            if (self == null)
            {
                self = new Export();
            }
            return self;
        }
        /**
        Export .txt súboru s kalibračnými bodmi na zadanú adresu.
         */
        public void calibrationFile(string path)
        {
            try {
                using (StreamWriter File = new StreamWriter(path))
                {
                    var graphData = new List<System.Windows.Point>();
                    graphData = _graphController.CalibrationPoints.CalibrationPointsList;

                    for (int i = 0; i < graphData.Count; i++)
                    {
                        File.WriteLine(graphData[i].X + "\t" + graphData[i].Y);
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Nepodarilo sa vytvoriť súbor so zadanou cestou !\n"+e.Message);
                return;
            }
        }

        /**
         * Export Maxím z grafu do .txt súboru.
         */
        public void MaximumValues(string path)
        {
            try
            {
                using (var File = new StreamWriter(path))
                {
                    var graphData = _graphController.GraphData.ActualPicture;
                    var treshold = _graphController.GraphData.Treshold;
                    var graphCalculator = new GraphCalculator();
                    var MaximumValues = graphCalculator.Peaks(graphData, treshold);

                    foreach (var maximum in MaximumValues)
                    {
                        string color = maximum.Key.Value.ToString().Equals(Brushes.Red) ? "Blue:" : maximum.Key.Value.Equals(Brushes.Green) ? "Green:" : "Red:";
                        File.WriteLine(string.Format("{0}\t{1}",color,maximum.Key.Key));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Nepodarilo sa vytvoriť súbor so zadanou cestou !" + e.Message);
                return;
            }
        }

        /**
         Export grafu ako .txt súbor s vlnovými dĺžkami.
         */
        public void chartData(string path)
        {
            try
            {
                using (var File = new StreamWriter(path))
                {
                    var graphPixelData = _graphController.GraphData.GraphDataInPixels;

                    foreach (var pixel in graphPixelData)
                    {
                        File.WriteLine(string.Format("{0} {1} {2}", pixel.R, pixel.G, pixel.B));
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Nepodarilo sa vytvoriť súbor so zadanou cestou !"+e.Message);
                return;
            }
        }

        internal void SaveConfig()
        {
            var doc = new XDocument();
            var spektrometer = new XElement("Spektrometer");
            doc.Add(spektrometer);

            spektrometer.Add(BasicConfiguration());
            spektrometer.Add(MeasurementConfiguration());
            spektrometer.Add(CameraConfiguration());

            doc.Save("Config.xml");
        }

        private XElement[] BasicConfiguration()
        {
            var imageController = ImageController.GetInstance();
            var result = new XElement[6];
            
            result[0] = new XElement("rowIndex", imageController.GetRowIndex());
            result[1] = new XElement("rowCount", imageController.GetRowCount());
            result[2] = new XElement("imageCount", imageController.GetImageCount());
            result[3] = new XElement("saveLocation", savePath);
            result[4] = new XElement("loadLocation", Import.GetInstance().loadPath);
            result[5] = new XElement("automaticSaveLocation", automaticSavePath);

            return result;
        }

        private XElement MeasurementConfiguration()
        {
            var graphData = GraphController.GetInstance().GraphData;
            var measurement = new XElement("Measurement");
            var xElement = new XElement("showPeaks", graphData.ShowPeaks);
            measurement.Add(xElement);

            xElement = new XElement("globalPeak", graphData.GlobalPeak);
            measurement.Add(xElement);

            var showValues = new XElement("showValues");
            xElement = new XElement("show", graphData.Treshold >= 0);
            showValues.Add(xElement);

            xElement = new XElement("offset", graphData.Treshold);
            showValues.Add(xElement);
            measurement.Add(showValues);

            xElement = new XElement("fillChart", graphData.Filter);
            measurement.Add(xElement);

            return measurement;
        }

        private XElement CameraConfiguration()
        {
            var calibData = GraphController.GetInstance().CalibrationPoints.CalibrationPointsList;
            var cameraController = CameraController.GetInstance();

            if (cameraController.GetCameraIndex() == -1)
                return null;
            var camera = new XElement("Camera");
            camera.SetAttributeValue("id", cameraController.GetSelectedCameraID());
            foreach (var point in calibData)
            {
                var xElement = new XElement("calibrationPoints");
                xElement.SetAttributeValue("x", point.X);
                xElement.SetAttributeValue("y", point.Y);
                camera.Add(xElement);
            }
            return camera;
        }

        /**
         Export aktuálneho obrázka z kamery ako .png súbor, na zadanú adresu.
         */
        public void cameraImage(string path)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var img = _imageController.LastImage().Clone();
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(img));
                        encoder.Save(fileStream);
                    }
                });
            }
            catch(Exception e)
            {
                MessageBox.Show("Nepodarilo sa vytvoriť súbor so zadanou cestou !"+e.Message);
                return;
            }
        }

        /**
         Export grafu ako .png súbor na zadanú adresu.
         */
        public void chartImage(string path, Canvas canvas)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            double dpi = 96d;

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvas);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = File.OpenWrite(path))
            {
                pngEncoder.Save(fs);
            }
        }

        public void StartSavingAutomatically(double time)
        {
            _time = time;
            _saving = true;
            automaticImageSaving = new Thread(new ThreadStart(SavingImages));
            automaticImageSaving.Start();
        }

        private void SavingImages()
        {
            var camera = CameraController.GetInstance();
            while (_saving)
            {
                if (camera.IsRunning())
                {
                    self.cameraImage($"{self.automaticSavePath}-{DateTime.Now.ToString("d.M.yyyy_HH-mm-ss.fff")}.png");
                    Thread.Sleep((int)(self._time * 1000));
                }
            }
        }

        public void StopSavingAutomatically()
        {
            _saving = false;
        }
    }
}
