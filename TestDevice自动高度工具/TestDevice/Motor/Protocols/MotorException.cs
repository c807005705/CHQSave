using System;
using System.Collections.Generic;
using System.Text;

namespace Motor
{
    public class MotorException : Exception
    {
        public ErrorValues ErrorCode { get; private set; }
        public string ErrorDesc { get; private set; }
        public MotorException(ErrorValues code, string desc)
            :base(desc)
        {
            this.ErrorCode = code;
            this.ErrorDesc = desc;
        }
    }
}
