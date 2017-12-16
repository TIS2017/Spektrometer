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
                    var lines = ImageCalculator.CutImage(bitmap, _imageInfo.rowIndex, _imageInfo.rowCount);
                        
                        if (_imageInfo.imageHistory.Count() == _imageInfo.historyCount)
                        {
                            var history = _imageInfo.imageHistory;
                            var line = ImageCalculator.Average(ref history);

                        }
                });
            throw new NotImplementedException();
        }

        public void SetRowIndex(int index)
        {

        }

        private void AddImage(Bitmap bitmap)
        {
            var lines = CutImage(ref bitmap);
            _imageInfo.addNewLine(ImageCalculator.Average(ref lines));
            _imageInfo.lastImage = bitmap;
        }
    }
}