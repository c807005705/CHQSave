using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.SetIndoorLight, "SetIndoorLight",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,		// 0：关闭（默认）1：开启
        }, MID.SetIndoorLightResponse)]
    public class SetIndoorLightMessage : MessageBase
    {
        public Byte Status { get; private set; }

        public SetIndoorLightMessage()
        {
            this.Status = 0;
        }

        public SetIndoorLightMessage(int status)
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
            return "SetIndoorLight(status=" + Status + ")";
        }
    }
    [MessageDesc(MID.SetIndoorLightResponse, "SetIndoorLightResponse", null, MID.SetIndoorLight)]
    public class SetIndoorLightResponseMessage : SimpleMessage
    { }
}
