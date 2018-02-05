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

        public Import Import
        {
            get
            {
                return _import;
            }
            set
            {
                _import = value;
            }
        }
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
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dialogWindow = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dialogWindow.DefaultExt = ".txt";
            dialogWindow.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dialogWindow.ShowDialog();

            // Get the selected file name
            if (result == true)
            {
                // Open document 
                string path = Path.GetFullPath(dialogWindow.FileName);
                Import.importCalibrationFile(path);
            }
        }

        private void ChartData(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialogWindow = new Microsoft.Win32.OpenFileDialog(); 
            dialogWindow.DefaultExt = ".txt";
            dialogWindow.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"; 
            Nullable<bool> result = dialogWindow.ShowDialog();

            if (result == true)
            {
                string path = Path.GetFullPath(dialogWindow.FileName);
                Import.importChartData(path);
            }
        }

        private void CameraImage(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialogWindow = new Microsoft.Win32.OpenFileDialog();
            dialogWindow.DefaultExt = ".png";
            dialogWindow.Filter = "png Image|*.png|Bitmap Image|*.bmp|Jpeg Image|*.jpg|Gif Image|*.gif";

            Nullable<bool> result = dialogWindow.ShowDialog();

            if (result == true)
            {
                string path = Path.GetFullPath(dialogWindow.FileName);
                Import.importCameraImage(path);
            }
        }
    }
}
