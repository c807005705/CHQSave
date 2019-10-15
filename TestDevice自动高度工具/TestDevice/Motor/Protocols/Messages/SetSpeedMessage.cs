using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;
using Mobot;

namespace Motor
{
    [MessageDesc(MID.SetSpeed, "SetSpeed",
        new TYPE_DESC[] {
                TYPE_DESC.UInt16,		// 电机速度
                TYPE_DESC.UInt16,		// 电机速度
                TYPE_DESC.UInt16,		// 电机速度
        }, MID.SetSpeedResponse)]
    public class SetSpeedMessage : MessageBase
    {
        public Point3 Speed { get; private set; }
        public SetSpeedMessage()
        {
            this.Speed = Point3.Empty;
        }
        public SetSpeedMessage(int x, int y, int z)
            : this(new Point3(x, y, z))
        { }
        public SetSpeedMessage(Point3 speed)
        {
            this.Speed = speed;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            UInt16 SPEED_X = (UInt16)RawData[offset++];
            UInt16 SPEED_Y = (UInt16)RawData[offset++];
            UInt16 SPEED_Z = (UInt16)RawData[offset++];

            this.Speed = new Point3(SPEED_X, SPEED_Y,SPEED_Z);
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (UInt16)Speed.X,
                (UInt16)Speed.Y,
                (UInt16)Speed.Z,
            };
        }
        public override string ToString()
        {
            return string.Format("SetSpeed(x={0}, y={1}, z={2})",
                Speed.X, Speed.Y, Speed.Z);
        }
    }
    [MessageDesc(MID.SetSpeedResponse, "SetSpeedResponse", null, MID.SetSpeed)]
    public class SetSpeedResponseMessage : SimpleMessage
    {
        public override string ToString()
        {
            return "SetSpeedResponse()";
        }
    }
}
