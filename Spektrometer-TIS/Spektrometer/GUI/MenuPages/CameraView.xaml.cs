using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Diagnostics;

using Spektrometer.Logic;

namespace Spektrometer.GUI
{
    /// <summary>
    /// Interaction logic for CameraView.xaml
    /// </summary>
    public partial class CameraView : MenuComponent
    {
        private CameraController _cameraController;
        private ImageController _imageController;
        public List<string> list = new List<string>();

        public CameraController CameraController
        {
            get
            {
                return _cameraController;
            }
            set
            {
                _cameraController = value;
            }
        }
        public ImageController ImageController
        {
            get
            {
                return _imageController;
            }
            set
            {
                _imageController = value;
            }
        }
        public CameraView(MainWindow mainWindow) : base(mainWindow)
        {
            InitializeComponent();
            SetCameraLIst();
            DefaulteSettings();
        }

        public void DefaulteSettings()
        {
            this.rowIndex.Text = ImageController.GetRowIndex().ToString();
            this.rowCount.Text = ImageController.GetRowCount().ToString();
            BtnImageAreaSet.Content = "Set";

            // Nefunguje uplne spravne, opravit - zobrazenie *
            int ix = CameraController.GetCameraIndex();
            if (ix != -1)
            {
                //Debug.WriteLine("Test");
                CameraListBox.SelectedItem = list[ix];
            }
            BtnCameraChoice.Content = "Set";
        }

        public void SetCameraLIst()
        {
            list = CameraController.GetCameraList();
            if (list != null)
            {
                CameraListBox.ItemsSource = list;
            }
        }

        public void SetRowIndex(int rowIndex)
        {
            this.rowIndex.Text = rowIndex.ToString();
            BtnImageAreaSet.Content = "Set";
        }

        private void MenuButton(object sender, RoutedEventArgs e)
        {
            _imageController.SendImageEvent -= SetCameraImage;
            _imageController.NewRowIndex -= SetRowIndex;
            MainWindow.ChangeFrameContent(new MenuView(MainWindow));
        }

        protected override void SetReferencesFromSpektrometerService()
        {
            _cameraController = CameraController.GetInstance();
            _imageController = ImageController.GetInstance();
            _imageController.SendImageEvent += SetCameraImage;
            _imageController.NewRowIndex += SetRowIndex;
        }

        private void SetCamera(object sender, RoutedEventArgs e)
        {
            if (CameraController.GetCameraIndex() != -1)
            {
                CameraController.CameraStop();
            }
            string selected = this.CameraListBox.Text;
            this.CameraListBox.SelectedItem = selected;
            int ix = list.FindIndex(item => item == selected);
            _cameraController.SelectCamera(ix);
            BtnCameraChoice.Content = "Set";

        }

        public void SetCameraImage(BitmapSource bitmap)
        {
            CameraImage.Source = bitmap;
        }

        private void CameraChosen(object sender, SelectionChangedEventArgs e)
        {
            BtnCameraChoice.Content = "*Set";
        }

        private void RowIndexChanged(object sender, TextChangedEventArgs e)
        {
            BtnImageAreaSet.Content = "*Set";
        }

        private void RowCountChanged(object sender, TextChangedEventArgs e)
        {
            BtnImageAreaSet.Content = "*Set";
        }

        private void ImageAreaSet(object sender, RoutedEventArgs e)
        {
            int y = Int32.Parse(this.rowIndex.Text);
            int h = Int32.Parse(this.rowCount.Text);
            ImageController.SetRowIndex(y);
            ImageController.SetRowCount(h);
            BtnImageAreaSet.Content = "Set";
        }

        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            if (CameraController.GetCameraIndex() != -1)
            {
                CameraController.ShowSettings();
            } else { return; }
        }
    }
}
