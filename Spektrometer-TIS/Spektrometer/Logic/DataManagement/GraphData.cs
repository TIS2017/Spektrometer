using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spektrometer.Logic
{

    public enum Filter
    {
        RGB, R, G, B
    }
    public enum DisplayFormat
    {
        pixel, wavelength
    }

    public class GraphData
    {
        public delegate void Update();

        internal Update NewData { get; set; }

        public List<Color> pixelData
        {
            get { return pixelData; }
            set
            {
                pixelData = value;
                NewData();
            }
        }
        public List<double> intesityData
        {
            get {  return intesityData;  }
            set
            {
                intesityData = value;
                NewData();
            }
        }

        public List<Color> referencedPicture
        {
            get {  return referencedPicture;  }
            set
            {
                referencedPicture = value;
                NewData();
            }
        }
        public List<Color> actualPicture
        {
            get  {  return actualPicture;  }
            set
            {
                actualPicture = value;
                NewData();
            }
        }
        public bool showPeaks
        {
            get  {  return showPeaks;  }
            set
            {
                showPeaks = value;
                NewData();
            }
        }
        public bool globalPeak
        {
            get  {  return globalPeak;  }
            set
            {
                globalPeak = value;
                NewData();
            }
        }
        public bool showValues
        {
            get  {  return showValues;  }
            set
            {
                showValues = value;
                NewData();
            }
        }
        public double treshold
        {
            get  {  return treshold;  }
            set
            {
                treshold = value;
                NewData();
            }
        }
        public bool subtraction
        {
            get  {  return subtraction;  }
            set
            {
                subtraction = value;
                NewData();
            }
        }
        public bool division
        {
            get  {  return division;  }
            set
            {
                division = value;
                NewData();
            }
        }
        public bool fillChart
        {
            get {  return fillChart;  }
            set
            {
                fillChart = value;
                NewData();
            }
        }
        public Filter filter
        {
            get  {  return filter;  }
            set
            {
                filter = value;
                NewData();
            }
        }
        public DisplayFormat displayformat
        {
            get  {  return displayformat;  }
            set
            {
                displayformat = value;
                NewData();
            }
        }

        public GraphData()
        {
            pixelData = new List<Color>();
            intesityData = new List<double>();
            referencedPicture = new List<Color>();
            actualPicture = new List<Color>();
            showPeaks = false;
            globalPeak = false;
            showValues = false;
            treshold = 0;
            subtraction = false;
            division = false;
            fillChart = false;
            filter = Filter.RGB;
            displayformat = DisplayFormat.pixel;
        }
    }
}
