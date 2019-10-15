using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using Mobot;
using TestDevice;

namespace Motor
{

    [MessageDesc(MID.BatchMode, "BatchMode",
        new TYPE_DESC[] {
            TYPE_DESC.UInt8,            //Z
        }, MID.BatchModeResponse)]
    public class BatchModeMessage : MessageBase
    {
        public BatchMode Mode { get; private set; }
        public BatchModeMessage(BatchMode mode)
        {
            this.Mode = mode;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Mode = (BatchMode)((byte)RawData[offset++]);

        }
        public override object[] ToRawData()
        {
            object[] msgobj = new object[] {
                (byte)this.Mode,
            };
            string frameBuffer = string.Empty;
            for (int i = 0; i < msgobj.Length; i++)
            {
                frameBuffer += string.Format("{0:X2} ", (object)msgobj[i]);
            }
            return msgobj;
        }
        public override string ToString()
        {
            return string.Format("BatchModeMessage({0})", EnumHelper.GetName(typeof(BatchMode), this.Mode));
        }
    }
    [MessageDesc(MID.BatchMode, "BatchMode",
        new TYPE_DESC[] {
            TYPE_DESC.UInt8,            //Z
            TYPE_DESC.UInt16,           //表示循环次数（0-65535）
        }, MID.BatchModeResponse)]
    public class BatchTimesMessage : MessageBase
    {
        public BatchMode Mode { get; private set; }
        public int Times { get; private set; }
        public BatchTimesMessage(int times)
        {
            this.Mode = BatchMode.Times;
            this.Times = times;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Mode = (BatchMode)((byte)RawData[offset++]);
            this.Times = (UInt16)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            object[] msgobj = new object[] {
                (byte)this.Mode,
                (UInt16)Times,
            };
            string frameBuffer = string.Empty;
            for (int i = 0; i < msgobj.Length; i++)
            {
                frameBuffer += string.Format("{0:X2} ", (object)msgobj[i]);
            }
            return msgobj;
        }
        public override string ToString()
        {
            return string.Format("BatchModeMessage({0},{1})", EnumHelper.GetName(typeof(BatchMode), this.Mode), Times);
        }
    }

    [MessageDesc(MID.BatchModeResponse, "BatchModeResponse", null, MID.BatchMode)]
    public class BatchResponseMessage : SimpleMessage { }
}
