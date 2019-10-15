using Device_Link_LTSMC;
using Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskOption
{
    /// <summary>
    /// 任务接口方法
    /// </summary>
   partial class TaskFormat
    {
        private void FlatA(ServerRevItem revItem)
        {
            ControlDevice.MoveToPositionUV(59, 145);
            if (ControlDevice.GetAxisPosition(Axis.W) == 0)
                ControlDevice.PreHeight(Axis.W, 12);
            ControlDevice.PreHeight(Axis.W, revItem.GetValue<int>("depth"));
            ControlDevice.PreHeight(Axis.W, -revItem.GetValue<int>("depth"));
        }
        private void Skill1(ServerRevItem revItem)
        {
            ControlDevice.MoveToPositionUV(revItem.GetValue<int>("x"), revItem.GetValue<int>("y"));
            ControlDevice.SkillClick(40000, revItem.GetValue<int>("depth"));
            ControlDevice.MoveToPositionUV(59, 145);
        }
        private void Skill2(ServerRevItem revItem)
        {
            ControlDevice.MoveToPositionUV(revItem.GetValue<int>("x"), revItem.GetValue<int>("y"));
            ControlDevice.SkillClick(40000, revItem.GetValue<int>("depth"));
            ControlDevice.MoveToPositionUV(59, 145);
        }
        private void Skill3(ServerRevItem revItem)
        {
            ControlDevice.MoveToPositionUV(revItem.GetValue<int>("x"), revItem.GetValue<int>("y"));
            ControlDevice.SkillClick(40000, revItem.GetValue<int>("depth"));
            ControlDevice.MoveToPositionUV(59, 145);
        }
        private void SkillPlus1(ServerRevItem revItem)
        {
            ControlDevice.MoveToPositionUV(revItem.GetValue<int>("x"), revItem.GetValue<int>("y"));
            if (ControlDevice.GetAxisPosition(Axis.W) == 0)
                ControlDevice.PreHeight(Axis.W, 12);
            ControlDevice.SkillClick(40000, revItem.GetValue<int>("depth"));
            ControlDevice.MoveToPositionUV(59, 145);
        }
        private void SkillPlus2(ServerRevItem revItem)
        {
            ControlDevice.MoveToPositionUV(revItem.GetValue<int>("x"), revItem.GetValue<int>("y"));
            if (ControlDevice.GetAxisPosition(Axis.W) == 0)
                ControlDevice.PreHeight(Axis.W, 12);
            ControlDevice.SkillClick(40000, revItem.GetValue<int>("depth"));
        }
        private void SkillPlus3(ServerRevItem revItem)
        {
            ControlDevice.MoveToPositionUV(revItem.GetValue<int>("x"), revItem.GetValue<int>("y"));
            if (ControlDevice.GetAxisPosition(Axis.W) == 0)
                ControlDevice.PreHeight(Axis.W, 12);
            ControlDevice.SkillClick(40000, revItem.GetValue<int>("depth"));
            ControlDevice.MoveToPositionUV(59, 145);
        }
        private void StopMove(ServerRevItem revItem)
        {
            double xPosition = ControlDevice.GetAxisPosition(Axis.X);
            double yPosition = ControlDevice.GetAxisPosition(Axis.Y);
            double xMovePosition = 68 + config.UserConfig.CircleRadius * (Math.Cos((Math.PI / 180) * revItem.GetValue<int>("angle")));
            double yMovePosition = 68 + config.UserConfig.CircleRadius * (Math.Sin((Math.PI / 180) * revItem.GetValue<int>("angle")));
            ControlDevice.SetLineInterMove(2000, Convert.ToInt32(xPosition - xMovePosition), Convert.ToInt32(yPosition - yMovePosition));
        }
  
        private void ClickWeapons(ServerRevItem revItem)
        {
            ControlDevice.MoveToPositionXY(revItem.GetValue<int>("pointX"), revItem.GetValue<int>("pointY"));
            if (ControlDevice.GetAxisPosition(Axis.Z) == 0)
                ControlDevice.PreHeight(Axis.Z, 12);
            ControlDevice.PreHeight(Axis.Z, revItem.GetValue<int>("depth"));
            ControlDevice.PreHeight(Axis.Z, -revItem.GetValue<int>("depth"));
        }
    }
}
