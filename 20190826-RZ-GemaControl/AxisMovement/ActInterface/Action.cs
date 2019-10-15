using Device_Link_LTSMC;
using Interface.Interface;
using Interface.Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActInterface
{

    public class Action : IActionInterface, ITaskInoke
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
        public IControlDevice ConDevice { get; set; }

        public bool IsLink => throw new NotImplementedException();

        public Config config { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public void DirMove(double angle)
        {
            double xPosition = ConDevice.GetAxisPosition(Axis.X);
            double yPosition = ConDevice.GetAxisPosition(Axis.Y);
            double xMovePosition = 68 + config.UserConfig.CircleRadius * (Math.Cos((Math.PI / 180) * (angle % 360)));
            double yMovePosition = 68 + config.UserConfig.CircleRadius * (Math.Sin((Math.PI / 180) * (angle % 360)));
            ConDevice.SetLineInterMove(2000, Convert.ToInt32(xMovePosition - xPosition), Convert.ToInt32(yMovePosition - yPosition));
        }
    

//        /// <summary>
//        /// 左下
//        /// </summary>
//        public void BottomLeft(int x, int y, int depth)
//        {
//            if (ConDevice.GetAxisPosition(Axis.Z) > 18)
//                ConDevice.PreHeight(Axis.Z, -depth);


//            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
//            {
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.SetLineInterMove(10000, -x, -y);


//            }
//            else
//            {
//                ConDevice.MoveToPositionXY(68, 113);
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.SetLineInterMove(10000, -x, -y);

//            }
//        }
//        /// <summary>
//        /// 右下
//        /// </summary>
//        public void BottomRight(int x, int y, int depth)
//        {
//            if (ConDevice.GetAxisPosition(Axis.Z) > 18)
//                ConDevice.PreHeight(Axis.Z, -depth);

//            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
//            {
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.SetLineInterMove(10000, x, -y);


//            }
//            else
//            {
//                ConDevice.MoveToPositionXY(68, 113);
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.SetLineInterMove(10000, x, -y);
//            }
//        }

//        /// <summary>
//        /// 左
//        /// </summary>
//        public void Left(double x, double y, int depth)
//        {
//            if (ConDevice.GetAxisPosition(Axis.Z) > 18)
//                ConDevice.PreHeight(Axis.Z, -depth);

//            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
//            {
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.MoveToPositionXY(x, y);


//            }
//            else
//            {
//                ConDevice.MoveToPositionXY(68, 113);
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.MoveToPositionXY(x, y);

//            }
//        }
//        /// <summary>
//        /// 右
//        /// </summary>
//        public void Right(double x, double y, int depth)
//        {
//            if (ConDevice.GetAxisPosition(Axis.Z) > 18)
//                ConDevice.PreHeight(Axis.Z, -depth);

//            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
//            {
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.MoveToPositionXY(x, y);


//            }
//            else
//            {
//                ConDevice.MoveToPositionXY(68, 113);
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.MoveToPositionXY(x, y);

//            }
//        }

//        /// <summary>
//        /// 上
//        /// </summary>
//        public void Up(double x, double y, int depth)
//        {
//            if (ConDevice.GetAxisPosition(Axis.Z) > 1)
//                ConDevice.PreHeight(Axis.Z, -depth);

//            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
//            {
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.MoveToPositionXY(x, y);


//            }
//            else
//            {
//                ConDevice.MoveToPositionXY(68, 113);
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.MoveToPositionXY(x, y);

//            }
//        }
//        public void Down(double x, double y, int depth)
//        {
//            if (ConDevice.GetAxisPosition(Axis.Z) > 18)
//                ConDevice.PreHeight(Axis.Z, -depth);

//            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
//            {
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.MoveToPositionXY(x, y);


//            }
//            else
//            {
//                ConDevice.MoveToPositionXY(68, 113);
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.MoveToPositionXY(x, y);

//            }
//        }
//        /// <summary>
//        /// 右上
//        /// </summary>
//        public void UpperRight(int x, int y, int depth)
//        {
//            if (ConDevice.GetAxisPosition(Axis.Z) > 18)
//                ConDevice.PreHeight(Axis.Z, -depth);

//            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
//            {
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 28);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.SetLineInterMove(10000, x, y);


//            }
//            else
//            {
//                ConDevice.MoveToPositionXY(68, 113);
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.SetLineInterMove(10000, x, y);

//            }
//;
//        }
//        /// <summary>
//        /// 左上
//        /// </summary>
//        public void TopLeft(int x, int y, int depth)
//        {
//            if (ConDevice.GetAxisPosition(Axis.Z) > 18)
//                ConDevice.PreHeight(Axis.Z, -depth);

//            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
//            {
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.SetLineInterMove(10000, -x, y);


//            }
//            else
//            {
//                ConDevice.MoveToPositionXY(68, 113);
//                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
//                    ConDevice.PreHeight(Axis.Z, 12);
//                ConDevice.PreHeight(Axis.Z, depth);
//                ConDevice.SetLineInterMove(10000, -x, y);

//            }
//        }
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
        /// 释放技能1
        /// </summary>
        public void Skill1(double u, double v, int depth)
        {

            //ConDevice.MoveToPositionUV(u, v);
            //ConDevice.SkillClick(40000, depth);
            //ConDevice.MoveToPositionUV(59, 145);

        }
        /// <summary>
        /// 释放技能2
        /// </summary>
        public void Skill2(double u, double v, int depth)
        {
            ConDevice.MoveToPositionUV(u, v);
            ConDevice.SkillClick(40000, depth);
            ConDevice.MoveToPositionUV(59, 145);
        }
        /// <summary>
        /// 释放技能3
        /// </summary>
        public void Skill3(double u, double v, int depth)
        {
            ConDevice.MoveToPositionUV(u, v);
            ConDevice.SkillClick(40000, depth);
            ConDevice.MoveToPositionUV(59, 145);
        }
        /// <summary>
        /// 技能1加点
        /// </summary>
        public void SkillPlus1(double u, double v, int depth)
        {
            ConDevice.MoveToPositionUV(u, v);
            ConDevice.SkillClick(40000, depth);
            ConDevice.MoveToPositionUV(59, 145);
        }
        /// <summary>
        /// 技能2加点
        /// </summary>
        public void SkillPlus2(double u, double v, int depth)
        {
            ConDevice.MoveToPositionUV(u, v);
            ConDevice.SkillClick(40000, depth);
            ConDevice.MoveToPositionUV(59, 145);
        }
        /// <summary>
        /// 技能3加点
        /// </summary>
        public void SkillPlus3(double u, double v, int depth)
        {
            ConDevice.MoveToPositionUV(u, v);
            ConDevice.SkillClick(40000, depth);
            ConDevice.MoveToPositionUV(59, 145);
        }
        public void DirAxisClick(int pointX, int pointY,int depth)
        {
            ConDevice.MoveToPositionXY(pointX,pointY);
            if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                ConDevice.PreHeight(Axis.Z, 12);
            ConDevice.PreHeight(Axis.Z, depth);
            ConDevice.PreHeight(Axis.Z, -depth);
        }
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Link(string ip, int port)
        {
           
                Server.Start(ip, port);
                Server.ReciverRequest += ReciverRequest;
                
            
        }
        /// <summary>
        /// 接受接口请求
        /// </summary>
        /// <param name="obj">消息封装</param>
        private void ReciverRequest(ServerRevItem obj)
        {
            this.DoInterface(obj).ContinueWith(t => {
                ServerRevItem serverRev = t.Result;
                var sendmsg = JsonConvert.SerializeObject(serverRev.ReturnObj);
               // Log.log("发送数据：{0}", sendmsg);
                Server.Send(serverRev.FromSocket, sendmsg);
            });
        }
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 初始化接口
        /// </summary>
        public void InitInterface()
        {
           // if (isInit) return;
            runFunction.Clear();

            //runFunction.Add(OptionRun.UP, UP);
            //runFunction.Add(OptionRun.Down, Down);
            //runFunction.Add(OptionRun.Left, Left);
            //runFunction.Add(OptionRun.Right, Right);
            //runFunction.Add(OptionRun.TopLeft, TopLeft);
            //runFunction.Add(OptionRun.UpperRight, UpperRight);
            //runFunction.Add(OptionRun.BottomLeft, BottomLeft);
            //runFunction.Add(OptionRun.BottomRight, BottomRight);
            runFunction.Add(OptionRun.DirMove, DirMove);
            runFunction.Add(OptionRun.FlatA, FlatA);
            runFunction.Add(OptionRun.Skill1, Skill1);
            runFunction.Add(OptionRun.Skill2, Skill2);
            runFunction.Add(OptionRun.Skill3, Skill3);
            runFunction.Add(OptionRun.SkillPlus1, SkillPlus1);
            runFunction.Add(OptionRun.SkillPlus2, SkillPlus2);
            runFunction.Add(OptionRun.SkillPlus3, SkillPlus3);
            runFunction.Add(OptionRun.StopMove, StopMove);
            runFunction.Add(OptionRun.ClickWeapons, ClickWeapons);

        }

        private void DirMove(ServerRevItem obj)
        {
            double xPosition = ConDevice.GetAxisPosition(Axis.X);
            double yPosition = ConDevice.GetAxisPosition(Axis.Y);
            double xMovePosition = 68 + config.UserConfig.CircleRadius * (Math.Cos((Math.PI / 180) * obj.GetValue<int>("angle")));
            double yMovePosition = 68 + config.UserConfig.CircleRadius * (Math.Sin((Math.PI / 180) * obj.GetValue<int>("angle")));
            ConDevice.SetLineInterMove(2000, Convert.ToInt32(xPosition - xMovePosition), Convert.ToInt32(yPosition - yMovePosition));
        }

        private void ClickWeapons(ServerRevItem obj)
        {
            ConDevice.MoveToPositionXY(obj.GetValue<int>("pointX"), obj.GetValue<int>("pointY"));
            if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                ConDevice.PreHeight(Axis.Z, 12);
            ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
            ConDevice.PreHeight(Axis.Z, -obj.GetValue<int>("depth"));
        }

        private void StopMove(ServerRevItem obj)
        {
            if (ConDevice.GetAxisPosition(Axis.Z)>15)
            ConDevice.PreHeight(Axis.Z, -9);
        }

        private void SkillPlus3(ServerRevItem obj)
        {
            ConDevice.MoveToPositionUV(obj.GetValue<int>("x"), obj.GetValue<int>("y"));
            if (ConDevice.GetAxisPosition(Axis.W) == 0)
                ConDevice.PreHeight(Axis.W, 12);
            ConDevice.SkillClick(40000, obj.GetValue<int>("depth"));
            ConDevice.MoveToPositionUV(59, 145);
        }

        private void SkillPlus2(ServerRevItem obj)
        {
            ConDevice.MoveToPositionUV(obj.GetValue<int>("x"), obj.GetValue<int>("y"));
            if (ConDevice.GetAxisPosition(Axis.W) == 0)
                ConDevice.PreHeight(Axis.W, 12);
            ConDevice.SkillClick(40000, obj.GetValue<int>("depth"));
            ConDevice.MoveToPositionUV(59, 145);
        }

        private void SkillPlus1(ServerRevItem obj)
        {
            ConDevice.MoveToPositionUV(obj.GetValue<int>("x"), obj.GetValue<int>("y"));
            if (ConDevice.GetAxisPosition(Axis.W) == 0)
                ConDevice.PreHeight(Axis.W, 12);
            ConDevice.SkillClick(40000, obj.GetValue<int>("depth"));
            ConDevice.MoveToPositionUV(59, 145);
        }

        private void Skill3(ServerRevItem obj)
        {
            ConDevice.MoveToPositionUV(obj.GetValue<int>("x"), obj.GetValue<int>("y"));
            ConDevice.SkillClick(40000, obj.GetValue<int>("depth"));
            ConDevice.MoveToPositionUV(59, 145);
        }

        private void Skill2(ServerRevItem obj)
        {
            ConDevice.MoveToPositionUV(obj.GetValue<int>("x"), obj.GetValue<int>("y"));
            ConDevice.SkillClick(40000, obj.GetValue<int>("depth"));
            ConDevice.MoveToPositionUV(59, 145);
        }

        private void Skill1(ServerRevItem obj)
        {
            ConDevice.MoveToPositionUV(obj.GetValue<int>("x"), obj.GetValue<int>("y"));
            ConDevice.SkillClick(40000, obj.GetValue<int>("depth"));
            ConDevice.MoveToPositionUV(59, 145);
        }

        private void FlatA(ServerRevItem obj)
        {
            ConDevice.MoveToPositionUV(59, 145);
            if (ConDevice.GetAxisPosition(Axis.W) == 0)
                ConDevice.PreHeight(Axis.W, 12);
            ConDevice.PreHeight(Axis.W, obj.GetValue<int>("depth"));
            ConDevice.PreHeight(Axis.W, -obj.GetValue<int>("depth"));
        }

        private void BottomRight(ServerRevItem obj)
        {

            //if (ConDevice.GetAxisPosition(Axis.Z) > 18)
            //    ConDevice.PreHeight(Axis.Z, -obj.GetValue<int>("depth"));

            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
            {
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.SetLineInterMove(10000, obj.GetValue<int>("x"), -obj.GetValue<int>("y"));
                

            }
            else
            {
                ConDevice.MoveToPositionXY(68, 113);
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.SetLineInterMove(10000, obj.GetValue<int>("x"), -obj.GetValue<int>("y"));
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
            }
        }

        private void BottomLeft(ServerRevItem obj)
        {

            //if (ConDevice.GetAxisPosition(Axis.Z) > 18)
            //    ConDevice.PreHeight(Axis.Z, -obj.GetValue<int>("depth"));

            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
            {
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.SetLineInterMove(10000, -obj.GetValue<int>("x"), -obj.GetValue<int>("y"));
              

            }
            else
            {
                ConDevice.MoveToPositionXY(68, 113);

                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.SetLineInterMove(10000, -obj.GetValue<int>("x"), -obj.GetValue<int>("y"));
               
            }
        }

        private void UpperRight(ServerRevItem obj)
        {
            //if (ConDevice.GetAxisPosition(Axis.Z) > 18)
            //    ConDevice.PreHeight(Axis.Z, -obj.GetValue<int>("depth"));

            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
            {
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.SetLineInterMove(10000, obj.GetValue<int>("x"), obj.GetValue<int>("y"));
               

            }
            else
            {
                ConDevice.MoveToPositionXY(68, 113);
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.SetLineInterMove(10000, obj.GetValue<int>("x"), obj.GetValue<int>("y"));
                
            }
        }

        private void TopLeft(ServerRevItem obj)
        {
            //if (ConDevice.GetAxisPosition(Axis.Z) > 18)
            //    ConDevice.PreHeight(Axis.Z, -obj.GetValue<int>("depth"));

            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
            {
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.SetLineInterMove(10000, -obj.GetValue<int>("x"), obj.GetValue<int>("y"));
               

            }
            else
            {
                ConDevice.MoveToPositionXY(68, 113);
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.SetLineInterMove(10000 , -obj.GetValue<int>("x"), obj.GetValue<int>("y"));
                
            }
        }

        private void Right(ServerRevItem obj)
        {
            //if (ConDevice.GetAxisPosition(Axis.Z) > 18)
            //    ConDevice.PreHeight(Axis.Z, -obj.GetValue<int>("depth"));

            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
            {
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.MoveToPositionXY(obj.GetValue<double>("x"), obj.GetValue<double>("y"));
             

            }
            else
            {
                ConDevice.MoveToPositionXY(68, 113);
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.MoveToPositionXY(obj.GetValue<double>("x"), obj.GetValue<double>("y"));
               
            }
        }

        private void Left(ServerRevItem obj)
        {

            //if (ConDevice.GetAxisPosition(Axis.Z) > 18)
            //    ConDevice.PreHeight(Axis.Z, -obj.GetValue<int>("depth"));

            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
            {
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.MoveToPositionXY(-obj.GetValue<double>("x"), obj.GetValue<double>("y"));
               

            }
            else
            {
                ConDevice.MoveToPositionXY(68, 113);
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.MoveToPositionXY(-obj.GetValue<double>("x"), obj.GetValue<double>("y"));
              
            }
        }

        private void Down(ServerRevItem obj)
        {
            //if (ConDevice.GetAxisPosition(Axis.Z) > 18)
            //    ConDevice.PreHeight(Axis.Z, -obj.GetValue<int>("depth"));

            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
            {
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.MoveToPositionXY(obj.GetValue<double>("x"),- obj.GetValue<double>("y"));
               

            }
            else
            {
                ConDevice.MoveToPositionXY(68, 113);
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.MoveToPositionXY(obj.GetValue<double>("x"), -obj.GetValue<double>("y"));
               
            }
        }

        /// <summary>
        /// 上
        /// </summary>
        /// <param name="obj"></param>
        private void UP(ServerRevItem obj)
        {
            //if (ConDevice.GetAxisPosition(Axis.Z) > 18)
            //    ConDevice.PreHeight(Axis.Z, -obj.GetValue<int>("depth"));

            if (ConDevice.GetAxisPosition(Axis.X) == 68 && ConDevice.GetAxisPosition(Axis.Y) == 113)
            {
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.MoveToPositionXY(obj.GetValue<double>("x"), obj.GetValue<double>("y"));
                

            }
            else
            {
                ConDevice.MoveToPositionXY(68, 113);
                if (ConDevice.GetAxisPosition(Axis.Z) == 0)
                    ConDevice.PreHeight(Axis.Z, 12);
                ConDevice.PreHeight(Axis.Z, obj.GetValue<int>("depth"));
                ConDevice.MoveToPositionXY(obj.GetValue<double>("x"), obj.GetValue<double>("y"));
                
            }
        }

        public void LoadFactoryConfig()
        {
            throw new NotImplementedException();
        }

        public void LoadDefaultUserConfig()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveFacktory()
        {
            throw new NotImplementedException();
        }

        public Task<ServerRevItem> DoInterface(InterfaceItem interfaceItem)
        {
            try
            {
                //Log.log("开始执行接口,", interfaceItem.InterfaceName);
                OptionRun optionRun = interfaceItem.InterfaceName.GetEnumType<OptionRun>();
                Dictionary<string, string> directory = new Dictionary<string, string>();
                interfaceItem.Paramters.Foreach(c =>
                {
                    directory.Add(c.Name, c.Value);
                });
                return DoInterface(new ServerRevItem(directory)
                {
                    OptionRun = optionRun
                });
            }
            catch (Exception ex)
            {
                //Log.log("执行接口异常!");
               // Log.error(ex);
                return null;
            }
        }
        /// <summary>
        /// 执行接口
        /// </summary>
        /// <param name="serverRevItem"></param>
        /// <returns></returns>
        public Task<ServerRevItem> DoInterface(ServerRevItem serverRevItem)
        {
            Task<ServerRevItem> task = new Task<ServerRevItem>(new Func<object, ServerRevItem>(obj => {
                ServerRevItem revItem = obj as ServerRevItem;
                try
                {
                    if (IsWaring)
                    {
                        throw new Exception("Waring!");
                    }
                    runFunction[revItem.OptionRun](
                        revItem
                    );
                }
                catch (Exception ex)
                {
                    //Log.waring("执行接口异常!");
                    //Log.error(ex);
                    revItem.ReturnObj.Msg = ex.Message;
                    revItem.ReturnObj.Result = false;
                }
                return revItem;
            }), serverRevItem);
            task.Start();
            return task;
        }

        public List<InterfaceItem> GetInterfaceList()
        {
            throw new NotImplementedException();
        }

        public void StopMove()
        {
            if(ConDevice.GetAxisPosition(Axis.Z)<=12)
            {
                return;
            }
            ConDevice.PreHeight(Axis.Z, -9);
        }

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
