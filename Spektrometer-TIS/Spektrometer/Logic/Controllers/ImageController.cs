using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Spektrometer.GUI;

namespace Spektrometer.Logic
{
    public class ImageController
    {
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
        private static ImageController imageControllerInstance = null; 

        private ImageController()
        {
            _imageCalculator = new ImageCalculator();
            _imageInfo = new ImageInfo();
            _graphController = GraphController.GetInstance();
            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public static ImageController GetInstance()
        {
            if (imageControllerInstance == null)
            {
                imageControllerInstance = new ImageController();
            }
            return imageControllerInstance;
        }

        internal void NewImage(BitmapSource bitmap)
        {
            var line =  _imageCalculator.CutImageAndMakeAverage(ref bitmap, _imageInfo.rowIndex, _imageInfo.rowCount);
            if (Monitor.TryEnter(_imageInfo))
            {
                _imageInfo.addNewLine(line);
                if (_imageInfo.historyCount == _imageInfo.imageHistory.Count())
                {
                    var lineOfImages = _imageCalculator.Average(_imageInfo.imageHistory);
                    _graphController.GraphData.ActualPicture = lineOfImages;
                    SendImageEvent(bitmap);
                    SetLastImage(bitmap);
                    _imageInfo.imageHistory = new Stack<List<Color>>();
                }
                Monitor.Exit(_imageInfo);
            }
        }

        public void SetRowIndex(int index)
        {
            try
            {
                Monitor.Enter(_imageInfo);
                _imageInfo.rowIndex = index;
                if (_imageInfo.lastImage != null)
                {
                    CheckAndRepairRowIndex();
                    NewImage(_imageInfo.lastImage);
                }
                _imageInfo.imageHistory = new Stack<List<Color>>();
                NewRowIndex(_imageInfo.rowIndex);
            }
            finally
            {
                Monitor.Exit(_imageInfo);
            }
        }

        internal void CheckAndRepairRowIndex()
        {
            var index = _imageInfo.rowIndex;
            var count = _imageInfo.rowCount;
            var imageHeight = _imageInfo.lastImage.PixelHeight;
            if (index + count >= imageHeight)
                index = imageHeight - count - 1;
            if (index - count < 0)
                index = count + 1;
            _imageInfo.rowIndex = index;
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
                if (_imageInfo.lastImage != null)
                {
                    CheckAndRepairRowIndex();
                    NewRowIndex(_imageInfo.rowIndex);
                    NewImage(_imageInfo.lastImage);
                }
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
        
        public BitmapSource LastImage()
        {
           return _imageInfo.lastImage;
        }

        public void SetLastImage(BitmapSource bitmap)
        {
            _imageInfo.lastImage = bitmap;
        }
    }
}