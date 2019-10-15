using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 设置触发压力值
    /// </summary>
    [MessageDesc(MID.TriggerSet, "TriggerSet",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        //触发压力值 16-255
        }, MID.TriggerSetResponse)]
    public class TriggerSetMessage : MessageBase
    {
        public int Value { get; private set; }

        public TriggerSetMessage() { }

        public TriggerSetMessage(int value)
        {
            this.Value = (Byte)value;
        }

        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Value = (Byte)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (Byte)Value,
            };
        }
        public override string ToString()
        {
            return "TriggerSet(" + Value + ")";
        }
    }
    [MessageDesc(MID.TriggerSetResponse, "TriggerSetResponse", null, MID.TriggerSet)]
    public class TriggerSetResponseMessage : SimpleMessage
    { }
}
