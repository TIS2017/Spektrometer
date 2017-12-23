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
        public List<Point> CalibrationPointsList
        {
            get { return CalibrationPointsList; }
            set
            {
                CalibrationPointsList = value;
                NewData();
            }
        }

        public CalibrationPoints()
        {
            CalibrationPointsList = new List<Point>();
        }

        public void AddPoint(Point point)
        {
            CalibrationPointsList.Add(point);
        }
    }
}