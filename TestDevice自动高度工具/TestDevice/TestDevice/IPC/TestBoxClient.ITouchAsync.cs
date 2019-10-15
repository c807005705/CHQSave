using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;
using System.Text;

namespace Mobot.TestBox
{
    partial class TestBoxClient : ITouchAsync
    {
        public void KeyPressAsync(string key, int duration = 0)
        {
            lock (obj)
                obj.KeyPressAsync(key, duration);
        }

        public void KeyPressAsync(List<MobileKeyInfo> list, int clicks, int duration)
        {
            lock (obj)
                obj.KeyPressAsync(list, clicks, duration);
        }

        public void KeyPressAsync(MobileKeyInfo keyInfo, int clicks, int duration)
        {
            lock (obj)
                obj.KeyPressAsync(keyInfo, clicks, duration);
        }

        public void KeyPressAsync(double x, double y, double z, int clicks, int duration)
        {
            lock (obj)
                obj.KeyPressAsync(x, y, z, clicks, duration);
        }

        public void TouchClickAsync(int pixelX, int pixelY, int duration = 100)
        {
            lock (obj)
                obj.TouchClickAsync(pixelX, pixelY, duration);
        }

        public void TouchClickAsync(double touchZ, int pixelX, int pixelY, int clicks, int duration = 100)
        {
            lock (obj)
                obj.TouchClickAsync(touchZ, pixelX, pixelY, clicks, duration);
        }

        public void TouchDragAsync(double touchZ, IList<Point> points, int duration)
        {
            lock (obj)
                obj.TouchDragAsync(touchZ, points, duration);
        }

        public void TouchDragAsync(Point start, Point end, int duration = 100)
        {
            lock (obj)
                obj.TouchDragAsync(start, end, duration);
        }
        public void TouchMoveAsync(int pixelX, int pixelY)
        {
            lock (obj)
                obj.TouchMoveAsync(pixelX, pixelY);
        }

        public void TouchMoveZAsync(double? touchZ = default(double?))
        {
            lock (obj)
                obj.TouchMoveZAsync(touchZ);
        }
    }
}
