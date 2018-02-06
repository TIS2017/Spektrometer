using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;

namespace Spektrometer.Logic
{
    public class CalibrationPoints
    {
        public delegate void Update();

        internal Update NewData;
        private List<Point> _calibrationList;

        public List<Point> CalibrationPointsList
        {
            get { return _calibrationList; }
            set
            {
                _calibrationList = value;
                if (NewData == null)
                {
                    return;
                }
                    NewData();
            }
        }

        public CalibrationPoints()
        {
            _calibrationList = new List<Point>();
        }

        public void AddPoint(Point point)
        {
            CalibrationPointsList.Add(point);
        }
    }
}