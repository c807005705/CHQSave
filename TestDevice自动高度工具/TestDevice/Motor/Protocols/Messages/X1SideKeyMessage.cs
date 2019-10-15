using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 三个侧键点击命令
    /// </summary>
    [MessageDesc(MID.SideKey, "SideKey",
        new TYPE_DESC[] {
                TYPE_DESC.UInt8,        // 表示三个按键
                TYPE_DESC.UInt8,        // 0~255表示0~25.5秒按键时间，达到按键时间后按键缩回
        }, MID.SideKeyResponse)]
    public class SideKeyMessage : MessageBase
    {
        public Byte Key { get; private set; }
        public Byte Time { get; private set; }
        public SideKeyMessage()
        {
            this.Key = 0;
            this.Time = 0;
        }
        public SideKeyMessage(int key, int time)
        {
            this.Key = (Byte)key;
            this.Time = (Byte)time;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Key = (Byte)RawData[offset++];
            this.Time = (Byte)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (Byte)Key,
                (Byte)Time,
            };
        }
        public override string ToString()
        {
            return string.Format("SideKey(Key={0}, Time={1})", Key, Time);
        }
    }
    [MessageDesc(MID.SideKeyResponse, "SideKeyResponse", null, MID.SideKey)]
    public class SideKeyResponseMessage : SimpleMessage { }
}
