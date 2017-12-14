using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spektrometer.GUI;

namespace Spektrometer.Logic
{
    public class SpektrometerService
    {
        public GraphCalculator GraphCalculator { get; private set; }
        public ImageCalculator ImageCalculator { get; private set; }
        public CameraController CameraController { get; set; }
        public GraphController GraphController { get; private set; }
        public ImageController ImageController { get; private set; }
        public Export Export { get; private set; }
        public Import Import { get; private set; }
        public GraphView GraphView { get; private set; }
        public CameraRecordView CameraRecordView { get; private set; }
        public TopToolBar TopToolBar { get; private set; }
        public MenuComponent MenuComponent { get; set; }

        public SpektrometerService()
        {
            classInitialization();
        }

        private void classInitialization()
        {
            graphView = new GraphView();
            cameraRecordView = new CameraRecordView();
            menuComponent = new MenuView();
            GraphCalculator = new GraphCalculator();
            graphController = new GraphController(graphView, graphCalculator);
            imageCalculator = new ImageCalculator(graphController);
            imageController = new ImageController(cameraRecordView, imageCalculator);
            cameraController = new CameraController(imageController);
            export = new Export(graphController, imageController);
            import = new Import(graphController, imageController);
            topToolBar = new TopToolBar(cameraController, graphController);
        }
    }
}
