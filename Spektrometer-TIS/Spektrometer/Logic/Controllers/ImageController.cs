using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Spektrometer.GUI;

namespace Spektrometer.Logic
{
    public class ImageController
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public delegate void SendImageHandler(BitmapSource bitmap);
        public delegate void NewRowIndexHandler(int rowIndex);
        public delegate void NewRowCountHandler(int rowCount);

        public SendImageHandler SendImageEvent { get; set; }
        public NewRowIndexHandler NewRowIndex { get; set; }
        public NewRowCountHandler NewRowCount { get; set; }

        private ImageCalculator _imageCalculator;
        private ImageInfo _imageInfo;
        private GraphController _graphController;
        private Dispatcher _dispatcher;

        public ImageController()
        {
            _imageCalculator = new ImageCalculator();
            _imageInfo = new ImageInfo();
            _graphController = new GraphController();
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        internal void NewImage(Bitmap bitmap)
        {
            var bmp = (Bitmap)bitmap.Clone();
            _dispatcher.BeginInvoke(
            new ThreadStart(() =>
            {
                var line = _imageCalculator.CutImageAndMakeAverage(bitmap, _imageInfo.rowIndex, _imageInfo.rowCount);
                if (Monitor.TryEnter(_imageInfo))
                {
                    _imageInfo.addNewLine(line);
                    if (_imageInfo.historyCount == _imageInfo.imageHistory.Count())
                    {
                        var lineOfImages = _imageCalculator.Average(_imageInfo.imageHistory);
                        _graphController.GraphData.ActualPicture = lineOfImages;
                        CreateBitmapSourceAndCallSendImageEvent(bmp);
                        SetLastImage(bmp);
                        _imageInfo.imageHistory = new Stack<List<Color>>();
                    }
                    Monitor.Exit(_imageInfo);
                }
            }));
        }

        public void SetRowIndex(int index)
        {
            try
            {
                Monitor.Enter(_imageInfo);
                _imageInfo.rowIndex = index;
                _imageInfo.imageHistory = new Stack<List<Color>>();
                NewRowIndex(index);
            }
            finally
            {
                Monitor.Exit(_imageInfo);
            }
        }

        public int GetRowIndex()
        {
            return _imageInfo.rowIndex;
        }

        public void SetRowCount(int count)
        {
            try
            {
                Monitor.Enter(_imageInfo);
                _imageInfo.rowCount = count;
                _imageInfo.imageHistory = new Stack<List<Color>>();
                NewRowCount(count);
            }
            finally
            {
                Monitor.Exit(_imageInfo);
            }
        }

        public int GetRowCount()
        {
            return _imageInfo.rowCount;
        }

        public void SetImageCount(int count)
        {
            try
            {
                Monitor.Enter(_imageInfo);
                _imageInfo.historyCount = count;
                _imageInfo.imageHistory = new Stack<List<Color>>();
            }
            finally
            {
                Monitor.Exit(_imageInfo);
            }
        }
        
        public Bitmap LastImage()
        {
           return _imageInfo.lastImage;
        }

        public void CreateBitmapSourceAndCallSendImageEvent(Bitmap bitmap)
        {
            var hBitmap = bitmap.GetHbitmap();
            
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap
            (
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );

            SendImageEvent(bitmapSource);

            DeleteObject(hBitmap);
        }

        public void SetLastImage(Bitmap bitmap)
        {
            _imageInfo.lastImage = bitmap;
        }
    }
}