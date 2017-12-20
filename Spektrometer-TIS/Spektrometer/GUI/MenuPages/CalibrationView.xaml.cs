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
    /// Interaction logic for CalibrationView.xaml
    /// </summary>
    public partial class CalibrationView : MenuComponent
    {
        private Import _import;
        private GraphController _graphController;

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
        public GraphController GraphController
        {
            get
            {
                return _graphController;
            }
            set
            {
                _graphController = value;
            }
        }

        public CalibrationView(MainWindow mainWindow) : base(mainWindow)
        {
            InitializeComponent();
        }

        private void MenuButton(object sender, RoutedEventArgs e)
        {
            MainWindow.ChangeFrameContent(new MenuView(MainWindow));
        }

        protected override void SetReferencesFromSpektrometerService()
        {
            _graphController = SpektrometerService.GraphController;
            _import = SpektrometerService.Import;
        }
    }
}
