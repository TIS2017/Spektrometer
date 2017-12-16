using System;
using System.Collections.Generic;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spektrometer.Logic
{
    public class ImageCalculator
    {
        public static List<Color> Average(ref Stack<List<Color>> pics)
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

            for(int i=0; i<R.Count; i++)
            {
                avg.Add(Color.FromArgb(A[i], R[i], G[i], B[i]));
            }

            return avg;
        }

        public static List<Color> CutImage(Bitmap bitmap, int rowIndex, int rowCount)
        {
            throw new NotImplementedException();
            //var result = new Stack<List<Color>>();
            //var row = _imageInfo.rowIndex;
            //var rowCount = _imageInfo.rowCount;
            //for (var rowIndex = row; rowIndex < row + rowCount; rowIndex++)
            //{
            //    var colorList = new List<Color>();
            //    for (var column = 0; column < 1280; column++)
            //    {
            //        colorList.Add(bitmap.GetPixel(column, rowIndex));
            //    }
            //    result.Push(colorList);
            //}
            //return result;
        }
    }
}
