using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

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
        public int globalMax(List<Color> pic)
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
        public Dictionary<KeyValuePair<int, Brush>, int> Peaks(List<Color> pic, double threshold)
        {
            var indexes = new Dictionary<KeyValuePair<int,Brush>, int>();
            var epsilon = 1;
            var maxIndexAndValueRed = new KeyValuePair<int, int>(-1, -1);
            var maxIndexAndValueGreen = new KeyValuePair<int, int>(-1, -1);
            var maxIndexAndValueBlue = new KeyValuePair<int, int>(-1, -1);
            var previousValueRed = -1;
            var previousValueGreen = -1;
            var previousValueBlue = -1;
            for (int i = 0; i < pic.Count; i++)
            {
                var indexAndValueRed = new KeyValuePair<int,int>(i,pic[i].R);
                var indexAndValueGreen = new KeyValuePair<int, int>(i, pic[i].G);
                var indexAndValueBlue = new KeyValuePair<int, int>(i, pic[i].B);

                if (i > 0)
                {
                    previousValueRed = pic[i-1].R;
                    previousValueGreen = pic[i-1].G;
                    previousValueBlue = pic[i-1].B;
                }
                if (indexAndValueRed.Value >= threshold && maxIndexAndValueRed.Value < indexAndValueRed.Value)
                {
                    maxIndexAndValueRed = new KeyValuePair<int,int> (indexAndValueRed.Key,indexAndValueRed.Value);
                }
                if (indexAndValueGreen.Value >= threshold && maxIndexAndValueGreen.Value < indexAndValueGreen.Value)
                {
                    maxIndexAndValueGreen = new KeyValuePair<int, int>(indexAndValueGreen.Key, indexAndValueGreen.Value);
                }
                if (indexAndValueBlue.Value >= threshold && maxIndexAndValueBlue.Value < indexAndValueBlue.Value)
                {
                    maxIndexAndValueBlue = new KeyValuePair<int, int>(indexAndValueBlue.Key, indexAndValueBlue.Value);
                }
                if (i - maxIndexAndValueRed.Key > epsilon && maxIndexAndValueRed.Value > 0 && previousValueRed > indexAndValueRed.Value)
                {
                    indexes.Add(new KeyValuePair<int, Brush>(maxIndexAndValueRed.Key,Brushes.Red), maxIndexAndValueRed.Value);
                    maxIndexAndValueRed = new KeyValuePair<int, int>(-1, -1);
                }
                if (i - maxIndexAndValueGreen.Key > epsilon && maxIndexAndValueGreen.Value > 0 && previousValueGreen > indexAndValueGreen.Value)
                {
                    indexes.Add(new KeyValuePair<int, Brush>(maxIndexAndValueGreen.Key, Brushes.Green), maxIndexAndValueGreen.Value);
                    maxIndexAndValueGreen = new KeyValuePair<int, int>(-1, -1);
                }
                if (i - maxIndexAndValueBlue.Key > epsilon && maxIndexAndValueBlue.Value > 0 && previousValueBlue > indexAndValueBlue.Value)
                {
                    indexes.Add(new KeyValuePair<int, Brush>(maxIndexAndValueBlue.Key, Brushes.Blue), maxIndexAndValueBlue.Value);
                    maxIndexAndValueBlue = new KeyValuePair<int, int>(-1, -1);
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
        public List<double> convertPixelsToNanometers(List<Color> picture)
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
 