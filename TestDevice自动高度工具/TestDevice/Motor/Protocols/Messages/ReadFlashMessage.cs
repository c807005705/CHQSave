using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.ReadFlash, "ReadFlash",
        new TYPE_DESC[] {
                TYPE_DESC.UInt16,		//区域索引
        }, MID.ReadFlashResponse)]
    public class ReadFlashMessage : MessageBase
    {
        public UInt16 Index { get; private set; }
        public ReadFlashMessage()
        {
            this.Index = 0;
        }
        public ReadFlashMessage(UInt16 index)
        {
            this.Index = index;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            UInt16 index = (UInt16)RawData[offset++];

            this.Index = index;
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (UInt16)Index,
            };
        }
    }
    [MessageDesc(MID.ReadFlashResponse, "ReadFlashResponse",
        new TYPE_DESC[] {
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
        }, MID.ReadFlash)]
    public class ReadFlashResponseMessage : MessageBase
    {
        /// <summary>
        /// 配置数据。
        /// </summary>
        public byte[] Data { get; private set; }

        public ReadFlashResponseMessage()
        {
            this.Data = null;
        }
        public ReadFlashResponseMessage(byte[] data)
        {
            this.Data = data;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
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

            this.Data = data;
        }
        public override object[] ToRawData()
        {
            int offset = 0;
            return new object[] {
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
}
