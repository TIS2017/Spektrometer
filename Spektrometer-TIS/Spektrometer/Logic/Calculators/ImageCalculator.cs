using System;
using System.Collections.Generic;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Spektrometer.Logic
{
    public class ImageCalculator
    {
        public List<Color> Average(Stack<List<Color>> pics)
        {
            List<Color> avg = new List<Color>();
            List<Color> pom = new List<Color>();

            List<int> A = new List<int>();
            List<int> R = new List<int>();
            List<int> G = new List<int>();
            List<int> B = new List<int>();


            for (int i = 0; i < pics.Count; i++)
            {
                for (int j = 0; j < pics.ElementAt(i).Count; j++)
                {
                    pom = pics.ElementAt(i);
                    A[j] = A[j] + pom[j].A;
                    R[j] = R[j] + pom[j].R;
                    G[j] = G[j] + pom[j].G;
                    B[j] = B[j] + pom[j].B;
                }
            }

            for (int i = 0; i < R.Count; i++)
            {
                avg.Add(Color.FromArgb(A[i], R[i], G[i], B[i]));
            }
            return avg;
        }
        public List<Color> CutImageAndMakeAverage(Bitmap bitmap, int rowIndex, int rowCount)
        {
            List<Color> result = new List<Color>();
            try
            {
                for (int col = 0; col < bitmap.Width; col++)
                {
                    Color temp = new Color();
                    int alphaColor = 0;
                    int redColor = 0;
                    int greenColor = 0;
                    int blueColor = 0;

                    for (int row = (rowIndex - rowCount); row <= (rowIndex + rowCount); row++)
                    {
                        temp = bitmap.GetPixel(col, row);
                        alphaColor += temp.A;
                        redColor += temp.R;
                        greenColor += temp.G;
                        blueColor += temp.B;
                    }
                    result.Add(Color.FromArgb(alphaColor / (rowCount * 2), redColor / (rowCount * 2), greenColor / (rowCount * 2), blueColor / (rowCount * 2)));
                }
                return result;
            }
            catch (IndexOutOfRangeException)
            {
            }
        }
    }
}
