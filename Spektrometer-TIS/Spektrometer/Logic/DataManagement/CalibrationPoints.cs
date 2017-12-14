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
    }

    public struct Point
    {
        public double x, y;
    }
}