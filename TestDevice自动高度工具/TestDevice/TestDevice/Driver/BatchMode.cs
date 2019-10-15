
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using Mobot;

namespace TestDevice
{
    public enum BatchMode : byte
    {
        /// <summary>
        /// 0=批处理命令发送结束，开始执行
        /// </summary>
        [StringValue("End")]
        End = 0,
        /// <summary>
        /// 1=批处理命令开始发送
        /// </summary>
        [StringValue("Start")]
        Start = 1,
        /// <summary>
        /// 2=清空当前批处理命令队列
        /// </summary>
        [StringValue("Clear")]
        Clear = 2,
        /// <summary>
        /// 3=表示循环次数（0-65535）
        /// </summary>
        [StringValue("Times")]
        Times = 3,
    }
}
