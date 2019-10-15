using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobot;

namespace TestDevice
{
    /// <summary>
    /// 手机按键按动的参数。
    /// </summary>
    [Serializable]
    public struct MobileKeyPressArgs
    {
        public static readonly MobileKeyPressArgs Empty;

        /// <summary>
        /// 按动的方向。
        /// </summary>
        public Axis Axis;
        /// <summary>
        /// 按动的深度。
        /// </summary>
        public double Depth;
        /// <summary>
        /// 移动的速度
        /// </summary>
        public int Speed;
        public MobileKeyPressArgs(Axis axis, double depth, int speed)
        {
            this.Axis = axis;
            this.Depth = depth;
            this.Speed = speed;
        }

        /// <summary>
        /// 格式：X+0+0
        /// </summary>
        public override string ToString()
        {
            return EnumHelper.GetName(typeof(Axis), this.Axis)
                + ((Depth >= 0) ? "+" : "")
                + Depth
                + "+" + Speed;
        }
        /// <summary>
        /// 格式：X+0+0
        /// </summary>
        public static MobileKeyPressArgs Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s", "参数不能为空。");
            }
            s = s.Trim();
            string[] str = s.Split(new char[] { '+', '-' });
            if (str.Length < 2)
            {
                throw new ArgumentException("格式不正确，应该为: X+0+0", "s");
            }

            Axis axis = (Axis)EnumHelper.Parse(typeof(Axis), str[0]);
            double depth = double.Parse(str[1]);
            if (s[1] == '-') depth *= -1;
            int speed = 0;
            if (str.Length == 3)
            {
                speed = int.Parse(str[2]);
            }
            return new MobileKeyPressArgs(axis, depth, speed);
        }
    }
}
