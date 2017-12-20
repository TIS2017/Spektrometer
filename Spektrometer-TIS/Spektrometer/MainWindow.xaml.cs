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

using Spektrometer.GUI;
using Spektrometer.Logic;

namespace Spektrometer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SpektrometerService spektrometerService { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            spektrometerService = new SpektrometerService(this);
            menu.Content = new MenuView(this);
        }

        public void ChangeFrameContent(Page page)
        {
            menu.Content = page;
        }
    }
}
