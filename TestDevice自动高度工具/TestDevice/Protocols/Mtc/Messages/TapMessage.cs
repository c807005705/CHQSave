using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Mobot.TestBox.Protocols.Mtc.Messages
{
    [MessageDesc(MessageID.TAP_COM_ID, "Tap",
        new TYPE_DESC[]{
            TYPE_DESC.Int16,		//TAP_X: The X-coordinate of where to tap the stylus, in pixels
            TYPE_DESC.Int16,		//TAP_Y: The Y-coordinate of where to tap the stylus, in pixels
            TYPE_DESC.UInt16,		//NUMBER: The number of times to tap
            TYPE_DESC.UInt32,		//TAP_DOWN_DELAY: The number of milliseconds to wait between stylus down and stylus up
            TYPE_DESC.UInt32		//TAP_DELAY: The number of milliseconds to wait between stylus up and stylus down
        }, MessageID.TAP_RESP_ID)]
    public class TapMessage : MessageBase
    {
        /// <summary>
        /// XY坐标（单位像素）
        /// </summary>
        public Point TapLocation { get; private set; }
        /// <summary>
        /// Tap次数
        /// </summary>
        public UInt16 Number { get; private set; }
        /// <summary>
        /// Tap时间间隔（UpToDown）
        /// </summary>
        public UInt32 TapDownDelay { get; private set; }
        /// <summary>
        /// Tap时间间隔（DownToUp）
        /// </summary>
        public UInt32 TapDelay { get; private set; }

        public TapMessage()
        {
            this.TapLocation = Point.Empty;
            this.Number = 0;
            this.TapDownDelay = 0;
            this.TapDelay = 0;
        }
        public TapMessage(Point location, UInt16 number, UInt32 downDelay, UInt32 delay)
        {
            this.TapLocation = location;
            this.Number = number;
            this.TapDownDelay = downDelay;
            this.TapDelay = delay;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            Int16 TAP_X = (Int16)RawData[offset++];
            Int16 TAP_Y = (Int16)RawData[offset++];
            this.Number = (UInt16)RawData[offset++];
            this.TapDownDelay = (UInt32)RawData[offset++];
            this.TapDelay = (UInt32)RawData[offset++];

            this.TapLocation = new Point(TAP_X, TAP_Y);
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (Int16)TapLocation.X,
                (Int16)TapLocation.Y,
                (UInt16)Number,
                (UInt32)TapDownDelay,
                (UInt32)TapDelay,
            };
        }
        public override string ToString()
        {
            return string.Format("Tap(x={0}, y={1}, clicks={2}, delay={3}, interval={4})",
                this.TapLocation.X,
                this.TapLocation.Y,
                this.Number,
                this.TapDownDelay,
                this.TapDelay);
        }
    }
    [MessageDesc(MessageID.TAP_RESP_ID, "TapResponse", null, MessageID.TAP_COM_ID)]
    public class TapResponseMessage : SimpleMessage
    { }
}
