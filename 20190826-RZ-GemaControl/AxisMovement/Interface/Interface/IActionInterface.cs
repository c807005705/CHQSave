using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
   public interface IActionInterface
    {
       
        /// <summary>
        /// 平A
        /// </summary>
        void FlatA(int depth);
        /// <summary>
        /// 停止移动
        /// </summary>
        void StopMove();
        /// <summary>
        /// 方向轴单击
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="depth"></param>
        void DirAxisClick(int pointX, int pointY,int depth);
        /// <summary>
        /// 技能轴单击
        /// </summary>
        void killAxisClick(int pointX, int pointY, int depth);
        /// <summary>
        ///轮盘移动
        /// </summary>
        /// <param name="angle"></param>
        void RouletteMove(double angle);
        /// <summary>
        /// 技能轴移动
        /// </summary>
        /// <param name="position"></param>
        void SkillMove(Point skillposition);
        /// <summary>
        /// 方向轴移动
        /// </summary>
        /// <param name="dirPosition"></param>
        void DirMove(Point dirPosition);
        /// <summary>
        /// 手动控制
        /// </summary>
        void ManualControl(string name ,int moveDistance);



    }
}
