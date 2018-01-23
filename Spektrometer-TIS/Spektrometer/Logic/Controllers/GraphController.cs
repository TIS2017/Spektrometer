using System;
using System.Collections.Generic;

using System.ComponentModel;
using LiveCharts.Defaults;
using LiveCharts.Geared;

using Spektrometer.GUI;
using System.Timers;
using System.Windows.Controls;
using System.Threading;
using System.Threading.Tasks;

namespace Spektrometer.Logic
{
    public class GraphController : INotifyPropertyChanged
    {
        public GraphData GraphData { get; }
        private GraphCalculator _graphCalculator;
        public CalibrationPoints CalibrationPoints { get; }
        private static GraphController graphControllerInstance;

        public delegate void Force();
        public Force ForceUpdate { get; set; }
        
        private Func<double, string> _formatter;
        private double _from;
        private double _to;
        private double _rangeValue;

        public GraphController()
        {
            GraphData = new GraphData();
            GraphData.OnCalculationDataChange += Recalculate;
            _graphCalculator = new GraphCalculator();
            graphControllerInstance = this;
            From = 0;
            To = 1280;
        }

        public static GraphController GetInstance()
        {
            if (graphControllerInstance == null)
            {
                graphControllerInstance = new GraphController();
            }
            return graphControllerInstance;
        }

        public object Mapper { get; set; }
        public GearedValues<double> ValuesRed { get; set; }
        public GearedValues<double> ValuesGreen { get; set; }
        public GearedValues<double> ValuesBlue { get; set; }

        public double From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged("From");
            }
        }

        public double To
        {
            get { return _to; }
            set
            {
                _to = value;
                OnPropertyChanged("To");
            }
        }


        public double RangeValue
        {
            get { return _rangeValue; }
            set
            {
                _rangeValue = value;
                OnPropertyChanged("RangeValue");
            }
        }

        public Func<double, string> Formatter
        {
            get { return _formatter; }
            set
            {
                _formatter = value;
                OnPropertyChanged("Formatter");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public GraphController DataContext { get; private set; }

        
        // Zavola sa po kazdej zmene GraphData a CalibrationPoints
        public void Recalculate()
        {
            var tmp = GraphData.ActualPicture;
            // TODO:
            GraphData.PixelData = tmp;

            if (Monitor.TryEnter(this)) {


                var listOfRedValues = new List<double>();
                var listOfGreenValues = new List<double>();
                var listOfBlueValues = new List<double>();
                for (int i = 0; i < GraphData.PixelData.Count; i++)
                {
                    listOfRedValues.Add(GraphData.PixelData[i].R);
                    listOfGreenValues.Add(GraphData.PixelData[i].G);
                    listOfBlueValues.Add(GraphData.PixelData[i].B);
                }

                ValuesRed = listOfRedValues.AsGearedValues().WithQuality(Quality.High);
                ValuesBlue = listOfGreenValues.AsGearedValues().WithQuality(Quality.High);
                ValuesGreen = listOfBlueValues.AsGearedValues().WithQuality(Quality.High);

                OnPropertyChanged();
                ForceUpdate();

                Monitor.Exit(this);
            }
        }
    }
}
