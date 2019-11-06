using Device_Link_LTSMC;
using Interface.Interface;
using Interface.Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActInterface
{

    public class ActionList : IActionInterface
    {
        private Dictionary<OptionRun, Action<ServerRevItem>> runFunction = new Dictionary<OptionRun, Action<ServerRevItem>>();
        /// <summary>
        /// 是否处于警告
        /// </summary>
        private bool isWaring = false;
        /// <summary>
        /// 是否初始化接口过
        /// </summary>
        private bool isInit = false;
        /// <summary>
        /// 通讯服务
        /// </summary>
        public IServer Server { get; set; }
        /// <summary>
        /// 控制器端口
        /// </summary>
        public IControlDevice ConDevice { get; set; }
  

        public bool IsLink => throw new NotImplementedException();

        public Config config { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public void RouletteMove(double angle)
        {
            double xPosition = ConDevice.GetAxisPosition(Axis.X);
            double yPosition = ConDevice.GetAxisPosition(Axis.Y);
            double xMovePosition = config.UserConfig.DiscCenter.X + config.UserConfig.CircleRadius * (Math.Cos((Math.PI / 180) * (angle % 360)));
            double yMovePosition = config.UserConfig.DiscCenter.Y + config.UserConfig.CircleRadius * (Math.Sin((Math.PI / 180) * (angle % 360)));
            ConDevice.SetLineInterMove(2000, (int)(xMovePosition - xPosition), (int)(yMovePosition - yPosition));
        }
        public void SkillMove(Point skillposition)
        {
            if (ConDevice.GetAxisPosition(Axis.W) <= config.FactoryConfig.WaitingHeight)
            {
                // ConDevice.
                ConDevice.SetLineInterMove(2000, (int)(ConDevice.GetAxisPosition(Axis.U) - skillposition.X),
                    (int)(ConDevice.GetAxisPosition(Axis.V) - skillposition.Y));
            }
            else
            {
                ConDevice.AxisMoveTo(Axis.W, config.FactoryConfig.WaitingHeight);
                ConDevice.SetLineInterMove(2000, (int)(ConDevice.GetAxisPosition(Axis.U) - skillposition.X),
                    (int)(ConDevice.GetAxisPosition(Axis.V) - skillposition.Y));
            }
            ConDevice.AxisMoveTo(Axis.W, config.FactoryConfig.WContinuousClickHeight);
            ConDevice.AxisMoveTo(Axis.W, config.FactoryConfig.WaitingHeight);
        }

        public void DirMove(Point dirPosition)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 手动控制
        /// </summary>
        public void ManualControl(string name,int moveDistance)
        {
            switch (name)
            {
                case "Movebackward"://前Y
                   ConDevice.AxisMoveTo
                    break;
                case "MoveForward"://后Y
                   
                    break;
                case "MoveLift"://左X
                   
                    break;
                case "MoveRight"://右X
                   
                  
                    break;
                case "MoveUp"://上Z
                   
                    break;
                case "MoveDown"://下Z
                   
                    break;
                case "SkillBackword":
                  
                    break;
                case "SkillForword":
                   
                    break;
                case "SkillLift":
                   
                    break;
                case "SkillRight":
                   
                    break;
                case "SkillUp":
                  
                    break;
                case "SkillDown":
                   
                    break;
            }
        
    }

        /// <summary>
        /// 平A
        /// </summary>
        public void FlatA(int depth)
        {
            ConDevice.MoveToPositionUV(59, 145);
            if (ConDevice.GetAxisPosition(Axis.W) == 0)
                ConDevice.PreHeight(Axis.W, 12);
            ConDevice.PreHeight(Axis.W, depth);
            ConDevice.PreHeight(Axis.W, -depth);
        }
       /// <summary>
       /// 方向轴点击
       /// </summary>
       /// <param name="pointX"></param>
       /// <param name="pointY"></param>
       /// <param name="depth"></param>
        public void DirAxisClick(int pointX, int pointY,int depth)
        {
            ConDevice.MoveToPositionXY(pointX,pointY);
            if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                ConDevice.PreHeight(Axis.Z, 12);
            ConDevice.PreHeight(Axis.Z, depth);
            ConDevice.PreHeight(Axis.Z, -depth);
        }
        /// <summary>
        /// 停止移动
        /// </summary>
        public void StopMove()
        {
            if (ConDevice.GetAxisPosition(Axis.Z) <= 12)
            {
                return;
            }
            ConDevice.PreHeight(Axis.Z, -9);
        }
        /// <summary>
        /// 技能轴点击
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="depth"></param>
        public void killAxisClick(int pointX, int pointY, int depth)
        {
            double CurrentX = ConDevice.GetAxisPosition(Axis.U);
            double CurrentY = ConDevice.GetAxisPosition(Axis.V);
            ConDevice.SetLineInterMove(40000, (int)(pointX - CurrentX), (int)(pointY - CurrentY));
        }

        public bool IsWaring
        {
            get
            {
                return isWaring;
            }
        }
     
      

      
    }
}
