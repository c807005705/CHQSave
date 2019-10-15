using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 麦克探头缩进伸出命令
    /// </summary>
    [MessageDesc(MID.MikeInOut, "MikeInOut",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        // 0：表示缩进
                                    // 1：表示伸出
        }, MID.MikeInOutResponse)]
    public class MikeInOutMessage : MessageBase
    {
        public Byte Status { get; private set; }

        public MikeInOutMessage() { }
        public MikeInOutMessage(int status)
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
            return string.Format("MikeInOut(Status={0})", Status);
        }
    }
    [MessageDesc(MID.MikeInOutResponse, "MikeInOutResponse", null, MID.MikeInOut)]
    public class MikeInOutResponseMessage : SimpleMessage { }
}
