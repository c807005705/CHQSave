using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Mobot.TestBox.Protocols.Mtc.Messages
{
    [MessageDesc(MessageID.STYLUS_DOWN_COM_ID, "StylusDown",
       new TYPE_DESC[]{
            TYPE_DESC.Int16,		//STYLUS_X The X-coordinate of where to move the stylus before pressing down, in pixels
            TYPE_DESC.Int16		//STYLUS_Y The Y-coordinate of where to move the stylus before pressing down, in pixels
        }, MessageID.STYLUS_DOWN_RESP_ID)]
    public class StylusDownMessage : MessageBase
    {
        /// <summary>
        /// XY坐标（单位像素）
        /// </summary>
        public Point StylusLocation { get; private set; }
        public StylusDownMessage()
        {
            this.StylusLocation = Point.Empty;
        }
        public StylusDownMessage(Point location)
        {
            this.StylusLocation = location;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            Int16 STYLUS_X = (Int16)RawData[offset++];
            Int16 STYLUS_Y = (Int16)RawData[offset++];

            this.StylusLocation = new Point(STYLUS_X, STYLUS_Y);
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (UInt16)StylusLocation.X,
                (UInt16)StylusLocation.Y,
            };
        }
        public override string ToString()
        {
            return string.Format("StylusDown(x={0}, y={1})", this.StylusLocation.X, this.StylusLocation.Y);
        }
    }
    [MessageDesc(MessageID.STYLUS_DOWN_RESP_ID, "StylusDownResponse", null, MessageID.STYLUS_DOWN_COM_ID)]
    public class StylusDownResponseMessage : SimpleMessage
    { }
}
