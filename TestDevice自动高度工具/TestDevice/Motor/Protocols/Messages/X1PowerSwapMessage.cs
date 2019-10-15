using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 电源数据线插拔命令
    /// </summary>
    [MessageDesc(MID.PowerSwap, "PowerSwap",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        // 0：表示拔出
                                    // 1：表示插上
        }, MID.PowerSwapResponse)]
    public class PowerSwapMessage : MessageBase
    {
        public Byte Status { get; private set; }

        public PowerSwapMessage() { }
        public PowerSwapMessage(int status)
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
            return string.Format("PowerSwap(Status={0})", Status);
        }
    }
    [MessageDesc(MID.PowerSwapResponse, "PowerSwapResponse", null, MID.PowerSwap)]
    public class PowerSwapResponseMessage : SimpleMessage { }
}
