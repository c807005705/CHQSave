using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
    /// <summary>
    /// 控制板接口
    /// </summary>
   public interface IControlCom
    {
        /// <summary>
        /// 是否成功操作上次指令
        /// </summary>
        bool IsSuccessOption { get; }
        /// <summary>
        ///初始化
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="bandRoate"></param>
        /// <param name="parity"></param>
        /// <param name="dataP"></param>
        /// <param name="stopBits"></param>
        void Init(string portName, int bandRoate, Parity parity, int dataP, StopBits stopBits);
        /// <summary>
        /// 打开
        /// </summary>
        void Open();
        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
        /// <summary>
        /// 释放
        /// </summary>
        //void Dispose();
        ///// <summary>
        ///// 读取移动轴压力值
        ///// </summary>
        ///// <returns></returns>
        //double ReadPressValue1();
        ///// <summary>
        ///// 读取技能轴压力轴
        ///// </summary>
        ///// <returns></returns>
        //double ReadPressValue2();
        ///// <summary>
        ///// 读取版本号
        ///// </summary>
        ///// <returns></returns>
        //string ReadVersion();
        ///// <summary>
        ///// 设置移动轴压力校准值
        ///// </summary>
        ///// <param name="radio"></param>
        //void ReadPressRadio1(int radio);
        ///// <summary>
        ///// 设置技能轴压力校准值
        ///// </summary>
        ///// <param name="radio"></param>
        //void ReadPressRadio2(int radio);
        ///// <summary>
        ///// 设置移动最小压力值
        ///// </summary>
        ///// <param name="value"></param>
        //void SetCanTriggerMinPressComm1(double value);
        ///// <summary>
        ///// 设置技能轴最小压力值
        ///// </summary>
        ///// <param name="value"></param>
        //void SetCanTriggerMinPressComm2(double value);




    }
}
