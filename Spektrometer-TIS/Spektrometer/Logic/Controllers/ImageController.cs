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

        public delegate void SendImage(BitmapSource bitmap);

        public SendImage SendImageEvent;
        private CameraRecordView _cameraRecordViewer;
        private ImageCalculator _imageCalculator;
        private ImageInfo _imageInfo;
        private GraphController _graphController;
        private Dispatcher _dispatcher;

        public ImageController(CameraRecordView crv)
        {
            _imageCalculator = new ImageCalculator();
            _cameraRecordViewer = crv;
            _imageInfo = new ImageInfo();
            _cameraRecordViewer.NewLineIndex += SetRowIndex;
            _cameraRecordViewer.SetRowIndex(_imageInfo.rowIndex);
            _cameraRecordViewer.SetRowCount(_imageInfo.rowCount);
            SendImageEvent += _cameraRecordViewer.SetNewImage;
            _graphController = new GraphController();
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void SetAsReferencedPicture()
        {
            Task.Factory.StartNew(() =>
            {
                Monitor.Enter(_imageInfo);
                var lineOfImage = _imageCalculator.CutImageAndMakeAverage(_imageInfo);
                _graphController.GraphData.ReferencedPicture = lineOfImage;
                Monitor.Exit(_imageInfo);
            });
        }

        internal void NewImage(Bitmap bitmap)
        {
            var bmp = (Bitmap)bitmap.Clone();
            _dispatcher.BeginInvoke(
            new ThreadStart(() =>
            {
                var line = _imageCalculator.CutImageAndMakeAverage(_imageInfo);
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
                _cameraRecordViewer.SetRowIndex(index);
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
                _cameraRecordViewer.SetRowCount(count);
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