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
                string path = Path.GetFullPath(dialogWindow.FileName);
                _import.ImportCalibrationFile(path);
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
                string path = Path.GetFullPath(dialogWindow.FileName);
                _import.ImportChartData(path);
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
                string path = Path.GetFullPath(dialogWindow.FileName);
                _import.ImportCameraImage(path);
            }
        }
    }
}
