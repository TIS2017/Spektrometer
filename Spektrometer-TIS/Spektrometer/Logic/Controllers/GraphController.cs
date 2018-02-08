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

        public delegate void RedrawChartHandler(List<Color> pixelData);
        public RedrawChartHandler RedrawChart;
        public delegate List<int> GetPeakIndexesHandler(List<System.Windows.Media.Color> pic, double threshold);
        public GetPeakIndexesHandler GetPeakIndexes;

        private GraphController()
        {
            GraphData = new GraphData();
            GraphData.OnCalculationDataChange += Recalculate;
            _graphCalculator = new GraphCalculator();
            _imageCalculator = new ImageCalculator();
            GetPeakIndexes += _graphCalculator.peaks;

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
            // TODO:
            GraphData.GraphDataInPixels = tmp;

            if(GraphData.Subtraction)
            {
                _imageCalculator.DifferenceBetweenActualAndReferencePicture(GraphData.ActualPicture, GraphData.ReferencedPicture); 
            }

            if (GraphData.Division)
            {
                _imageCalculator.DivisionOfActualAndReferencePicture(GraphData.ActualPicture, GraphData.ReferencedPicture);
            }

            if (GraphData.ShowValues)
            {
                _graphCalculator.peaks(GraphData.ActualPicture, GraphData.Treshold);
            }

            if(GraphData.ShowPeaks)
            {
                // TODO:
            }

            if(GraphData.GlobalPeak)
            {
                _graphCalculator.globalMax(GraphData.ActualPicture);
            }

            if (RedrawChart == null) {
                throw new NullReferenceException("");
            } else
            {
                RedrawChart(tmp);
            }
            
            
        }
    }
}
