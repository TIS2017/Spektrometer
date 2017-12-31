using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for CameraRecordView.xaml
    /// </summary>
    public partial class CameraRecordView : ScrollViewer
    {
        private ImageController _imageController;
        public CameraRecordView(ImageController ic)
        {
            InitializeComponent();
            _imageController = ic;
            _imageController.NewRowIndex += SetRowIndex;
            _imageController.NewRowCount += SetRowCount;
            _imageController.SendImageEvent += SetNewImage;
            SetRowIndex(_imageController.GetRowIndex());
            SetRowCount(_imageController.GetRowCount());
        }

        private void NewRow(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(image);
            _imageController.NewRowIndex((int)p.Y);
        }

        public void SetNewImage(BitmapSource bitmap)
        {
            image.Source = bitmap;
        }

        public void SetRowIndex(int rowIndex)
        {
            TranslateTransform translate = new TranslateTransform(0, rowIndex);
            rectangle.RenderTransform = translate;
        }

        public void SetRowCount(int count)
        {
            rectangle.Height = count;
        }
    }
}
