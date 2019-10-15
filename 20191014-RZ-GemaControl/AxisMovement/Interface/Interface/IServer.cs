using Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
    public interface IServer
    {
        /// <summary>
        /// 启动server
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        void Start(string ip, int port);
        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
        /// <summary>
        /// 释放
        /// </summary>
        void Dispose();
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        void Send(Socket socket, string msg);
        /// <summary>
        /// 接受到请求
        /// </summary>
        Action<ServerRevItem> ReciverRequest { get; set; }
    }
}
