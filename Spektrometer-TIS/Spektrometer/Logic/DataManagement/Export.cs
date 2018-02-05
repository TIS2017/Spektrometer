using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        private GraphController GraphController;
        private ImageController _imageController;
        internal static string savePath = Directory.GetCurrentDirectory();

        public Export()
        {
            GraphController = GraphController.GetInstance();
            _imageController = ImageController.GetInstance();
        }

        /**
        Export .txt súboru s kalibračnými bodmi na zadanú adresu.
         */
        public void calibrationFile(string path)
        {
            try {
                StreamWriter File = new StreamWriter(path);

                var graphData = new List<System.Windows.Point>();
                graphData = GraphController.CalibrationPoints.CalibrationPointsList;

                for (int i = 0; i < graphData.Count; i++)
                {
                    File.WriteLine(graphData[i].X + " " + graphData[i].Y);
                }
                File.Close();
            }
            catch
            {
                MessageBox.Show("Nepodarilo sa vytvoriť súbor so zadanou cestou !");
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
                StreamWriter File = new StreamWriter(path);

                List<double> intesityData = new List<double>();
                intesityData = GraphController.GraphData.IntesityData;

                for (int i = 0; i < intesityData.Count; i++)
                {
                    File.WriteLine(intesityData);
                }
                File.Close();
            }
            catch
            {
                MessageBox.Show("Nepodarilo sa vytvoriť súbor so zadanou cestou !");
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
            var result = new XElement[5];
            
            result[0] = new XElement("rowIndex", imageController.GetRowIndex());
            result[1] = new XElement("rowCount", imageController.GetRowCount());
            result[2] = new XElement("imageCount", imageController.GetImageCount());
            result[3] = new XElement("saveLocation", savePath);
            result[4] = new XElement("loadLocation", Import.loadPath);

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
                var img = _imageController.LastImage();
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(img));
                    encoder.Save(fileStream);
                }
            }
            catch
            {
                MessageBox.Show("Nepodarilo sa vytvoriť súbor so zadanou cestou !");
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
    }
}
