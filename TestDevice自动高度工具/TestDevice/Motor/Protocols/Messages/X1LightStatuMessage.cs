using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 三色状态指示灯命令
    /// 黄灯会待机等待状态，测试运行时为绿灯状态，出现异常需要告警为红灯状态，三种灯在任何时候有且只有一盏灯亮
    /// </summary>
    [MessageDesc(MID.LightStatu, "LightStatu",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        // 0：表示黄灯亮
                                    // 1：表示绿灯亮
                                    // 2：表示红灯亮
        }, MID.LightStatuResponse)]
    public class LightStatuMessage : MessageBase
    {
        public Byte Status { get; private set; }

        public LightStatuMessage() { }
        public LightStatuMessage(int status)
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
            return string.Format("LightStatu(Status={0})", Status);
        }
    }
    [MessageDesc(MID.LightStatuResponse, "LightStatuResponse", null, MID.LightStatu)]
    public class LightStatuResponseMessage : SimpleMessage { }
}
