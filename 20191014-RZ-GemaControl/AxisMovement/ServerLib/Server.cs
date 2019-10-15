using Interface.Interface;
using Interface.Items;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class Server : IServer
    {
        private SocketServer server = new SocketServer();
        /// <summary>
        /// 接受到请求
        /// </summary>
        public Action<ServerRevItem> ReciverRequest { get; set; }
        /// <summary>
        /// 日志
        /// </summary>
        public ILog Log { get; set; }
        /// <summary>
        /// 
        /// 关闭
        /// </summary>
        public void Close()
        {
            server.Close();
            Log.log("服务器被关闭!");
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose() => server.Dispose();
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        public void Send(Socket socket, string msg) => server.Send(msg, socket);
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Start(string ip, int port)
        {
            server.IsDelMsgAsyn = false;
            server.IsCheckLink = true;
            server.IsAutoSize = true;
            server.IsReciverForAll = true;
            server.InitSocket(ip, port);
            server.Start();
            server.RecDataEvent += Server_RecDataEvent;
        }
        /// <summary>
        /// 接受到数据
        /// </summary>
        /// <param name="obj">数据</param>
        private void Server_RecDataEvent(RecDataObject obj)
        {
            obj.DelEndString();
            Log.log("接收到数据:", obj.StringDatas);
            ServerRevItem item = null;
            try
            {
                item = new ServerRevItem(obj.StringDatas) { FromSocket = obj.Socket };
                item.OptionRun = item.GetValue<string>("OptionRun").GetEnumType<OptionRun>();
            }
            catch (Exception ex)
            {
                Log.waring("参数格式不正确!");
                Log.error(ex);
                ServerSendItem serverSend = new ServerSendItem();
                serverSend.Result = false;
                serverSend.Msg = "参数错误";
                Send(obj.Socket, Newtonsoft.Json.JsonConvert.SerializeObject(serverSend));
            }
            try
            {
                ReciverRequest?.Invoke(item);
            }
            catch (Exception ex)
            {
                Log.waring("处理异常!");
                Log.error(ex);
                ServerSendItem serverSend = new ServerSendItem();
                serverSend.Result = false;
                serverSend.Msg = "程序处理异常!";
                Send(obj.Socket, Newtonsoft.Json.JsonConvert.SerializeObject(serverSend));
            }

        }
    }
}
