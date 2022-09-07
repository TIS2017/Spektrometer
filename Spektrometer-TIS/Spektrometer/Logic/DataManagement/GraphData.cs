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
        RGB, R, G, B, MAX


    }
    public enum DisplayFormat
    {
        pixel, wavelength, parabola
    }

    public class GraphData
    {
        public delegate void NewDataEventHandler();

        internal NewDataEventHandler OnChartDataChange { get; set; }
        internal NewDataEventHandler OnCalculationDataChange { get; set; }
        private List<Color> _graphDataInPixels;
        private List<double> _intesityData;
        private List<Color> _referencedPicture;
        private List<Color> _actualPicture;
        private bool _globalPeak;
        private bool _fillChart;
        private bool _showValues;
        private int _treshold;
        private int _epsilon;
        private int _minheight;
        private bool _subtraction;
        private bool _division;
        private Filter _filter;
        private DisplayFormat _displayFormat;

        public List<Color> GraphDataInPixels
        {
            get { return _graphDataInPixels; }
            set
            {
                _graphDataInPixels = value;
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

        private bool processedPicture = true;
        public List<Color> ActualPicture
        {
            get  {  return _actualPicture;  }
            set
            {
                if (processedPicture)
                {
                    processedPicture = false;
                    _actualPicture = value;
                    if (OnCalculationDataChange == null)
                    {
                        processedPicture = true;
                        return;
                    }
                    OnCalculationDataChange();
                    processedPicture = true;
                }
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
        public int Treshold
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
        public int XMinDist
        {
            get { return _epsilon; }
            set
            {
                _epsilon = value;
                if (OnChartDataChange == null)
                    return;
                OnChartDataChange();
            }
        }
        public int MinValeyHeight
        {
            get { return _minheight; }
            set
            {
                _minheight = value;
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
            GraphDataInPixels = new List<Color>();
            IntesityData = new List<double>();
            ReferencedPicture = new List<Color>();
            ActualPicture = new List<Color>();
            GlobalPeak = false;
            ShowValues = false;
            Treshold = -1;
            Subtraction = false;
            Division = false;
            FillChart = false;            
            Filter = Filter.RGB;
            DisplayFormat = DisplayFormat.pixel;
        }
    }
}
