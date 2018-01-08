using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Spektrometer.Logic
{
    public class CameraController
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
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
            if (index >= 0 && index < _videoDevices.Count)
            {
                _cameraIndex = index;
                _videoSource = new VideoCaptureDevice(_videoDevices[index].MonikerString);
                _videoSource.NewFrame += new NewFrameEventHandler(Video_NewFrame);
            }
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

            CreateBitmapSourceAndCallSendImageEvent(bitmap);
        }

        public int GetCameraIndex()
        {
            return _cameraIndex;
        }
        
        public void CreateBitmapSourceAndCallSendImageEvent(Bitmap bitmap)
        {
            var bmp = (Bitmap)bitmap.Clone();
            Application.Current.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    var hBitmap = bmp.GetHbitmap();

                    BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap
                    (
                        hBitmap,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions()
                    );

                    _imageController.NewImage(bitmapSource);

                    DeleteObject(hBitmap);
                })
            );
        }
    }
}
