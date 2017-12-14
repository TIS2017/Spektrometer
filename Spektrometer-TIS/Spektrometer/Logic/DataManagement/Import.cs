using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spektrometer.Logic
{
    public class Import
    {
        private GraphController _graphController;
        private ImageController _imageController;

        public Import(GraphController gc, ImageController ic)
        {
            _graphController = gc;
            _imageController = ic;
        }

        public void importCalibrationFile(string path)
        {
            throw new System.NotImplementedException();
        }

        public void importChartData(string path)
        {
            throw new System.NotImplementedException();
        }

        public void importCameraImage(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
