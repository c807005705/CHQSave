using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TestDevice
{
    /// <summary>
    /// 测试盒三轴运动接口
    /// 机械坐标系
    /// </summary>
    public interface IStylus
    {
        void StylusMoveXY(double x, double y);
        void StylusMoveZ(double z);
        void StylusMoveSafe();
        void StylusMoveXYZ(double x, double y, double z);
        int StylusStatue();
        /// <summary>
        /// Z轴归位
        /// </summary>
        void StylusRestZ();
    }
}
