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
        private int _rowIndex;
        private int _changedRowIndex;
        private int _rowCount;
        private int _changedRowCount;
        private int _imageCount;
        private int _changedImageCount;
        private int _cameraIndex;

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
            _rowIndex = _changedRowIndex = ImageController.GetRowIndex();
            this.rowIndex.Text = _rowIndex.ToString();
            _rowCount = _changedRowCount = ImageController.GetRowCount();
            this.rowCount.Text = _rowCount.ToString();
            _imageCount = _changedImageCount = ImageController.GetImageCount();
            this.numOfPic.Text = _imageCount.ToString();
            BtnImageAreaSet.Content = "Set";

            // Nefunguje uplne spravne, opravit - zobrazenie *
            _cameraIndex = CameraController.GetCameraIndex();
            if (_cameraIndex != -1)
            {
                //Debug.WriteLine("Test");
                CameraListBox.SelectedItem = list[_cameraIndex];
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
            _rowIndex = rowIndex;
            this.rowIndex.Text = _rowIndex.ToString();
            BtnImageAreaSet.Content = "Set";
        }

        private void MenuButton(object sender, RoutedEventArgs e)
        {
            _imageController.SendImageEvent -= SetCameraImage;
            _imageController.NewRowIndex -= SetRowIndex;
            MainWindow.ChangeFrameContent(new MenuView(MainWindow));
        }

        protected override void SetReferences()
        {
            _cameraController = CameraController.GetInstance();
            _imageController = ImageController.GetInstance();
            _imageController.SendImageEvent += SetCameraImage;
            _imageController.NewRowIndex += SetRowIndex;
        }

        private void SetCamera(object sender, RoutedEventArgs e)
        {
            int selectedCamera = this.CameraListBox.SelectedIndex;
            if (selectedCamera != _cameraIndex)
            {
                if (CameraController.GetCameraIndex() != -1)
                {
                    CameraController.CameraStop();
                }
                _cameraIndex = selectedCamera;
                _cameraController.SelectCamera(_cameraIndex);
                BtnCameraChoice.Content = "Set";
            }
        }

        public void SetCameraImage(BitmapSource bitmap)
        {
            CameraImage.Source = bitmap;
        }

        private void CameraChosen(object sender, SelectionChangedEventArgs e)
        {
            if (CameraListBox.SelectedIndex != _cameraIndex)
                BtnCameraChoice.Content = "*Set";
        }

        private void RowIndexChanged(object sender, TextChangedEventArgs e)
        {
            if (ConvertableToInt(rowIndex.Text))
            {
                _changedRowIndex = Int32.Parse(rowIndex.Text);
            }
            BtnImageAreaSet.Content = "*Set";
        }

        private void RowCountChanged(object sender, TextChangedEventArgs e)
        {
            if (ConvertableToInt(rowCount.Text))
            {
                _changedRowCount = Int32.Parse(rowCount.Text);
            }
            BtnImageAreaSet.Content = "*Set";
        }

        private void numOfPic_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ConvertableToInt(numOfPic.Text))
            {
                _changedImageCount = Int32.Parse(numOfPic.Text);
            }
            BtnImageAreaSet.Content = "*Set";
        }

        private void ImageAreaSet(object sender, RoutedEventArgs e)
        {
            if (_changedRowIndex != _rowIndex)
            {
                _rowIndex = _changedRowIndex;
                ImageController.SetRowIndex(_rowIndex);
            }
            rowIndex.Text = _rowIndex.ToString();
            if (_changedRowCount != _rowCount)
            {
                _rowCount = _changedRowCount;
                ImageController.SetRowCount(_rowCount);
            }
            rowCount.Text = _rowCount.ToString();
            if (_changedImageCount != _imageCount)
            {
                _imageCount = _changedImageCount;
                ImageController.SetImageCount(_imageCount);
            }
            numOfPic.Text = _imageCount.ToString();
            BtnImageAreaSet.Content = "Set";
        }

        private bool ConvertableToInt(string number)
        {
            try
            {
                Int32.Parse(number);
            }
            catch(Exception)
            {
                return false;
            }
            return true;
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
