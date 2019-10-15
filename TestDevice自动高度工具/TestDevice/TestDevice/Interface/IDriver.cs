using Mobot.Utils.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Mobot;

namespace TestDevice
{
    public interface IDriver
    {
        object Driver { get; }
        ModeType ModeType { get; set; }
        /// <summary>
        /// 控制器固件版本（当连接时后数据有效）
        /// </summary>
        Version HardwareVersion { get; }
        UInt16 FlashPageSize { get; }
        /// <summary>
        /// 配置项片内FLASH页总数（当连接时后数据有效）
        /// </summary>
        UInt16 FlashPageCount { get; }
        Point3 Ticks { get; }
        int Trigger { get; set; }
        int BaudRate { get; set; }
        Version Reversion { get; }
        int Ping();
        object GetSystemInfo();
        void ResetDevice();
        void BatchReset();
        void StylusReset();
        void BatchMoveXY(int xTick, int yTick);
        void BatchMoveZ(int zTick);
        void BatchSetSpeed(Point3 point);
        byte[] ReadFlash(int index);
        void WriteFlash(int index, byte[] data);
        Version HardVersion();

        #region 扩展命令
        void SetIndoorLight(bool status);
        void SetUsbHub(int index, bool status);
        void SetUsbMode(int index, bool status);
        bool Pressure();

        #endregion
    }
}
