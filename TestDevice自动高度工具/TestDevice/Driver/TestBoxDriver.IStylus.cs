// Copyright 2011 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Motor;
using System.Threading;
using TestDevice;
using Mobot;

namespace Driver
{
    partial class TestBoxDriver : IStylus
    {
        /// <summary>
        /// 平面移动到某位置（机械坐标系）
        /// </summary>
        public void StylusMoveXY(double x, double y)
        {
            //int xTick = settings.MM2Ticks(x, Axis.X);
            //int yTick = settings.MM2Ticks(y, Axis.Y);
            this.driver.MoveXY((int)x, (int)y);
        }
        /// <summary>
        /// 垂直移动到指定位置（机械坐标系）
        /// </summary>
        public void StylusMoveZ(double z)
        {
            //int zTick = settings.MM2Ticks(z, Axis.Z);
            this.driver.MoveZ((int)z);
        }
        public void StylusMoveSafe()
        {
            this.driver.MoveZ(50);
        }
        public void StylusMoveXYZ(double x, double y, double z)
        {
            //int xTick = settings.MM2Ticks(x, Axis.X);
            //int yTick = settings.MM2Ticks(y, Axis.Y);
            //int zTick = settings.MM2Ticks(z, Axis.Z);
            this.driver.MoveXYZ((int)x, (int)y, (int)z);
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

        int IStylus.StylusStatue()
        {
            return this.driver.StylusStatue();
        }

        public void StylusRestZ()
        {
            this.driver.StylusRestZ();
        }
    }
}
