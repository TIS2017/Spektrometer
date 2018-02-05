using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using Microsoft.Win32;
using Spektrometer.Logic;

namespace Spektrometer.GUI
{
    /// <summary>
    /// Interaction logic for ExportView.xaml
    /// </summary>
    public partial class ExportView : MenuComponent
    {
        private Export _export;

        public Export Export
        {
            get
            {
                return _export;
            }
            set
            {
                _export = value;
            }
        }
        public ExportView(MainWindow mainWindow) : base(mainWindow)
        {
            InitializeComponent();
        }

        private void MenuButton(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new MenuView(MainWindow));
        }

        protected override void SetReferences()
        {
            _export = new Export();
        }

        private void CalibrationFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Save Calibration File"
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                string path = Path.GetFullPath(saveFileDialog.FileName);
                Export.calibrationFile(path);
            }
        }

        private void ChartData(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Save Chart Data File"
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                string path = Path.GetFullPath(saveFileDialog.FileName);
                Export.chartData(path);
            }
        }

        private void ChartImage(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Png Image|*.png",
                Title = "Save Chart Image"
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                string path = Path.GetFullPath(saveFileDialog.FileName);
                Export.chartImage(path, MainWindow.graphView.canGraph);
            }
        }

        private void CameraImage(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Png Image|*.png",
                Title = "Save Camera Image"
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                string path = Path.GetFullPath(saveFileDialog.FileName);
                Export.cameraImage(path);
            }
        }
    }
}
