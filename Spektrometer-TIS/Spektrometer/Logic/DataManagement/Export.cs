using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                graphData = GraphController.CalibrationPoints.calibrationData;

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
                // get image from imageController   
                Bitmap img = _imageController.LastImage();
                img.Save(path);
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
