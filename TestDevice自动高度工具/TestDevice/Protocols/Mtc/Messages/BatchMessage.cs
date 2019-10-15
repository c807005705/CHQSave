using System;
using System.Collections.Generic;
using System.Text;

namespace Mobot.TestBox.Protocols.Mtc.Messages
{
    [MessageDesc(MessageID.BATCH_COM_ID, "Batch",
                    new TYPE_DESC[]{
			            TYPE_DESC.Blob		//COMMANDS The array of bytes that comprise the commands to be executed
		            },
                    MessageID.BATCH_RESP_ID)]
    public class BatchMessage : MessageBase
    {
        /// <summary>
        /// Key值序列（每个Key为一个UInt8）
        /// </summary>
        public byte[] Commands { get; private set; }

        public BatchMessage()
        {
            this.Commands = null;
        }
        public BatchMessage(byte[] Series)
        {
            this.Commands = Series;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Commands = (byte[])RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (byte[])Commands
            };
        }
        public override string ToString()
        {
            return "Batch(" + BitConverter.ToString(this.Commands) + ")";
        }
    }


    [MessageDesc(MessageID.BATCH_RESP_ID, "BatchResponse", null, MessageID.BATCH_COM_ID)]
    public class BatchResponseMessage : SimpleMessage
    {
        public override string ToString()
        {
            return "BatchResponse()";
        }
    }
}
