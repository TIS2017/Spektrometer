using System;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        internal readonly FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        private VideoCaptureDevice _videoSource;
        private int _selectedWebcam = 0;

        public Form1()
        {
            InitializeComponent();
            foreach(FilterInfo device in VideoDevices)
            {
                deviceComboBox.Items.Add(device.Name);
            }
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventargs)
        {
            Bitmap bitmap = (Bitmap)(eventargs.Frame).Clone();

            pictureBox1.Image = bitmap;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedWebcam = deviceComboBox.SelectedIndex;
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
            }
            _videoSource = new VideoCaptureDevice(VideoDevices[_selectedWebcam].MonikerString);
            _videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_selectedWebcam > -1)
            {
                _videoSource.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_selectedWebcam > -1)
            {
                _videoSource.SignalToStop();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_selectedWebcam > -1)
            {
                _videoSource.DisplayPropertyPage(IntPtr.Zero);
            }
        }
    }
}
