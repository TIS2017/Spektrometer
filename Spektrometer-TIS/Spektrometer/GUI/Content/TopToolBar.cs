using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spektrometer.Logic;

namespace Spektrometer.GUI
{
    public class TopToolBar
    {
        private CameraController _cameraController;
        private GraphController _graphController;

        public TopToolBar(Logic.CameraController cc, GraphController gc)
        {
            _cameraController = cc;
            _graphController = gc;
        }
    }
}
