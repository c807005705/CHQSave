// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Mobot.TestBox.Protocols;
using System.Net;

namespace Mobot.TestBox.Protocols
{
    public abstract class ServerBase
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected IDevice device = null;

        protected MessageFormatter formatter = new MessageFormatter();
        /// <summary>
        /// Key=MessageID Value=Type
        /// </summary>
        protected Dictionary<byte, Type> dicMessageTypes = new Dictionary<byte, Type>();
        protected TransferProtocol protocol = new TransferProtocol();
        protected ServerBase(IDevice device)
        {
            this.device = device;
            device.DataReceived += new EventHandler(device_DataReceived);

            //初始化支持的消息列表
            foreach (Type type in this.GetMessageTypes())
            {
                MessageDescAttribute[] attribs = type.GetCustomAttributes(typeof(MessageDescAttribute), false) as MessageDescAttribute[];
                if (attribs.Length > 0 && attribs[0] != null)
                {
                    MessageDescAttribute attr = attribs[0];
                    formatter.AddDesc(attr.Description);

                    dicMessageTypes.Add(attr.Description.MID, type);
                }
            }
        }
        /// <summary>
        /// 获取支持的消息列表。
        /// </summary>
        /// <returns></returns>
        protected abstract Type[] GetMessageTypes();

        FrameBuffer buffer = new FrameBuffer(5 * 1024 * 1024);
        void device_DataReceived(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            byte[] data = device.ReadExists();
            int count = data.Length;
            System.Diagnostics.Debug.WriteLine( this.GetType() + "收到数据：" + count);

            stopwatch.Start();
            for (int i = 0; i < data.Length; i++)
                buffer.Received((byte)data[i]);

            System.Diagnostics.Debug.WriteLine(this.GetType() + "花费时间1：ms " + stopwatch.ElapsedMilliseconds);
            while(buffer.ReceivedFrame)
            {
                byte[] frame = buffer.ReadFrame();
                DealFrame(frame, frame.Length);
            }
            stopwatch.Stop();
            System.Diagnostics.Debug.WriteLine(this.GetType() + "花费时间：ms " + stopwatch.ElapsedMilliseconds);
        }

        byte[] payloadBuffer = new byte[5 * 1024 * 1024];
        byte[] writeBuffer = new byte[5 * 1024 * 1024];
        /// <summary>
        /// 使用指定的消息和计数值发送消息。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public void Execute(MessageBase message, byte counter)
        {
            int size = formatter.Format(payloadBuffer, 0, message);
            int bytesToWrite = 0;
            bytesToWrite = TransferProtocol.PrepareBuffer(writeBuffer, bytesToWrite, payloadBuffer, size, counter);
            device.Write(writeBuffer, 0, bytesToWrite);
        }

        public void Start()
        {//
            try
            {
                device.Open();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (IsOpenChanged != null)
                    IsOpenChanged(this, EventArgs.Empty);
            }

        }

        public void Stop()
        {
            try
            {
                device.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (IsOpenChanged != null)
                    IsOpenChanged(this, EventArgs.Empty);
            }
        }

        protected abstract void DealFrame(byte[] buffer, int size);

        public event EventHandler IsOpenChanged;
    }
}
