using System;
using System.Collections.Generic;
using System.Drawing;

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

        private Func<double, string> _formatter;
        private double _from;
        private double _to;
        private double _rangeValue;

        public GraphController()
        {
            GraphData = new GraphData();
            GraphData.NewData = update;
            _graphCalculator = new GraphCalculator();

            IsReading = false;

            From = 0;
            To = 1280;

            Read();

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

        public bool IsReading { get; set; }
        public GraphController DataContext { get; private set; }


        private void Read()
        {
            if (IsReading) return;

            IsReading = true;

            Action readFromThread = () =>
            {
                while (IsReading)
                {
                    Thread.Sleep(500);
                    update();
                    OnPropertyChanged();


                }
            };

            //2 different tasks adding a value every ms
            //add as many tasks as you want to test this feature
            Task.Factory.StartNew(readFromThread);
        }


        // Zavola sa po kazdej zmene GraphData a CalibrationPoints
        public void update()
        {
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

            //Random rnd = new Random();

            //int pocitadlo = 1;
            //bool logic = true;
            //for (var j = 0; j < 3; j++)
            //{
            //    var l = new List<double>();
            //    for (var i = 0; i < 1280; i++)
            //    {
            //        if (pocitadlo == 255 || pocitadlo == 0)
            //        {
            //            logic = !logic;
            //        }
            //        var cislo = (rnd.Next(pocitadlo));
            //        l.Add(rnd.Next(200, 255));
            //        if (logic)
            //        {
            //            pocitadlo++;
            //        }
            //        else
            //        {
            //            pocitadlo--;
            //        }

            //    }

            //    pocitadlo = 20;
            //    logic = true;

            //    if (j == 0)
            //    {
            //        ValuesRed = l.AsGearedValues().WithQuality(Quality.High);
            //    }
            //    else if (j == 1)
            //    {
            //        ValuesBlue = l.AsGearedValues().WithQuality(Quality.High);
            //    }
            //    else if (j == 2)
            //    {
            //        ValuesGreen = l.AsGearedValues().WithQuality(Quality.High);
            //    }
            
            //}
        }
    }
}
