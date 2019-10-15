using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobot;

namespace TestDevice
{
    [Serializable]
    public class ControllerSettings : IControllerSettings
    {
        /// <summary>
        /// 电机移动范围。
        /// </summary>
        Range[] tickRanges = null;
        /// <summary>
        /// 零点坐标处的Tick值(Tick)。
        /// </summary>
        int[] ZeroTicks;
        /// <summary>
        /// X/Y/Z轴一毫米多少个Tick值。
        /// </summary>
        double[] Multiples;
        /// <summary>
        /// 电机速度移动范围。
        /// </summary>
        Range[] MotorSpeedRanges;
        /// <summary>
        /// 电机速度
        /// </summary>
        Point3 MotorSpeeds;

        public ControllerSettings()
        {
            this.Reset();
        }

        public void Reset()
        {
            this.tickRanges = new Range[] {
                new Range(0, 1800),
                new Range(0, 940),
                new Range(0, 470)};
            this.ZeroTicks = new int[3];

            this.Multiples = new double[] { 10.0, 10.0, 10.0 };
            this.MotorSpeedRanges = new Range[] {
                new Range(1, 2000),
                new Range(1, 2000),
                new Range(1, 200)
            };
            this.MotorSpeeds = new Point3(150, 150, 50);
        }

        public void Import(IMotorSettings settings)
        {
            //固定的校正信息
            Range[] ranges = settings.MotorRangeTicks;
            this.tickRanges[(int)Axis.X] = ranges[0];
            this.tickRanges[(int)Axis.Y] = ranges[1];
            this.tickRanges[(int)Axis.Z] = ranges[2];

            Point3 zeroTicks = settings.ZeroTicks;
            this.ZeroTicks = new int[3];
            this.ZeroTicks[(int)Axis.X] = zeroTicks.X;
            this.ZeroTicks[(int)Axis.Y] = zeroTicks.Y;
            this.ZeroTicks[(int)Axis.Z] = zeroTicks.Z;

            Point3D multiples = settings.TicksPerMM;
            this.Multiples = new double[3];
            this.Multiples[(int)Axis.X] = multiples.X;
            this.Multiples[(int)Axis.Y] = multiples.Y;
            this.Multiples[(int)Axis.Z] = multiples.Z;
        }

        /// <summary>
        /// Tick值转换为毫米。Tick2Millimeter
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public double Ticks2MM(int ticks, Axis axis)
        {
            return ((double)(ticks - ZeroTicks[(int)axis])) / Multiples[(int)axis];
        }
        /// <summary>
        /// 毫米转换为Tick值。
        /// </summary>
        /// <param name="mm"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public int MM2Ticks(double mm, Axis axis)
        {
            return (int)(mm * Multiples[(int)axis]) + ZeroTicks[(int)axis];
        }

        public RangeD RangeX
        {
            get
            {
                Range range = tickRanges[(int)Axis.X];
                //double a = Ticks2MM(range.Minimum, Axis.X);
                //double b = Ticks2MM(range.Maximum, Axis.X);
                double a = range.Minimum;
                double b = range.Maximum;
                if (a < b)
                    return new RangeD(a, b);
                return new RangeD(b, a);
            }
        }
        public RangeD RangeY
        {
            get
            {
                Range range = tickRanges[(int)Axis.Y];
                //double a = Ticks2MM(range.Minimum, Axis.Y);
                //double b = Ticks2MM(range.Maximum, Axis.Y);
                double a = range.Minimum;
                double b = range.Maximum;
                if (a < b)
                    return new RangeD(a, b);
                return new RangeD(b, a);
            }
        }
        public RangeD RangeZ
        {
            get
            {
                Range range = tickRanges[(int)Axis.Z];
                //double a = Ticks2MM(range.Minimum, Axis.Z);
                //double b = Ticks2MM(range.Maximum, Axis.Z);
                double a = range.Minimum;
                double b = range.Maximum;
                if (a < b)
                    return new RangeD(a, b);
                return new RangeD(b, a);
            }
        }

        public Range MotorSpeedRangeX
        {
            get
            {
                return this.MotorSpeedRanges[0];
            }
        }
        public Range MotorSpeedRangeY
        {
            get
            {
                return this.MotorSpeedRanges[1];
            }
        }
        public Range MotorSpeedRangeZ
        {
            get
            {
                return this.MotorSpeedRanges[2];
            }
        }
        public Point3 MotorSpeed { get { return this.MotorSpeeds; } }
    }
}
