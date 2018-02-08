using System;
using System.Collections.Generic;
using System.Media;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Spektrometer.Logic
{
    public class GraphCalculator
    {
        private double a0, a1, a2;
        public GraphCalculator()
        {
        }

        /**
         * Vráti index (x-ovej osi) globálneho maxima v grafe,
         * ktorý treba na grafe vyznačiť
         */
        public int globalMax(List<System.Windows.Media.Color> pic)
        {
            int index = 0;
            int max = 0;
            for(int i=0; i<pic.Count; i++)
            {
                if ((pic[i].R + pic[i].G + pic[i].B) > max)
                {
                    index = i;
                    max = pic[i].R + pic[i].G + pic[i].B;
                }
            }
            return index;
        }

        /**
         * Vráti zoznam indexov (x-ová os), tých vrcholov,
         * ktorých hodnota (y-ová os) presiahla požadovanú hodnotu (threshold).
         */
        public List<int> peaks(List<System.Windows.Media.Color> pic, double threshold)
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < pic.Count; i++)
            {
                if (pic[i].R >= threshold || pic[i].G >= threshold || pic[i].B >= threshold)
                {
                    indexes.Add(i);
                }
            }
            return indexes;
        } 

        /**
         * Funkcia slúži na výpočet parametrov a0,a1,a2, pomocou ktorých funkcia convertToNanometers
         * konvertuje pixely do nanometrov, parametre a0,a1,a2 sa prepočítavajú nanovo, keď sa zmenia
         * kalibračné body.
         */
        public void calculateParameters(List<Point> calibrationPoints)
        {
            double X1, X2, X3, X4, Y1, Y2, Y3, Y4, nb;
            double xx, yy, xy, d0, dA, dB, dC;
            List<double> X = new List<double>();
            List<double> Y = new List<double>();

            X1 = 0; X2 = 0; X3 = 0; X4 = 0;
            Y1 = 0; Y2 = 0; Y3 = 0; Y4 = 0;

            for (int i = 0; i < calibrationPoints.Count; i++)
            {
                X.Add(calibrationPoints[i].X);
                Y.Add(calibrationPoints[i].Y);
            }

            nb = calibrationPoints.Count;
            for (int i = 0; i < calibrationPoints.Count; i++)
            {
                xx = X[i] * X[i];
                yy = Y[i] * Y[i];
                xy = X[i] * Y[i];

                X1 = X1 + X[i];
                X2 = X2 + xx;
                X3 = X3 + xx * X[i];
                X4 = X4 + xx * xx;

                Y1 = Y1 + Y[i];
                Y2 = Y2 + xy;
                Y3 = Y3 + xx * Y[i];
                Y4 = Y4 + yy;
            }

            d0 = nb * (X2 * X4 - X3 * X3) - X1 * (X1 * X4 - X2 * X3) + X2 * (X1 * X3 - X2 * X2);
            dA = Y1 * (X2 * X4 - X3 * X3) - Y2 * (X1 * X4 - X2 * X3) + Y3 * (X1 * X3 - X2 * X2);
            dB = nb * (Y2 * X4 - X3 * Y3) - X1 * (Y1 * X4 - X2 * Y3) + X2 * (Y1 * X3 - X2 * Y2);
            dC = nb * (X2 * Y3 - Y2 * X3) - X1 * (X1 * Y3 - Y1 * X3) + X2 * (X1 * Y2 - Y1 * X2);

            a0 = dA / d0;
            a1 = dB / d0;
            a2 = dC / d0;

        }

        /**
         * Funkcia vráti už pole nanometrov konkrétneho obrázka konverziou jeho pixelov.
         */
        public List<double> convertPixelsToNanometers(List<System.Windows.Media.Color> picture)
        {
            List<double> nanometers = new List<double>();
            for (int i = 0; i < picture.Count; i++)
            {
                nanometers.Add(a0 + a1 * i + a2 * i * i);
            }
            return nanometers;
        }
    }
}
 