using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Protocols;
using System.Drawing;
using Mobot.Utils.IO;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using Mobot;

namespace Motor
{
    public enum TriggerType
    {
        None = 0,
        /// <summary>
        /// 点击接触时(Z轴按下=>倒数第二次Z轴滑动,Trigger=1)
        /// </summary>
        [StringValue("TriggerHard")]
        HardStart = 1,
        /// <summary>
        /// 点击离开时(Z轴抬起=>最后一次Z轴滑动,Trigger=1)
        /// </summary>
        [StringValue("TriggerHardEnd")]
        HardEnd = 5,
        /// <summary>
        /// 滑动开始时(最后一次平滑,Trigger=2)
        /// </summary>
        [StringValue("TriggerStart")]
        MoveStart = 2,
        /// <summary>
        /// 滑动停止时(侧键接触=>倒数第二次平滑,Trigger=3)
        /// </summary>
        [StringValue("TriggerEnd")]
        MoveEnd = 3,
        /// <summary>
        /// 手写板滑动开始时(手写板第一次平滑,Trigger=2)
        /// </summary>
        [StringValue("TriggerMStart")]
        GroupStart = 4,
        /// <summary>
        /// 手写板点击离开时(第一次点击离开=>手写板第二次Z轴滑动,Trigger=1)
        /// </summary>
        [StringValue("TriggerMEnd")]
        GroupEnd = 6,
    }
}
