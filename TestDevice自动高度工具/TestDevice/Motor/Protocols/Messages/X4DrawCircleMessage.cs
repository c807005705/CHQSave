using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;
using Mobot;

namespace Motor
{
    /// <summary>
    /// 画圆孤
    /// </summary>
    [MessageDesc(MID.DrawCircle, "DrawCircle",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,         // 表示圆弧起始位置正切方向（0x01~0x08）
            TYPE_DESC.UInt8,         // 表示圆弧长度（0x01~0x04）
            TYPE_DESC.UInt16,        // 表示圆弧半径，单位0.1mm
        }, MID.DrawCircleResponse)]
    public class DrawCircleMessage : MessageBase
    {
        public CircleType Type { get; private set; }
        public CircleLength Length { get; private set; }
        public int Radiu { get; private set; }

        public DrawCircleMessage(int type, int length, int radiu)
        {
            this.Type = (CircleType)type;
            this.Length = (CircleLength)length;
            this.Radiu = radiu;
        }

        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Type = (CircleType)RawData[offset++];
            this.Length = (CircleLength)RawData[offset++];
            this.Radiu = (int)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (Byte)Type,
                (Byte)Length,
                (UInt16)Radiu,
            };
        }
        public override string ToString()
        {
            return string.Format("DrawCircle({0},{1},{1})", Type, Length, Radiu);
        }
    }

    [MessageDesc(MID.DrawCircleResponse, "DrawCircleResponse", null, MID.DrawCircle)]
    public class DrawCircleResponseMessage : SimpleMessage { }
    /// <summary>
    /// 圆弧方向
    /// </summary>
    public enum CircleType
    {
        None = 0,
        /// <summary>
        /// 左上(向前正切逆时针)
        /// </summary>
        [StringValue("LeftTop")]
        LeftTop,
        /// <summary>
        /// 右下(向后正切逆时针)
        /// </summary>
        [StringValue("RightBottom")]
        RightBottom,
        /// <summary>
        /// 左下(向左正切逆时针)
        /// </summary>
        [StringValue("LeftBottom")]
        LeftBottom,
        /// <summary>
        /// 右上(向右正切逆时针)
        /// </summary>
        [StringValue("RightTop")]
        RightTop,

        /// <summary>
        /// 上左(向前正切顺时针)
        /// </summary>
        [StringValue("TopRight")]
        TopRight,
        /// <summary>
        /// 下左(向后正切顺时针)
        /// </summary>
        [StringValue("BottomLeft")]
        BottomLeft,
        /// <summary>
        /// 上右(向左正切顺时针)
        /// </summary>
        [StringValue("TopLeft")]
        TopLeft,
        /// <summary>
        /// 下右(向右正切顺时针)
        /// </summary>
        [StringValue("BottomRight")]
        BottomRight,
    }
    /// <summary>
    /// 圆弧长度
    /// </summary>
    public enum CircleLength
    {
        None = 0,
        /// <summary>
        /// 四份之一圆弧
        /// </summary>
        [StringValue("Arc1")]
        One,
        /// <summary>
        /// 四份之二圆弧
        /// </summary>
        [StringValue("Arc2")]
        Two,
        /// <summary>
        /// 四份之三圆弧
        /// </summary>
        [StringValue("Arc3")]
        Three,
        /// <summary>
        /// 完整圆弧
        /// </summary>
        [StringValue("Arc4")]
        Four,
    }
}
