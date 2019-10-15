using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Mobot.TestBox.Protocols.Mtc.Messages
{
    [MessageDesc(MessageID.MOVE_COM_ID, "Move",
       new TYPE_DESC[]{
            TYPE_DESC.Int16,	// The X-coordinate of where to move the stylus, in pixels
            TYPE_DESC.Int16		// The Y-coordinate of where to move the stylus, in pixels
        }, MessageID.MOVE_RESP_ID)]
    public class MoveMessage : MessageBase
    {
        /// <summary>
        /// XY坐标（单位像素）
        /// </summary>
        public Point StylusLocation { get; private set; }
        public MoveMessage()
        {
            this.StylusLocation = Point.Empty;
        }
        public MoveMessage(Point location)
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
            return new object[]{
                (Int16)StylusLocation.X,
                (Int16)StylusLocation.Y
            };
        }
        public override string ToString()
        {
            return string.Format("Move(x={0} y={1})", this.StylusLocation.X, this.StylusLocation.Y);
        }
    }

    [MessageDesc(MessageID.MOVE_RESP_ID, "MoveResponse", null, MessageID.MOVE_COM_ID)]
    public class MoveResponseMessage : SimpleMessage
    { }
}
