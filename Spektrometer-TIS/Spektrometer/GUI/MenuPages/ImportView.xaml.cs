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
    /// Interaction logic for ImportView.xaml
    /// </summary>
    public partial class ImportView : Page
    {
        private Import _import;
        MainWindow mainWindow;

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
        public ImportView(MainWindow mainWindow)
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
