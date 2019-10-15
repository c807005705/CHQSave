// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;

namespace Mobot
{
    /// <summary>
    /// 串口输入输出事件数据。
    /// </summary>
    public class IOEventArgs : EventArgs
    {
        /// <summary>
        /// 输入输出方向。True=接收
        /// </summary>
        public bool IsReceived { get; private set; }
        /// <summary>
        /// 输入输出的数据。
        /// </summary>
        /// <value>数据</value>
        public string Data { get; private set; }

        public IOEventArgs(bool isReceived, string data)
        {
            this.IsReceived = isReceived;
            this.Data = data;
        }
    }
}
