
using System;
using System.Collections.Generic;
using Protocols;
using Mobot.Utils.IO;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using Mobot;
using TestDevice;
using Motor.Protocols.Messages;

namespace Motor
{
    /// <summary>
    /// MotoController
    /// </summary>
    public partial class MotorDriver : ClientBase, IMotorDriver
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Object BatchLocker = new Object();
        public static Point3 ResetSpeed = new Point3(20, 20, 100);
        public static Point3 NormalSpeed = new Point3(400, 400, 150);

        /// <summary>
        /// 电机坐标变化。
        /// </summary>
        public event EventHandler LocationChanged;
        public event EventHandler StateChanged;
        public event EventHandler<SerialPortIOEventArgs> IO;

        new Rs232Device device = null;
        public int Trigger { get; set; }

        public string PortName
        {
            get { return device.PortName; }
            set { device.PortName = value; }
        }
        public int BaudRate
        {
            get { return device.BaudRate; }
            set { device.BaudRate = value; }
        }

        /// <summary>
        /// 控制器固件版本（当连接时后数据有效）
        /// </summary>
        public Version HardwareVersion { get; private set; }
        /// <summary>
        /// 片内FLASH页大小（当连接时后数据有效）
        /// </summary>
        public UInt16 FlashPageSize { get; private set; }
        /// <summary>
        /// 配置项片内FLASH页总数（当连接时后数据有效）
        /// </summary>
        public UInt16 FlashPageCount { get; private set; }

        bool isOpen = false;
        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
            private set
            {
                if (isOpen == value)
                    return;
                isOpen = value;
                OnStateChanged();
            }
        }
        protected virtual void OnStateChanged()
        {
            if (this.StateChanged != null)
                this.StateChanged(this, EventArgs.Empty);
        }
        /// <summary>
        /// 刷新设备的状态。
        /// </summary>
        void RefreshState()
        {
            bool newValue = false;
            try
            {
                newValue = device.IsOpen;
            }
            catch (Exception ex)
            {
                log.Error("刷新设备的状态出错:" + ex);
                newValue = false;
            }
            //Control.BeginInvoke(new Action<bool>(MdoIsOpen), newValue);
            new Actions(MdoIsOpen).BeginInvoke(newValue, null, null);
            //this.IsOpen = newValue;
        }
        /// <summary>
        /// 改变this.IsOpen的委托
        /// </summary>
        /// <param name="iso"></param>
        public delegate void Actions(bool iso);
        private void MdoIsOpen(bool iso)
        {
            this.IsOpen = iso;
        }
        /// <summary>
        /// 
        /// </summary>
        public int InitialiteZ { get; private set; }
        /// <summary>
        /// 电机移动速度。
        /// </summary>
        public Point3 Speed { get; set; }

        private Point3 ticks = Point3.Empty;
        /// <summary>
        /// 电机当前坐标的Tick值(Tick)。
        /// </summary>
        public Point3 Ticks
        {
            get { return this.ticks; }
            set
            {
                if (this.ticks == value)
                    return;
                this.ticks = value;
                if (this.LocationChanged != null)
                    this.LocationChanged(this, EventArgs.Empty);
            }
        }
        private int direction = 0;
        /// <summary>
        /// 电机当前侧键方向。
        /// </summary>
        public int Direction
        {
            get { return this.direction; }
            private set
            {
                if (this.direction == value)
                    return;
                this.direction = value;

                //if (this.LocationChanged != null)
                //    this.LocationChanged(this, EventArgs.Empty);
            }
        }
        public Version Reversion { get; private set; }
        public string ControlCom { get; set; }
        private int resetTime = 25;
        public int ResetTime { get { return resetTime; } set { resetTime = value; } }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy == value)
                    return;
                isBusy = value;
            }
        }
        /// <summary>
        /// 屏幕检测失败，停止脚本
        /// </summary>
        public ModeType ModeType { get; set; }

        public MotorDriver(string portName, int baudRate)
            : base(new Rs232Device(portName, baudRate))
        {
            this.device = base.device as Rs232Device;
            Ticks = Point3.Empty;
            this.Reversion = new Version();
            this.Speed = Point3.Empty;

            this.HardwareVersion = new Version(0, 0, 0, 0);
            this.FlashPageCount = 2048;
            this.FlashPageSize = 1;
            this.device.IO += Device_IO;
        }
        private void Device_IO(object sender, SerialPortIOEventArgs e)
        {
            if (this.IO != null)
                this.IO(this, e);
        }

        protected override Type[] GetMessageTypes()
        {
            return MotorMessages.GetMessageTypes();
        }

        bool isSending = false;
        object obj = new object();
        public MessageBase Request(MessageBase message, double addTimeOut = 0, bool iReset = true)
        {
            lock (obj)
            {
                if (this.isSending)
                    throw new InvalidOperationException("上次操作尚未完成。");
                ResetType result = ResetType.None;
                this.isSending = true;
                MID respMid;
                MessageBase respMsg;
                try
                {
                    byte midValue;
                    base.Request(message, out midValue, out respMsg, addTimeOut);
                    respMid = (MID)midValue;
                }
                catch (Exception ex)
                {
                    this.isSending = false;
                    log.Error("设备命令执行错误:", ex);
                    if (iReset && ITimeOut(ex.Message))
                    {
                        result = ResetCom();
                        if (result == ResetType.Success)
                        {
                            respMsg = new ResetDeviceResponseMessage();
                            return respMsg;
                        }
                    }
                    UpdateErrorString(ex);
                    throw new Exception(ex.Message);
                }
                finally
                {
                    this.isSending = false;
                }
                if (respMid == MID.ResetDeviceResponse)
                {
                    log.Error(string.Format("{0} 返回空的重启响应消息", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                    return respMsg;
                }
                if (respMid == MID.ErrorResponse)
                {
                    FailureMessage failure = respMsg as FailureMessage;
                    string desc = ErrorHelper.ErrorCode(failure.Reason);
                    log.Error(desc);
                    throw new Exception(desc);
                }
                return respMsg;
            }
        }
        #region 重连
        public bool ITimeOut(string msg)
        {
            if (msg == null) return false;
            msg = msg.ToLower();
            if (msg.IndexOf("超时") != -1 ||
                msg.IndexOf("timeout") != -1 ||
                msg.IndexOf("连到系统上的设备没有发挥作用") != -1 ||
                msg.IndexOf("端口被关闭") != -1 ||
                msg.IndexOf("port is closed") != -1)
                return true;
            return false;
        }
        public ResetType ResetComUSB()
        {
            try
            {
                try
                {
                    Thread.Sleep(1000 * 1);
                    this.Stop();
                }
                catch (Exception ex) { log.Error("Stop1=>" + ex.Message); }
                string com = GetControlCom();
                ResetUSB(com, "USB重连", new byte[] { 0xA5, 0x55, 0x02, 0xFC, 0x5A });
                Thread.Sleep(1000 * 3);
                try
                {
                    base.Start(false);
                    return ResetType.USBSuccess;
                }
                catch
                {
                    return ResetCom();
                }
            }
            catch (Exception ex)
            {
                log.Error("重连端口失败：" + ex.Message);
                return ResetType.Error;
            }
        }
        public ResetType ResetCom()
        {
            try
            {
                try
                {
                    Thread.Sleep(100);
                    this.Stop();
                }
                catch (Exception ex) { log.Error("ResetCom.Stop=>" + ex.Message); }
                string com = GetControlCom();
                ResetUSB(com, "断电重连", new byte[] { 0xA5, 0x50, 0x02, 0xF7, 0x5A });
                Thread.Sleep(1000 * ResetTime);
                {
                    com = GetUSBCom();
                    if (com != null) this.PortName = com;
                    this.Start();
                }
                return ResetType.Success;
            }
            catch (Exception ex)
            {
                log.Error("重连端口失败：" + ex.Message);
                return ResetType.Error;
            }
        }
        private void ResetUSB(string com, string name, byte[] data)
        {
            SerialPort port = new SerialPort(com);
            try
            {
                port.Open();
                port.Write(data, 0, data.Length);

                byte[] buffer = new byte[port.ReadBufferSize];
                int length = port.Read(buffer, 0, buffer.Length);
                string str = null;
                for (int i = 0; i < length; i++)
                {
                    str = string.Format("{0}{1:X2} ", str, buffer[i]);
                }
                log.Error(name + "成功" + str.TrimEnd());
            }
            catch (Exception ex)
            {
                log.Error(name + "失败：" + ex);
                throw;
            }
            finally
            {
                port.Close();
            }
        }
        private string GetUSBCom()
        {
            List<USBDeviceHelper.DeviceProperties> usbDPs = new List<USBDeviceHelper.DeviceProperties>();
            List<USBDeviceHelper.DeviceProperties> result = new List<USBDeviceHelper.DeviceProperties>();
            USBDeviceHelper.GetUSBDevices(USBDeviceHelper.DefaultDeviceVID, USBDeviceHelper.DefaultDevicePID, ref usbDPs, true);
            foreach (USBDeviceHelper.DeviceProperties dp in usbDPs)
            {
                if (!dp.BaseFilename.Contains("Testbox Power Controler"))
                    return dp.COMPort;
            }
            return null;
        }
        private string GetControlCom()
        {
            string com = ControlCom;
            List<USBDeviceHelper.DeviceProperties> usbDPs = new List<USBDeviceHelper.DeviceProperties>();
            List<USBDeviceHelper.DeviceProperties> result = new List<USBDeviceHelper.DeviceProperties>();
            USBDeviceHelper.GetUSBDevices(USBDeviceHelper.DefaultDeviceVID, USBDeviceHelper.DefaultDevicePID, ref usbDPs, true);
            foreach (USBDeviceHelper.DeviceProperties dp in usbDPs)
            {
                if (dp.BaseFilename.Contains("Testbox Power Controler"))
                {
                    com = dp.COMPort;
                    break;
                }
            }
            if (com == null)
            {
                throw new Exception("控制口不存在");
            }
            return com;
        }

        #endregion

        #region 错误日志写到文件
        public string LastErrorString { get; private set; }
        private DateTime LastErrorUpdate;
        public void UpdateErrorString(Exception ex)
        {
            Console.WriteLine(ex.Message);
            //屏蔽2秒内的频繁错误更新
            if (DateTime.Now.Subtract(LastErrorUpdate).TotalSeconds < 2) return;

            LastErrorUpdate = DateTime.Now;
            LastErrorString = GetErrorString(ex);
            WriteErrorMessage(LastErrorString);
        }
        private string GetErrorString(Exception ex)
        {
            string result = string.Empty;

            if (ex is MotorException)
            {
                result = string.Format("[{0}][{1}]{2}", "控制器异常", (ex as MotorException).ErrorCode, ex.Message);
            }
            else
            {
                result = ex.Message;
            }
            log.Error("写到图片上的异常信息：" + ex);

            return result;
        }
        /// <summary>
        /// 错误日志写入到文件中
        /// </summary>
        private void WriteErrorMessage(string LastErrorString)
        {
            try
            {
                string txtPath = Path.Combine(Environment.CurrentDirectory, "logs", Process.GetCurrentProcess().Id + "_Error.xml");
                using (FileStream fs = File.Create(txtPath))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        string strData = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                                          + "<Root>"
                                          + "<DeviceErrorMessage>{0}"
                                          + "</DeviceErrorMessage>"
                                          + "</Root>", LastErrorString);
                        sw.Write(strData);
                        sw.Flush();
                        sw.Close();
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion

        #region 重载方法
        public override void Start(bool iReset = true)
        {
            try
            {
                base.Start();

                SystemInfoResponseMessage sysInfo = this.GetSystemInfo();
                this.HardwareVersion = sysInfo.HardwareVersion;
                this.FlashPageCount = sysInfo.FlashPageCount;
                this.FlashPageSize = sysInfo.FlashPageSize;
                if (iReset)
                {
                    this.BatchReset();
                }
            }
            catch (MotorException ex)
            {
                log.ErrorFormat("Start异常:" + ((int)ex.ErrorCode).ToString("X8") + " " + ex.ErrorDesc);
                throw ex;
            }
            catch (Exception ex)
            {
                try
                {
                    base.Stop();
                }
                catch
                { }
                log.Error("Start异常:" + ex);
                throw ex;
            }
            finally
            {
                RefreshState();
            }
        }
        public override void Stop()
        {
            try
            {
                lock (BatchLocker)
                {
                    base.Stop();
                }
            }
            catch (Exception ex)
            {
                log.Error("Stop异常:" + ex);
                throw ex;
            }
            finally
            {
                RefreshState();
            }
        }

        #endregion

        #region 同步方法
        /// <summary>
        /// 发送PING消息
        /// 设备是否忙碌
        /// 0 = Agent 空闲
        /// 1 = Agent 忙
        /// </summary>
        public int Ping()
        {
            log.InfoFormat("向测试盒发送命令 {0}()", MethodBase.GetCurrentMethod().Name);
            PingMessage request = new PingMessage();
            PingRespMessage response = Request(request) as PingRespMessage;
            //Agent状态
            this.IsBusy = (response.Status == 1);

            return response.Status;
        }

        /// <summary>
        /// 重置设备。
        /// </summary>
        public void ResetDevice()
        {
            log.InfoFormat("向测试盒发送命令 {0}()", MethodBase.GetCurrentMethod().Name);

            try
            {
                //无论是否部分连接测试盒，都尝试重新开启端口
                base.Start();
            }
            catch { }
            ResetDeviceMessage request = new ResetDeviceMessage();
            ResetDeviceResponseMessage response = Request(request) as ResetDeviceResponseMessage;
        }

        /// <summary>
        /// 获取位置
        /// </summary>
        public void GetPosition()
        {
            log.InfoFormat("向测试盒发送命令 {1}()", MethodBase.GetCurrentMethod().Name);
            GetPositionMessage request = new GetPositionMessage();
            GetPositionRespMessage response = Request(request) as GetPositionRespMessage;

            this.Ticks = response.StylusLocation;
        }

        public byte[] ReadFlash(int index)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, index);
            ReadFlashMessage request = new ReadFlashMessage((byte)index);
            ReadFlashResponseMessage response = Request(request) as ReadFlashResponseMessage;
            return response.Data;
        }

        public void WriteFlash(int index, byte[] data)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, index);
            WriteFlashMessage request = new WriteFlashMessage((byte)index, data);
            WriteFlashResponseMessage response = Request(request) as WriteFlashResponseMessage;
        }

        public SystemInfoResponseMessage GetSystemInfo()
        {
            log.InfoFormat("向测试盒发送命令 {0}()", MethodBase.GetCurrentMethod().Name);
            SystemInfoMessage request = new SystemInfoMessage();
            SystemInfoResponseMessage response = Request(request, 0, false) as SystemInfoResponseMessage;

            this.Reversion = response.HardwareVersion;
            return response;
        }

        /// <summary>
        /// 直接设置触发模式
        /// </summary>
        public void MoveZ(int z, int tigger = 0)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1},{2})", MethodBase.GetCurrentMethod().Name, z, tigger);
            MoveMessage request = new MoveMessage(z);
            request.Trigger = tigger;
            MoveResponseMessage response = Request(request) as MoveResponseMessage;

            //Move消息响应
            this.Ticks = new Point3(Ticks.X, Ticks.Y, z);
        }

        public void MoveXY(int x, int y)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1},{2})", MethodBase.GetCurrentMethod().Name, x, y);
            MoveMessage request = new MoveMessage(x, y);
            MoveResponseMessage response = Request(request) as MoveResponseMessage;

            //Move消息响应
            this.Ticks = new Point3(x, y, Ticks.Z);
        }

        public void MoveXYZ(int x, int y, int z)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1},{2},{3})", MethodBase.GetCurrentMethod().Name, x, y, z);
            MoveMessage request = new MoveMessage(x, y, z);
            MoveResponseMessage response = Request(request) as MoveResponseMessage;

            //Move消息响应
            this.Ticks = new Point3(x, y, z);
        }

        /// <summary>
        /// Z轴回零
        /// </summary>
        public void StylusRestZ()
        {
            log.InfoFormat("向测试盒发送命令 {0}()", MethodBase.GetCurrentMethod().Name);
            StylusRestZMessage pressure = new StylusRestZMessage();
            StylusRestZResponseMessage response = Request(pressure) as StylusRestZResponseMessage;
        }
        #endregion

        #region 异步批处理
        /// <summary>
        /// 重置电机位置。
        /// </summary>
        public void BatchReset()
        {
            log.InfoFormat("尝试重置电机位置, 将执行重置电机位置的Batch动作");
            if (this.ModeType == ModeType.Stop)
            {
                this.ModeType = ModeType.None;
                //this.Batch(
                //    new CommandReset(),
                //    new CommandMotorSpeedReset());
                new CommandReset().Work(this);
                new CommandMotorSpeedReset().Work(this);
            }
            else
            {
                //this.Batch(
                //    new CommandMoveZ(20),
                //    new CommandMoveXY(100, 100),
                //    new CommandReset(),
                //    new CommandMotorSpeedReset());
                new CommandMoveZ(20).Work(this);
                new CommandMoveXY(100, 100).Work(this);
                new CommandReset().Work(this);
                new CommandMotorSpeedReset().Work(this);
            }
        }
        /// <summary>
        /// 归位
        /// </summary>
        public void BatchReset2(Point2 normal)
        {
            log.InfoFormat("伪原点归位");
            this.Batch(
                    new CommandMoveZ(20),
                    new CommandMoveXY(normal.X, normal.Y),
                    new CommandMotorSpeedReset());
        }

        /// <summary>
        /// 设置电机移动速度。
        /// </summary>
        public void BatchSetSpeed(Point3 point)
        {
            if (point.X < 1 || point.Y < 1 || point.Z < 1)
                return;

            this.Batch(new CommandMotorSpeed(point));
        }
        public void BatchMoveXY(int xTick, int yTick)
        {
            this.Batch(new CommandMoveXY(xTick, yTick));
        }
        public void BatchMoveZ(int zTick)
        {
            this.Batch(new CommandMoveZ(zTick));
        }

        #endregion

        #region Batch执行
        public void Batch(params IMotorCommand[] actions)
        {
            try
            {
                if (actions == null || actions.Length < 1)
                    return;
                if (this.ModeType == ModeType.Stop)
                    throw new MotorException(ErrorValues.E07000001, "测试盒运行停止，请归位");
                this.ModeType = ModeType.Start;

                Update(actions);
                lock (BatchLocker)
                {
                    if (actions.Length == 1)
                    {
                        actions[0].Work(this);
                        return;
                    }

                    Random rd = new Random();
                    this.SetBatchMode(BatchMode.Clear);
                    this.SetBatchMode(BatchMode.Start);
                    int timeoutCount = 0;
                    foreach (IMotorCommand action in actions)
                    {
                        if (action is CommandSleep)
                        {
                            CommandSleep command = action as CommandSleep;
                            timeoutCount += command.MS;
                        }
                        action.Work(this);
                    }
                    this.ModeType = ModeType.Run;
                    timeoutCount += actions.Length * 1000;
                    this.SetBatchMode(BatchMode.End, timeoutCount / 20000);
                    if (this.ModeType == ModeType.Stop)
                        throw new MotorException(ErrorValues.E07000001, "测试盒运行停止，请归位");
                    this.ModeType = ModeType.Finished;
                }
            }
            catch (MotorException ex)
            {
                log.Error("Batch错误0x36：" + ((int)ex.ErrorCode).ToString("X8") + " " + ex.ErrorDesc);
                throw ex;
            }
        }
        private void Update(params IMotorCommand[] actions)
        {
            int count = 0;
            switch ((TriggerType)this.Trigger)
            {
                case TriggerType.None:
                    break;
                case TriggerType.MoveStart:
                case TriggerType.HardEnd:
                    count = 1;
                    break;
                case TriggerType.HardStart:
                case TriggerType.MoveEnd:
                case TriggerType.GroupStart:
                case TriggerType.GroupEnd:
                    count = 2;
                    break;
            }
            int lastZ = -1;
            switch ((TriggerType)this.Trigger)
            {
                case TriggerType.HardStart:
                case TriggerType.HardEnd:
                    for (int i = actions.Length - 1; i >= 0; i--)
                    {
                        if (HardTrigger(ref count, actions[i], ref lastZ))
                            break;
                    }
                    break;
                case TriggerType.GroupEnd:
                    for (int i = 0; i < actions.Length; i++)
                    {
                        if (HardTrigger(ref count, actions[i], ref lastZ, true))
                            break;
                    }
                    break;
                case TriggerType.MoveStart:
                case TriggerType.MoveEnd:
                    for (int i = actions.Length - 1; i >= 0; i--)
                    {
                        if (MoveTrigger(ref count, actions[i], this.Trigger))
                            break;
                    }
                    break;
                case TriggerType.GroupStart:
                    for (int i = 0; i < actions.Length; i++)
                    {
                        if (MoveTrigger(ref count, actions[i], (int)TriggerType.MoveStart))
                            break;
                    }
                    break;
            }
            this.Trigger = 0;
        }
        private bool HardTrigger(ref int count, IMotorCommand item, ref int lastZ, bool group = false)
        {
            if (item is CommandMoveZ)
            {
                CommandMoveZ moveZ = item as CommandMoveZ;
                if (group)
                {
                    if (moveZ.Z != lastZ)
                    {
                        count--;
                        lastZ = moveZ.Z;
                    }
                }
                else count--;
                if (count == 0)
                {
                    moveZ.Trigger = (int)TriggerType.HardStart;
                    return true;
                }
            }
            return false;
        }
        private bool MoveTrigger(ref int count, IMotorCommand item, int trigger)
        {
            if (item is CommandMoveXY)
            {
                count--;
                CommandMoveXY moveXY = item as CommandMoveXY;
                if (count == 0)
                {
                    moveXY.Trigger = trigger;
                    return true;
                }
            }
            else if (item is CommandMove)
            {
                count--;
                CommandMove move = item as CommandMove;
                if (count == 0)
                {
                    move.Trigger = trigger;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region 设备命令
        public void Reset()
        {
            log.InfoFormat("向测试盒发送命令 {0}()", MethodBase.GetCurrentMethod().Name);

            ResetMessage request = new ResetMessage();
            ResetResponseMessage response = Request(request) as ResetResponseMessage;
        }
        /// <summary>
        /// 停止当前运行
        /// </summary>
        public void ModeStop()
        {
            log.InfoFormat("向测试盒发送命令 {0}()", MethodBase.GetCurrentMethod().Name);

            int random = new Random().Next(240);
            byte[] buffer = new byte[] { 0xF1, 0x2D, 0xF4, (byte)random, 0x00, 0xF2 };
            buffer = GetBuffer(buffer);
            string msg = null;
            for (int i = 0; i < buffer.Length; i++)
            {
                msg = string.Format("{0}{1:X2} ", msg, buffer[i]);
            }
            log.WarnFormat("==============>运行停止<{0}>", msg);
            device.Write(buffer, 0, buffer.Length);

            //StopMessage request = new StopMessage();
            //StopResponseMessage response = Request(request) as StopResponseMessage;
            //device.Port.Write("STOPMOBOT");
        }
        private byte[] GetBuffer(byte[] buffer)
        {
            byte total = 0;
            for (int i = 0; i < buffer.Length - 2; i++)
            {
                total += buffer[i];
            }
            if (total < 0xF1 || total > 0xF4)
            {
                buffer[buffer.Length - 2] = total;
            }
            else
            {
                byte[] temp = new byte[buffer.Length + 1];
                for (int i = 0; i < buffer.Length - 2; i++)
                {
                    temp[i] = temp[i];
                }
                temp[temp.Length - 3] = 0xF3;
                switch (total)
                {
                    case 0xF1: temp[temp.Length - 2] = 0xB1; break;
                    case 0xF2: temp[temp.Length - 2] = 0xB2; break;
                    case 0xF3: temp[temp.Length - 2] = 0xB3; break;
                    case 0xF4: temp[temp.Length - 2] = 0xB4; break;
                }
                temp[temp.Length - 1] = buffer[buffer.Length - 1];
                buffer = temp;
            }
            return buffer;
        }
        /// <summary>
        /// 校准
        /// </summary>
        public void SetBatchMode(BatchMode mode, int addTimeOut = 0)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1},{2})", MethodBase.GetCurrentMethod().Name, mode, addTimeOut);
            BatchModeMessage request = new BatchModeMessage(mode);
            BatchResponseMessage response = Request(request, addTimeOut) as BatchResponseMessage;
        }
        /// <summary>
        /// 重复
        /// </summary>
        public void SetBatchTimes(int times)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, times);
            BatchTimesMessage request = new BatchTimesMessage(times);
            BatchResponseMessage response = Request(request) as BatchResponseMessage;
        }
        /// <summary>
        /// Batch停止
        /// </summary>
        public void BatchStop()
        {
            log.InfoFormat("向测试盒发送命令 {0}()", MethodBase.GetCurrentMethod().Name);
            BatchStopMessage request = new BatchStopMessage();
            BatchStopResponseMessage response = Request(request) as BatchStopResponseMessage;
        }

        #endregion

        #region 外部命令
        /// <summary>
        /// 设置内部光源
        /// </summary>
        public void SetIndoorLight(bool status)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, status);

            SetIndoorLightMessage request = new SetIndoorLightMessage(status ? 1 : 0);
            SetIndoorLightResponseMessage response = Request(request) as SetIndoorLightResponseMessage;
        }
        /// <summary>
        /// 设置手机测试状态
        /// </summary>
        public void TestStatu(bool statu1, bool statu2)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1},{2})", MethodBase.GetCurrentMethod().Name, statu1, statu2);

            TestStatuMessage request = new TestStatuMessage(statu1 ? 1 : 0, statu2 ? 1 : 0);
            TestStatuResponseMessage response = Request(request) as TestStatuResponseMessage;
        }
        /// <summary>
        /// 查询是否可以开始测试
        /// </summary>
        public byte TestQuery(bool status)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, status);

            TestQueryMessage request = new TestQueryMessage(status ? 1 : 0);
            TestQueryResponseMessage response = Request(request) as TestQueryResponseMessage;
            return response.Statu;
        }
        /// <summary>
        /// USBHUB手机充电使能或关闭
        /// true充电，false断电
        /// </summary>
        public void SetUsbHub(int index, bool status)
        {
            index = index * 2 + (status ? 1 : 0);
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, index);

            SetUsbHubMessage request = new SetUsbHubMessage(index);
            SetUsbHubResponseMessage response = Request(request) as SetUsbHubResponseMessage;
        }
        /// <summary>
        /// USB充电模式选择命令
        /// true数据通信，false无(充电)
        /// </summary>
        public void SetUsbMode(int index, bool status)
        {
            index = index * 2 + (status ? 1 : 0);
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, index);

            SetUsbModeMessage request = new SetUsbModeMessage(index);
            SetUsbModeResponseMessage response = Request(request) as SetUsbModeResponseMessage;
        }

        #endregion

        #region MMI测试
        /// <summary>
        /// 手机固定夹紧命令
        /// </summary>
        public void PhoneClamped(bool status)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, status);

            PhoneClampedMessage request = new PhoneClampedMessage(status ? 1 : 0);
            PhoneClampedResponseMessage response = Request(request) as PhoneClampedResponseMessage;
        }
        /// <summary>
        /// 三个侧键点击命令
        /// </summary>
        public void SideKey(int key, int time)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1},{2})", MethodBase.GetCurrentMethod().Name, key, time);

            SideKeyMessage request = new SideKeyMessage(key, time);
            SideKeyResponseMessage response = Request(request) as SideKeyResponseMessage;
        }
        /// <summary>
        /// 音频数据线插拔命令
        /// </summary>
        public void AudioSwap(bool status)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, status);

            AudioSwapMessage request = new AudioSwapMessage(status ? 1 : 0);
            AudioSwapResponseMessage response = Request(request) as AudioSwapResponseMessage;
        }
        /// <summary>
        /// 电源数据线插拔命令
        /// </summary>
        public void PowerSwap(bool status)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, status);

            PowerSwapMessage request = new PowerSwapMessage(status ? 1 : 0);
            PowerSwapResponseMessage response = Request(request) as PowerSwapResponseMessage;
        }
        /// <summary>
        /// 夹具推进推出命令
        /// </summary>
        public void FixInOut(bool status)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, status);

            FixInOutMessage request = new FixInOutMessage(status ? 1 : 0);
            FixInOutResponseMessage response = Request(request) as FixInOutResponseMessage;
        }
        /// <summary>
        /// 前置摄像头色卡缩进推出命令
        /// </summary>
        public void PreCamera(bool status)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, status);

            PreCameraMessage request = new PreCameraMessage(status ? 1 : 0);
            PreCameraResponseMessage response = Request(request) as PreCameraResponseMessage;
        }
        /// <summary>
        /// 后置摄像头背光灯命令
        /// </summary>
        public void BackCamera(bool status)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, status);

            BackCameraMessage request = new BackCameraMessage(status ? 1 : 0);
            BackCameraResponseMessage response = Request(request) as BackCameraResponseMessage;
        }
        /// <summary>
        /// 麦克探头缩进伸出命令
        /// </summary>
        public void MikeInOut(bool status)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, status);

            MikeInOutMessage request = new MikeInOutMessage(status ? 1 : 0);
            MikeInOutResponseMessage response = Request(request) as MikeInOutResponseMessage;
        }
        /// <summary>
        /// 翻转测试命令
        /// </summary>
        public void Flip()
        {
            log.InfoFormat("向测试盒发送命令 {0}()", MethodBase.GetCurrentMethod().Name);

            FlipMessage request = new FlipMessage();
            FlipResponseMessage response = Request(request) as FlipResponseMessage;
        }
        /// <summary>
        /// 三色状态指示灯命令
        /// </summary>
        public void LightStatu(int status)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, status);

            LightStatuMessage request = new LightStatuMessage(status);
            LightStatuResponseMessage response = Request(request) as LightStatuResponseMessage;
        }

        #endregion

        #region 指纹设备
        /// <summary>
        /// 笔头选择
        /// </summary>
        public void PenSelect(int pen)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, pen);
            PenMessage request = new PenMessage(pen);
            PenResponseMessage response = Request(request) as PenResponseMessage;
        }
        /// <summary>
        /// 笔头水平旋转角度协议
        /// </summary>
        public void PenAngle(int angele)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1})", MethodBase.GetCurrentMethod().Name, angele);

            PenAngleMessage request = new PenAngleMessage(angele);
            PenAngleResponseMessage response = Request(request) as PenAngleResponseMessage;
        }
        /// <summary>
        /// 笔头归位
        /// </summary>
        public void PenReset()
        {
            log.InfoFormat("向测试盒发送命令 {0}()", MethodBase.GetCurrentMethod().Name);

            PenResetMessage request = new PenResetMessage();
            PenResetResponseMessage response = Request(request) as PenResetResponseMessage;
        }
        /// <summary>
        /// 笔头角度归位
        /// </summary>
        public void PenAngleReset()
        {
            log.InfoFormat("向测试盒发送命令 {0}()", MethodBase.GetCurrentMethod().Name);

            PenAngleResetMessage request = new PenAngleResetMessage();
            PenAngleResetResponseMessage response = Request(request) as PenAngleResetResponseMessage;
        }

        /// <summary>
        /// 压力检测
        /// </summary>
        public bool Pressure()
        {
            log.InfoFormat("向测试盒发送命令 {0}()", MethodBase.GetCurrentMethod().Name);

            PressureMessage request = new PressureMessage();
            PressureResponseMessage response = Request(request) as PressureResponseMessage;
            return response.Status == 0x31;
        }
        /// <summary>
        /// 压力数值检测
        /// </summary>
        /// <returns></returns>
        public int StylusStatue()
        {
            log.InfoFormat("向测试盒发送命令 {0}()", MethodBase.GetCurrentMethod().Name);
            PressureGramsMessage pressure = new PressureGramsMessage();
            PressureGramsResponseMessage response = Request(pressure) as PressureGramsResponseMessage;
            return response.Value;
        }

        #endregion

        /// <summary>
        /// 画圆孤
        /// </summary>
        public void DrawCircle(int type, int length, int radiu)
        {
            log.InfoFormat("向测试盒发送命令 {0}({1},{2},{3})", MethodBase.GetCurrentMethod().Name, type, length, radiu);

            DrawCircleMessage request = new DrawCircleMessage(type, length, radiu);
            DrawCircleResponseMessage response = Request(request) as DrawCircleResponseMessage;
        }
    }
    public enum ResetType
    {
        None = 0,
        USBSuccess,
        Success,
        Error,
    }
}
