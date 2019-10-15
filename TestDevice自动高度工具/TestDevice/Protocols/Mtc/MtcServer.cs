// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using Mobot.TestBox.Protocols.Mtc.Messages;

namespace Mobot.TestBox.Protocols.Mtc
{
    public abstract partial class MtcServer : ServerBase
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected MtcServer(IDevice device)
            : base(device)
        { }

        protected override Type[] GetMessageTypes()
        {
            return MtcMessages.GetMessageTypes();
        }

        public byte[] payload = new byte[5 * 1024 * 1024];
        public int payloadLength = 0;
        protected override void DealFrame(byte[] buffer, int size)
        {
            //1. 预分发
            //dispatcher->PreDispatch( (unsigned char*)buf, msgsize ); // to set command number

            MessageBase response = null;
            byte counter = 0;
            try
            {
                payloadLength = 0;
                long consumed = 0;
                try
                {
                    //获取 payload
                    bool success = TransferProtocol.GetPayloadFrom(payload, 0, out counter, out consumed, buffer, size);
                    if (!success)
                        throw new Exception("获取payload失败");
                }
                catch (Exception ex)
                {//失败
                    log.Warn("错误:" + ex.Message);
                    counter = 0;
                    throw new ErrorException(0, ErrorValues.ERV_REQ_NOT_SUPPORTED, "不支持的请求");
                }

                //请求的消息特征码。
                MessageID reqMid = MessageID.Unknown;
                //请求的消息。
                MessageBase reqMsg = null;
                int r = ParseMessage(payload, 0, payloadLength, counter, out reqMid, out reqMsg);
                if (reqMsg == null)
                {
                    // something was wrong with command
                    log.Warn("some command dropped by dispatcher");
                    return;//忽略
                }

                //分发处理消息
                OnMessageReceived(reqMsg, counter);

                try
                {
                    response = Response(reqMid, reqMsg);
                }
                catch (Exception ex)
                {
                    log.Error("错误:", ex);
                    throw new ErrorException(1, ErrorValues.ERV_LOGIC, ex.Message);
                }
            }
            catch (ErrorException ex)
            {
                response = new FailureMessage(ex.Source, ex.Error, ex.Message);
            }
            try
            {
                Execute(response, counter);
                OnMessageSent(response, counter);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// Analyse commands.
        /// 
        /// int Dispatch(unsigned char *Buffer, long Size, Channel * channel)
        /// </summary>
        /// <param name="payload">pointer to the communication buffer with command.</param>
        /// <param name="payloadSize">Number of bytes in Buffer.</param>
        /// <returns>True - if it was Ping(), False - otherwise</returns>
        protected int ParseMessage(byte[] payload, int offset, int payloadSize, byte counter, out MessageID reqMid, out MessageBase reqMsg)
        {
            log.InfoFormat("Dispatch: START. Size={0}.", payloadSize);

            int Consumed = 0;
            //请求的消息特征码。
            reqMid = MessageID.Unknown;
            //请求的消息。
            reqMsg = null;
            log.InfoFormat("Dispatch: Buffer={0}.", BitConverter.ToString(payload, offset, 16));

            object[] rawData = null;
            byte mid = payload[offset];
            if (!dicMessageTypes.ContainsKey(mid))
            {//Unsupported request
                log.Info("Unsupported request");
                throw new ErrorException(1, ErrorValues.ERV_REQ_NOT_SUPPORTED,
                    string.Format("不支持的请求 MID=0x{0:X2} counter={1}", (int)reqMid, counter));
            }
            reqMid = (MessageID)mid;
            rawData = formatter.Parse(payload, offset, out Consumed);

            Type type = dicMessageTypes[mid];
            reqMsg = Activator.CreateInstance(type) as MessageBase;
            reqMsg.SetRawData(rawData);

            return Consumed;
        }

        protected abstract MessageBase Response(MessageID reqMid, MessageBase reqMsg);
    }
}
