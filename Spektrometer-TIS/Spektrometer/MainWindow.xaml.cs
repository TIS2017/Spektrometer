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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Spektrometer.GUI;
using Spektrometer.Logic;

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
            menu.Content = new MenuView(this);
            cameraController = CameraController.GetInstance();
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
    }
}
