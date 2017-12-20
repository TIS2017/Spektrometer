using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

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
                // nastav delegatovi v imageController SendImageEvent += tvoja funkcia
            }
        }
        public CameraView(MainWindow mainWindow) : base(mainWindow)
        {
            InitializeComponent();
            SetCameraLIst();
        }

        public void SetCameraLIst()
        {
            list = CameraController.getCameraList();
            if (list != null)
            {
                CameraListBox.ItemsSource = list;
            }
        }

        private void MenuButton(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new MenuView(MainWindow));
        }

        protected override void SetReferencesFromSpektrometerService()
        {
            _cameraController = SpektrometerService.CameraController;
            _imageController = SpektrometerService.ImageController;
        }

        private void SetCamera(object sender, RoutedEventArgs e)
        {
            string selected = this.CameraListBox.Text;
            int ix = list.FindIndex(item => item == selected);
            _cameraController.selectCamera(ix);
        }
    }
}
