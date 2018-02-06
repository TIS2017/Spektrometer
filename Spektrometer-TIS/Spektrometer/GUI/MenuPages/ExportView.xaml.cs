using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
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

        public ExportView(MainWindow mainWindow) : base(mainWindow)
        {
            InitializeComponent();
            saveImg.Text = "1";
        }

        private void MenuButton(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new MenuView(MainWindow));
        }

        protected override void SetReferences()
        {
            _export = Export.GetInstance();
        }

        private void CalibrationFile(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Save Calibration File",
                InitialDirectory = _export.savePath
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                var fileName = Path.GetFileName(saveFileDialog.FileName);
                _export.savePath = Path.GetDirectoryName(saveFileDialog.FileName);
                _export.calibrationFile($"{_export.savePath}{Path.DirectorySeparatorChar}{fileName}");
            }
        }

        private void ChartData(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Save Chart Data File",
                InitialDirectory = _export.savePath
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                var fileName = Path.GetFileName(saveFileDialog.FileName);
                _export.savePath = Path.GetDirectoryName(saveFileDialog.FileName);
                _export.chartData($"{_export.savePath}{Path.DirectorySeparatorChar}{fileName}");
            }
        }

        private void ChartImage(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Png Image|*.png",
                Title = "Save Chart Image",
                InitialDirectory = _export.savePath
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                var fileName = Path.GetFileName(saveFileDialog.FileName);
                _export.savePath = Path.GetDirectoryName(saveFileDialog.FileName);
                _export.chartImage($"{_export.savePath}{Path.DirectorySeparatorChar}{fileName}", MainWindow.graphView.canGraph);
            }
        }

        private void CameraImage(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Png Image|*.png",
                Title = "Save Camera Image",
                InitialDirectory = _export.savePath
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                var fileName = Path.GetFileName(saveFileDialog.FileName);
                _export.savePath = Path.GetDirectoryName(saveFileDialog.FileName);
                _export.cameraImage($"{_export.savePath}{Path.DirectorySeparatorChar}{fileName}");
            }
        }

        private void SaveChecboxUnchecked(object sender, RoutedEventArgs e)
        {
            this.path.Content = "path: ";
            _export.StopSavingAutomatically();
            saveImg.IsEnabled = true;
        }

        private void SaveCheckBoxChecked(object sender, RoutedEventArgs e)
        {

            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "Png Image|*.png",
                Title = "Save Camera Image",
                InitialDirectory = _export.automaticSavePath,
                AddExtension = false
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                _export.automaticSavePath = Path.GetDirectoryName(saveFileDialog.FileName);
                var fileName = Path.GetFileName(saveFileDialog.FileName);
                this.path.Content = $"path: {_export.automaticSavePath}{Path.DirectorySeparatorChar}{fileName}";
                if (!Double.TryParse(saveImg.Text, out double sec))
                {
                    sec = 1;
                    saveImg.Text = "1";
                }
                saveImg.IsEnabled = false;
                _export.StartSavingAutomatically(sec);
            }
            else
            {
                saveChecbox.IsChecked = false;
            }
        }
    }
}
