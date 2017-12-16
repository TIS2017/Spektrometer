using System;
using System.Collections.Generic;
using System.Drawing;

using System.ComponentModel;
using LiveCharts.Defaults;
using LiveCharts.Geared;

using Spektrometer.GUI;

namespace Spektrometer.Logic
{
    public class GraphController : INotifyPropertyChanged
    {
        public GraphData GraphData { get; }
        private GraphCalculator _graphCalculator;
        public CalibrationPoints CalibrationPoints { get; }

        private Func<double, string> _formatter;
        private double _from;
        private double _to;
        private double _rangeValue;

        public GraphController()
        {
            //GraphData = new GraphData();
            //GraphData.NewData = update;
            //CalibrationPoints = new CalibrationPoints();
            //CalibrationPoints.NewData = update;
            _graphCalculator = new GraphCalculator();


            var now = DateTime.Now;
            Random rnd = new Random();

            var r = new Random();
            for (var j = 0; j < 3; j++)
            {
                var l = new List<Double>();
                for (var i = 0; i < 1280; i++)
                {
                    var cislo = r.Next(100);
                    l.Add(cislo);
                }

                if (j == 0)
                {
                    ValuesRed = l.AsGearedValues().WithQuality(Quality.Low);

                }
                else if (j == 1)
                {
                    ValuesBlue = l.AsGearedValues().WithQuality(Quality.Low);
                }
                else if (j == 2)
                {
                    ValuesGreen = l.AsGearedValues().WithQuality(Quality.Low);
                }

            }


            From = 0;
            To = 1280;
        }

        public object Mapper { get; set; }
        public GearedValues<Double> ValuesRed { get; set; }
        public GearedValues<Double> ValuesGreen { get; set; }
        public GearedValues<Double> ValuesBlue { get; set; }

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


        // Zavola sa po kazdej zmene GraphData a CalibrationPoints
        public void update()
        {
            throw new System.NotImplementedException();
        }
    }
}
