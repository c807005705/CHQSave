using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.SetUsbMode, "SetUsbMode",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,		// 0：打开数据通信模式，最大500mA电流充电(关闭交流充电)
                                    // 1：关闭数据通信模式，启动大电流充电模式(打开交流充电)（默认）(2A)
            TYPE_DESC.UInt8,		// 2：打开数据通信模式（新USB）
            TYPE_DESC.UInt8,		// 3：关闭数据通信模式（新USB）(默认)
        }, MID.SetUsbModeResponse)]
    public class SetUsbModeMessage : MessageBase
    {
        public Byte Status { get; private set; }

        public SetUsbModeMessage()
        {
            this.Status = 0;
        }

        public SetUsbModeMessage(int status)
        {
            this.Status = (Byte)status;
        }

        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            Byte status = (Byte)RawData[offset++];

            this.Status = status;
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (Byte)Status
            };
        }
        public override string ToString()
        {
            return "SetUsbMode(status=" + Status + ")";
        }
    }
    [MessageDesc(MID.SetUsbModeResponse, "SetUsbModeResponse", null, MID.SetUsbMode)]
    public class SetUsbModeResponseMessage : SimpleMessage
    { }
}
