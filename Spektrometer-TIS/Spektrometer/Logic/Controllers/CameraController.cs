using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spektrometer.Logic
{
    public class CameraController
    {
        public delegate void StopCameraHandler();

        public StopCameraHandler CameraStop;

        private ImageController _imageController;
        private FilterInfoCollection _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        private VideoCaptureDevice _videoSource;
        private int _cameraIndex;

        public CameraController(ImageController ic)
        {
            _imageController = ic;
            _cameraIndex = -1;
            CameraStop += StopCamera;
        }

        /* Vrati list pripojenych zariadeni */
        public List<string> GetCameraList()
        {
            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            List<string> cameraList = new List<string>();
            foreach (FilterInfo device in _videoDevices)
            {
                cameraList.Add(device.Name);
            }
            return cameraList;
        }


        public void SelectCamera(int index)
        {
            _cameraIndex = index;
            _videoSource = new VideoCaptureDevice(_videoDevices[index].MonikerString);
            _videoSource.NewFrame += new NewFrameEventHandler(Video_NewFrame);
        }

        public void ShowSettings()
        {
            if (_videoSource != null)
            {
                _videoSource.DisplayPropertyPage(IntPtr.Zero);
            }
        }

        public void CameraStart()
        {
            CameraStop();
            if (_videoSource != null)
            {
                _videoSource.Start();
            }
        }

        private void StopCamera()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
            }
        }

        private void Video_NewFrame(object sender, NewFrameEventArgs eventargs)
        {
            Bitmap bitmap = eventargs.Frame;

            _imageController.NewImage(bitmap);
        }

        public int GetCameraIndex()
        {
            return _cameraIndex;
        }
    }
}
