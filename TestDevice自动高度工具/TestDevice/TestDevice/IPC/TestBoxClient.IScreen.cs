using Mobot.Utils.Caches;
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
    partial class TestBoxClient : IScreen
    {
        public RawImage CurrentImage
        {
            get
            {
                lock (obj)
                    return obj.CurrentImage;
            }
        }

        public Bitmap CurrentScreen
        {
            get
            {
                lock (obj)
                    return obj.CurrentScreen;
            }
        }

        public int RefreshInterval
        {
            get
            {
                lock (obj)
                    return obj.RefreshInterval;
            }
            set
            {
                lock (obj)
                    obj.RefreshInterval = value;
            }
        }

        public Size Size
        {
            get
            {
                lock (obj)
                    return obj.Size;
            }
            set
            {
                lock (obj)
                    obj.Size = value;
            }
        }

        public event EventHandler ScreenChanged;

        public bool CameraFocus(out Bitmap bitmap)
        {
            bool result;
            RawImage temp;
            lock (obj)
                result = obj.CameraFocus(out temp);
            bitmap = temp.Image;
            return result;
        }
        public bool CameraFocus(out RawImage bitmap)
        {
            lock (obj)
                return obj.CameraFocus(out bitmap);
        }

        public bool CameraFocus(DisplayInfo displayInfo, DatumMarks datumMarks, out Bitmap bitmap)
        {
            bool result;
            RawImage temp;
            lock (obj)
                result = obj.CameraFocus(displayInfo, datumMarks, out temp);
            bitmap = temp.Image;
            return result;
        }
        public bool CameraFocus(DisplayInfo displayInfo, DatumMarks datumMarks, out RawImage bitmap)
        {
            lock (obj)
                return obj.CameraFocus(displayInfo, datumMarks, out bitmap);
        }

        public RawImage GetCurrentImage()
        {
            lock (obj)
                return obj.GetCurrentImage();
        }

        public RawImage GetCurrentImage(bool onlyOkey, bool gridVisible1, bool gridVisible2, bool marksVisible)
        {
            lock (obj)
                return obj.GetCurrentImage(onlyOkey, gridVisible1, gridVisible2, marksVisible);
        }

        public Bitmap GetCurrentScreen()
        {
            lock (obj)
                return obj.GetCurrentScreen();
        }

        public Bitmap GetCurrentScreen(bool onlyOkey, bool gridVisible1, bool gridVisible2, bool marksVisible)
        {
            lock (obj)
                return obj.GetCurrentScreen(onlyOkey, gridVisible1, gridVisible2, marksVisible);
        }

        public RawImage GetOriginalImage()
        {
            lock (obj)
                return obj.GetOriginalImage();
        }

        public Bitmap GetOriginalScreen()
        {
            lock (obj)
                return obj.GetOriginalScreen();
        }

        public bool RefreshScreen()
        {
            lock (obj)
                return obj.RefreshScreen();
        }
    }
}
