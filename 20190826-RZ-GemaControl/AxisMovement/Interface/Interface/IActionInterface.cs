using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
   public interface IActionInterface
    {
        ///// <summary>
        ///// 上
        ///// </summary>
        //void Up(double x, double y, int depth);
        ///// <summary>
        ///// 下
        ///// </summary>
        //void Down(double x, double y, int depth);
        ///// <summary>
        ///// 左
        ///// </summary>
        //void Left(double x, double y, int depth);
        ///// <summary>
        ///// 右
        ///// </summary>
        //void Right(double x, double y, int depth);
        ///// <summary>
        ///// 左上
        ///// </summary>
        //void TopLeft(int x, int y, int depth);
        ///// <summary>
        ///// 右上
        ///// </summary>
        //void UpperRight(int x, int y, int depth);
        ///// <summary>
        ///// 左下
        ///// </summary>
        //void BottomLeft(int x, int y, int depth);
        ///// <summary>
        ///// 右下
        ///// </summary>
        //void BottomRight(int x, int y, int depth);
        /// <summary>
        /// 平A
        /// </summary>
        void FlatA(int depth);
        /// <summary>
        /// 释放技能1
        /// </summary>
        void Skill1(double u, double v, int depth);
        /// <summary>
        /// 释放技能2
        /// </summary>
        void Skill2(double u, double v, int depth);
        /// <summary>
        /// 释放技能3
        /// </summary>
        void Skill3(double u, double v, int depth);
        /// <summary>
        /// 技能1加点
        /// </summary>
        void SkillPlus1(double u, double v, int depth);
        /// <summary>
        /// 技能2加点
        /// </summary>
        void SkillPlus2(double u, double v, int depth);
        /// <summary>
        /// 技能3加点
        /// </summary>
        void SkillPlus3(double u, double v, int depth);
        /// <summary>
        /// 停止移动
        /// </summary>
        void StopMove();
        /// <summary>
        /// 方向轴单机
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="depth"></param>
        void DirAxisClick(int pointX, int pointY,int depth);
        /// <summary>
        /// 技能轴单机
        /// </summary>
        void killAxisClick(int pointX, int pointY, int depth);
        /// <summary>
        ///轮盘移动
        /// </summary>
        /// <param name="angle"></param>
        void DirMove(double angle);



    }
}
