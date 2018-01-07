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

using Spektrometer.Logic;

namespace Spektrometer.GUI
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : MenuComponent
    {
        private CameraController _cameraController;

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
        public MenuView(MainWindow mainWindow) : base(mainWindow)
        {
            InitializeComponent();
        }

        private void ShowCameraView(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new CameraView(MainWindow));
        }

        private void ShowMeasurementView(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new MeasurementView(MainWindow));
        }

        private void ShowCalibrationView(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new CalibrationView(MainWindow));
        }

        private void ShowImportView(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new ImportView(MainWindow));
        }

        private void ShowExportView(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new ExportView(MainWindow));
        }

        protected override void SetReferencesFromSpektrometerService()
        {
            _cameraController = SpektrometerService.CameraController;
        }

        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            if (CameraController.GetCameraIndex() != -1)
            {
                CameraController.ShowSettings();
            }
        }
    }
}
