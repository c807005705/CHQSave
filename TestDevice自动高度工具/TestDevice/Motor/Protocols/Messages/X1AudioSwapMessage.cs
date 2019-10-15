using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 音频数据线插拔命令
    /// </summary>
    [MessageDesc(MID.AudioSwap, "AudioSwap",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        // 0：表示拔出
                                    // 1：表示插上
        }, MID.AudioSwapResponse)]
    public class AudioSwapMessage : MessageBase
    {
        public Byte Status { get; private set; }

        public AudioSwapMessage() { }
        public AudioSwapMessage(int status)
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
            return string.Format("AudioSwap(Status={0})", Status);
        }
    }
    [MessageDesc(MID.AudioSwapResponse, "AudioSwapResponse", null, MID.AudioSwap)]
    public class AudioSwapResponseMessage : SimpleMessage { }
}
