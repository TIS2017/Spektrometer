using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spektrometer.Logic;

namespace Spektrometer.GUI
{
    public class CameraRecordView
    {
        public delegate void OnClick(int y);
        public OnClick NewLineIndex { get; set; }

        public CameraRecordView()
        {
            NewLineIndex(870);
        }
    }
}
