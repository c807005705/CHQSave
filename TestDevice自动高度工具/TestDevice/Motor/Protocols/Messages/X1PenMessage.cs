using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 笔头选择协议
    /// </summary>
    [MessageDesc(MID.Pen, "Pen",
        new TYPE_DESC[] {
                TYPE_DESC.UInt8,        // 0x00-0x05：表示第一至六个笔头中的一个笔头
        }, MID.PenResponse)]
    public class PenMessage : SimpleMessage
    {
        public Byte Status { get; private set; }
        public PenMessage()
        {
            this.Status = 0x00;
        }
        public PenMessage(int status)
        {
            this.Status = (Byte)status;
        }
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
            return string.Format("PenMessage(Status={0})", Status);
        }
    }

    [MessageDesc(MID.PenResponse, "PenResponse", null, MID.Pen)]
    public class PenResponseMessage : SimpleMessage { }
}
