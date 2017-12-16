using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spektrometer.Logic
{
    public class CalibrationPoints
    {
        public delegate void Update();

        internal Update NewData;
        public List<Point> calibrationData
        {
            get { return calibrationData; }
            set
            {
                calibrationData = value;
                NewData();
            }
        }

        public CalibrationPoints()
        {
            calibrationData = new List<Point>();
        }

        public void addPoint(Point point)
        {
            calibrationData.Add(point);
        }
    }

    public struct Point
    {
        public double x, y;
    }
}