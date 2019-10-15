using System;
using System.Collections.Generic;
using System.Text;
using TestDevice;

namespace Protocols
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageBase Message { get; private set; }
        public int Counter { get; private set; }
        public MessageReceivedEventArgs(MessageBase message, int counter)
        {
            this.Message = message;
            this.Counter = counter;
        }
    }
}
