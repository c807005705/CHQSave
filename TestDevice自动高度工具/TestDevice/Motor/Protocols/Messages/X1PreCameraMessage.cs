using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 前置摄像头色卡缩进推出命令
    /// 该命令执行同时前置摄像头背光灯同时配合亮灭
    /// </summary>
    [MessageDesc(MID.PreCamera, "PreCamera",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        // 0：表示缩进
                                    // 1：表示推出
        }, MID.PreCameraResponse)]
    public class PreCameraMessage : MessageBase
    {
        public Byte Status { get; private set; }

        public PreCameraMessage() { }
        public PreCameraMessage(int status)
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
            return string.Format("PreCamera(Status={0})", Status);
        }
    }
    [MessageDesc(MID.PreCameraResponse, "PreCameraResponse", null, MID.PreCamera)]
    public class PreCameraResponseMessage : SimpleMessage { }
}
