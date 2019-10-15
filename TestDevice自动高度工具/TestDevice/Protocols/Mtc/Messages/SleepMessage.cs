using System;
using System.Collections.Generic;
using System.Text;

namespace Mobot.TestBox.Protocols.Mtc.Messages
{
    [MessageDesc(MessageID.SLEEP_COM_ID, "Sleep",
        new TYPE_DESC[]{
            TYPE_DESC.UInt32		// The number of milliseconds to sleep before returning the response
        }, MessageID.SLEEP_RESP_ID)]
    public class SleepMessage : MessageBase
    {
        /// <summary>
        /// Sleep的时间（单位毫秒）
        /// </summary>
        public UInt32 Milliseconds { get; private set; }
        public SleepMessage()
        {
            this.Milliseconds = 0;
        }
        public SleepMessage(UInt32 ms)
        {
            this.Milliseconds = ms;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            UInt32 MILLISECONDS = (UInt32)RawData[offset++];

            this.Milliseconds = MILLISECONDS;
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (UInt32)Milliseconds
            };
        }
        public override string ToString()
        {
            return string.Format("Sleep(ms={0})", this.Milliseconds);
        }
    }

    [MessageDesc(MessageID.SLEEP_RESP_ID, "SleepResponse", null, MessageID.SLEEP_COM_ID)]
    public class SleepResponseMessage : SimpleMessage
    { }
}
