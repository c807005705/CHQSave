
using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Mobot.Utils.IO;

namespace Protocols
{
    /// <summary>
    /// 表示一个通用通信设备。
    /// </summary>
    public interface IDevice
    {
        Mobot.Utils.IO.SerialPort Port { get; }
        void Open();
        void Close();
        int Write(byte[] buffer, int offset, int count);
        byte[] ReadExists();
        event EventHandler DataReceived;
        event EventHandler<SerialPortIOEventArgs> IO;
        bool IsOpen { get; }
        bool HasData { get; }
    }
}
