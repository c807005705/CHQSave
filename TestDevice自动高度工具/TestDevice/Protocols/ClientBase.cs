
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using TestDevice;

namespace Protocols
{
    /// <summary>
    /// MTC 上位机程序基类。
    /// </summary>
    public abstract class ClientBase
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected IDevice device = null;

        protected MessageFormatter formatter = new MessageFormatter();
        /// <summary>
        /// Key=MessageID Value=Type
        /// </summary>
        protected Dictionary<byte, Type> dicMessageTypes = new Dictionary<byte, Type>();
        public ClientBase(IDevice device)
        {
            this.device = device;

            //初始化支持的消息列表
            foreach (Type type in this.GetMessageTypes())
            {
                MessageDescAttribute[] attribs = type.GetCustomAttributes(typeof(MessageDescAttribute), false) as MessageDescAttribute[];
                try
                {
                    
                    if (attribs.Length > 0 && attribs[0] != null)
                    {
                        MessageDescAttribute attr = attribs[0];
                        formatter.AddDesc(attr.Description);

                        dicMessageTypes.Add(attr.Description.MID, type);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        /// <summary>
        /// 获取支持的消息列表。
        /// </summary>
        /// <returns></returns>
        protected abstract Type[] GetMessageTypes();

        static byte g_Number = 0;
        /// <summary>
        /// 获取一个计数。
        /// </summary>
        /// <returns></returns>
        static byte GetCounter()
        {
            // generate mesage ID
            byte Number = (byte)(new Random().Next(byte.MaxValue));
            if (Number == g_Number)
                Number++;
            if (Number == 0)
                g_Number = 1;
            else
                g_Number = Number;

            return g_Number;
        }

        public virtual void Start(bool iReset = true)
        {//
            device.Open();
        }
        public virtual void Stop()
        {
            device.Close();
        }

        static FrameBuffer buffer = new FrameBuffer(5 * 1024 * 1024);
        static byte[] payloadBuffer = new byte[5 * 1024 * 1024];
        static byte[] writeBuffer = new byte[5 * 1024 * 1024];


        public static byte[] payload = new byte[5 * 1024 * 1024];
        public int payloadLength = 0;

        int timerOut = 10 * 2400;//超时时间
        public int RequestTimeout
        {
            get { return this.timerOut; }
            set { this.timerOut = value; }
        }
        public Mobot.Utils.IO.SerialPort Port
        {
            get { return device.Port; }
        }

        /// <summary>
        /// 同步执行命令，返回响应的消息。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="respMid">响应的消息特征码</param>
        /// <param name="respMsg">响应的消息数据</param>
        /// <returns></returns>
        public virtual void Request(MessageBase message, out byte respMid, out MessageBase respMsg, double addTimeOut = 0)
        {
            //this.IsBusy = true;
            //产生一个计数
            byte counter = GetCounter();

            int size = formatter.Format(payloadBuffer, 0, message);
            int bytesToWrite = 0;
            bytesToWrite = TransferProtocol.PrepareBuffer(writeBuffer, bytesToWrite, payloadBuffer, size, counter);

            string msg = string.Empty;
            for (int i = 0; i < bytesToWrite; i++)
            {
                msg = string.Format("{0}{1:X2} ", msg, writeBuffer[i]);
            }
            log.WarnFormat("==============>{0}<{1}>", message, msg.Trim());

            //如果发送数据发现端口已经关闭，再次打开
            if (!device.IsOpen)
                device.Open();

            device.Write(writeBuffer, 0, bytesToWrite);

            OnMessageSent(message, counter);

            //对于重启消息单独判断
            byte reqMid = payloadBuffer[0];
            log.WarnFormat("消息类型值:{0}", reqMid);
            if (reqMid == 0x49)
            {
                log.ErrorFormat("{0} 已发送重启消息给测试盒，将马上返回响应消息", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                respMid = 0xC9;
                Type type = dicMessageTypes[respMid];
                respMsg = Activator.CreateInstance(type) as MessageBase;
                return;
            }

            DateTime startTime = DateTime.Now;
            //读取数据，直到收到跟本次发送相关的帧
            //while (true)
            {
                //收取一帧
                while (!buffer.ReceivedFrame)
                {
                    if (device.HasData)
                    {//读取数据
                        byte[] data = device.ReadExists();
                        string dataStr = string.Empty;

                        for (int i = 0; i < data.Length; i++)
                        {
                            buffer.Received((byte)data[i]);
                            dataStr = string.Format("{0}{1:X2} ", dataStr, data[i]);
                        }
                        log.WarnFormat("读取数据帧:{0}", dataStr.Trim());
                    }
                    if (!buffer.ReceivedFrame)
                    {
                        //如果还没有收到，检查是否有必要继续等待
                        TimeSpan span = DateTime.Now.Subtract(startTime);
                        if (span.TotalMilliseconds > timerOut + addTimeOut * 20000)//超时
                            throw new Exception("收到数据超时");
                    }

                    //Thread.Sleep(1);
                    //Application.DoEvents();
                }

                //bool hasResult = false;
                //逐条判断帧数据
                while (buffer.ReceivedFrame)
                {
                    byte[] frame = buffer.ReadFrame();

                    // 解析数据帧
                    byte respCounter;

                    //获取payload
                    long consumed = 0;
                    if (!TransferProtocol.GetPayloadFrom(payload, 0, out respCounter, out consumed, frame, frame.Length))
                    {
                        log.Error("解析失败");
                        continue;
                    }
                    if (respCounter != counter)
                    {
                        log.Error("不是期望的帧");
                        continue;
                    }
                }
            }

            int Consumed = 0;
            respMid = payload[0];
            if (dicMessageTypes.ContainsKey(respMid))
            {
                object[] rawData = formatter.Parse(payload, 0, out Consumed);

                Type type = dicMessageTypes[respMid];
                respMsg = Activator.CreateInstance(type) as MessageBase;
                respMsg.SetRawData(rawData);

                OnMessageReceived(respMsg, counter);
            }
            else
            {
                //"Unsupported Response."
                throw new Exception("收到未知消息：0x" + ((int)respMid).ToString("X2"));
            }
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        protected virtual void OnMessageReceived(MessageBase message, int counter)
        {
            if (MessageReceived != null)
                MessageReceived(this, new MessageReceivedEventArgs(message, counter));
        }
        public event EventHandler<MessageReceivedEventArgs> MessageSent;
        protected virtual void OnMessageSent(MessageBase message, int counter)
        {
            if (MessageSent != null)
                MessageSent(this, new MessageReceivedEventArgs(message, counter));
        }
    }
}
