using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Spektrometer.GUI;

namespace Spektrometer.Logic
{
    public class ImageController
    {
        private CameraRecordView _cameraRecordViewer;
        private CameraView _cameraView;
        public ImageInfo imageInfo { get; }
        private ImageCalculator _imageCalculator;

        public ImageController(CameraRecordView crv, ImageCalculator iCa)
        {
            _cameraRecordViewer = crv;
            _cameraView = null;
            imageInfo = new ImageInfo();
            _imageCalculator = iCa;
            _cameraRecordViewer.NewLineIndex += imageInfo.setRowIndex;
        }
        
        public List<Bitmap> getPictureArray()
        {
            throw new NotImplementedException();
        }

        public void setAsReferencedPicture()
        {
            throw new NotImplementedException();
        }

        internal void newImage(Bitmap bitmap)
        {
            throw new NotImplementedException();
        }
    }
}