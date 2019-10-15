using System;
using System.Collections.Generic;
using System.Text;

namespace TestDevice
{
    /// <summary>
    /// 测试盒Batch运行状态
    /// </summary>
    public enum ModeType
    {
        None = 0,
        Start,
        Run,
        Finished,
        Stop,
    }
    /// <summary>
    /// 屏幕测试运行状态
    /// </summary>
    public enum ScreenTestType
    {
        None = 0,
        Run,
        Pause,
        Finished,
    }
}
