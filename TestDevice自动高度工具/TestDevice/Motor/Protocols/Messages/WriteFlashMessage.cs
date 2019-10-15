using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.WriteFlash, "WriteFlash",
        new TYPE_DESC[] {
            TYPE_DESC.UInt16,		//区域索引
                
            TYPE_DESC.UInt8,		//数据
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            
            TYPE_DESC.UInt8,		//数据
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            
            TYPE_DESC.UInt8,		//数据
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,

            TYPE_DESC.UInt8,		//数据
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
            TYPE_DESC.UInt8,
        }, MID.WriteFlashResponse)]
    public class WriteFlashMessage : MessageBase
    {
        public UInt16 Index { get; private set; }
        public byte[] Data { get; private set; }
        public WriteFlashMessage()
        {
            this.Index = 0;
            this.Data = null;
        }
        public WriteFlashMessage(UInt16 index, byte[] data)
        {
            this.Index = index;
            this.Data = data;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            UInt16 index = (UInt16)RawData[offset++];
            byte[] data = new byte[] {
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
                (Byte)RawData[offset++],
            };

            this.Index = index;
            this.Data = data;
        }
        public override object[] ToRawData()
        {
            int offset = 0;
            return new object[] {
                (UInt16)Index,
                
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
                (Byte)Data[offset++],
            };
        }
    }
    [MessageDesc(MID.WriteFlashResponse, "WriteFlashResponse", null, MID.WriteFlash)]
    public class WriteFlashResponseMessage : SimpleMessage
    { }
}
