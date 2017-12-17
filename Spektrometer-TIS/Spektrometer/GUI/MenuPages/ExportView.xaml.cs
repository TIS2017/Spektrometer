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
    /// Interaction logic for ExportView.xaml
    /// </summary>
    public partial class ExportView : Page
    {
        private Export _export;
        MainWindow mainWindow;

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
        public ExportView(MainWindow mainWindow) : base()
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
