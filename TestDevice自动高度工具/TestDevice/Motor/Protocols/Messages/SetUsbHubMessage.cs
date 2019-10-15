using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.SetUsbHub, "SetUsbHub",
        new TYPE_DESC[]{
            TYPE_DESC.UInt16,		// 0：关闭手机充电
                                    // 1：启用手机充电(默认)
                                    // 2：关闭手机充电（新USB）
                                    // 3：启用手机充电（新USB）(默认)
        }, MID.SetUsbHubResponse)]
    public class SetUsbHubMessage : MessageBase
    {
        public UInt16 Status { get; private set; }

        public SetUsbHubMessage()
        {
            this.Status = 0;
        }

        public SetUsbHubMessage(int status)
        {
            this.Status = (UInt16)status;
        }

        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            UInt16 status = (UInt16)RawData[offset++];

            this.Status = status;
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (UInt16)Status
            };
        }
        public override string ToString()
        {
            return "SetUsbHubMessage(status=" + Status + ")";
        }
    }
    [MessageDesc(MID.SetUsbHubResponse, "SetUsbHubResponse", null, MID.SetUsbHub)]
    public class SetUsbHubResponseMessage : SimpleMessage
    { }
}
