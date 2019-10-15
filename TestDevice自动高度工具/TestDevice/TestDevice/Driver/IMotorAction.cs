using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestDevice
{
    /// <summary>
    /// 机械手动作。
    /// </summary>
    public interface IMotorAction
    {
        IMotorCommand ToMotorCommand(IControllerSettings settings);
    }
}
