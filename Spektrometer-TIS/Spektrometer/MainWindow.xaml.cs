using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Spektrometer.GUI;
using Spektrometer.Logic;
using System.Threading;

namespace Spektrometer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CameraController cameraController;

        public MainWindow()
        {
            InitializeComponent();
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-EN");

            Import import = Import.GetInstance();
            import.LoadConfig();
            Closing += OnWindowClosing;

            menu.Content = new MenuView(this);
            cameraController = CameraController.GetInstance();
        }

        public void switchToNanometers()
        {
            dispayFormat.SelectedIndex = 1;
        }
        
        public void ChangeFrameContent(Page page)
        {
            menu.Content = page;
        }

        private void StartButton(object sender, RoutedEventArgs e)
        {
            if (cameraController.GetCameraIndex() != -1)
            {
                signal.Source = new BitmapImage(new Uri("/Spektrometer;component/GUI/images/signal-on.png", UriKind.Relative));
               cameraController.CameraStart();
            }
        }

        private void StopButton(object sender, RoutedEventArgs e)
        {
            signal.Source = new BitmapImage(new Uri("/Spektrometer;component/GUI/images/signal-off.png", UriKind.Relative));
            cameraController.CameraStop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var open = menu.FindResource("open") as Storyboard;
            open.Begin();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            cameraController.CameraStop();
            var export = Export.GetInstance();
            export.SaveConfig();
        }

        private void DisplayFormatChanged(object sender, SelectionChangedEventArgs e)
        {
            var graphController = GraphController.GetInstance();
            if (dispayFormat.SelectedIndex == 0)
            {
                graphController.GraphData.DisplayFormat = DisplayFormat.pixel;
            }
            else
            {
                if (graphController.CalibrationPoints.CalibrationPointsList.Count < 3)
                {
                    dispayFormat.SelectedIndex = 0;
                    MessageBox.Show("Insufficient number of calibration points.");
                }
                else
                {
                    if (dispayFormat.SelectedIndex == 1)
                        graphController.GraphData.DisplayFormat = DisplayFormat.wavelength;
                    else
                        graphController.GraphData.DisplayFormat = DisplayFormat.parabola;
                }
            }
        }
    }
}
