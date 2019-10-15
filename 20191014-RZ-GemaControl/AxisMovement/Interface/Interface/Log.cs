using Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
    public interface ILog
    {
         /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="objs"></param>
        void log(params object [] objs);
        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="objs"></param>
        void waring(params object[] objs);
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="objs"></param>
        void error(params object[] objs);
        /// <summary>
        /// 日志输出
        /// </summary>
        Action<LogType, string> LogMsg { get; set; }
    }
}
