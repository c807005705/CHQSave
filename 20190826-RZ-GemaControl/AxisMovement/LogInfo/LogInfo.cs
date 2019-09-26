using Interface.Interface;
using Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogInfo
{
    public class LogInfo : ILog
    {
      
        /// <summary>
        /// 日志文件名称
        /// </summary>
        /// <param name="name"></param>
        public LogInfo(string name)
        {
            Log.ErroStringEvent += Log_ErroStringEvent;
            Log.FileName = name;
        }
        /// <summary>
        /// 日志文件名称
        /// </summary>
        public LogInfo()
        {
            Log.ErroStringEvent += Log_ErroStringEvent;
        }
        /// <summary>
        /// 消息回调
        /// </summary>
        /// <param name="obj"></param>
        private void Log_ErroStringEvent(xyxandwxx.LobLib.LogMessage obj)
        {
            LogType logType = LogType.Normal;
            if (obj.Type == xyxandwxx.LobLib.LogErroType.Waring)
            {
                logType = LogType.Waring;
            }
            else if (obj.Type == xyxandwxx.LobLib.LogErroType.Erro)
            {
                logType = LogType.Error;
            }
            LogMsg?.Invoke(logType, obj.NormalMessage + obj.DetailMessage);
        }

        private xyxandwxx.LobLib.LogInfo Log = new xyxandwxx.LobLib.LogInfo() { FileName = "指纹日志" };
        public Action<LogType, string> LogMsg { get; set; }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="objs"></param>
        public void error(params object[] objs)
        {
            Log.erro(objs);
        }
        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="objs"></param>
        public void log(params object[] objs)
        {
            Log.log(objs);
        }
         /// <summary>
         /// 警告日志
         /// </summary>
         /// <param name="objs"></param>
        public void waring(params object[] objs)
        {
            Log.waring(objs);
        }
    }
}
