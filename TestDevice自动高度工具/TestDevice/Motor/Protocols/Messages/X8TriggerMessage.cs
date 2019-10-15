using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 手动测试触发命令
    /// </summary>
    [MessageDesc(MID.Trigger, "Trigger",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        //0x01表示接触触发，0x02表示离开触发
        }, MID.TriggerResponse)]
    public class TriggerMessage : MessageBase
    {
        public Byte Statu1 { get; private set; }

        public TriggerMessage() { }

        public TriggerMessage(int statu1)
        {
            this.Statu1 = (Byte)statu1;
        }

        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Statu1 = (Byte)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (Byte)Statu1,
            };
        }
        public override string ToString()
        {
            return "Trigger(" + Statu1 + ")";
        }
    }
    [MessageDesc(MID.TriggerResponse, "TriggerResponse", null, MID.Trigger)]
    public class TriggerResponseMessage : SimpleMessage
    { }
}
