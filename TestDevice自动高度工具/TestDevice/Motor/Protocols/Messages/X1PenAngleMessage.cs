using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 笔头水平旋转角度协议
    /// </summary>
    [MessageDesc(MID.PenAngle, "PenAngle",
        new TYPE_DESC[] {
                TYPE_DESC.UInt16,        // 0x0000-0x0167：表示0-360度旋转角度
        }, MID.PenAngleResponse)]
    public class PenAngleMessage : SimpleMessage
    {
        public UInt16 Status { get; private set; }
        public PenAngleMessage() { }
        public PenAngleMessage(int status)
        {
            if (status < 0) status = 0;
            if (status > 360) status = 360;
            this.Status = (UInt16)status;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Status = (UInt16)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (UInt16)Status 
            };
        }
        public override string ToString()
        {
            return string.Format("PenAngleMessage(Status={0})", Status);
        }
    }

    [MessageDesc(MID.PenAngleResponse, "PenAngleResponse", null, MID.PenAngle)]
    public class PenAngleResponseMessage : SimpleMessage { }
}
