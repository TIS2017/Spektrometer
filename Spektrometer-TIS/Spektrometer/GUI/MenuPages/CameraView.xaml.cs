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
    /// Interaction logic for CameraView.xaml
    /// </summary>
    public partial class CameraView : Page
    {
        private CameraController _cameraController;
        MainWindow mainWindow;
        public CameraView(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void MenuButton(object sender, RoutedEventArgs e)
        {
            mainWindow.navigationController(new MenuView(mainWindow));
        }
    }
}
