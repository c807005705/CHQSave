using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 翻转测试命令
    /// 软件逻辑上要把翻转命令设置到手机夹紧和夹具推出命令之后，
    /// 夹具未推出或者手机未夹紧的情况下不能发送翻转测试命令，该命令测试耗时会在20秒
    /// </summary>
    [MessageDesc(MID.Flip, "Flip",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        // 0：
                                    // 1：表示测试
        }, MID.FlipResponse)]
    public class FlipMessage : MessageBase
    {
        public Byte Status { get; private set; }

        public FlipMessage()
        {
            this.Status = 1;
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
            return string.Format("Flip(Status={0})", Status);
        }
    }
    [MessageDesc(MID.FlipResponse, "FlipResponse", null, MID.Flip)]
    public class FlipResponseMessage : SimpleMessage { }
}
