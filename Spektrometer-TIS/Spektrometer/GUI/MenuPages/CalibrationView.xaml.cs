using System;
using System.IO;
using System.Windows;
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

        protected override void SetReferences()
        {
            _graphController = GraphController.GetInstance();
            _import = new Import();
        }

        private void CalibrationFile(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dialogWindow = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dialogWindow.DefaultExt = ".txt";
            dialogWindow.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dialogWindow.ShowDialog();

            // Get the selected file name
            if (result == true)
            {
                // Open document 
                string path = Path.GetFullPath(dialogWindow.FileName);
                Import.importCalibrationFile(path);
            }
        }
    }
}
