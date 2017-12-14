using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spektrometer.Logic
{
    public class CameraController
    {
        private ImageController _imageController;

        public CameraController(ImageController ic)
        {
            _imageController = ic;
        }

        public List<int> getCameraList()
        {
            throw new System.NotImplementedException();
        }

        public void showSettings()
        {
            throw new System.NotImplementedException();
        }

        public void cameraStart()
        {
            throw new System.NotImplementedException();
        }

        public void cameraStop()
        {
            throw new System.NotImplementedException();
        }
    }
}
