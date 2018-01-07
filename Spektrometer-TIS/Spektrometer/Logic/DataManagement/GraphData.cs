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
        private List<Color> _pixelData;
        private List<double> _intesityData;
        private List<Color> _referencedPicture;
        private List<Color> _actualPicture;
        private bool _showPeaks;
        private bool _globalPeak;
        private bool _showValues;
        private double _treshold;
        private bool _subtraction;
        private bool _division;
        private bool _fillChart;
        private Filter _filter;
        private DisplayFormat _displayFormat;
        public List<Color> PixelData
        {
            get { return _pixelData; }
            set
            {
                _pixelData = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }
        public List<double> IntesityData
        {
            get {  return _intesityData;  }
            set
            {
                _intesityData = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }

        public List<Color> ReferencedPicture
        {
            get {  return _referencedPicture;  }
            set
            {
                _referencedPicture = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }
        public List<Color> ActualPicture
        {
            get  {  return _actualPicture;  }
            set
            {
                _actualPicture = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }
        public bool ShowPeaks
        {
            get  {  return _showPeaks;  }
            set
            {
                _showPeaks = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }
        public bool GlobalPeak
        {
            get  {  return _globalPeak;  }
            set
            {
                _globalPeak = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }
        public bool ShowValues
        {
            get  {  return _showValues;  }
            set
            {
                _showValues = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }
        public double Treshold
        {
            get  {  return _treshold;  }
            set
            {
                _treshold = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }
        public bool Subtraction
        {
            get  {  return _subtraction;  }
            set
            {
                _subtraction = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }
        public bool Division
        {
            get  {  return _division;  }
            set
            {
                _division = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }
        public bool FillChart
        {
            get {  return _fillChart;  }
            set
            {
                _fillChart = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }
        public Filter Filter
        {
            get  {  return _filter;  }
            set
            {
                _filter = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }
        public DisplayFormat DisplayFormat
        {
            get  {  return _displayFormat;  }
            set
            {
                _displayFormat = value;
                if (NewData == null)
                    return;
                NewData();
            }
        }

        public GraphData()
        {
            PixelData = new List<Color>();
            IntesityData = new List<double>();
            ReferencedPicture = new List<Color>();
            ActualPicture = new List<Color>();
            ShowPeaks = false;
            GlobalPeak = false;
            ShowValues = false;
            Treshold = 0;
            Subtraction = false;
            Division = false;
            FillChart = false;
            Filter = Filter.RGB;
            DisplayFormat = DisplayFormat.pixel;
        }
    }
}
