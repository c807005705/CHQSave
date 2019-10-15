using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.Sleep, "Sleep",
        new TYPE_DESC[]{
            TYPE_DESC.UInt16,		//Z
        }, MID.SleepResponse)]
    public class SleepMessage : MessageBase
    {
        public int Ms { get; private set; }
        public SleepMessage()
        {
            this.Ms = 0;
        }
        public SleepMessage(int ms)
            : this()
        {
            this.Ms = ms;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Ms = (UInt16)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (UInt16)this.Ms
            };
        }
        public override string ToString()
        {
            return string.Format("Sleep(ms={0})", this.Ms);
        }
    }

    [MessageDesc(MID.SleepResponse, "SleepResponse", null, MID.Sleep)]
    public class SleepResponseMessage : SimpleMessage
    {
        public override string ToString()
        {
            return "SleepResponse()";
        }
    }
}
