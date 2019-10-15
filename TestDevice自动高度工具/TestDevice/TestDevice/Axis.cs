using System;
using System.Collections.Generic;
using System.Text;
using Mobot;

namespace TestDevice
{
    public enum Axis
    {
        /// <summary>
        /// X轴
        /// </summary>
        [StringValue("X")]
        X = 0,
        [StringValue("Y")]
        Y = 1,
        [StringValue("Z")]
        Z = 2,
    }
}
