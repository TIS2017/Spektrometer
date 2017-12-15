using System;
using System.Collections.Generic;
using System.Drawing;

using Spektrometer.GUI;

namespace Spektrometer.Logic
{
    public class GraphController
    {
        public GraphData GraphData { get; }
        private GraphView _graphViewer;
        private GraphCalculator _graphCalculator;
        public CalibrationPoints CalibrationPoints { get; }

        public GraphController(GraphView gv, GraphCalculator gc)
        {
            GraphData = new GraphData();
            GraphData.NewData = update;
            CalibrationPoints = new CalibrationPoints();
            CalibrationPoints.NewData = update;
            _graphViewer = gv;
            _graphCalculator = gc;
        }
        
        // Zavola sa po kazdej zmene GraphData a CalibrationPoints
        public void update()
        {
            throw new System.NotImplementedException();
        }
    }
}
