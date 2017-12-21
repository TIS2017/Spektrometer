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
        private ImageController _imageController;
        private FilterInfoCollection _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        private VideoCaptureDevice _videoSource;

        public CameraController(ImageController ic)
        {
            _imageController = ic;
        }

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

        public void CameraStop()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
            }
        }

        private void Video_NewFrame(object sender, NewFrameEventArgs eventargs)
        {
            Bitmap bitmap = (Bitmap)(eventargs.Frame).Clone();

            _imageController.NewImage(bitmap);
        }
    }
}
