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
        private CameraRecordView _cameraRecordViewer;
        private CameraView _cameraView;
        private ImageInfo _imageInfo;
        private GraphController _graphController;

        public ImageController(CameraRecordView crv)
        {
            _cameraRecordViewer = crv;
            _cameraView = null;
            _imageInfo = new ImageInfo();
            _cameraRecordViewer.NewLineIndex += SetRowIndex;
        }
        
        public List<Color> GetPictureArray()
        {
            throw new NotImplementedException();
        }

        public void SetAsReferencedPicture()
        {
            throw new NotImplementedException();
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
                            _graphController.GraphData.actualPicture = lineOfImages;
                        }
                        // Tu pridu funkcie na zobrazovanie obrazkov
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