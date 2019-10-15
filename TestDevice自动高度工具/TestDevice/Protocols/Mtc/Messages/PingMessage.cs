using System;
using System.Collections.Generic;
using System.Text;

namespace Mobot.TestBox.Protocols.Mtc.Messages
{
    [MessageDesc(MessageID.PING_COM_ID, "Ping", null, MessageID.PING_RESP_ID)]
    public class PingMessage : SimpleMessage
    { }
    [MessageDesc(MessageID.PING_RESP_ID, "PingResponse",
        new TYPE_DESC[] {
            TYPE_DESC.UInt32		// Agent's status: 0 - is not busy, 1 - busy
        }, MessageID.PING_COM_ID)]
    public class PingRespMessage : MessageBase
    {
        public UInt32 Status { get; private set; }
        public PingRespMessage()
        {
            this.Status = 0;
        }
        public PingRespMessage(UInt32 status)
        {
            this.Status = status;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            UInt32 STATUS = (UInt32)RawData[offset++];

            this.Status = STATUS;
        }
        public override object[] ToRawData()
        {
            object[] value = new object[1];

            int offset = 0;
            value[offset++] = (UInt32)Status;

            return value;
        }
        public override string ToString()
        {
            return "Ping()";
        }
    }
}
