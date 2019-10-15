using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestDevice
{
    /// <summary>
    /// 电机命令。
    /// </summary>
    public interface IMotorCommand
    {
        void Work(IMotorDriver driver);
    }
}
