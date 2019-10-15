using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Mobot;

namespace TestDevice
{
    /// <summary>
    /// 手机按键信息。
    /// </summary>
    [Serializable]
    public struct MobileKeyInfo
    {
        /// <summary>
        /// 要点击的平面区域。
        /// </summary>
        public RectangleD Bounds;

        public Double Z;
        /// <summary>
        /// 笔的位置。
        /// </summary>
        public Point3D Location
        {
            get
            {
                Point2D center = new Point2D(
                    Bounds.X + Bounds.Width / 2.0,
                    Bounds.Y + Bounds.Height / 2.0);
                return new Point3D(center.X, center.Y, this.Z);
            }
        }


        public String Desc;

        public Point3D RLocationA;

        public Point3D RLocationB;


        /// <summary>
        /// 按键按动的参数。
        /// </summary>
        public MobileKeyPressArgs PressArgs;
        public MobileKeyInfo(RectangleD bounds, double z, MobileKeyPressArgs pressArgs, string desc, Point3D rLocationA, Point3D rLocationB)
        {
            this.Bounds = bounds;
            this.Z = z;
            this.PressArgs = pressArgs;
            this.Desc = desc;
            this.RLocationA = rLocationA;
            this.RLocationB = rLocationB;
        }
        public MobileKeyInfo(Point3D location, MobileKeyPressArgs pressArgs, string desc, Point3D rLocationA, Point3D rLocationB)
        {
            this.Bounds = new RectangleD(location.X, location.Y, 0, 0);
            this.Z = location.Z;
            this.PressArgs = pressArgs;
            this.Desc = desc;
            this.RLocationA = rLocationA;
            this.RLocationB = rLocationB;
        }
        public MobileKeyInfo(double x, double y, double z, MobileKeyPressArgs pressArgs, string desc, Point3D rLocationA, Point3D rLocationB)
        {
            this.Bounds = new RectangleD(x, y, 0, 0);
            this.Z = z;
            this.PressArgs = pressArgs;
            this.Desc = desc;
            this.RLocationA = rLocationA;
            this.RLocationB = rLocationB;
        }
        public MobileKeyInfo(double x, double y, double z)
        {
            this.Bounds = new RectangleD(x, y, 0, 0);
            this.Z = z;
            this.PressArgs = new MobileKeyPressArgs(Axis.X, 0, 0);
            this.Desc = string.Empty;
            this.RLocationA = Point3D.Empty;
            this.RLocationB = Point3D.Empty;
        }

        public override string ToString()
        {
            return string.Format("{0}; {1}; {2}; {3}; {4}; {5}", this.Bounds, this.Z, this.PressArgs, this.Desc, this.RLocationA, this.RLocationB);
        }

        /// <summary>
        /// 格式：(0, 0, 100, 50); 10.0; X+0+0; 'desc';(0,0,0,0)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static MobileKeyInfo Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s", "s 不能为空。");
            if (s.Trim() == string.Empty)
                throw new ArgumentException("s为空字符串。", "s");

            string[] parts = s.Split(';');

            RectangleD bounds = RectangleD.Empty;
            Double z = 0.0;
            MobileKeyPressArgs args = MobileKeyPressArgs.Empty;
            Point3D rLocationA = Point3D.Empty;
            Point3D rLocationB = Point3D.Empty;
            String desc = "";
            if (parts.Length > 0)
                bounds = RectangleD.Parse(parts[0]);
            if (parts.Length > 1)
                z = Double.Parse(parts[1]);
            if (parts.Length > 2)
                args = MobileKeyPressArgs.Parse(parts[2]);
            if (parts.Length > 3)
                desc = parts[3];
            if (parts.Length > 4)
                rLocationA = Point3D.Parse(parts[4]);
            if (parts.Length > 5)
                rLocationB = Point3D.Parse(parts[5]);


            return new MobileKeyInfo(bounds, z, args, desc, rLocationA, rLocationB);
        }
    }
}
