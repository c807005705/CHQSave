using Mobot.Utils.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Mobot;

namespace TestDevice
{
    public interface IDisplay
    {
        DisplayInfo DisplayInfo { get; set; }
        WorkspaceInfo WorkspaceInfo { get; set; }
        DatumMarks DatumMarks { get; set; }
        CalculateCoefficient CalculateCoefficient { get; set; }
        void PointToMotor(int pixelX, int pixelY, out double x, out double y);
    }
}
