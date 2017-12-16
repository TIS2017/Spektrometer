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

namespace Spektrometer.GUI
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : Page
    {
        MainWindow mainWindow;
        public MenuView(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.bla(new CameraView());
        }
    }
}
