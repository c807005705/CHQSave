using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;
using System.Text;
using Mobot.Utils.IO;

namespace Mobot.TestBox
{
    partial class TestBoxClient : IDriver
    {
        public int BaudRate
        {
            get
            {
                lock (obj)
                    return obj.BaudRate;
            }
            set
            {
                lock (obj)
                    obj.BaudRate = value;
            }
        }

        public ushort FlashPageCount
        {
            get
            {
                lock (obj)
                    return obj.FlashPageCount;
            }
        }

        public ushort FlashPageSize
        {
            get
            {
                lock (obj)
                    return obj.FlashPageSize;
            }
        }

        public Version HardwareVersion
        {
            get
            {
                lock (obj)
                    return obj.HardwareVersion;
            }
        }

        public Version Reversion
        {
            get
            {
                lock (obj)
                    return obj.Reversion;
            }
        }

        public Point3 Ticks
        {
            get
            {
                lock (obj)
                    return obj.Ticks;
            }
        }

        public int Trigger
        {
            get
            {
                lock (obj)
                    return obj.Trigger;
            }
            set
            {
                lock (obj)
                    obj.Trigger = value;
            }
        }

        public object Driver
        {
            get
            {
                lock (obj)
                    return obj.Driver;
            }
        }

        public event EventHandler<SerialPortIOEventArgs> IO;

        public void BatchMoveXY(int xTick, int yTick)
        {
            lock (obj)
                obj.BatchMoveXY(xTick, yTick);
        }

        public void BatchMoveZ(int zTick)
        {
            lock (obj)
                obj.BatchMoveZ(zTick);
        }

        public void BatchReset()
        {
            lock (obj)
                obj.BatchReset();
        }

        public void StylusReset()
        {
            lock (obj)
                obj.StylusReset();
        }

        public void BatchSetSpeed(Point3 point)
        {
            lock (obj)
                obj.BatchSetSpeed(point);
        }

        public object GetSystemInfo()
        {
            lock (obj)
                return obj.GetSystemInfo();
        }

        public Version HardVersion()
        {
            lock (obj)
                return obj.HardVersion();
        }

        public int Ping()
        {
            lock (obj)
                return obj.Ping();
        }

        public byte[] ReadFlash(int index)
        {
            lock (obj)
                return obj.ReadFlash(index);
        }

        public void ResetDevice()
        {
            lock (obj)
                obj.ResetDevice();
        }

        public void WriteFlash(int index, byte[] data)
        {
            lock (obj)
                obj.WriteFlash(index, data);
        }

        public void Checked(int status)
        {
            lock (obj)
                obj.Checked(status);
        }

        public bool PenVibration(int time)
        {
            lock (obj)
                return obj.PenVibration(time);
        }

        public void SetIndoorLight(bool status)
        {
            lock (obj)
                obj.SetIndoorLight(status);
        }

        public void SetUsbHub(int index, bool status)
        {
            lock (obj)
                obj.SetUsbHub(index, status);
        }

        public void SetUsbMode(int index, bool status)
        {
            lock (obj)
                obj.SetUsbMode(index, status);
        }

        public bool Pressure()
        {
            lock (obj)
                return obj.Pressure();
        }
    }
}
