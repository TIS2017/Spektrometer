using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Spektrometer.Logic
{
    public class ImageInfo
    {
        public BitmapSource lastImage { get; set; }
        public Stack<List<Color>> imageHistory { get; set; }
        public int historyCount { get; set; }
        public int rowIndex { get; set; }
        public int rowCount { get; set; }

        public ImageInfo()
        {
            lastImage = null;
            imageHistory = new Stack<List<Color>>();
            historyCount = 1;
            rowIndex = 1;
            rowCount = 1;
        }

        internal void addNewLine(List<Color> colorLine)
        {
            imageHistory.Push(colorLine);
        }

        public void setRowIndex(int index)
        {
            rowIndex = index;
        }
    }
}