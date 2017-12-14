using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spektrometer.Logic
{
    public class ImageCalculator
    {
        private GraphController _graphController;

        public ImageCalculator(GraphController gc)
        {
            _graphController = gc;
        }

        public List<Color> makeAverage(ref Stack<List<Color>> pics)
        {
            throw new NotImplementedException();
        }
    }
}
