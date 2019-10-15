using System;
using System.Collections.Generic;
using System.Text;

namespace Mobot.TestBox.Protocols.Mtc.Messages
{
    [MessageDesc(MessageID.KEY_PRESS_COM_ID, "KeyPress/Release",
        new TYPE_DESC[]{
            TYPE_DESC.Blob		//SERIES: A series of commands, giving the keys to be pressed/released or 
			            // giving the time to wait before executing the next command
			            // A byte value of 0x01 is followed by the virtual key value (UInt8)
			            // of the key to be pressed.

			            // A byte value of 0x02 is followed by the virtual key value (UInt8)
			            // of the key to be released.

			            // A byte value of 0x03 indicates that the following virtual key (UInt8)
			            // is to be pressed and then immediately released.

			            // A byte value of 0x04 if followed by the wait time in milliseconds (UInt32)

        }, MessageID.KEY_PRESS_RESP_ID)]
    public class KeyPressMessage : MessageBase
    {
        /// <summary>
        /// Key值序列（每个Key为一个UInt8）
        /// </summary>
        public byte[] Series { get; private set; }

        public KeyPressMessage()
        {
            this.Series = null;
        }
        public KeyPressMessage(byte[] Series)
        {
            this.Series = Series;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Series = (byte[])RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (byte[])Series
            };
        }
        public override string ToString()
        {
            return string.Format("KeyPress(series={0})", BitConverter.ToString(this.Series));
        }
    }

    [MessageDesc(MessageID.KEY_PRESS_RESP_ID, "KeyPressResponse", null, MessageID.KEY_PRESS_COM_ID)]
    public class KeyPressResponseMessage : SimpleMessage
    { }
}
