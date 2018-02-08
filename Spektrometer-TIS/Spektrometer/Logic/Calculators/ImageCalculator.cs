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
            List<Color> temp = new List<Color>();

            var A = new List<int>(new int[1280]);
            var R = new List<int>(new int[1280]);
            var G = new List<int>(new int[1280]);
            var B = new List<int>(new int[1280]);
            for (int i = 0; i < pics.Count; i++)
            {
                for (int j = 0; j < pics.ElementAt(i).Count; j++)
                {
                    temp = pics.ElementAt(i);
                    A[j] += temp[j].A;
                    R[j] += temp[j].R;
                    G[j] += temp[j].G;
                    B[j] += temp[j].B;
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
                    blue[column] += pixelBuffer[index];
                    if (green.Count <= column)
                        green.Add(0);
                    green[column] += pixelBuffer[index + 1];
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
                if ((actualPicture[i].A - referencePicture[i].A) < 0)
                {
                    A = 0;
                }
                else
                {
                    A = actualPicture[i].A - referencePicture[i].A;
                }
                if ((actualPicture[i].R - referencePicture[i].R) < 0)
                {
                    R = 0;
                }
                else
                {
                    R = actualPicture[i].R - referencePicture[i].R;
                }
                if ((actualPicture[i].G - referencePicture[i].G) < 0)
                {
                    G = 0;
                }
                else
                {
                    G = actualPicture[i].G - referencePicture[i].G;
                }
                if ((actualPicture[i].B - referencePicture[i].B) < 0)
                {
                    B = 0;
                }
                else
                {
                    B = actualPicture[i].B - referencePicture[i].B;
                }
                result.Add(Color.FromArgb(Convert.ToByte(A), Convert.ToByte(R), Convert.ToByte(G), Convert.ToByte(B)));
            }

            return result;
        }

        /**
         * Funkcia vráti podiel aktuálneho a referenčného obrázka.
         */
        public List<Color> DivisionOfActualAndReferencePicture(List<Color> actualPicture, List<Color> referencePicture)
        {
            int redColor = 0;
            int greenColor = 0;
            int blueColor = 0;
            List<Color> result = new List<Color>();

            for (int i = 0; i < actualPicture.Count; i++)
            {
                if (referencePicture[i].R > 0)
                {
                    if ((actualPicture[i].R / referencePicture[i].R) * 255 >= 255)
                    {
                        redColor = 255;
                    }
                    else
                    {
                        redColor = (actualPicture[i].R / referencePicture[i].R) * 255;
                    }
                }
                else
                {
                    redColor = actualPicture[i].R;
                }
                if (referencePicture[i].G > 0)
                {
                    if ((actualPicture[i].G / referencePicture[i].G) * 255 >= 255)
                    {
                        greenColor = 255;
                    }
                    else
                    {
                        greenColor = (actualPicture[i].G / referencePicture[i].G) * 255;
                    }
                }
                else
                {
                    greenColor = actualPicture[i].G;
                }
                if (referencePicture[i].B > 0)
                {
                    if ((actualPicture[i].B / referencePicture[i].B) * 255 >= 255)
                    {
                        blueColor = 255;
                    }
                    else
                    {
                        blueColor = (actualPicture[i].B / referencePicture[i].B) * 255;
                    }
                }
                else
                {
                    blueColor = actualPicture[i].B;
                }
                result.Add(Color.FromArgb(Convert.ToByte(255), Convert.ToByte(redColor), Convert.ToByte(greenColor), Convert.ToByte(blueColor)));
            }
            return result;
        }
    }
}
