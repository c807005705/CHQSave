using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
    /// <summary>
    ///移动轴操作
    /// </summary>
    public interface ImoveAxis
    {     
        /// <summary>
        /// X轴
        /// </summary>
        void Axis_X();
        /// <summary>
        /// Y轴
        /// </summary>
        void Axis_Y();
        /// <summary>
        /// Z轴 
        /// </summary>
        void Axis_Z();
    }
}
