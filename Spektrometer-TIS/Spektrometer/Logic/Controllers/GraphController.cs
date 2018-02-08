using System;
using System.Collections.Generic;

using System.ComponentModel;

using Spektrometer.GUI;
using System.Timers;
using System.Windows.Controls;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Spektrometer.Logic
{
    public class GraphController
    {
        public GraphData GraphData { get; }
        private GraphCalculator _graphCalculator;
        private ImageCalculator _imageCalculator;
        public CalibrationPoints CalibrationPoints { get; }
        private static GraphController graphControllerInstance;

        public delegate void RedrawChartHandler();

        public RedrawChartHandler RedrawChart { get; set; }

        private GraphController()
        {
            GraphData = new GraphData();
            GraphData.OnCalculationDataChange += Recalculate;
            GraphData.OnChartDataChange += UpdateGraph;
            _graphCalculator = new GraphCalculator();

            CalibrationPoints = new CalibrationPoints();
        }

        public static GraphController GetInstance()
        {
            if (graphControllerInstance == null)
            {
                graphControllerInstance = new GraphController();
            }
            return graphControllerInstance;
        }
             
        // Zavola sa po kazdej zmene GraphData a CalibrationPoints
        public void Recalculate()
        {
            var tmp = GraphData.ActualPicture;
            if (GraphData.Subtraction)
            {
                _imageCalculator.DifferenceBetweenActualAndReferencePicture(GraphData.ActualPicture, GraphData.ReferencedPicture);
            }

            if (GraphData.Division)
            {
                _imageCalculator.DivisionOfActualAndReferencePicture(GraphData.ActualPicture, GraphData.ReferencedPicture);
            }
            GraphData.GraphDataInPixels = tmp;
        }

        public void UpdateGraph()
        {
            RedrawChart?.Invoke();
        }
    }
}
