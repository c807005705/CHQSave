using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobot;

namespace Motor
{
    /// <summary>
    /// 设备类型。
    /// </summary>
    public enum DeviceType : uint
    {
        /// <summary>
        /// 未知。
        /// </summary>
        [StringValue("")]
        Unknown = 0,
        [StringValue("通用手机测试盒(1)")]
        TestBox = 1,
        [StringValue("保留(2)")]
        TestBox_2 = 2,
        [StringValue("保留(3)")]
        TestBox_3 = 3,
        [StringValue("保留(4)")]
        TestBox_4 = 4,
        [StringValue("保留(5)")]
        TestBox_5 = 5,
    }
}
