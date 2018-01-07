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
    public class Import
    {
        private GraphController GraphController;
        private ImageController _imageController;

        public Import(GraphController gc, ImageController ic)
        {
            GraphController = gc;
            _imageController = ic;
        }

        /**
         Importuje dvojstĺpcový .txt súbor s kalibračnými bodmi.
         Stĺpce sú oddelené medzerami.
         Prvý stĺpec je x-ová súradnica
         Druhý stĺpec vlnová dĺžka.
            */
        public void importCalibrationFile(string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("Súbor sa nenašiel !");
                return;
            }
            String[] riadok;
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
                    Point point = new Point();
                    point.x = x;
                    point.y = y;

                    GraphController.CalibrationPoints.addPoint(point);
                }
            }
           calibFile.Close();
        }

        /** 
         Importuje dáta z grafu ako .txt súbor,
         kde prvý stĺpec sú hodnoty červenej zložky,
         druhý stĺpec oddelený medzerou sú hodnoty zelenej zložky,
         tretí modrej, ako List sa potom posiela GraphControlleru.
            */
        public void importChartData(string path)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("Súbor sa nenašiel !");
                return;
            }

            String[] riadok;
            int R = 0;
            int G = 0;
            int B = 0;
            List<Color> graphData = new List<Color>();

            StreamReader chart = new StreamReader(path);
            while (!chart.EndOfStream)
            {
                riadok = chart.ReadLine().Split(' ');
                try
                {
                    R = Convert.ToInt32(riadok[0]);
                    G = Convert.ToInt32(riadok[1]);
                    B = Convert.ToInt32(riadok[2]);
                    graphData.Add(Color.FromArgb(R, G, B));
                }
                catch
                {
                }               
            }
            chart.Close();
            
           /* for(int i=0; i<R.Count; i++)
            {
                graphData.Add(Color.FromArgb(R[i], G[i], B[i]));
            } */       
            GraphController.GraphData.pixelData = graphData;
        }

        /**
         Importuje obrázok spektrometra zo súboru ako bitmapu a potom uloží v ImageControlleri,
         ako new Image...
         */
        public void importCameraImage(string path)
        {
            Bitmap image = new Bitmap(path);
            _imageController.NewImage(image);
        }
    }
}
