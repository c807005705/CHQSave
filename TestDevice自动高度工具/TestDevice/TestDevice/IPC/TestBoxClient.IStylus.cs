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
    partial class TestBoxClient : IStylus
    {
        public void StylusMoveSafe()
        {
            lock (obj)
                obj.StylusMoveSafe();
        }

        public void StylusMoveXY(double x, double y)
        {
            lock (obj)
                obj.StylusMoveXY(x, y);
        }

        public void StylusMoveXYZ(double x, double y, double z)
        {
            lock (obj)
                obj.StylusMoveXYZ(x, y, z);
        }

        public void StylusMoveZ(double z)
        {
            lock (obj)
                obj.StylusMoveZ(z);
        }

        public void StylusPress(MobileKeyInfo keyInfo, int duration)
        {
            lock (obj)
                obj.StylusPress(keyInfo, duration);
        }
    }
}
