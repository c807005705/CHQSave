using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;
using Mobot;

namespace Motor
{
    [MessageDesc(MID.Move, "Move",
        new TYPE_DESC[]{
            TYPE_DESC.UInt16,		// The X-coordinate of where to move the stylus, in pixels
            TYPE_DESC.UInt16,		// The Y-coordinate of where to move the stylus, in pixels
            TYPE_DESC.UInt16,		// The Z-coordinate of where to move the stylus, in pixels
            TYPE_DESC.UInt8,		// 0x00，表示该移动为正常移动，不做其他处理。
                                    // 0x01，表示该移动需要在压力传感器压力增大要阀值触发拍照，即点击接触触发拍照。
                                    // 0x02，表示该移动需要在移动开始触发拍照，即滑动开始触发拍照。
                                    // 0x03，表示该移动需要在移动到指定位置时触发拍照，即移动停止触发拍照。
        }, MID.MoveResponse)]
    public class MoveMessage : MessageBase
    {
        /// <summary>
        /// 控制，即不移动该轴。
        /// </summary>
        const UInt16 NullValue = 0xFF00;
        /// <summary>
        /// XYZ坐标（单位刻度）
        /// </summary>
        public Point3 StylusLocation { get; private set; }
        public int Trigger { get; set; }
        public MoveMessage()
        {
            this.StylusLocation = Point3.Empty;
        }
        public MoveMessage(int xTick, int yTick, int zTick)
        {
            this.StylusLocation = new Point3(xTick, yTick, zTick);
        }
        public MoveMessage(int xTick, int yTick)
            : this(xTick, yTick, NullValue)
        { }
        public MoveMessage(int zTick)
            : this(NullValue, NullValue, zTick)
        { }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            UInt16 STYLUS_X = (UInt16)RawData[offset++];
            UInt16 STYLUS_Y = (UInt16)RawData[offset++];
            UInt16 STYLUS_Z = (UInt16)RawData[offset++];
            Byte status = (Byte)RawData[offset++];

            this.StylusLocation = new Point3(STYLUS_X, STYLUS_Y, STYLUS_Z);
            this.Trigger = status;
        }
        public override object[] ToRawData()
        {
            if (Trigger == 0)
            {
                return new object[] {
                    (UInt16)StylusLocation.X,
                    (UInt16)StylusLocation.Y,
                    (UInt16)StylusLocation.Z,
                };
            }
            else
            {
                return new object[] {
                    (UInt16)StylusLocation.X,
                    (UInt16)StylusLocation.Y,
                    (UInt16)StylusLocation.Z,
                    (Byte)Trigger,
                };
            }
        }
        public override string ToString()
        {
            List<string> args = new List<string>();
            if (StylusLocation.X != NullValue)
                args.Add("x=" + StylusLocation.X);
            if (StylusLocation.Y != NullValue)
                args.Add("y=" + StylusLocation.Y);
            if (StylusLocation.Z != NullValue)
                args.Add("z=" + StylusLocation.Z);
            if (Trigger != 0)
                args.Add("T=" + Trigger);

            return "Move(" + string.Join(", ", args.ToArray()) + ")";
        }
    }
    [MessageDesc(MID.MoveResponse, "MoveResponse", null, MID.Move)]
    public class MoveResponseMessage : SimpleMessage
    { }
}
