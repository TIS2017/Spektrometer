using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
            saveImg.Text = "1";
        }

        private void MenuButton(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new MenuView(MainWindow));
        }

        protected override void SetReferencesFromSpektrometerService()
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
                Filter = "Bitmap Image|*.bmp|Jpeg Image|*.jpg|Gif Image|*.gif",
                Title = "Save Chart Image"
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                string path = Path.GetFullPath(saveFileDialog.FileName);
                Export.chartImage(path);
            }
        }

        private void CameraImage(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Bitmap Image|*.bmp|Jpeg Image|*.jpg|Gif Image|*.gif",
                Title = "Save Camera Image"
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                string path = Path.GetFullPath(saveFileDialog.FileName);
                Export.cameraImage(path);
            }
        }

        private void saveChecboxUnChacked(object sender, RoutedEventArgs e)
        {
            saveImg.Text = "1";
            this.path.Content = "path: ";
            // call method to stop saving data
        }

        private void saveCheckBoxChecked(object sender, RoutedEventArgs e)
        {

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Bitmap Image|*.bmp|Jpeg Image|*.jpg|Gif Image|*.gif",
                    Title = "Save Camera Image"
                };
                saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                string path = Path.GetFullPath(saveFileDialog.FileName);
                this.path.Content += path;
                int sec;
                if (Int32.TryParse(saveImg.Text, out sec))
                {
                    // call method to save data with param sec
                }
                else
                {
                    sec = 1;
                    saveImg.Text = "1";

                    // call method to save data with param sec
                }
            }
        }
    }
}
