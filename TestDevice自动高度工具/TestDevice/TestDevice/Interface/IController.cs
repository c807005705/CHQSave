using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Mobot;

namespace TestDevice
{
    public interface IController
    {
        bool IsReset { get; set; }
        bool IContinue { get; set; }
        Dictionary<string, MobileKeyInfo> Keys { get; }
        bool DebugMode { get; set; }
        Point3 MotorSpeed { get; set; }
        double? QuickPointZ { get; set; }
        double? TouchPadClickZ { get; set; }
        Point2D? TouchReset { get; set; }
        int DelayBeforeReset { get; set; }
        IControllerSettings ControllerSettings { get; }
        double StylusLocationX { get; }
        double StylusLocationY { get; }
        double StylusLocationZ { get; }
        Point3D StylusLocation { get; }
        void Reset();
        void ModeStop();
    }
}
