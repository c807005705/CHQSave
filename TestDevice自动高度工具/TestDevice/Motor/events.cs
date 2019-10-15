// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;
using Mobot.TestBox.Protocols.Motor;

namespace Mobot.TestBox.Motor
{
    /// <summary>
    /// 为 ErrorReceived 事件准备数据。
    /// </summary>
    public class ErrorReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 获取或设置事件类型。
        /// </summary>
        /// <value>ErrorValues 值之一。</value>
        public ErrorValues ErrorCode { get; private set; }
        public string ErrorDesc {get; private set;}
        public ErrorReceivedEventArgs(ErrorValues code, string desc)
        {
            this.ErrorCode = code;
            this.ErrorDesc = desc;
        }
    }
}
