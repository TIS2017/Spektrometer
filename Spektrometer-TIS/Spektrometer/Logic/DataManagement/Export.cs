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
        Podmienka minimálne 3 kalibračné body.
         */
        public void calibrationFile(string path)
        {
            try {
                StreamWriter File = new StreamWriter(path);

                List<Point> calibrationPoint = new List<Point>();
                calibrationPoint = GraphController.CalibrationPoints.calibrationData;

                for (int i = 0; i < calibrationPoint.Count; i++)
                {
                    File.WriteLine(calibrationPoint[i].x + " " + calibrationPoint[i].y);
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
         Export grafu ako .txt súbor s RGB zložkami.
         */
        public void chartData(string path)
        {
            try
            {
                StreamWriter File = new StreamWriter(path);
                List<Color> Pixel = new List<Color>();
                Pixel = GraphController.GraphData.pixelData;

                for (int i = 0; i < Pixel.Count; i++)
                {
                    File.WriteLine(Pixel[i].R+" "+Pixel[i].G+" "+Pixel[i].B);
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
                // get aktual image from imageController   
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
