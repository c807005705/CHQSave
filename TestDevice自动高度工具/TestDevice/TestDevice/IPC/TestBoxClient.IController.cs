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
    partial class TestBoxClient : IController
    {
        public bool CanCapture
        {
            get
            {
                lock (obj)
                    return obj.CanCapture;
            }
        }

        public IControllerSettings ControllerSettings
        {
            get
            {
                lock (obj)
                    return obj.ControllerSettings;
            }
        }

        public bool DebugMode
        {
            get
            {
                lock (obj)
                    return obj.DebugMode;
            }

            set
            {
                lock (obj)
                    obj.DebugMode = value;
            }
        }

        public int DelayBeforeReset
        {
            get
            {
                lock (obj)
                    return obj.DelayBeforeReset;
            }
            set
            {
                lock (obj)
                    obj.DelayBeforeReset = value;
            }
        }

        public bool IsBusy
        {
            get
            {
                lock (obj)
                    return obj.IsBusy;
            }
        }

        public bool IsReset
        {
            get
            {
                lock (obj)
                    return obj.IsReset;
            }
            set
            {
                lock (obj)
                    obj.IsReset = value;
            }
        }

        public Dictionary<string, MobileKeyInfo> Keys
        {
            get
            {
                lock (obj)
                    return obj.Keys;
            }
        }

        public Point3 MotorSpeed
        {
            get
            {
                lock (obj)
                    return obj.MotorSpeed;
            }
            set
            {
                lock (obj)
                    obj.MotorSpeed = value;
            }
        }

        public double? QuickPointZ
        {
            get
            {
                lock (obj)
                    return obj.QuickPointZ;
            }
            set
            {
                lock (obj)
                    obj.QuickPointZ = value;
            }
        }

        public Point3D StylusLocation
        {
            get
            {
                lock (obj)
                    return obj.StylusLocation;
            }
        }

        public double StylusLocationX
        {
            get
            {
                lock (obj)
                    return obj.StylusLocationX;
            }
        }

        public double StylusLocationY
        {
            get
            {
                lock (obj)
                    return obj.StylusLocationY;
            }
        }

        public double StylusLocationZ
        {
            get
            {
                lock (obj)
                    return obj.StylusLocationZ;
            }
        }

        public double? TouchPadClickZ
        {
            get
            {
                lock (obj)
                    return obj.TouchPadClickZ;
            }
            set
            {
                lock (obj)
                    obj.TouchPadClickZ = value;
            }
        }

        public Point2D? TouchReset
        {
            get
            {
                lock (obj)
                    return obj.TouchReset;
            }
            set
            {
                lock (obj)
                    obj.TouchReset = value;
            }
        }

        public ICameraSettings GetCameraSettings()
        {
            lock (obj)
                return obj.GetCameraSettings();
        }

        public void Reset()
        {
            lock (obj)
                obj.Reset();
        }

        public void SetCameraSettings(ICameraSettings value)
        {
            lock (obj)
                obj.SetCameraSettings(value);
        }

        public void SyncCameraSettings(Point2 check)
        {
            lock (obj)
                obj.SyncCameraSettings(check);
        }
    }
}
