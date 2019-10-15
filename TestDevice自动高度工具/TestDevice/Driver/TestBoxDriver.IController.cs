using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Mobot;
using TestDevice;
using Motor;

namespace Driver
{
    partial class TestBoxDriver : IController
    {
        private bool _reset = true;
        public bool IsReset { get { return _reset; } set { _reset = value; } }
        private bool _iContinue = true;
        public bool IContinue { get { return _iContinue; } set { _iContinue = value; } }
        Dictionary<string, MobileKeyInfo> dicKeys = new Dictionary<string, MobileKeyInfo>();
        public Dictionary<string, MobileKeyInfo> Keys { get { return dicKeys; } }
        bool debugMode = true;
        /// <summary>
        /// 异步执行
        /// </summary>
        public bool DebugMode
        {
            get { return this.debugMode; }
            set { this.debugMode = value; }
        }
        Point3 motorSpeed = Point3.Empty;
        /// <summary>
        /// 获取或设置电机速度。
        /// </summary>
        public Point3 MotorSpeed
        {
            get
            {
                motorSpeed = driver.Speed;
                return motorSpeed;
            }
            set
            {
                motorSpeed = value;
                if (motorSpeed == Point3.Empty)
                    motorSpeed = MotorDriver.NormalSpeed;
                if (driver.IsOpen)
                {
                    try
                    {
                        driver.BatchSetSpeed(motorSpeed);
                        while (driver.IsBusy)
                            Application.DoEvents();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
        }
        /// <summary>
        /// 连续点击Z轴归位高度
        /// </summary>
        public double? QuickPointZ { get; set; }
        /// <summary>
        /// 触屏点击高度
        /// </summary>
        public double? TouchPadClickZ { get; set; }

        private Point2 normalLocation;
        private Point2D? _touchReset;
        /// <summary>
        /// 归位坐标
        /// </summary>
        public Point2D? TouchReset
        {
            get { return _touchReset; }
            set
            {
                _touchReset = value;
                int x = 0, y = 0;
                if (TouchReset != null && TouchReset.Value != Point2D.Empty)
                {
                    x = settings.MM2Ticks(TouchReset.Value.X, Axis.X);
                    y = settings.MM2Ticks(TouchReset.Value.Y, Axis.Y);
                }
                normalLocation = new Point2(x, y);
            }
        }
        public int DelayBeforeReset { get; set; }

        private ControllerSettings settings = null;
        public IControllerSettings ControllerSettings
        {
            get { return this.settings; }
        }
        public double StylusLocationX
        {
            get { return settings.Ticks2MM(driver.Ticks.X, Axis.X); }
        }
        public double StylusLocationY
        {
            get { return settings.Ticks2MM(driver.Ticks.Y, Axis.Y); }
        }
        public double StylusLocationZ
        {
            get { return settings.Ticks2MM(driver.Ticks.Z, Axis.Z); }
        }
        /// <summary>
        /// 机械手触笔的位置(毫米)。
        /// </summary>
        public Point3D StylusLocation
        {
            get
            {
                return new Point3D(this.StylusLocationX, this.StylusLocationY, this.StylusLocationZ);
            }
        }
        /// <summary>
        /// 将配置重置为测试盒的Flash配置。测试盒必须处于连接状态。
        /// </summary>
        public void Reset()
        {
            if (!driver.IsOpen)
                throw new Exception("连接设备后才可以获取设备配置。");

            FlashStream stream = null;
            TestBoxMemory menory = null;
            try
            {//存储配置
                stream = new FlashStream(this);
                menory = new TestBoxMemory(stream);
            }
            finally
            {
                if (settings != null)
                {
                    try
                    {
                        menory.Save();
                    }
                    finally
                    {
                        //settings = null;
                    }
                }
                if (stream != null)
                {
                    try
                    {
                        stream.Close();
                    }
                    finally
                    {
                        stream = null;
                    }
                }
            }
        }
        internal void Import(TestBoxMemory settings)
        {
            this.DisplayInfo = settings.DisplayInfo;
            this.WorkspaceInfo = settings.WorkspaceInfo;
            this.DatumMarks = settings.DatumMarks;
            this.CalculateCoefficient = settings.CalculateCoefficient;
        }
    }
}
