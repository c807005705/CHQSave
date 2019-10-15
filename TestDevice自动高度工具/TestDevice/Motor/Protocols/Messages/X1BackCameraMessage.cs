using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 后置摄像头背光灯命令
    /// </summary>
    [MessageDesc(MID.BackCamera, "BackCamera",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        // 0：表示背光灯关
                                    // 1：表示背光灯开
        }, MID.BackCameraResponse)]
    public class BackCameraMessage : MessageBase
    {
        public Byte Status { get; private set; }

        public BackCameraMessage() { }
        public BackCameraMessage(int status)
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
            return string.Format("BackCamera(Status={0})", Status);
        }
    }
    [MessageDesc(MID.BackCameraResponse, "BackCameraResponse", null, MID.BackCamera)]
    public class BackCameraResponseMessage : SimpleMessage { }
}
