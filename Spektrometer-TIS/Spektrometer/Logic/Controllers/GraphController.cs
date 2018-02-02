﻿using System;
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
        public CalibrationPoints CalibrationPoints { get; }
        private static GraphController graphControllerInstance;

        public delegate void RedrawChartHandler(List<Color> pixelData);
        public RedrawChartHandler RedrawChart;

        private GraphController()
        {
            GraphData = new GraphData();
            GraphData.OnCalculationDataChange += Recalculate;
            _graphCalculator = new GraphCalculator();          
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
            GraphData.PixelData = tmp;

            if (RedrawChart == null) {
                throw new NullReferenceException("");
            } else
            {
                RedrawChart(tmp);
            }
            
            
        }
    }
}
