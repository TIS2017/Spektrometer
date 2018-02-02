using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Spektrometer.Logic
{
    public class ImageCalculator
    {
        public List<Color> Average(Stack<List<Color>> pics)
        {
            List<Color> avg = new List<Color>();
            List<Color> pom = new List<Color>();

            var A = new List<int>();
            var R = new List<int>();
            var G = new List<int>();
            var B = new List<int>();

            for(int i=0; i<1280; i++)
            {
                A.Add(0);
                R.Add(0);
                G.Add(0);
                B.Add(0);
            }

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
                avg.Add(Color.FromArgb(Convert.ToByte(A[i]/pics.Count), Convert.ToByte(R[i]/pics.Count), Convert.ToByte(G[i]/pics.Count), Convert.ToByte(B[i]/pics.Count)));
            }
            return avg;
        }
        public List<Color> CutImageAndMakeAverage(ref BitmapSource bitmap, int rowIndex, int rowCount)
        {
            var format = bitmap.Format;
            if (format != PixelFormats.Bgr24 &&
                format != PixelFormats.Bgr32 &&
                format != PixelFormats.Bgra32 &&
                format != PixelFormats.Pbgra32)
            {
                throw new InvalidOperationException("BitmapSource must have Bgr24, Bgr32, Bgra32 or Pbgra32 format!");
            }

            var width = bitmap.PixelWidth;
            var sourceRect = new Int32Rect(0, rowIndex - rowCount, width, 2*rowCount+1);
            var numPixels = (2 * rowCount + 1) * width;
            var bytesPerPixel = format.BitsPerPixel / 8;
            var pixelBuffer = new byte[numPixels * bytesPerPixel];

            bitmap.CopyPixels(sourceRect, pixelBuffer, width * bytesPerPixel, 0);

            var red = new List<long>();
            var green = new List<long>();
            var blue = new List<long>();
            var result = new List<Color>();
            for (int row = 0; row < 2 * rowCount + 1; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    var index = column * bytesPerPixel + row * width * bytesPerPixel;
                    if (blue.Count <= column)
                        blue.Add(0);
                    blue[column] += pixelBuffer[index + 1];
                    if (green.Count <= column)
                        green.Add(0);
                    green[column] += pixelBuffer[index];
                    if (red.Count <= column)
                        red.Add(0);
                    red[column] += pixelBuffer[index + 2];
                    if (row == 2 * rowCount)
                    {
                        var blueByte = Convert.ToByte(blue[column] / (row + 1));
                        var greenByte = Convert.ToByte(green[column] / (row + 1));
                        var redByte = Convert.ToByte(red[column] / (row + 1));
                        result.Add(Color.FromRgb(redByte,greenByte,blueByte));
                    }
                }
            }
            return result;
        }

        public List<Color> DifferenceBetweenActualAndReferencePicture(List<Color> actualPicture, List<Color> referencePicture)
        {
            int A = 0;
            int R = 0;
            int G = 0;
            int B = 0;
            List<Color> result = new List<Color>();

            for(int i =0; i<actualPicture.Count; i++)
            {
                A = actualPicture[i].A - referencePicture[i].A;
                R = actualPicture[i].R - referencePicture[i].R;
                G = actualPicture[i].G - referencePicture[i].G;
                B = actualPicture[i].B - referencePicture[i].B;
                result.Add(Color.FromArgb(Convert.ToByte(A), Convert.ToByte(R), Convert.ToByte(G), Convert.ToByte(B)));
            }

            return result;
        }
    }
}
