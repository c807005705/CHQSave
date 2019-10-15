// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;

namespace Mobot.TestBox.Motor
{
    /// <summary>
    /// 电机移动速度。
    /// </summary>
    public enum MotorSpeed : byte
    {
        /// <summary>
        /// 最慢
        /// </summary>
        [StringValue("最慢")]
        Slowest = 1,
        /// <summary>
        /// 慢速
        /// </summary>
        [StringValue("慢速")]
        Slow = 2,
        /// <summary>
        /// 中速
        /// </summary>
        [StringValue("中速")]
        Normal = 3,
        /// <summary>
        /// 快速
        /// </summary>
        [StringValue("快速")]
        Fast = 4,
        /// <summary>
        /// 最快
        /// </summary>
        [StringValue("最快")]
        Fastest = 5,
    }
}
