using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Spektrometer.Logic
{
    public class GraphCalculator
    {

        public GraphCalculator()
        {
        }

        /**
         * Vráti index (x-ovej osi) globálneho maxima v grafe,
         * ktorý treba na grafe vyznačiť
         * **/
        public int globalMax(List<Color> pic)
        {
            int index = 0;
            int max = -9;
            for(int i=0; i<pic.Count; i++)
            {
                if (pic[i].R > max)
                {
                    index = i;
                    max = pic[i].R;
                }
                else if (pic[i].G > max)
                {
                    index = i;
                    max = pic[i].G;
                }
                else if (pic[i].B > max)
                {
                    index = i;
                    max = pic[i].B;
                }
            }
            return index;
        }

        /**
         * Vráti zoznam indexov (x-ová os), tých vrcholov,
         * ktorých hodnota (y-ová os) presiahla požadovanú hodnotu (threshold).
         * */
        public List<int> peaks(List<Color> pic, double threshold)
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
         * Prepočítava hodnoty na grafe pomocou kalibračných bodov a aktuálneho obrázka.
         */
        public List<double> calculate(List<Color> picture, List<Point> calibrationPoints)
        {
            throw new System.NotImplementedException();

        }

        // int a0, a1, a2, a3;

        /*
        
        Private Sub Kalib_Click()
            If kal = True Then
                kal = False
                For i = d To h
                    X(i) = i
                Next i
            Else
                If k0 > 0 Then
                kal = True
                a0 = k0
                a1 = k1
                a2 = k2
                For i = d To h
                    X(i) = fcy(X(i))
                    z(i) = i
                Next i
                End If
            End If
            xD = X(d): xH = X(h)
            Mapa
        End Sub

     
        Public Function fcy(xG) As Single

            fcy = a0 + a1* xG + a2* xG * xG

        End Function

        Sub StvUni()
            Dim R As Single
            Dim X1, X2, X3, X4, Y1, Y2, Y3, Y4, nb
            Dim xx, yy, xy, d0, dA, dB, dC
            Dim s0, ep, Sa, sb, Sc, rQ, sQ
            '            M N S   p r e   f u n k c i u  :       y =  A + B.x  + C.x.x
            '
            X1 = 0: X2 = 0: X3 = 0: X4 = 0: Y1 = 0: Y2 = 0: Y3 = 0: Y4 = 0
            nb = 1 + Int((h - d) / sdh)
            For i = d To h Step sdh
              xx = X(i) * X(i);
              yy = Y(i) * Y(i);
              xy = X(i) * Y(i);
              
              X1 = X1 + X(i);
              X2 = X2 + xx;
              X3 = X3 + xx * X(i);
              X4 = X4 + xx * xx

              Y1 = Y1 + Y(i);
              Y2 = Y2 + xy;
              Y3 = Y3 + xx * Y(i);
              Y4 = Y4 + yy;

            Next i
            d0 = nb * (X2 * X4 - X3 * X3) - X1 * (X1 * X4 - X2 * X3) + X2 * (X1 * X3 - X2 * X2)
            dA = Y1 * (X2 * X4 - X3 * X3) - Y2 * (X1 * X4 - X2 * X3) + Y3 * (X1 * X3 - X2 * X2)
            dB = nb * (Y2 * X4 - X3 * Y3) - X1 * (Y1 * X4 - X2 * Y3) + X2 * (Y1 * X3 - X2 * Y2)
            dC = nb * (X2 * Y3 - Y2 * X3) - X1 * (X1 * Y3 - Y1 * X3) + X2 * (X1 * Y2 - Y1 * X2)
           
            a0 = 0: a1 = 0: a2 = 0
            a0 = dA / d0;
            a1 = dB / d0;
            a2 = dC / d0;
        */

        /* funkcia len na výpočet parametrov na prevod, ktoré sa vypočítajú pri vložení kalibračných bodov zo súboru.
           body a0,a1,a2 sa potom používajú kým sa nezmenia kalibračné body. */
        public void StvUni(List<Point> calibrationPoints) {

            Single X1, X2, X3, X4, Y1, Y2, Y3, Y4, nb;
            Single xx, yy, xy, d0, dA, dB, dC;
            int h, d;
            List<int> X = new List<int>();
            List<int> Y = new List<int>();


            //            M N S   p r e   f u n k c i u  :       y =  A + B.x  + C.x.x

            X1 = 0; X2 = 0; X3 = 0; X4 = 0;
            Y1 = 0; Y2 = 0; Y3 = 0; Y4 = 0;


            // nepotrebné môže byť aj od 0 - 1280p;
            d = (int)(calibrationPoints[0].x);
            h = (int)(calibrationPoints[calibrationPoints.Count].x);

            nb = 1 + (h - d);
            for (int i = d; i < h; i++){
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


            Single a0 = 0; Single a1 = 0; Single a2 = 0;
            a0 = dA / d0;
            a1 = dB / d0;
            a2 = dC / d0;

            //return a0,a1,a2;
        }

        // vráti už nm konkrétneho obrázka všetkých pixelov
        // List<Color> picture
        public Single fcy(Single xG, Single a0, Single a1, Single a2)
        {
            return a0 + a1 * xG + a2 * xG * xG;
        }
    }
}