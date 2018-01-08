using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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
        public delegate void NewDataEventHandler();

        internal NewDataEventHandler OnChartDataChange { get; set; }
        internal NewDataEventHandler OnCalculationDataChange { get; set; }
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
                if (OnChartDataChange == null)
                    return;
                OnChartDataChange();
            }
        }
        public List<double> IntesityData
        {
            get {  return _intesityData;  }
            set
            {
                _intesityData = value;
                if (OnCalculationDataChange == null)
                    return;
                OnCalculationDataChange();
            }
        }

        public List<Color> ReferencedPicture
        {
            get {  return _referencedPicture;  }
            set
            {
                _referencedPicture = value;
                if (OnCalculationDataChange == null)
                    return;
                OnCalculationDataChange();
            }
        }
        public List<Color> ActualPicture
        {
            get  {  return _actualPicture;  }
            set
            {
                _actualPicture = value;
                if (OnCalculationDataChange == null)
                    return;
                OnCalculationDataChange();
            }
        }
        public bool ShowPeaks
        {
            get  {  return _showPeaks;  }
            set
            {
                _showPeaks = value;
                if (OnChartDataChange == null)
                    return;
                OnChartDataChange();
            }
        }
        public bool GlobalPeak
        {
            get  {  return _globalPeak;  }
            set
            {
                _globalPeak = value;
                if (OnChartDataChange == null)
                    return;
                OnChartDataChange();
            }
        }
        public bool ShowValues
        {
            get  {  return _showValues;  }
            set
            {
                _showValues = value;
                if (OnChartDataChange == null)
                    return;
                OnChartDataChange();
            }
        }
        public double Treshold
        {
            get  {  return _treshold;  }
            set
            {
                _treshold = value;
                if (OnChartDataChange == null)
                    return;
                OnChartDataChange();
            }
        }
        public bool Subtraction
        {
            get  {  return _subtraction;  }
            set
            {
                _subtraction = value;
                if (OnCalculationDataChange == null)
                    return;
                OnCalculationDataChange();
            }
        }
        public bool Division
        {
            get  {  return _division;  }
            set
            {
                _division = value;
                if (OnCalculationDataChange == null)
                    return;
                OnCalculationDataChange();
            }
        }
        public bool FillChart
        {
            get {  return _fillChart;  }
            set
            {
                _fillChart = value;
                if (OnChartDataChange == null)
                    return;
                OnChartDataChange();
            }
        }
        public Filter Filter
        {
            get  {  return _filter;  }
            set
            {
                _filter = value;
                if (OnChartDataChange == null)
                    return;
                OnChartDataChange();
            }
        }
        public DisplayFormat DisplayFormat
        {
            get  {  return _displayFormat;  }
            set
            {
                _displayFormat = value;
                if (OnCalculationDataChange == null)
                    return;
                OnCalculationDataChange();
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
