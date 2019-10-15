using Device_Link_LTSMC;
using Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
    /// <summary>
    /// 控制器接口
    /// </summary>
    public interface IControlDevice
    {
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        bool Link(Dictionary<Axis, AxisInfo> infos);
        /// <summary>
        /// 移动轴到指定坐标
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        void AxisMoveTo(Axis axis, double position, double speed = -1);
        /// <summary>
        /// 移动轴到指定坐标
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        void MoveToPositionUV(double u, double v);
       
        /// <summary>
        /// 移动轴水平移动到指定距离
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void MoveToPositionXY(double x, double y);
        /// <summary>
        /// 移动轴垂直轴向归零
        /// </summary>
        void zVecAxisAllToZero();
        /// <summary>
        /// 所有轴向归零
        /// </summary>                                                                  
        void AllToZero();
        /// <summary>
        /// 单轴归零
        /// </summary>
        /// <param name="axis"></param>
        void AxisRunToZero(Axis axis);
        /// <summary>
        /// 技能轴垂直轴向归零
        /// </summary>
        void wVecAxisAllToZero();
        /// <summary>
        /// 等待操作结束
        /// </summary>
        void WaitForEnd();
        /// <summary>
        /// 设置输出端口
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="portSatte"></param>
        void SetOutPort(int idx,PortSatte portSatte);
        /// <summary>
        /// 读取输入端口
        /// </summary>
        /// <param name="idx"></param>
       PortSatte ReadInPort(int idx);
        /// <summary>
        /// 获取当前轴向的坐标
        /// </summary>
        /// <param name="axis"></param>
        double GetAxisPosition(Axis axis);
        /// <summary>
        /// 停止所有轴
        /// </summary>
        void StopAll();
        /// <summary>
        /// 获取坐标信息
        /// </summary>
        /// <returns></returns>
        string GetPositionInfo();
        /// <summary>
        /// 保存设备配置
        /// </summary>
        /// <param name="msg"></param>
        void SaveCongigToDevice(string msg);
        /// <summary>
        /// 清楚所有警告
        /// </summary>
        void GetClearAllAxisWaring();
        /// <summary>
        /// 是否正在移动
        /// </summary>
        /// <returns></returns>
        bool IsMove(Axis axis);
        /// <summary>
        /// 是否连接
        /// </summary>
        /// <returns></returns>
        bool IsLink();
        /// <summary>
        /// 轴移动
        /// </summary>
        /// <param name="distance"></param>
        void MoveAxis(Axis axis, double distance, bool syn, Direction direction);  
        /// <summary>
        /// 清除坐标
        /// </summary>
        void ClearPiont(Axis axis);

        void moveX();
        /// <summary>
        /// 技能点击
        /// </summary>
        void SkillClick(double Speed,int depth);
        /// <summary>
        /// 方向轴点击
        /// </summary>
        /// <param name="Speed"></param>
        /// <param name="depth"></param>
        void MoveClick(double Speed, int depth);

        /// <summary>
        /// 移动轴直线补偿移动
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="Axis"></param>
        /// <param name="TargetPosition"></param>
        /// <param name="targetPosition"></param>
        void SetLineInterMove(int speed ,int XTargetPosition,int YTargetPosition);

        /// <summary>
        /// 轴速度
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        double AxisSpeed(Axis axis, double speed);

        void PreHeight(Axis axis, int height);
        void stopAxis(Axis axis);
        void ContinuousMotion(Axis axis ,Direction direction);
        void DirMove(double angle);

    }
}
