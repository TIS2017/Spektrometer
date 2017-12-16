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
    /// Interaction logic for CameraRecordView.xaml
    /// </summary>
    
    public partial class CameraRecordView : Canvas
    {
        public delegate void OnClick(int y);
        public OnClick NewLineIndex { get; set; }
        public CameraRecordView()
        {
            InitializeComponent();
        }
    }
}
