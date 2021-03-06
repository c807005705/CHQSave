﻿using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

namespace Mobot.TestBox
{
    /// <summary>
    ///     服务端
    /// </summary>
    /// <typeparam name="T">子类型</typeparam>
    public class IPCServer<T> : MarshalByRefObject, IDisposable
    {
        private IpcChannel serverChannel;

        /// <summary>
        ///     启动
        ///     默认单实例
        /// </summary>
        public void Start()
        {
            Start(WellKnownObjectMode.Singleton);
        }

        /// <summary>
        ///     启动
        /// </summary>
        public void Start(WellKnownObjectMode mode)
        {
            var serverProvider = new BinaryServerFormatterSinkProvider();
            var clientProvider = new BinaryClientFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;
            IDictionary props = new Hashtable();
            props["portName"] = string.Format("ServerChannel-Server.{0}", typeof(T).Name);
            serverChannel = new IpcChannel(props, clientProvider, serverProvider);
            // 注册这个IPC信道.
            ChannelServices.RegisterChannel(serverChannel, true);
            // 向信道暴露一个远程对象.
            var type = MethodBase.GetCurrentMethod().DeclaringType;
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(T), "IPCObject", mode);
        }

        /// <summary>
        ///     停止服务
        /// </summary>
        public void Stop()
        {
            if (serverChannel != null)
            {
                ChannelServices.UnregisterChannel(serverChannel);
                serverChannel.StopListening(null);
                serverChannel = null;
            }
        }

        #region Dispose

        /// <summary>
        ///     Disposes the instance of SocketClient.
        /// </summary>
        public bool Disposed;

        /// <summary>
        ///     释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                Disposed = true;
                if (disposing)
                {
                    Stop();
                }
            }
            Disposed = true;
        }

        /// <summary>
        ///     析构
        /// </summary>
        ~IPCServer()
        {
            Dispose(false);
        }

        #endregion
    }
}