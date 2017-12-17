using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Spektrometer.GUI;

namespace Spektrometer.Logic
{
    public class SpektrometerService
    {
        public GraphCalculator GraphCalculator { get; private set; }
        public CameraController CameraController { get; set; }
        public GraphController GraphController { get; private set; }
        public ImageController ImageController { get; private set; }
        public Export Export { get; private set; }
        public Import Import { get; private set; }
        public GraphView GraphView { get; private set; }
        public CameraRecordView CameraRecordView { get; private set; }
        public TopToolBar TopToolBar { get; private set; }
        public Page MenuComponent { get; set; }

        public SpektrometerService(MainWindow mainWindow)
        {
            classInitialization(mainWindow);
        }

        private void classInitialization(MainWindow mainWindow)
        {
            GraphView = mainWindow.graphView;
            CameraRecordView = mainWindow.cameraRecordView;
            GraphController = (GraphController)GraphView.DataContext;
            GraphCalculator = new GraphCalculator();
            ImageController = new ImageController(CameraRecordView);
            CameraController = new CameraController(ImageController);
            Export = new Export(GraphController, ImageController);
            Import = new Import(GraphController, ImageController);
            TopToolBar = new TopToolBar(CameraController, GraphController);
        }

        public void setReferences(Page menu)
        {
            if (menu is MenuView)
            {
                ((MenuView)menu).CameraController = CameraController;
            }
            else if (menu is CameraView)
            {
                ((CameraView)menu).CameraController = CameraController;
                ((CameraView)menu).ImageController = ImageController;
            }
            else if (menu is CalibrationView)
            {
                ((CalibrationView)menu).Import = Import;
                ((CalibrationView)menu).GraphController = GraphController;
            }
            else if (menu is ExportView)
            {
                ((ExportView)menu).Export = Export;
            }
            else if (menu is ImportView)
            {
                ((ImportView)menu).Import = Import;
            }
            else if (menu is MeasurementView)
            {
                ((MeasurementView)menu).GraphController = GraphController;
            }
        }
    }
}
