// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using Mobot.TestBox.Protocols.Motor;
using System.Threading;

namespace Mobot.TestBox.Motor
{
    partial class Controller
    {
        public MotorDriver Driver
        {
            get { return driver; }
        }
        bool debugMode = true;
        /// <summary>
        /// 异步执行
        /// </summary>
        public bool DebugMode
        {
            get { return this.debugMode; }
            set { this.debugMode = value; }
        }

        private int delayBeforeReset = 0;
        /// <summary>
        ///  当队列为空时，测试盒等待一段时间后再Reset。(毫秒）
        /// </summary>
        public int DelayBeforeReset
        {
            get { return this.delayBeforeReset; }
            set
            {
                if (this.delayBeforeReset == value)
                    return;
                this.delayBeforeReset = value;
            }
        }

        /// <summary>
        /// 电机坐标变化时。
        /// </summary>
        public event EventHandler StylusLocationChanged
        {
            add { driver.LocationChanged += value; }
            remove { driver.LocationChanged -= value; }
        }

        public event EventHandler StateChanged
        {
            add { driver.StateChanged += value; }
            remove { driver.StateChanged -= value; }
        }

        /// <summary>
        /// 等待按的键。
        /// </summary>
        Queue<ActionBatch> queueKeys = new Queue<ActionBatch>();
        BackgroundWorker bw = null;

        #region 异步方法
        /// <summary>
        /// 重置电机
        /// </summary>
        public void StylusReset()
        {
            driver.BatchReset();
        }
        /// <summary>
        /// 异步按键。
        /// </summary>
        public void KeyPressAsync(double x, double y, double z, int clicks, int duration)
        {
            StylusPressAsync(new MobileKeyInfo(x, y, z), clicks, duration);
        }
        public void StylusPressAsync(MobileKeyInfo keyInfo, int clicks, int duration)
        {
            List<IMotorAction> list = new List<IMotorAction>();
            List<IMotorAction> temp = StylusPressList(keyInfo, duration);
            for (int i = 0; i < clicks; i++)
            {
                list.AddRange(temp);
            }
            BatchActionsAsync(list);
        }
        public List<IMotorAction> StylusPressList(MobileKeyInfo keyInfo, int duration)
        {
            //KeyPress 动作
            MobileKeyPressArgs pressArgs = keyInfo.PressArgs;

            //按下的位置
            Point3D locationPressed = new Point3D(
                keyInfo.Location.X + ((pressArgs.Axis == Axis.X) ? pressArgs.Depth : 0),
                keyInfo.Location.Y + ((pressArgs.Axis == Axis.Y) ? pressArgs.Depth : 0),
                keyInfo.Location.Z + ((pressArgs.Axis == Axis.Z) ? pressArgs.Depth : 0));

            //带有移动速度的按键
            bool isDepthPress = (
                pressArgs.Speed > 0
                && pressArgs.Depth != 0
                && HardwareVersion.Major != 1); //一代测试盒不支持批处理中改变速度

            //未按下的位置
            Point3D locationNotPressed = keyInfo.Location;
            Point3D locationInnerPressed = keyInfo.Location;
            locationInnerPressed.Z = (this.QuickPointZ != null) ? this.QuickPointZ.Value : 50;//连续点击高度

            List<IMotorAction> list = new List<IMotorAction>();


            //XY移动到目标位置，Z到初始高度或安全高度。未按下状态
            list.Add(new BatchMoveXY(locationNotPressed.X, locationNotPressed.Y));

            if (keyInfo.PressArgs.Axis == Axis.Z)
            {
                if (isDepthPress)
                {
                    Point3 speed = MoveSpeed(pressArgs);
                    list.Add(new BatchMotorSpeed(speed));
                }
                //按下但不抬起
                list.Add(new BatchMoveZ(locationPressed.Z));
                list.Add(new BatchSleep(duration));

                if (isDepthPress)
                {
                    list.Add(new BatchMotorSpeedReset());
                }
            }
            else
            {
                //按下回原xy位但不抬起
                list.Add(new BatchMoveZ(locationNotPressed.Z));

                if (locationNotPressed.X != locationPressed.X || locationNotPressed.Y != locationPressed.Y)
                {
                    if (isDepthPress)
                    {
                        Point3 speed = MoveSpeed(pressArgs);
                        list.Add(new BatchMotorSpeed(speed));
                    }
                    list.Add(new BatchMoveXY(locationPressed.X, locationPressed.Y));
                    list.Add(new BatchSleep(duration));
                    list.Add(new BatchMoveXY(locationNotPressed.X, locationNotPressed.Y));

                    if (isDepthPress)
                    {
                        list.Add(new BatchMotorSpeedReset());
                    }
                }
                else
                {
                    list.Add(new BatchSleep(duration));
                }
            }

            if (this.QuickPointZ == null)
                list.Add(new BatchMoveSafeZ());
            else
                list.Add(new BatchMoveZ(locationInnerPressed.Z));

            return list;
        }
        private Point3 MoveSpeed(MobileKeyPressArgs e)
        {
            Point3 speed = new Point3(MotorSpeed.X, MotorSpeed.Y, MotorSpeed.Z);
            switch (e.Axis)
            {
                case Axis.X:
                    speed.X = e.Speed;
                    break;
                case Axis.Y:
                    speed.Y = e.Speed;
                    break;
                case Axis.Z:
                    speed.Z = e.Speed;
                    break;
            }

            return speed;
        }

        /// <summary>
        /// 拖动
        /// </summary>
        public void StylusDragAsync(double touchZ, IList<Point2D> points, int duration)
        {
            try
            {
                if (points.Count < 2) return;

                //未按下的位置
                double zInnerPressed = (this.QuickPointZ != null) ? this.QuickPointZ.Value : 50;//连续点击高度
                Point2D xyPressed = new Point2D(points[0].X, points[0].Y);

                //拖动动作
                List<IMotorAction> list = new List<IMotorAction>();

                //XY移动到目标位置，Z到初始高度或安全高度。未按下状态
                list.Add(new BatchMoveXY(xyPressed.X, xyPressed.Y));

                //第一点
                list.Add(new BatchWorkZ(touchZ));
                list.Add(new BatchSleep(duration));

                //第二点
                list.Add(new BatchMoveXY(points[points.Count - 1].X, points[points.Count - 1].Y));
                //list.Add(new BatchWorkZ(zNotPressed));

                BatchActionsAsync(list);
            }
            catch (MotorException ex)
            {
                log.Error(ex);
                throw ex;

            }
        }
        /// <summary>
        /// 垂直移动到近零位。
        /// </summary>
        /// <param name="z"></param>
        public void StylusMoveSafeAsync()
        {
            this.BatchActions(new BatchMoveSafeZ());
        }
        /// <summary>
        /// 垂直移动到指定位置。
        /// </summary>
        /// <param name="z"></param>
        public void StylusMoveZAsync(double z)
        {
            this.BatchActions(new BatchMoveZ(z));
        }

        #endregion

        #region 同步方法
        /// <summary>
        /// 平面移动到某位置。
        /// </summary>
        /// <param name="x">X坐标(毫米)</param>
        /// <param name="y">Y坐标(毫米)</param>
        public void StylusMoveXY(double x, double y)
        {
            int xTick = settings.MM2Ticks(x, Axis.X);
            int yTick = settings.MM2Ticks(y, Axis.Y);
            this.driver.MoveXY(xTick, yTick);
        }
        /// <summary>
        /// 平面移动到某位置。
        /// </summary>
        /// <param name="x">X坐标(毫米)</param>
        /// <param name="y">Y坐标(毫米)</param>
        public void StylusMoveXYZ(double x, double y, double z)
        {
            int xTick = settings.MM2Ticks(x, Axis.X);
            int yTick = settings.MM2Ticks(y, Axis.Y);
            int zTick = settings.MM2Ticks(z, Axis.Z);
            this.driver.MoveXYZ(xTick, yTick, zTick);
        }
        /// <summary>
        /// 垂直移动到近零位。
        /// </summary>
        public void StylusMoveSafe()
        {
            this.driver.MoveZ(50);
        }
        /// <summary>
        /// 垂直移动到指定位置。
        /// </summary>
        /// <param name="z"></param>
        public void StylusMoveZ(double z)
        {
            int zTick = settings.MM2Ticks(z, Axis.Z);
            this.driver.MoveZ(zTick);
        }
        public void StylusPress(MobileKeyInfo keyInfo, int duration)
        {
            //KeyPress 动作
            MobileKeyPressArgs pressArgs = keyInfo.PressArgs;

            //按下的位置
            Point3D locationPressed = new Point3D(
                keyInfo.Location.X + ((pressArgs.Axis == Axis.X) ? pressArgs.Depth : 0),
                keyInfo.Location.Y + ((pressArgs.Axis == Axis.Y) ? pressArgs.Depth : 0),
                keyInfo.Location.Z + ((pressArgs.Axis == Axis.Z) ? pressArgs.Depth : 0));

            //未按下的位置
            Point3D locationNotPressed = keyInfo.Location;
            Point3D locationInnerPressed = keyInfo.Location;
            locationInnerPressed.Z = (this.QuickPointZ != null) ? this.QuickPointZ.Value : 50;//连续点击高度

            //带有移动速度的按键
            bool isDepthPress = (
                pressArgs.Speed > 0
                && pressArgs.Depth != 0
                && HardwareVersion.Major != 1); //一代测试盒不支持批处理中改变速度

            //XY移动到目标位置，Z到初始高度或安全高度。未按下状态
            StylusMoveXY(locationNotPressed.X, locationNotPressed.Y);


            if (keyInfo.PressArgs.Axis == Axis.Z)
            {
                if (isDepthPress)
                {
                    Point3 speed = MoveSpeed(pressArgs);
                    this.driver.BatchSetSpeed(speed);
                }
                //按下但不抬起
                StylusMoveZ(locationPressed.Z);
                Thread.Sleep(duration);

                if (isDepthPress)
                {
                    this.driver.BatchSetSpeed(MotorSpeed);
                }
            }
            else
            {
                if (locationNotPressed.X != locationPressed.X || locationNotPressed.Y != locationPressed.Y)
                {
                    if (isDepthPress)
                    {
                        Point3 speed = MoveSpeed(pressArgs);
                        this.driver.BatchSetSpeed(speed);
                    }
                    //按下回原xy位但不抬起
                    StylusMoveXYZ(locationPressed.X, locationPressed.Y, locationPressed.Z);
                    StylusMoveXYZ(locationNotPressed.X, locationNotPressed.Y, locationPressed.Z);
                    if (isDepthPress)
                    {
                        this.driver.BatchSetSpeed(MotorSpeed);
                    }
                }
            }

            if (this.QuickPointZ == null)
                StylusMoveSafe();
            else
                StylusMoveZ(locationInnerPressed.Z);
        }

        #endregion
    }
}
