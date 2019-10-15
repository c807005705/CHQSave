using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TestDevice
{
    /// <summary>
    /// 测试盒三轴运动接口
    /// 触屏坐标系
    /// </summary>
    public interface ITouch
    {
        /// <summary>
        /// 移动(同步)
        /// </summary>
        void TouchMove(int pixelX, int pixelY);
        /// <summary>
        /// 点击(同步)
        /// </summary>
        void TouchClick(int pixelX, int pixelY, int duration = 100);
        /// <summary>
        /// 拖动(同步)
        /// </summary>
        void TouchDrag(Point start, Point end, int duration = 100);
        /// <summary>
        /// 轻扫
        /// </summary>
        void Scavenging(Point start, Point end, int duration = 100);
        /// <summary>
        /// 获取坐标系数
        /// </summary>
        /// <param name="rateWidth">返回width系数</param>
        /// <param name="rateHeight">返回height系数</param>
        void TestBoxPosition(out double rateWidth, out double rateHeight);
        /// <summary>
        /// 左上
        /// </summary>
        PointF LeftUpPoint { get; set; }
        /// <summary>
        /// 右下
        /// </summary>
        PointF RigntDownPoint { get; set; }
        /// <summary>
        /// 屏幕分辨率
        /// </summary>
        Size ImageSize { get; set; }
    }
}
