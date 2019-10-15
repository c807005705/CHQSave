
using System;
using System.Collections.Generic;
using System.Text;
using TestDevice;

namespace Protocols
{
    /// <summary>
    /// 没有参数的命令。
    /// </summary>
    public class SimpleMessage : MessageBase
    {
        static SimpleMessage()
        {
        }
        public override void SetRawData(object[] RawData)
        {
            //
        }
        public override object[] ToRawData()
        {
            return null;
        }
    }
}

