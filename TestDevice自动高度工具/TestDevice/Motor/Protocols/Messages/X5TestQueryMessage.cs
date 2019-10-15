using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.TestQuery, "TestQuery",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        //0x01表示查询是否可以开始测试
        }, MID.TestQueryResponse)]
    public class TestQueryMessage : MessageBase
    {
        public Byte Statu { get; private set; }

        public TestQueryMessage() { }

        public TestQueryMessage(int statu)
        {
            this.Statu = (Byte)statu;
        }

        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Statu = (Byte)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (Byte)Statu,
            };
        }
        public override string ToString()
        {
            return "TestQuery(" + Statu + ")";
        }
    }
    [MessageDesc(MID.TestQueryResponse, "TestQueryResponse",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,         // 返回是否可以开始测试
        }, MID.TestQuery)]
    public class TestQueryResponseMessage : MessageBase
    {
        public Byte Statu { get; private set; }

        public TestQueryResponseMessage() { }

        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Statu = (byte)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (byte)Statu,
            };
        }
        public override string ToString()
        {
            return string.Format("TestQuery({0})", Statu);
        }
    }
}
