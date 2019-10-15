using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobot;

namespace TestDevice
{
    public interface IMotorSettings
    {
        DateTime DeviceDate { get; }
        Guid DeviceGuid { get; }
        string DeviceManufacturer { get; }
        Range[] MotorRangeTicks { get; }
        string DeviceSerialNumber { get; }
        Point3D TicksPerMM { get; }
        Point3 ZeroTicks { get; }
    }
}
