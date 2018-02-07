using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Spektrometer.Logic;

namespace Spektrometer.GUI
{
    /// <summary>
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : MenuComponent
    {
        private Import _import;

        public ImportView(MainWindow mainWindow) : base(mainWindow)
        {
            InitializeComponent();
        }

        private void MenuButton(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new MenuView(MainWindow));
        }

        protected override void SetReferences()
        {
            _import = Import.GetInstance();
        }

        private void CalibrationFile(object sender, RoutedEventArgs e)
        {
            var dialogWindow = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = _import.loadPath
            };

            bool? result = dialogWindow.ShowDialog();
            
            if (result == true)
            {
                _import.loadPath = Path.GetFullPath(dialogWindow.FileName);
                _import.ImportCalibrationFile(_import.loadPath);
            }
        }

        private void ChartData(object sender, RoutedEventArgs e)
        {
            var dialogWindow = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = _import.loadPath
            };
            bool? result = dialogWindow.ShowDialog();

            if (result == true)
            {
                _import.loadPath = Path.GetFullPath(dialogWindow.FileName);
                _import.ImportChartData(_import.loadPath);
            }
        }

        private void CameraImage(object sender, RoutedEventArgs e)
        {
           var dialogWindow = new OpenFileDialog
           {
                DefaultExt = ".png",
                Filter = "png Image|*.png",
                InitialDirectory = _import.loadPath
            };

            bool? result = dialogWindow.ShowDialog();

            if (result == true)
            {
                _import.loadPath = Path.GetFullPath(dialogWindow.FileName);
                _import.ImportCameraImage(_import.loadPath);
            }
        }
    }
}
