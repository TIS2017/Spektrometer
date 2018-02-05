using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Spektrometer.Logic
{
    public class Import
    {
        private GraphController GraphController;
        private ImageController _imageController;

        public Import()
        {
            GraphController = GraphController.GetInstance();
            _imageController = ImageController.GetInstance();
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

                    GraphController.CalibrationPoints.AddPoint(point);
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

            string[] riadok;
            List<int> R = new List<int>();
            List<int> G = new List<int>();
            List<int> B = new List<int>();
            List<Color> graphData = new List<Color>();

            StreamReader chart = new StreamReader(path);
            while (!chart.EndOfStream)
            {
                riadok = chart.ReadLine().Split(' ');
                try
                {
                    R.Add(Convert.ToInt32(riadok[0]));
                    G.Add(Convert.ToInt32(riadok[1]));
                    B.Add(Convert.ToInt32(riadok[2]));
                }
                catch
                {
                }               
            }
            chart.Close();
            
            for(int i=0; i<R.Count; i++)
            {
                graphData.Add(Color.FromRgb(Convert.ToByte(R[i]), Convert.ToByte(G[i]), Convert.ToByte(B[i])));
            }        
            GraphController.GraphData.PixelData = graphData;
        }

        /**
         Importuje obrázok z kamery, ako .png a potom uloží ImageController,
         ako new Image...
         */
        public void importCameraImage(string path)
        {
            try
            {
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
