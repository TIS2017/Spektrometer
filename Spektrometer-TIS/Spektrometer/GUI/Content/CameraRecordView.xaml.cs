
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Spektrometer.Logic;

namespace Spektrometer.GUI
{
    /// <summary>
    /// Interaction logic for CameraRecordView.xaml
    /// </summary>
    public partial class CameraRecordView : ScrollViewer
    {
        private ImageController _imageController;
        private int _rowIndex;
        private int _count;
        private double _maxImageHeight;
        public CameraRecordView() 
        {
            InitializeComponent();
            _imageController = ImageController.GetInstance();
            _imageController.NewRowIndex += SetRowIndex;
            _imageController.NewRowCount += SetRowCount;
            _imageController.SendImageEvent += SetNewImage;
            SetRowIndex(_imageController.GetRowIndex());
            SetRowCount(_imageController.GetRowCount());
            _rowIndex = 1;
            _count = 1;
        }

        private void NewRow(object sender, MouseButtonEventArgs e)
        {
            double rowIndex = e.GetPosition(image).Y / scrollViewGrid.ActualHeight * _maxImageHeight;

            _imageController.SetRowIndex((int)rowIndex);
        }

        public void SetNewImage(BitmapSource bitmap)
        {
            _maxImageHeight = bitmap.PixelHeight;
            image.Source = bitmap;
        }

        public void SetRowIndex(int rowIndex)
        {
            _rowIndex = rowIndex;
            double row = rowIndex / _maxImageHeight * scrollViewGrid.ActualHeight - _count;
            TranslateTransform translate = new TranslateTransform(0, row);
            rectangle.RenderTransform = translate;

            if (image.Source != null)
            {
                double scrollTo = row - ActualHeight / 2 < 0 ? 0 : row - ActualHeight / 2;
                ScrollToVerticalOffset(scrollTo);
            }
        }

        public void SetRowCount(int count)
        {
            _count = count;
            rectangle.Height = 2 * count + 1;
            SetRowIndex(_rowIndex);
        }
    }
}
