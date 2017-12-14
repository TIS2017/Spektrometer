using System;
using System.Collections.Generic;
using System.Drawing;

using Spektrometer.GUI;

namespace Spektrometer.Logic
{
    public class GraphController
    {
        private GraphData _graphData;
        private GraphView _graphViewer;
        private GraphCalculator _graphCalculator;
        private CalibrationPoints _calibrationPoints;

        public GraphController(GraphView gv, GraphCalculator gc)
        {
            _graphData = new GraphData();
            _graphData.NewData = update;
            _calibrationPoints = new CalibrationPoints();
            _calibrationPoints.NewData = update;
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
