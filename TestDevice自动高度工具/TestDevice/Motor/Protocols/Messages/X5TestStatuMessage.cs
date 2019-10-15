using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.TestStatu, "TestStatu",
        new TYPE_DESC[]{
            TYPE_DESC.UInt8,        //左侧手机测试状态
            TYPE_DESC.UInt8,        //右侧手机测试状态
                                    //0x00表示失败红灯，0x01表示成功绿灯
        }, MID.TestStatuResponse)]
    public class TestStatuMessage : MessageBase
    {
        public Byte Statu1 { get; private set; }
        public Byte Statu2 { get; private set; }

        public TestStatuMessage() { }

        public TestStatuMessage(int statu1, int statu2)
        {
            this.Statu1 = (Byte)statu1;
            this.Statu2 = (Byte)statu2;
        }

        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Statu1 = (Byte)RawData[offset++];
            Byte statu2 = (Byte)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (Byte)Statu1,
                (Byte)Statu2
            };
        }
        public override string ToString()
        {
            return "TestStatu(" + Statu1 + "," + Statu2 + ")";
        }
    }
    [MessageDesc(MID.TestStatuResponse, "TestStatuResponse", null, MID.TestStatu)]
    public class TestStatuResponseMessage : SimpleMessage
    { }
}
