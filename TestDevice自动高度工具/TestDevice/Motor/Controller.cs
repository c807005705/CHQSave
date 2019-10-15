// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Threading;
using Mobot.Utils.IO;
using Mobot.TestBox.Protocols.Motor;
using Mobot.TestBox.Cameras;

namespace Mobot.TestBox.Motor
{
    public partial class Controller : IMotorDriver
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int StylusDirection
        {
            get
            {
                return driver.Direction;
            }
        }

        public Point3D StylusLocation
        {
            get
            {
                return new Point3D(
                    settings.Ticks2MM(driver.Ticks.X, Axis.X),
                    settings.Ticks2MM(driver.Ticks.Y, Axis.Y),
                    settings.Ticks2MM(driver.Ticks.Z, Axis.Z));
            }
        }
        /// <summary>
        /// 机械手触笔的位置(毫米)。
        /// </summary>
        public double StylusLocationX
        {
            get { return settings.Ticks2MM(driver.Ticks.X, Axis.X); }
        }
        public double StylusLocationY
        {
            get { return settings.Ticks2MM(driver.Ticks.Y, Axis.Y); }
        }
        public double StylusLocationZ
        {
            get { return settings.Ticks2MM(driver.Ticks.Z, Axis.Z); }
        }

        public Point2 Normal { get; set; }
        /// <summary>
        /// 判断是否在拍照位置。
        /// </summary>
        public bool CanCapture
        {
            get
            {
                return !this.Moving && driver.Ticks.X == Normal.X && driver.Ticks.Y == Normal.Y;
            }
        }
        public bool Moving { get; private set; }

        private ControllerSettings settings = null;
        public IControllerSettings ControllerSettings
        {
            get { return this.settings; }
        }

        readonly MotorDriver driver = null;
        public Controller(string portName, int baudRate)
        {
            this.driver = new MotorDriver(portName, baudRate);
            this.settings = new ControllerSettings();
            this.Moving = false;
        }

        public Version HardwareVersion
        {
            get { return this.Driver.HardwareVersion; }
        }

        private string serialNumber = string.Empty;
        public string SerialNumber
        {
            get { return this.serialNumber; }
            set { this.serialNumber = value; }
        }
        public string PortName
        {
            get { return driver.PortName; }
            set { driver.PortName = value; }
        }
        public int BaudRate
        {
            get { return driver.BaudRate; }
            set { driver.BaudRate = value; }
        }
        public bool IsOpen
        {
            get { return driver.IsOpen; }
        }
        int sideclickdirection = 0;
        public int SideClickDirection
        {
            get
            {
                return this.sideclickdirection;
            }
            set
            {
                if (this.sideclickdirection == value)
                    return;
                this.sideclickdirection = value;
            }
        }
        /// <summary>
        /// 归位坐标
        /// </summary>
        public Point2D? TouchReset { get; set; }

        /// <summary>
        /// 连续点击Z轴归位高度
        /// </summary>
        public double? QuickPointZ { get; set; }
        /// <summary>
        /// 触屏点击高度
        /// </summary>
        public double? TouchPadClickZ { get; set; }

        Point3 motorSpeed = Point3.Empty;
        /// <summary>
        /// 获取或设置电机速度。
        /// </summary>
        public Point3 MotorSpeed
        {
            get
            {
                motorSpeed = driver.Speed;
                return motorSpeed;
            }
            set
            {
                motorSpeed = value;
                if (driver.IsOpen)
                {
                    try
                    {
                        driver.BatchSetSpeed(motorSpeed);
                        while (driver.IsBusy)
                            Application.DoEvents();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
        }

        Dictionary<string, MobileKeyInfo> dicKeys = new Dictionary<string, MobileKeyInfo>();
        public Dictionary<string, MobileKeyInfo> Keys
        {
            get { return dicKeys; }
        }

        public void Connect()
        {
            //清空动作队列
            queueKeys.Clear();
            try
            {
                driver.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DisConnect()
        {
            try
            {
                //清空动作队列
                queueKeys.Clear();
                driver.Stop();
            }
            catch (Exception ex)
            {
                log.Error("driver.stop时出错。", ex);
            }
            //driver.Status = AgentStatus.NotReady;
        }

        public event EventHandler<SerialPortIOEventArgs> IO
        {
            add { driver.IO += value; }
            remove { driver.IO -= value; }
        }
        public bool IsBusy { get { return bw != null && bw.IsBusy; } }
        private bool _reset = true;
        public bool IsReset { get { return _reset; } set { _reset = value; } }
        public void BatchActionsAsync(IList<IMotorAction> actions, bool ithrow = false)
        {
            if (actions == null)
                return;
            lock (queueKeys)
            {
                queueKeys.Enqueue(new ActionBatch(actions));
            }

            if (debugMode)
            {
                if (bw == null)
                {
                    bw = new BackgroundWorker();
                    bw.WorkerReportsProgress = true;
                    bw.WorkerSupportsCancellation = true;
                    bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                }
                if (!bw.IsBusy)
                    bw.RunWorkerAsync();
            }
            else
            {
                try
                {
                    DoWork(null);
                    //同步操作，则回零位
                    //BatchReset();
                }
                catch (Exception ex)
                {
                    //出错了，尝试归零
                    log.Error("测试盒点击过程中出现异常。", ex);
                    //若有安全高度，先到安全高度再出场归位
                    if (ithrow)
                        throw;
                    else
                        driver.BatchReset();
                }
            }
        }

        //DateTime lastQuickZ = DateTime.Now;
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            DoWork(bw);
        }
        void DoWork(BackgroundWorker bw)
        {
            try
            {
                this.Moving = true;
                DoWork2(bw);
            }
            finally
            {
                this.Moving = false;
            }
        }
        void DoWork2(BackgroundWorker bw)
        {
            while (true)
            {
                if (bw != null && bw.CancellationPending)
                    break;

                //如果队列为空，则等待一段时间
                int count = 0;
                lock (queueKeys)
                    count = queueKeys.Count;
                int startTime = Environment.TickCount;
                while (count < 1 && Environment.TickCount - startTime < this.delayBeforeReset)
                {
                    Thread.Sleep(100);
                    lock (queueKeys)
                        count = queueKeys.Count;
                }

                ActionBatch action = null;
                lock (queueKeys)
                {
                    if (queueKeys.Count < 1)
                        break;
                    action = queueKeys.Dequeue();
                }
                if (action == null)
                    continue;
                if (driver.Ticks.X == 0 && driver.Ticks.Y == 0 && action.GetType() == typeof(BatchWorkZ))
                    continue;

                this.BatchActions(action.Parts.ToArray());
                //Thread.Sleep(300);
                if (this.IsReset)
                {
                    //若有安全高度，先到安全高度再出场归位
                    BatchReset();
                }
                else
                {
                    if (this.QuickPointZ != null)
                        this.StylusMoveZAsync((double)this.QuickPointZ);
                    else
                        this.StylusMoveSafeAsync();
                }
            }
        }

        /// <summary>
        /// 写入日志方法
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLog(string message)
        {
            string strMessage = string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), message);
            string path = @"D:\TestBoxAgent.log";
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path).Close();
            }
            using (System.IO.StreamWriter sw = System.IO.File.AppendText(path))
            {
                sw.WriteLine(strMessage);
                sw.Flush();
                sw.Close();
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                log.Error("测试盒点击过程中出现异常。", e.Error);
            lock (queueKeys)
            {
                if (queueKeys.Count > 0)
                {
                    if (!bw.IsBusy)
                        bw.RunWorkerAsync();
                }
            }
        }
        public Point2 GetNormal()
        {
            int x = 0, y = 0;
            if (TouchReset != null && TouchReset.Value != Point2D.Empty)
            {
                x = settings.MM2Ticks(TouchReset.Value.X, Axis.X);
                y = settings.MM2Ticks(TouchReset.Value.Y, Axis.Y);
            }
            return new Point2(x, y);
        }
        public void BatchReset()
        {
            if (TouchReset != null)
                driver.BatchReset2(Normal, MotorSpeed);
            else
                driver.BatchReset();
        }

        public void BatchActions(params IMotorAction[] actions)
        {
            List<MotorDriver.ICommand> commands = new List<MotorDriver.ICommand>();
            foreach (IMotorAction item in actions)
            {
                if (item is BatchSleep)
                {
                    BatchSleep temp = item as BatchSleep;

                    for (int i = 0; i < temp.MS / 20000; i++)
                    {
                        BatchSleep t = new BatchSleep(20000);
                        commands.Add(t.ToMotorCommand(this.settings));
                    }

                    if (temp.MS % 20000 > 0)
                    {
                        BatchSleep t = new BatchSleep(temp.MS % 20000);
                        commands.Add(t.ToMotorCommand(this.settings));
                    }

                    continue;
                }
                commands.Add(item.ToMotorCommand(this.settings));
            }
            driver.Batch(commands.ToArray());
        }
    }
}
