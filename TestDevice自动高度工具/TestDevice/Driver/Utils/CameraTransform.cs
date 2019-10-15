// Copyright 2011 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using Mobot.Utils;

namespace Mobot.TestBox
{
    public enum DetectionType
    {
        Unknown = 0,
        MarksDetection,
        EdgeDetection
    }

    /// <summary>
    /// 相机图像变换参数。
    /// </summary>
    public class CameraTransform
    {
        /// <summary>
        /// The center of the box
        /// </summary>
        public PointF Center { get; private set; }

        /// <summary>
        /// The size of the box
        /// </summary>
        public SizeF Size { get; private set; }

        /// <summary>
        /// 旋转角度 RotateAngle，单位为度。
        /// The angle between the horizontal axis and the first side (i.e. width) in degrees.
        /// </summary>
        /// <remarks>Possitive value means counter-clock wise rotation</remarks>
        public float Angle { get; private set; }

        /// <summary>
        /// 相机画面相对手机的朝向。
        /// </summary>
        public CameraDirection Direction { get; private set; }
        /// <summary>
        /// 剪辑的区域。
        /// </summary>
        public Rectangle ClipBounds { get; private set; }
        /// <summary>
        /// 校准方式
        /// </summary>
        public DetectionType DetectionMethod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center">The center of the box.</param>
        /// <param name="size">The size of the box.</param>
        /// <param name="angle">The angle of the box in degrees. Possitive value means counter-clock wise rotation.</param>
        public CameraTransform(PointF center, SizeF size, float angle, CameraDirection direction, DetectionType method)
        {
            this.Center = center;
            this.Size = size;
            this.Angle = angle;
            this.Direction = direction;
            this.ClipBounds = new Rectangle((int)(center.X - size.Width / 2), (int)(center.Y - size.Height / 2), (int)size.Width, (int)size.Height);
            this.DetectionMethod = method;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="center">The center of the box.</param>
        /// <param name="size">The size of the box.</param>
        /// <param name="angle">The angle of the box in degrees. Possitive value means counter-clock wise rotation.</param>
        public CameraTransform(PointF center, SizeF size, float angle, CameraDirection direction, Rectangle clipBounds, DetectionType method)
        {
            this.Center = center;
            this.Size = size;
            this.Angle = angle;
            this.Direction = direction;
            this.ClipBounds = clipBounds;
            this.DetectionMethod = method;
        }
    }
}
