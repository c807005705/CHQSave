using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mobot.Utils;
using System.Threading;
using System.Drawing;
using TestDevice;
using Motor;

namespace Driver
{
    partial class TestBoxDriver : ITouch
    {
        /// <summary>
        /// 左上
        /// </summary>
        public PointF LeftUpPoint { get; set; }
        /// <summary>
        /// 右下
        /// </summary>
        public PointF RigntDownPoint { get; set; }
        /// <summary>
        /// 屏幕分辨率
        /// </summary>
        public Size ImageSize { get; set; }
        /// <summary>
        /// 移动（触屏坐标系）
        /// </summary>
        public void TouchMove(int pixelX, int pixelY)
        {
            double x, y;
            this.PointToMotor(pixelX, pixelY, out x, out y);
            this.StylusMoveXY(x, y);
        }
        /// <summary>
        /// 移动（机械坐标系）
        /// </summary>
        public void TouchMoveZ(double? touchZ = null)
        {
            if (touchZ == null)
                touchZ = TouchPadClickZ == null ? StylusLocationZ : TouchPadClickZ.Value;
            this.StylusMoveZ(touchZ ?? 0);
        }
        /// <summary>
        /// 点击（触屏坐标系）
        /// </summary>
        public void TouchClick(int pixelX, int pixelY, int duration = 100)
        {
            double touchZ = TouchPadClickZ == null ? StylusLocationZ : TouchPadClickZ.Value;
            double x, y;
            this.PointToMotor(pixelX, pixelY, out x, out y);
            this.StylusMoveXY(x, y);
            this.StylusMoveZ(touchZ);
            Thread.Sleep(duration);
            touchZ = this.QuickPointZ != null ? this.QuickPointZ.Value : 50;//连续点击高度
            this.StylusMoveZ(touchZ);
        }
        /// <summary>
        /// 点击（触屏坐标系）
        /// </summary>
        public void TouchDrag(Point start, Point end, int duration = 100)
        {
            double touchZ = TouchPadClickZ == null ? StylusLocationZ : TouchPadClickZ.Value;
            this.StylusMoveXY(start.X, start.Y);
            this.StylusMoveZ(touchZ);
            Thread.Sleep(duration);
            this.StylusMoveXY(end.X, end.Y);
        }

        public void Scavenging(Point start, Point end, int duration = 100)
        {
            List<IMotorAction> list = new List<IMotorAction>();
            double x, y, x1, y1;
            this.PointToMotor(start.X, start.Y, out x, out y);
            this.PointToMotor(end.X, end.Y, out x1, out y1);
            list.Add(new BatchMoveXY(x, y));
            list.Add(new BatchMoveZ(TouchPadClickZ ?? 50));
            list.Add(new BatchMove(x1, y1, TouchPadClickZ ?? 50));
            list.Add(new BatchMoveZ(QuickPointZ ?? 50));
            this.BatchActionsAsync(list);
        }
    }
}
