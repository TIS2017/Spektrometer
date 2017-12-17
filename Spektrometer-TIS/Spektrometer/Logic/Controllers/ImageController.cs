using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spektrometer.GUI;

namespace Spektrometer.Logic
{
    public class ImageController
    {
        public delegate void SendImage(Bitmap bitmap);

        public SendImage SendImageEvent;
        private CameraRecordView _cameraRecordViewer;
        private ImageInfo _imageInfo;
        private GraphController _graphController;

        public ImageController(CameraRecordView crv)
        {
            _cameraRecordViewer = crv;
            _imageInfo = new ImageInfo();
            _cameraRecordViewer.NewLineIndex += SetRowIndex;
            _graphController = new GraphController();
        }

        public void SetAsReferencedPicture()
        {
            Task.Factory.StartNew(() =>
            {
                Monitor.Enter(_imageInfo);
                var lineOfImages = ImageCalculator.Average(_imageInfo.imageHistory);
                _graphController.GraphData.ReferencedPicture = lineOfImages;
                Monitor.Exit(_imageInfo);
            });
        }

        internal void NewImage(Bitmap bitmap)
        {
            Task.Factory.StartNew(() =>
                {
                    var line = ImageCalculator.CutImage(bitmap, _imageInfo.rowIndex, _imageInfo.rowCount);
                    if (Monitor.TryEnter(_imageInfo))
                    {
                        _imageInfo.addNewLine(line);
                        if (_imageInfo.historyCount == _imageInfo.imageHistory.Count())
                        {
                            var lineOfImages = ImageCalculator.Average(_imageInfo.imageHistory);
                            _graphController.GraphData.ActualPicture = lineOfImages;
                        }
                        // Tu pridu funkcie na zobrazovanie obrazkov
                        _imageInfo.imageHistory = new Stack<List<Color>>();
                        Monitor.Exit(_imageInfo);
                    }
                });
        }

        public void SetRowIndex(int index)
        {
            try
            {
                Monitor.Enter(_imageInfo);
                _imageInfo.rowIndex = index;
                _imageInfo.imageHistory = new Stack<List<Color>>();
            }
            finally
            {
                Monitor.Exit(_imageInfo);
            }
        }

        public void SetRowCount(int count)
        {
            try
            {
                Monitor.Enter(_imageInfo);
                _imageInfo.rowCount = count;
                _imageInfo.imageHistory = new Stack<List<Color>>();
            }
            finally
            {
                Monitor.Exit(_imageInfo);
            }
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
    }
}