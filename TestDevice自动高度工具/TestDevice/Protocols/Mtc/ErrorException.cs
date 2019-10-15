using System;
using System.Collections.Generic;
using System.Text;

namespace Mobot.TestBox.Protocols.Mtc
{
    public class ErrorException : Exception
    {
        public new byte Source { get; private set; }
        public ErrorValues Error { get; private set; }
        public ErrorException(byte source, ErrorValues error, string message)
            : base(message)
        {
            this.Source = source;
            this.Error = error;
        }
    }
}
