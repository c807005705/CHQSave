using Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 读取压力数值
    /// </summary>

    [MessageDesc(MID.StylusRestZ, "StylusRestZ",
        new TYPE_DESC[] {
                TYPE_DESC.UInt8,        //包编号
        }, MID.StylusRestZResponse)]
    public class StylusRestZMessage : SimpleMessage
    {

        private static int PacketNum = 0;

        public byte Packet { get; set; }

        public StylusRestZMessage()
        {
            Packet = (byte)(PacketNum);
            PacketNum++;
        }


        public override object[] ToRawData()
        {
            return new object[] { Packet };
        }
    }


    [MessageDesc(MID.StylusRestZResponse, "StylusRestZResponse",
        null, MID.StylusRestZ)]
    public class StylusRestZResponseMessage : MessageBase
    {

        public StylusRestZResponseMessage() { }

        public override void SetRawData(object[] RawData)
        {

        }

        public override object[] ToRawData()
        {
            return new object[] { };
        }

        public override string ToString()
        {
            return string.Format("PressResponse");
        }
    }
}
