using Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestDevice;

namespace Motor.Protocols.Messages
{
    /// <summary>
    /// 读取压力数值
    /// </summary>

    [MessageDesc(MID.PressureGrams, "PressureGrams",
        new TYPE_DESC[] {
                TYPE_DESC.UInt8,        // 0x00-0x05：表示第一至六个笔头中的一个笔头
        }, MID.PressureGramsResponse)]
    public class PressureGramsMessage : SimpleMessage {

        private static int PacketNum = 0;

        public byte Packet { get; set; }

        public PressureGramsMessage()
        {
            Packet = (byte)(PacketNum);
            PacketNum++;
        }


        public override object[] ToRawData()
        {
            return new object[] { Packet };
        }
    }


    [MessageDesc(MID.PressureGramsResponse, "PressureGramsResponse",
        new TYPE_DESC[] {
            TYPE_DESC.UInt8,		
            TYPE_DESC.UInt8	   
        }, MID.PressureGrams)]
    public class PressureGramsResponseMessage : MessageBase
    {
        /// <summary>
        /// 压力值（克）
        /// </summary>
        public int Value { get; set; }

        public PressureGramsResponseMessage() { }

        public override void SetRawData(object[] RawData)
        {

                Value = (int)(
                    ((byte)RawData[0] << 8) | ((byte)RawData[1])
                    );
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (int)Value
            };
        }
        public override string ToString()
        {
            return string.Format("PressResponse({0})", Value);
        }
    }
}
