using System;
using System.Collections.Generic;
using System.Text;
using Mobot;

namespace TestDevice
{
    public interface IControllerSettings
    {
        void Reset();
        RangeD RangeX { get; }
        RangeD RangeY { get; }
        RangeD RangeZ { get; }
        Range MotorSpeedRangeX { get; }
        Range MotorSpeedRangeY { get; }
        Range MotorSpeedRangeZ { get; }
        Point3 MotorSpeed { get; }
        void Import(IMotorSettings settings);
        int MM2Ticks(double mm, Axis axis);
    }
}
