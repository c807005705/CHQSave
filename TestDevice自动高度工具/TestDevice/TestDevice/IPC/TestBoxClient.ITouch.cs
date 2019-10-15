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
    partial class TestBoxClient : ITouch
    {
        public void KeyPress(string key, int duration = 0)
        {
            lock (obj)
                obj.KeyPress(key, duration);
        }
        public void TouchClick(int pixelX, int pixelY, int duration = 100)
        {
            lock (obj)
                obj.TouchClick(pixelX, pixelY, duration);
        }
        public void TouchDrag(Point start, Point end, int duration = 100)
        {
            lock (obj)
                obj.TouchDrag(start, end, duration);
        }
        public void TouchMove(int pixelX, int pixelY)
        {
            lock (obj)
                obj.TouchMove(pixelX, pixelY);
        }
    }
}
