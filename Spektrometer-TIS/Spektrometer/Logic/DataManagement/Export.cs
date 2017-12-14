using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spektrometer.Logic
{
    public class Export
    {
        private GraphController _graphController;
        private ImageController _imageController;

        public Export(GraphController gc, ImageController ic)
        {
            _graphController = gc;
            _imageController = ic;
        }

        public void calibrationFile(string path)
        {
            throw new System.NotImplementedException();
        }

        public void chartData(string path)
        {
            throw new System.NotImplementedException();
        }

        public void cameraImage(string path)
        {
            throw new System.NotImplementedException();
        }

        public void chartImage(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
