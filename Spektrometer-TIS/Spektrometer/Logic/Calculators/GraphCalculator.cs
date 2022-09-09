using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Spektrometer.Logic
{
    public class GraphCalculator
    {
        private double a0, a1, a2, a3;
        public GraphCalculator()
        {
        }

        /**
         * Vráti index (x-ovej osi) globálneho maxima v grafe,
         * ktorý treba na grafe vyznačiť
         */
        public KeyValuePair<int, int> globalMax(List<Color> pic)
        {
            int index = 0;
            int max = 0;
            for (int i = 0; i < pic.Count; i++)
            {
                if ((pic[i].R + pic[i].G + pic[i].B) > max)
                {
                    index = i;
                    max = pic[i].R + pic[i].G + pic[i].B;
                }
            }
            int theHighestValue = Math.Max(Math.Max(pic[index].R, pic[index].G), pic[index].B);
            return new KeyValuePair<int, int>(index, theHighestValue);
        }

        /**
         * Vráti zoznam indexov (x-ová os), tých vrcholov,
         * ktorých hodnota (y-ová os) presiahla požadovanú hodnotu (threshold).
         */
        public Dictionary<KeyValuePair<Brush, int>, int> Peaks(List<Color> pic, double threshold, int epsilon, int minheight)
        {
            Dictionary<KeyValuePair<Brush, int>, int> indexes = new Dictionary<KeyValuePair<Brush, int>, int>();
            int[] lastIndexX = new int[4];  // r,g,b,max
            double[] valeyBottomSinceLastPeak = new double[4];
            int[] lastPeakHeight = new int[4];
            KeyValuePair<Brush, int>[] lastKey = new KeyValuePair<Brush, int>[4];
            bool[] plateau = new bool[4];
            for (int i = 0; i < 4; i++)
            {
                lastIndexX[i] = -epsilon - 1;
                lastPeakHeight[i] = -minheight;
                plateau[i] = false;
            }
            valeyBottomSinceLastPeak[0] = pic[0].R;
            valeyBottomSinceLastPeak[1] = pic[0].G;
            valeyBottomSinceLastPeak[2] = pic[0].B;
            int max = pic[0].R;
            if (pic[0].G > max) max = pic[0].G;
            if (pic[0].B > max) max = pic[0].B;
            valeyBottomSinceLastPeak[3] = max;
            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.Chocolate };

            int[] pix = new int[4];
            int[] nextPix = new int[4];
            int[] lastPix = new int[4];
            int[] passAround;

            for (int i = 0; i < pic.Count - 2; i++)
            {
                // i, pic[i].R, pic[i].G, pic[i].B, max
                max = pic[i + 1].R;
                if (pic[i + 1].G > max) max = pic[i + 1].G;
                if (pic[i + 1].B > max) max = pic[i + 1].B;

                passAround = lastPix;
                lastPix = pix;
                pix = nextPix;
                nextPix = passAround;

                nextPix[0] = pic[i + 1].R;
                nextPix[1] = pic[i + 1].G;
                nextPix[2] = pic[i + 1].B;
                nextPix[3] = max;

                if (i == 0) continue;

                for (int j = 0; j < 4; j++)
                {
                    bool isAPeak = (pix[j] > lastPix[j]) &&
                                   (pix[j] >= nextPix[j]) &&
                                   (pix[j] >= threshold);
                    bool newPeak = false;
                    bool wasValeySinceLastPeak = ((pix[j] - valeyBottomSinceLastPeak[j]) >= minheight) &&
                                                 ((lastPeakHeight[j] - valeyBottomSinceLastPeak[j]) >= minheight);
                    bool gapWideEnoughSinceLast = i - lastIndexX[j] >= epsilon;
                    int peakI = i;
                    if (isAPeak &&
                        gapWideEnoughSinceLast &&
                        wasValeySinceLastPeak)
                    {
                        newPeak = true;
                    }
                    else if (isAPeak &&
                             (pix[j] > lastPeakHeight[j]))
                    // && ((!wasValeySinceLastPeak) || !gapWideEnoughSinceLast))
                    {
                        try { indexes.Remove(lastKey[j]); } catch (Exception) { }
                        newPeak = true;
                    }
                    else if (plateau[j] &&
                        (pix[j] < lastPix[j]) &&
                        (i - lastIndexX[j] > 2))
                    {
                        indexes.Remove(lastKey[j]);
                        peakI = (i + lastIndexX[j] - 1) / 2;
                        newPeak = true;
                    }

                    if (newPeak)
                    {
                        lastKey[j] = new KeyValuePair<Brush, int>(brushes[j], peakI);
                        indexes.Add(lastKey[j], pix[j]);
                        lastPeakHeight[j] = pix[j];
                        lastIndexX[j] = peakI;
                        valeyBottomSinceLastPeak[j] = pix[j];
                        plateau[j] = i == peakI;
                    }
                    else
                    {
                        if (pix[j] < valeyBottomSinceLastPeak[j])
                            valeyBottomSinceLastPeak[j] = pix[j];
                        if (pix[j] != lastPix[j]) plateau[j] = false;
                    }
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
        public List<double> convertPixelsToNanometers(int amount)
        {
            List<double> nanometers = new List<double>();
            for (int i = 0; i < amount; i++)
            {
                nanometers.Add(a0 + a1 * i + a2 * i * i);
            }
            return nanometers;
        }

        public List<double> convertPixelsToNanometersCubic(int amount)
        {
            List<double> nanometers = new List<double>();
            for (int i = 0; i < amount; i++)
            {
                nanometers.Add(a0 + a1 * i + a2 * i * i + a3 * i * i * i);
            }
            return nanometers;
        }

        private void solveEquations(double[,] a, int n)
        {
            for (int i = 0; i < n; i++)
            {
                if (i > 0)
                    for (int j = 0; j < i; j++)
                        a[i, i] -= a[j, i] * a[j, i];

                if (a[i, i] < 0) return;
                a[i, i] = Math.Sqrt(a[i, i]);
                if (i < n - 1)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        if (i > 0)
                            for (int k = 0; k < i; k++)
                                a[i, j] -= a[k, i] * a[k, j];
                        a[i, j] = a[i, j] / a[i, i];
                    }
                }
            }
            double[] p = new double[n];
            for (int i = 0; i < n; i++)
            {
                p[i] = a[i, n];
                if (i > 0)
                    for (int r = 0; r < i; r++)
                        p[i] -= a[r, i] * p[r];
                p[i] /= a[i, i];
            }
            for (int i = n - 1; i >= 0; i--)
            {
                if (i < n - 1)
                    for (int r = i + 1; r < n; r++)
                        p[i] -= a[i, r] * p[r];                            
                p[i] /= a[i, i];
            }
            a0 = p[0];
            a1 = p[1];
            a2 = p[2];
            a3 = p[3];
        }

        public void calculateParametersCubic(List<Point> calibrationPoints)
        {
            double[,] a = new double[4, 5];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 5; j++)
                    a[i, j] = 0;
            a[0, 0] = calibrationPoints.Count;

            for (int i = 0; i < calibrationPoints.Count; i++)
            {
                double x = calibrationPoints[i].X;
                double y = calibrationPoints[i].Y;
                double xx = x * x;
                double xy = x * y;
                a[0, 1] += x;
                a[0, 2] += xx;
                a[0, 3] += xx * x;
                a[1, 3] += xx * xx;
                a[2, 3] += xx * xx * x;
                a[3, 3] += xx * xx * xx;
                a[0, 4] += y;
                a[1, 4] += xy;
                a[2, 4] += xx * y;
                a[3, 4] += xx * xy;
            }
            a[1, 1] = a[0, 2];
            a[1, 2] = a[0, 3];
            a[2, 2] = a[1, 3];

            solveEquations(a, 4);
        }
    }
}
 