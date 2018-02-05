using System;
using System.IO;
using System.Windows;
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
            _import = new Import();
        }

        private void CalibrationFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialogWindow = new Microsoft.Win32.OpenFileDialog();
            
            dialogWindow.DefaultExt = ".txt";
            dialogWindow.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            
            bool? result = dialogWindow.ShowDialog();
            
            if (result == true)
            {
                string path = Path.GetFullPath(dialogWindow.FileName);
                _import.ImportCalibrationFile(path);
            }
        }

        private void ChartData(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialogWindow = new Microsoft.Win32.OpenFileDialog(); 
            dialogWindow.DefaultExt = ".txt";
            dialogWindow.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"; 
            bool? result = dialogWindow.ShowDialog();

            if (result == true)
            {
                string path = Path.GetFullPath(dialogWindow.FileName);
                _import.ImportChartData(path);
            }
        }

        private void CameraImage(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialogWindow = new Microsoft.Win32.OpenFileDialog();
            dialogWindow.DefaultExt = ".png";
            dialogWindow.Filter = "png Image|*.png|Bitmap Image|*.bmp|Jpeg Image|*.jpg|Gif Image|*.gif";

            bool? result = dialogWindow.ShowDialog();

            if (result == true)
            {
                string path = Path.GetFullPath(dialogWindow.FileName);
                _import.ImportCameraImage(path);
            }
        }
    }
}
