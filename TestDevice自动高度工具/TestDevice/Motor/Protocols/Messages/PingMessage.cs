using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.Ping, "Ping", null, MID.PingResponse)]
    public class PingMessage : SimpleMessage
    {
        public override string ToString()
        {
            return "Ping()";
        }
    }
    [MessageDesc(MID.PingResponse, "PingResponse",
        new TYPE_DESC[] {
            TYPE_DESC.UInt32		// Agent's status: 0 - is not busy, 1 - busy
        }, MID.Ping)]
    public class PingRespMessage : MessageBase
    {
        public int Status { get; private set; }
        public PingRespMessage()
        {
            this.Status = 0;
        }
        public PingRespMessage(int status)
        {
            this.Status = status;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Status = (int)(UInt32)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (UInt32)Status
            };
        }
        public override string ToString()
        {
            return string.Format("PingResponse(status={0)", this.Status);
        }
    }
}
