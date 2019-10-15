using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestDevice
{
    /// <summary>
    /// 机械手组合动作。CombinationActions
    /// </summary>
    public class ActionBatch
    {
        private List<IMotorAction> parts = new List<IMotorAction>();
        public ActionBatch(params IMotorAction[] parts)
        {
            this.parts.AddRange(parts);
        }
        public ActionBatch(IEnumerable<IMotorAction> parts)
        {
            this.parts.AddRange(parts);
        }

        public List<IMotorAction> Parts
        {
            get
            {
                return this.parts;
            }
        }
    }
}
