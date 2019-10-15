using Device_Link_LTSMC;
using Interface.Interface;
using Interface.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ControlDec
{
    public class ConDevice : IControlDevice
    {
        /// <summary>
        /// 雷赛控制器
        /// </summary>
        public Device_Link_LTSMC.Device_Net_LSTMC lTSMC = new Device_Link_LTSMC.Device_Net_LSTMC(new List<Axis>() {
            Axis.X, Axis.Y, Axis.Z, Axis.U, Axis.V, Axis.W });
        //LineInterData lineInterData = new LineInterData();
        // MoveXYZ moveXYZ = new MoveXYZ();



        /// <summary>
        /// 配置
        /// </summary>
        private Dictionary<Axis, AxisInfo> infos = null;
        public ILog Log { get; set; }
        /// <summary>
        /// 是否准备好可以执行下一个任务
        /// </summary>
        public bool IsReady { get { return lTSMC.IsLink && lTSMC.IsAllReady; } }
        private Thread conCick = null;
        /// <summary>
        /// 所有轴向归零
        /// </summary>
        public void AllToZero()
        {
            Log.log("开始回零操作，停止任何操作！");
            lTSMC.StopAll();
            Log.log("设置轴向归零参数");
            AxisRunToZero(Axis.Z);
            AxisRunToZero(Axis.W);
            loop();
            Log.log("所有垂直轴向归零成功");
            AxisRunToZero(Axis.X);
            AxisRunToZero(Axis.Y);
            AxisRunToZero(Axis.U);
            AxisRunToZero(Axis.V);        
            loop();
            Log.log("水平轴向归零成功");
            lTSMC.AllToClearPositionZero();
            Log.log("所有坐标清零成功");

        }
        /// <summary>
        /// 单轴归零
        /// </summary>
        /// <param name="axis"></param>
        public void AxisRunToZero(Axis axis)
        {
            double RetZeroSpeed = infos[axis].RunSpeed;
            Direction direction = infos[axis].Direction;
            try
            {
                switch (axis)
                {
                    case Axis.X:
                        lTSMC[axis].SetStartFindZeroParameters(RetZeroSpeed, direction);
                        break;
                    case Axis.Y:
                        lTSMC[axis].SetStartFindZeroParameters(RetZeroSpeed, direction);
                        break;
                    case Axis.Z:
                        lTSMC[axis].SetStartFindZeroParameters(RetZeroSpeed, direction);
                        break;
                    case Axis.U:
                        lTSMC[axis].SetStartFindZeroParameters(RetZeroSpeed, direction);
                        break;
                    case Axis.V:
                        lTSMC[axis].SetStartFindZeroParameters(RetZeroSpeed, direction);
                        break;
                    case Axis.W:
                        lTSMC[axis].SetStartFindZeroParameters(RetZeroSpeed, direction);
                        break;
                }
                loop();
                ClearPiont(axis);
            }
            catch (Exception ex)

            {
                Log.error("回零失败" + ex);
            }

        }
        /// <summary>
        /// 等待
        /// </summary>
        /// <param name="time"></param>
        private void loop(int time = 20)
        {
            while (!IsReady) { Thread.Sleep(time); }
        }
       
        /// <summary>
        /// 获取当前轴向的坐标
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public double GetCurrentPosition(Axis axis)
        {
            return Math.Abs(lTSMC[axis].GetAxisCurrentPosition());
        }
        /// <summary>
        /// 获取MM为单位的坐标
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        protected double getMMPosition(Axis axis)
        {

            return GetCurrentPosition(axis) / Math.Abs(infos[axis].Scale);
        }
        /// <summary>
        /// 获取轴的坐标
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public double GetAxisPosition(Axis axis)
        {
            return getMMPosition(axis);
        }

        /// <summary>
        /// 清楚所有警告
        /// </summary>
        public void GetClearAllAxisWaring()
        {
            lTSMC.SetClearAllAxisWaring();
        }
        /// <summary>
        /// 获取控制器坐标信息
        /// </summary>
        /// <returns></returns>
        public string GetPositionInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in this.infos)
            {
                stringBuilder.Append((item.Value.Name + " : " + getMMPosition(item.Key) + "  "));

            }
            return stringBuilder.ToString();
        }


        /// <summary>
        /// 是否连接
        /// </summary>
        /// <returns></returns>
        public bool IsLink() => lTSMC.IsLink;
        /// <summary>
        /// 是否正在移动
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool IsMove(Axis axis)
        {
            return lTSMC[axis].GetAxisCurrentState();
        }
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public bool Link(Dictionary<Axis, AxisInfo> infos)
        {

            try
            {
                this.infos = infos;
                lTSMC.TargetIP = "192.168.5.11";
                lTSMC.LinkDevice();
                foreach (Axis item in Enum.GetValues(typeof(Axis)))
                {
                    lTSMC[item].ZeroMode = infos[item].ZeroMode;
                    lTSMC[item].RunSpeed = infos[item].RunSpeed;
                    lTSMC[item].AccTime = infos[item].AccTime;
                    lTSMC[item].DecTime = infos[item].DecTime;
                    lTSMC[item].StartSpeed = infos[item].LowZeroSpeed;
                }
                //setEnable();
                AllToZero();
                lTSMC.SetOutPort(1, PortSatte.L);
                lTSMC.SetOutPort(0, PortSatte.L);
                return true;
            }
            catch (Exception ex)
            {
                Log.log("设备初始化失败！内容", ex);
                return true;
            }
        }
        /// <summary>
        /// 设置使能
        /// </summary>
        private void setEnable()
        {
            lTSMC[Axis.X].SetEmergencyStopIOMap(PortSatte.L, 1);
            lTSMC[Axis.Y].SetEmergencyStopIOMap(PortSatte.L, 1);
            lTSMC[Axis.Z].SetEmergencyStopIOMap(PortSatte.L, 1);
            lTSMC[Axis.U].SetEmergencyStopIOMap(PortSatte.L, 1);
            lTSMC[Axis.V].SetEmergencyStopIOMap(PortSatte.L, 1);
            lTSMC[Axis.W].SetEmergencyStopIOMap(PortSatte.L, 1);

        }
        /// <summary>
        /// 单轴移动到指定坐标
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        public void AxisMoveTo(Axis axis, double position, double speed = -1)
        {
            double value = getMMPosition(axis);
            Log.log("正在移动{0}轴" + axis);
            //if (position >= 0)

           // {
                switch (axis)
                {
                    case Axis.U:
                    case Axis.X:
                        lTSMC[axis].SetRunDistanceAsync((position - getMMPosition(axis)) * (5000 / 25));
                        break;
                    case Axis.V:
                    case Axis.Y:
                        lTSMC[axis].SetRunDistanceSync((position - getMMPosition(axis)) * (5000 / 16));
                        break;
                    case Axis.Z:
                    case Axis.W:
                        lTSMC[axis].SetRunDistanceSync((position + getMMPosition(axis)) * (5000 /7 ));
                        break;
                
                            }
           // }
           // else
           // {
                //Log.log("输入坐标错误");
       // }
        }

        /// <summary>
        /// 方向轴水平移动到指定坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void MoveToPositionXY(double x, double y)
        {
            AxisMoveTo(Config.MovingXAxis, x );
            AxisMoveTo(Config.MovingYAxis, y );
            loop();
        }
        /// <summary>
        /// 技能释放轴水平移动到指定坐标
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        public void MoveToPositionUV(double u, double v)
        {
            AxisMoveTo(Config.SkillReleaseUAxis, u );
            AxisMoveTo(Config.SkillReleaseVAxis, v );
            loop();
        }
        /// <summary>
        /// 读取输入端口的高低电平
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public PortSatte ReadInPort(int idx) => lTSMC.ReadInPort(idx);
        /// <summary>
        /// 设置输出口状态
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="portSatte"></param>
        public void SetOutPort(int idx, PortSatte portSatte) => lTSMC.SetOutPort(idx, portSatte);

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="msg"></param>
        public void SaveCongigToDevice(string msg)
        {
            if (!lTSMC.WriteConfigToDevice(System.Text.Encoding.UTF8.GetBytes(msg)))
            {
                Log.error("保存配置文件失败");
            }
        }
        /// <summary>
        /// 停止所有轴
        /// </summary>
        public void StopAll()
        {
            lTSMC.StopAll();
            thread.Abort();

        }
        /// <summary>
        /// 等待结束
        /// </summary>
        public void WaitForEnd()
        {
            loop();
        }
        /// <summary>
        /// Z轴归零
        /// </summary>
        public void zVecAxisAllToZero()
        {
            Log.log("移动垂直轴开始归零");
            AxisRunToZero(Config.MovingZAxis);
            lTSMC[Config.MovingZAxis].PositionZeroClear();
            loop();
            Log.log("移动垂直轴归零成功");
        }
        /// <summary>
        /// W轴归零
        /// </summary>
        public void wVecAxisAllToZero()
        {
            Log.log("技能释放垂直轴开始归零");
            AxisRunToZero(Config.SkillReleaseWAxis);
            lTSMC[Config.SkillReleaseWAxis].PositionZeroClear();
            loop();
            Log.log("技能释放垂直轴归零成功");
        }
        /// <summary>
        /// 方向盘点击移动
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="distance"></param>
        /// <param name="syn"></param>
        /// <param name="direction"></param>
        public void MoveAxis(Axis axis ,double distance, bool syn, Direction direction)
        {
            if (syn)
            {
                if (direction == Direction.Forward)
                    lTSMC[axis].SetRunDistanceAsync(distance);
                else
                    lTSMC[axis].SetRunDistanceAsync(-distance);
                loop();
            }
            else
            {
                if (direction == Direction.Forward)
                    lTSMC[axis].SetRunDistanceSync(distance);
                else
                    lTSMC[axis].SetRunDistanceSync(-distance);
                loop();
            }

        }
           
        /// <summary>
        ///坐标清零
        /// </summary>
        /// <param name="axis"></param>
        public void ClearPiont(Axis axis)
        {
            lTSMC[axis].PositionZeroClear();
        }

        public void moveX()
        {
            //LineInterData lineInterData = new LineInterData(10000, 0.01, 0.01);

            //lineInterData.Add(new LineInterItem(Axis.X,10000 ));

            //lineInterData.Add(new LineInterItem(Axis.Y, 15000));

            //lTSMC.SetLineInter(lineInterData);


            //MoveX(75 * (5000 / 25), true, Direction.Forward);
            //MoveY(115 * (5000 / 16), true, Direction.Forward);
            //loop();
            //MoveZ(10 * (5000 / 7), false, Direction.Back);
            Task<bool> xisover = lTSMC[Axis.X].SetRunDistanceAsync(68 * (5000 / 25));
            Task<bool> yisover = lTSMC[Axis.Y].SetRunDistanceAsync(113 * (5000 / 16));
            Task<bool> Uisover = lTSMC[Axis.U].SetRunDistanceAsync(59 * (5000 / 25));
            Task<bool> Visover = lTSMC[Axis.V].SetRunDistanceAsync(145 * (5000 / 16));
            xisover.Wait();//等待
            yisover.Wait();//等待
            Uisover.Wait();
            Visover.Wait();
           
            lTSMC[Axis.X].SetRunDistance(-(9 * (5000 / 25)));
            lTSMC[Axis.Y].SetRunDistance(27 * (5000 / 16));
            //MoveClick();
            lTSMC[Axis.X].SetRunDistance((9 * (5000 / 25)));
            lTSMC[Axis.Y].SetRunDistance(-(27 * (5000 / 16)));
            loop();
            //lTSMC[Axis.Z].SetRunDistanceSync(-(12 * (5000 / 7)));  
            lTSMC[Axis.W].SetRunDistanceSync(-(12 * (5000 / 7)));           
            lTSMC[Axis.Z].SetRunDistanceSync(-(6 * (5000 / 7)));
             lTSMC[Axis.U].SetRunDistanceSync(25 * (5000 / 25));
            lTSMC[Axis.V].SetRunDistanceSync(-4 * (5000 / 16));
            ContinuousClick();
            lTSMC[Axis.U].SetRunDistanceSync(-25 * (5000 / 25));
            lTSMC[Axis.V].SetRunDistanceSync(4 * (5000 / 16));
            
            LineInterData lineInterData = new LineInterData(10000, 0.01, 0.01);
            lineInterData.Add(new LineInterItem(Axis.X, 5 * (5000 / 25)));
            lineInterData.Add(new LineInterItem(Axis.Y, 8 * (5000 / 16)));
            lTSMC.SetLineInter(lineInterData);
            loop();
            moveThread();
            timeInterval();
        }
        /// <summary>
        ///z点击
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ContinuousClick()
        {
            lTSMC[Axis.W].SetRunDistanceSync(-(9 * (5000 / 7)));
            lTSMC[Axis.W].SetRunDistanceSync((9 * (5000 / 7)));

        }
        //public void ConCick()
        //{
        //    conCick = new Thread(timeInterval)
        //    {

        //        IsBackground = true 
        //    };
        //    conCick.Start();
        //}

        Thread thread = null;
        Thread Thread1 = null;
        public void moveThread()
        {
            System.Timers.Timer timer1 = new System.Timers.Timer();
            timer1.Enabled = true;
            timer1.Interval = 14000;//执行间隔时间
            timer1.Start();
            timer1.Elapsed += new ElapsedEventHandler(Moveclick);
            //Thread1 = new Thread(Moveclick);
            //Thread1.IsBackground = true;
            //Thread1.Start();
        }
        /// <summary>
        /// 打圈移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Moveclick(object sender, ElapsedEventArgs e)
        {
            //while (true)
            //{
            for (int i = 0; i < 8; i++)
            {
                lTSMC[Axis.X].RunSpeed = 10000;
                lTSMC[Axis.Y].RunSpeed = 10000;
                lTSMC[Axis.X].SetRunDistanceSync(-15 * (5000 / 25));
                lTSMC[Axis.Y].SetRunDistanceSync(-15 * (5000 / 16));
                lTSMC[Axis.X].SetRunDistanceSync(15 * (5000 / 25));
                lTSMC[Axis.Y].SetRunDistanceSync(15 * (5000 / 16));
            }

           // }
        }
        /// <summary>
        /// 循环技能点击线程
        /// </summary>
            public void timeInterval()
        {
            //System.Timers.Timer timer1 = new System.Timers.Timer();
            //timer1.Enabled = true;
            //timer1.Interval = 7000;//执行间隔时间
            //timer1.Start();
            //timer1.Elapsed += new ElapsedEventHandler(click);
            thread = new Thread(click);
            thread.IsBackground = true;
            thread.Start();
            
        }

        private void click(/*object sender, ElapsedEventArgs e*/)
        {

            try
            {
                Thread.Sleep(14000);
                while (true)
                {
                    
                    for (int j = 0; j < 2; j++)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            lTSMC[Axis.W].SetRunDistanceSync(-(9 * (5000 / 7)));
                            lTSMC[Axis.W].SetRunDistanceSync((9 * (5000 / 7)));
                        }
                        lTSMC[Axis.U].SetRunDistanceSync(21 * (5000 / 25));
                        lTSMC[Axis.W].SetRunDistanceSync(-(9 * (5000 / 7)));
                        lTSMC[Axis.W].SetRunDistanceSync((9 * (5000 / 7)));
                        lTSMC[Axis.U].SetRunDistanceSync(-21 * (5000 / 25));
                    }
                    lTSMC[Axis.U].SetRunDistanceSync(13 * (5000 / 25));
                    lTSMC[Axis.V].SetRunDistanceSync(-12 * (5000 / 16));
                    lTSMC[Axis.W].SetRunDistanceSync(-(9 * (5000 / 7)));
                    lTSMC[Axis.W].SetRunDistanceSync((9 * (5000 / 7)));
                    lTSMC[Axis.U].SetRunDistanceSync(-13 * (5000 / 25));
                    lTSMC[Axis.V].SetRunDistanceSync(12 * (5000 / 16));

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString() + "操作异常");
            }

        }
        /// <summary>
        /// 方向轴点击
        /// </summary>
        public void MoveClick(double Speed, int depth)
        {
            lTSMC[Axis.Z].RunSpeed = Speed;
            // lTSMC[Axis.Z].SetRunDistanceSync(-(12 * (5000 / 7)));
            lTSMC[Axis.Z].SetRunDistanceSync(-(depth * (5000 / 7)));
            lTSMC[Axis.Z].SetRunDistanceSync((depth * (5000 / 7)));
        }
        /// <summary>
        /// 技能轴点击
        /// </summary>
        /// <param name="Speed"></param>
        /// <param name="depth"></param>
        public void SkillClick(double Speed, int depth)
        {
            lTSMC[Axis.W].RunSpeed = Speed;
            //lTSMC[Axis.W].SetRunDistanceSync(-12);
            lTSMC[Axis.W].SetRunDistanceSync(-depth * (5000 / 7));
            lTSMC[Axis.W].SetRunDistanceSync(depth * (5000 / 7));
        }
        ///// <summary>
        ///// 方向轴点击
        ///// </summary>
        ///// <param name="Speed"></param>
        ///// <param name="depth"></param>
        //public void MoveClick(double Speed, int depth)
        //{
        //    lTSMC[Axis.Z].RunSpeed = Speed;
        //    //lTSMC[Axis.Z].SetRunDistanceSync(-12);
        //    lTSMC[Axis.Z].SetRunDistanceSync(-depth);
        //    lTSMC[Axis.Z].SetRunDistanceSync(depth);
        //}
        /// <summary>
        /// 移动轴直线补偿
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="Axis"></param>
        /// <param name="TargetPosition"></param>
        /// <param name="targetPosition"></param>
        public void SetLineInterMove(int speed, int XTargetPosition, int  YTargetPosition)
        {
            LineInterData InstanceName = new LineInterData(speed, 0.01, 0.01);
            InstanceName.Add(new LineInterItem(Axis.X, XTargetPosition *(5000/25)));
            InstanceName.Add(new LineInterItem(Axis.Y, YTargetPosition * (5000 / 16)));
            lTSMC.SetLineInter(InstanceName);
            loop();
        }
        /// <summary>
        /// 轴速度
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public double AxisSpeed(Axis axis,double speed)
        {
           return lTSMC[axis].RunSpeed = speed;

        }
        /// <summary>
        /// 垂直轴运动
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="height"></param>
        public void PreHeight(Axis axis, int height)
        {
            lTSMC[axis].SetRunDistanceSync(-height * (5000 / 7));
        }

        public void stopAxis(Axis axis)
        {
            lTSMC[axis].Stop();
        }

        public void ContinuousMotion(Axis axis, Direction direction)
        {
            lTSMC[axis].SetAlwaysRunning(direction);
        }
        public void DirMove(double angle)
        {
            double xPosition = GetAxisPosition(Axis.X);
            double yPosition = GetAxisPosition(Axis.Y);
            double xMovePosition = 70 + 10 * (Math.Cos((Math.PI / 180) * (angle % 360)));
            double yMovePosition = 124 +10 * (Math.Sin((Math.PI / 180) * (angle % 360)));
            SetLineInterMove(40000, Convert.ToInt32(xMovePosition - xPosition), Convert.ToInt32(yMovePosition - yPosition));
        }
    }
}

       
     

       
   
