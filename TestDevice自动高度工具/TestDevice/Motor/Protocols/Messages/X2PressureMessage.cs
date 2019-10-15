using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 压力检测
    /// </summary>
    [MessageDesc(MID.Pressure, "Pressure", null, MID.PressureResponse)]
    public class PressureMessage : SimpleMessage { }

    [MessageDesc(MID.PressureResponse, "PressureResponse",
        new TYPE_DESC[] {
            TYPE_DESC.UInt8		// 0x30:表示该点击深度压力值未到合适值
                                // 0x31:表示该点击深度压力值合适
        }, MID.Pressure)]
    public class PressureResponseMessage : MessageBase
    {
        public Byte Status { get; private set; }

        public PressureResponseMessage() { }

        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Status = (Byte)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (Byte)Status
            };
        }
        public override string ToString()
        {
            return string.Format("PressResponse({0})", Status);
        }
    }
}
