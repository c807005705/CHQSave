using System;
using System.Collections.Generic;
using System.Text;

namespace Mobot.TestBox.Protocols.Mtc
{
    public class MessageDescAttribute : Mobot.TestBox.Protocols.MessageDescAttribute
    {
        public MessageDescAttribute(MessageID mid, string name, TYPE_DESC[] desc, MessageID linkedMid)
            : base((byte)mid, name, desc, (byte)linkedMid)
        { }
    }
}
