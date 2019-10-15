using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{    /// <summary>
    /// A Response Type indicating an error
    /// </summary>
    [MessageDesc(MID.ErrorResponse, "Failure",
        new TYPE_DESC[] {
            TYPE_DESC.UInt8,    //SOURCE: A value indicating the source of the failure
            TYPE_DESC.UInt32,   //REASON: A Reason for failure
            TYPE_DESC.TStr      //TEXT: A text message explaining the reason failure
        }, MID.ErrorResponse)]
    public class FailureMessage : MessageBase
    {
        public Byte Source { get; private set; }
        public ErrorValues Reason { get; private set; }
        public String Text { get; private set; }
        public FailureMessage()
        {
            this.Source = 0;
            this.Reason = 0;
            this.Text = "";
        }
        public FailureMessage(byte source, ErrorValues reason, string desc)
        {
            this.Source = source;
            this.Reason = reason;
            this.Text = desc;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            this.Source = (Byte)RawData[offset++];
            this.Reason = (ErrorValues)(UInt32)RawData[offset++];
            this.Text = (String)RawData[offset++];
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (Byte)Source,
                (UInt32)Reason,
                (String)Text
            };
        }
        public override string ToString()
        {
            return string.Format("Failure(source={0} reason={1})", this.Source, this.Reason);
        }
    }
}
