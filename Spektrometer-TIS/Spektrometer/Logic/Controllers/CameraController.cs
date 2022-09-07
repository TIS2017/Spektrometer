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

        public StopCameraHandler CameraStop { get; set; }

        public volatile bool cameraStarted = false;

        private ImageController _imageController;
        private FilterInfoCollection _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        private VideoCaptureDevice _videoSource;
        private int _cameraIndex;
        private readonly int REQUARED_WIDTH = 1280;
        private static CameraController cameraControllerInstance;

        private CameraController()
        {
            _imageController = ImageController.GetInstance();
            _cameraIndex = -1;
            CameraStop += StopCamera;
        }

        public static CameraController GetInstance()
        {
            if (cameraControllerInstance == null)
            {
                cameraControllerInstance = new CameraController();
            }
            return cameraControllerInstance;
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

        public string GetSelectedCameraID()
        {
            if (_cameraIndex >= 0)
                return _videoDevices[_cameraIndex].MonikerString;
            return null;
        }

        public int GetIndexOfCameraByID(string monikerString)
        {
            var cameraIndex = -1;

            for (var i = 0; i < _videoDevices.Count; i++)
            {
                if (_videoDevices[i].MonikerString.Equals(monikerString))
                {
                    cameraIndex = i;
                    break;
                }
            }

            return cameraIndex;
        }

        public void SelectCamera(int index)
        {
            if (index >= 0 && index < _videoDevices.Count)
            {
                _cameraIndex = index;
                _videoSource = new VideoCaptureDevice(_videoDevices[index].MonikerString);
                CheckAndSetVideoResolution();
                _videoSource.NewFrame += new NewFrameEventHandler(Video_NewFrame);
            }
        }

        private void CheckAndSetVideoResolution()
        {
            foreach (VideoCapabilities vc in _videoSource.VideoCapabilities) 
            {
                if (vc.FrameSize.Width == REQUARED_WIDTH)
                {
                    _videoSource.VideoResolution = vc;
                    break;
                }
            }
            if (_videoSource.VideoResolution == null)
                MessageBox.Show("Camera does not support the required width: " + REQUARED_WIDTH);
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
                cameraStarted = true;
            }
        }

        private void StopCamera()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                cameraStarted = false;
                _videoSource.SignalToStop();
            }
        }

        bool frame_processed = true;

        private void Video_NewFrame(object sender, NewFrameEventArgs eventargs)
        {
            if (frame_processed)
            {
                frame_processed = false;
                Bitmap bitmap = eventargs.Frame;

                CreateBitmapSourceAndCallSendImageEvent(bitmap);
                frame_processed = true;
            }
        }

        public int GetCameraIndex()
        {
            return _cameraIndex;
        }

        bool previousFrameFinished = true;
        public void CreateBitmapSourceAndCallSendImageEvent(Bitmap bitmap)
        {
            if (!previousFrameFinished) return;
            previousFrameFinished = false;
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
                    Task.Yield();
                    previousFrameFinished = true;
                })
            );
        }

        public bool IsRunning()
        {
            try
            {
                if (_videoSource != null && _videoSource.IsRunning)
                    return true;
            }
            catch
            {
            }
            return false;
        }
    }
}
