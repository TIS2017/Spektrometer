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
        private GraphController GraphController;

        public ImageCalculator(GraphController gc)
        {
            GraphController = gc;
        }

        public List<Color> makeAverage(ref Stack<List<Color>> pics)
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


        public void GraphData_update(List<Color> avg)
        {
            GraphController.GraphData.actualPicture = avg;
        }
    }
}
