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
         * Vráti zoznam indexov (x-ová os),
         * ktorých hodnota presiahla požadovanú hodnotu (threshold)
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
        public List<double> calculate(List<Point> calibrationPoints)
        {
            throw new System.NotImplementedException();
        }
    }
}