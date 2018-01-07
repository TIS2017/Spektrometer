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

        public SpektrometerService(MainWindow mainWindow)
        {
            ClassInitialization(mainWindow);
        }

        private void ClassInitialization(MainWindow mainWindow)
        {
            GraphView = mainWindow.graphView;
            GraphController = (GraphController)GraphView.DataContext;
            GraphCalculator = new GraphCalculator();
            ImageController = new ImageController(GraphController);
            CameraController = new CameraController(ImageController);
            Export = new Export(GraphController, ImageController);
            Import = new Import(GraphController, ImageController);
            TopToolBar = new TopToolBar(CameraController, GraphController);
            CameraRecordView = new CameraRecordView(ImageController);
        }
    }
}
