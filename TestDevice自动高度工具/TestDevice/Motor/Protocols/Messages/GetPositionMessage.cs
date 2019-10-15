using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;
using Mobot;

namespace Motor
{
    [MessageDesc(MID.GetPosition, "GetPosition", null, MID.GetPositionResponse)]
    public class GetPositionMessage : SimpleMessage
    { }
    [MessageDesc(MID.GetPositionResponse, "GetPositionResponse",
        new TYPE_DESC[] {
            TYPE_DESC.UInt16,		// The X-coordinate of where to move the stylus, in pixels
            TYPE_DESC.UInt16,		// The Y-coordinate of where to move the stylus, in pixels
            TYPE_DESC.UInt16		// The Z-coordinate of where to move the stylus, in pixels
        }, MID.GetPosition)]
    public class GetPositionRespMessage : MessageBase
    {
        public Point3 StylusLocation { get; private set; }
        public GetPositionRespMessage()
        {
            this.StylusLocation = Point3.Empty;
        }
        public GetPositionRespMessage(Point3 location)
        {
            this.StylusLocation = location;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            UInt16 STYLUS_X = (UInt16)RawData[offset++];
            UInt16 STYLUS_Y = (UInt16)RawData[offset++];
            UInt16 STYLUS_Z = (UInt16)RawData[offset++];

            this.StylusLocation = new Point3(STYLUS_X, STYLUS_Y, STYLUS_Z);
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (UInt16)StylusLocation.X,
                (UInt16)StylusLocation.Y,
                (UInt16)StylusLocation.Z,
            };
        }
        public override string ToString()
        {
            return string.Format("GetPositionResponse(x={0}, y={1}, z={2})",
                StylusLocation.X, StylusLocation.Y, StylusLocation.Z);
        }
    }
}
