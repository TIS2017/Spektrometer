using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Spektrometer.Logic
{
    public class Export
    {
        private GraphController GraphController;
        private ImageController _imageController;

        public Export(GraphController gc, ImageController ic)
        {
            GraphController = gc;
            _imageController = ic;
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
        public void chartImage(string path)
        {
            // treba získať obrázok (Bitmapu) z GraphView...
            throw new System.NotImplementedException();
          /*  RenderTargetBitmap rtb = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(uielement);
            image.Source = rtb;*/
        }
    }
}
