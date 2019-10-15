using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TestDevice
{
    /// <summary>
    /// 测试盒三轴运动接口
    /// </summary>
    public interface ITouchAsync
    {
        /// <summary>
        /// 移动(异步)
        /// </summary>
        void TouchMoveAsync(int pixelX, int pixelY);
        void TouchMoveZAsync(double? touchZ = null);
        /// <summary>
        /// 点击(异步)
        /// </summary>
        void TouchClickAsync(int pixelX, int pixelY, int duration = 100);
        void TouchClickAsync(double touchZ, int pixelX, int pixelY, int clicks, int duration = 100);
        /// <summary>
        /// 拖动(异步)
        /// </summary>
        void TouchDragAsync(Point start, Point end, int duration = 100);
        void TouchDragAsync(double touchZ, IList<Point> points, int duration);
        /// <summary>
        /// 按键(异步)
        /// </summary>
        void KeyPressAsync(string key, int duration = 0);
        void KeyPressAsync(double x, double y, double z, int clicks, int duration);
        void KeyPressAsync(MobileKeyInfo keyInfo, int clicks, int duration);
        void KeyPressAsync(List<MobileKeyInfo> list, int clicks, int duration);
    }
}
