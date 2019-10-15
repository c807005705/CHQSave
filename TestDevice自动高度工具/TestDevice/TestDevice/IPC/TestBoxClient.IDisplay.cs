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
    partial class TestBoxClient : IDisplay
    {
        public CalculateCoefficient CalculateCoefficient
        {
            get
            {
                lock (obj)
                    return obj.CalculateCoefficient;
            }
            set
            {
                lock (obj)
                    obj.CalculateCoefficient = value;
            }
        }

        public ICamera Camera
        {
            get
            {
                lock (obj)
                    return obj.Camera;
            }
        }

        public bool CameraIsOpen
        {
            get
            {
                lock (obj)
                    return obj.CameraIsOpen;
            }
        }

        public string CameraSerialNumber
        {
            get
            {
                lock (obj)
                    return obj.CameraSerialNumber;
            }
        }

        public CameraTransform CameraTransform
        {
            get
            {
                lock (obj)
                    return obj.CameraTransform;
            }
            set
            {
                lock (obj)
                    obj.CameraTransform = value;
            }
        }

        public CameraTypeEnum CameraType
        {
            get
            {
                lock (obj)
                    return obj.CameraType;
            }
        }

        public Size CurrentResolution
        {
            get
            {
                lock (obj)
                    return obj.CurrentResolution;
            }
        }

        public DatumMarks DatumMarks
        {
            get
            {
                lock (obj)
                    return obj.DatumMarks;
            }
            set
            {
                lock (obj)
                    obj.DatumMarks = value;
            }
        }

        public DisplayInfo DisplayInfo
        {
            get
            {
                lock (obj)
                    return obj.DisplayInfo;
            }
            set
            {
                lock (obj)
                    obj.DisplayInfo = value;
            }
        }

        public double ExposureTime
        {
            get
            {
                lock (obj)
                    return obj.ExposureTime;
            }
            set
            {
                lock (obj)
                    obj.ExposureTime = value;
            }
        }

        public Rectangle OutputWindowBounds
        {
            get
            {
                lock (obj)
                    return obj.OutputWindowBounds;
            }
        }

        public Size2D PixelPerMM
        {
            get
            {
                lock (obj)
                    return obj.PixelPerMM;
            }
        }

        public WorkspaceInfo WorkspaceInfo
        {
            get
            {
                lock (obj)
                    return obj.WorkspaceInfo;
            }
            set
            {
                lock (obj)
                    obj.WorkspaceInfo = value;
            }
        }

        public void MotorToPoint(double x, double y, out int pixelX, out int pixelY)
        {
            lock (obj)
                obj.MotorToPoint(x, y, out pixelX, out pixelY);
        }

        public void PointToMotor(int pixelX, int pixelY, out double x, out double y)
        {
            lock (obj)
                obj.PointToMotor(pixelX, pixelY, out x, out y);
        }

        public void SetOutputWindow(Rectangle bounds)
        {
            lock (obj)
                obj.SetOutputWindow(bounds);
        }
    }
}
