using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 夹具推进推出命令
    /// </summary>
    [MessageDesc(MID.FixInOut, "FixInOut",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        // 0：表示推进
                                    // 1：表示推出
        }, MID.FixInOutResponse)]
    public class FixInOutMessage : MessageBase
    {
        public Byte Status { get; private set; }

        public FixInOutMessage() { }
        public FixInOutMessage(int status)
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
            return string.Format("FixInOut(Status={0})", Status);
        }
    }
    [MessageDesc(MID.FixInOutResponse, "FixInOutResponse", null, MID.FixInOut)]
    public class FixInOutResponseMessage : SimpleMessage { }
}
