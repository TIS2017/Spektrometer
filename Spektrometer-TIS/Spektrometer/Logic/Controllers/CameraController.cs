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

        public List<string> getCameraList()
        {
            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            List<string> cameraList = new List<string>();
            foreach (FilterInfo device in _videoDevices)
            {
                cameraList.Add(device.Name);
            }
            return cameraList;
        }

        public void selectCamera(int index)
        {
            _videoSource = new VideoCaptureDevice(_videoDevices[index].MonikerString);
            _videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
        }

        public void showSettings()
        {
            if (_videoSource != null)
            {
                _videoSource.DisplayPropertyPage(IntPtr.Zero);
            }
        }

        public void cameraStart()
        {
            cameraStop();
            if (_videoSource != null)
            {
                _videoSource.Start();
            }
        }

        public void cameraStop()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
            }
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventargs)
        {
            Bitmap bitmap = (Bitmap)(eventargs.Frame).Clone();

            _imageController.NewImage(bitmap);
        }
    }
}
