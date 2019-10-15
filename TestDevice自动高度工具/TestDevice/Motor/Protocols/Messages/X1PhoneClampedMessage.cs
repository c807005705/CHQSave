using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 手机固定夹紧命令
    /// </summary>
    [MessageDesc(MID.PhoneClamped, "PhoneClamped",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        // 0：表示放松
                                    // 1：表示夹紧
        }, MID.PhoneClampedResponse)]
    public class PhoneClampedMessage : MessageBase
    {
        public Byte Status { get; private set; }

        public PhoneClampedMessage() { }
        public PhoneClampedMessage(int status)
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
            return string.Format("PhoneClamped(Status={0})", Status);
        }
    }
    [MessageDesc(MID.PhoneClampedResponse, "PhoneClampedResponse", null, MID.PhoneClamped)]
    public class PhoneClampedResponseMessage : SimpleMessage { }
}
