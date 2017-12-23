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
        public List<Color> CutImageAndMakeAverage(ImageInfo imageInfo)
        {
            //List<Color> result = new List<Color>();
            //List<int> A = new List<int>();
            //List<int> R = new List<int>();
            //List<int> G = new List<int>();
            //List<int> B = new List<int>();
            //int width = bitmap.Width;

            //for (int row = rowIndex - rowCount; row <= rowIndex + rowCount; row++)
            //{
            //    List<Color> pom = new List<Color>();

            //    for (int col = 0; col < width; col++)
            //    {
            //        pom.Add(bitmap.GetPixel(col, row));
            //        A[col] += pom[col].A;
            //        R[col] += pom[col].R;
            //        G[col] += pom[col].G;
            //        B[col] += pom[col].B;
            //    }
            //}
            //for (int i = 0; i < width; i++)
            //{
            //    result.Add(Color.FromArgb(A[i], R[i], G[i], B[i]));
            //}
            //return result;
            return new List<Color>();
        }
    }
}
