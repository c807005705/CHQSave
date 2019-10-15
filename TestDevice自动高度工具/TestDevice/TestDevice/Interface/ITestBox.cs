using Mobot.Utils.Caches;
using Mobot.Utils.IO;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace TestDevice
{
    public interface ITestBox : ITouch, ITouchAsync, IStylus, IController, IDriver, IDisplay
    {
        event EventHandler LocationChanged;
        event EventHandler StateChanged;

        event EventHandler<SerialPortIOEventArgs> IO;

        bool IsOpen { get; }
        string PortName { get; set; }
        string SerialNumber { get; set; }
        string LastErrorString { get; }
        void Open();
        void Close();
        bool UpdatePort();
        List<USBDeviceHelper.DeviceProperties> GetSerialNumbers();
        void BatchActionsAsync(IList<IMotorAction> actions);
    }
}
