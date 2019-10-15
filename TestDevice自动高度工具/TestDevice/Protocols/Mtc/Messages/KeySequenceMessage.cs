using System;
using System.Collections.Generic;
using System.Text;

namespace Mobot.TestBox.Protocols.Mtc.Messages
{
    [MessageDesc(MessageID.KEY_SEQUENCE_COM_ID, "KeySequence",
        new TYPE_DESC[]{
            TYPE_DESC.UInt32,		//KEY_DOWN_DELAY: The number of milliseconds to hold a key down before it is released
            TYPE_DESC.UInt32,		//INTER_KEY_DELAY: The number of milliseconds to wait between keys
            TYPE_DESC.Blob		//SEQUENCE: The virtual key codes to press and release, each key is a UInt8
        }, MessageID.KEY_SEQUENCE_RESP_ID)]
    public class KeySequenceMessage : MessageBase
    {
        public UInt32 KeyDownDelay { get; private set; }
        public UInt32 InterKeyDelay { get; private set; }
        /// <summary>
        /// Key值序列（每个Key为一个UInt8）
        /// </summary>
        public byte[] Sequence { get; private set; }

        public KeySequenceMessage()
        {
            this.KeyDownDelay = 0;
            this.InterKeyDelay = 0;
            this.Sequence = null;
        }
        public KeySequenceMessage(UInt32 keyDownDelay, UInt32 interKeyDelay, byte[] sequence)
        {
            this.KeyDownDelay = keyDownDelay;
            this.InterKeyDelay = interKeyDelay;
            this.Sequence = sequence;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.KeyDownDelay = (UInt32)RawData[offset++];
            this.InterKeyDelay = (UInt32)RawData[offset++];
            this.Sequence = (byte[])RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (UInt32)KeyDownDelay,
                (UInt32)InterKeyDelay,
                (byte[])Sequence
            };
        }

        public override string ToString()
        {
            return string.Format("KeySequence(delay={0} interval={1} series={2})",
                this.KeyDownDelay,
                this.InterKeyDelay,
                BitConverter.ToString(this.Sequence));
        }
    }
    [MessageDesc(MessageID.KEY_SEQUENCE_RESP_ID, "KeySequenceResponse", null, MessageID.KEY_SEQUENCE_COM_ID)]
    public class KeySequenceResponseMessage : SimpleMessage
    { }
}
