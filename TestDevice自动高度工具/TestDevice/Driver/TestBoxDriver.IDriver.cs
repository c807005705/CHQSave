// Copyright 2011 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using Mobot.Utils.IO;
using Motor;
using TestDevice;
using Mobot;

namespace Driver
{
    partial class TestBoxDriver : IDriver
    {
        readonly MotorDriver driver = null;
        public object Driver { get { return this.driver; } }

        public event EventHandler<SerialPortIOEventArgs> IO
        {
            add { driver.IO += value; }
            remove { driver.IO -= value; }
        }
        public Version HardwareVersion { get { return driver.HardwareVersion; } }
        public UInt16 FlashPageSize { get { return driver.FlashPageSize; } }
        public UInt16 FlashPageCount { get { return driver.FlashPageCount; } }
        public Point3 Ticks { get { return driver.Ticks; } }
        public ModeType ModeType { get { return driver.ModeType; } set { driver.ModeType = value; } }
        public int Trigger
        {
            get { return driver.Trigger; }
            set { driver.Trigger = value; }
        }
        public int BaudRate
        {
            get
            {
                return driver.BaudRate;
            }
            set
            {
                driver.BaudRate = value;
            }
        }
        public Version Reversion { get { return driver.Reversion; } }
        public int Ping()
        {
            return driver.Ping();
        }
        public object GetSystemInfo()
        {
            return driver.GetSystemInfo();
        }
        public void ResetDevice()
        {
            driver.ResetDevice();
        }
        public void BatchReset()
        {
            if (TouchReset != null)
                driver.BatchReset2(normalLocation);
            else
                driver.BatchReset();
        }
        public void StylusReset()
        {
            driver.BatchReset();
        }
        public void BatchMoveXY(int xTick, int yTick)
        {
            driver.BatchMoveXY(xTick, yTick);
        }
        public void BatchMoveZ(int zTick)
        {
            driver.BatchMoveZ(zTick);
        }
        public void BatchSetSpeed(Point3 point)
        {
            driver.BatchSetSpeed(point);
        }
        public byte[] ReadFlash(int index)
        {
            return driver.ReadFlash(index);
        }
        public void WriteFlash(int index, byte[] data)
        {
            driver.WriteFlash(index, data);
        }
        public Version HardVersion()
        {
            return driver.HardwareVersion;
        }

        #region 扩展命令
        /// <summary>
        /// 设置内部光源
        /// </summary>
        public void SetIndoorLight(bool status)
        {
            driver.SetIndoorLight(status);
        }
        /// <summary>
        /// USBHUB手机充电使能或关闭
        /// </summary>
        public void SetUsbHub(int index, bool status)
        {
            driver.SetUsbHub(index, status);
        }
        /// <summary>
        /// USB充电模式选择命令
        /// </summary>
        public void SetUsbMode(int index, bool status)
        {
            driver.SetUsbMode(index, status);
        }
        /// <summary>
        /// 压力检测
        /// </summary>
        public bool Pressure()
        {
            return driver.Pressure();
        }
        /// <summary>
        /// 停止当前运行
        /// </summary>
        public void ModeStop()
        {
            driver.ModeStop();
        }

        #endregion
    }
}
