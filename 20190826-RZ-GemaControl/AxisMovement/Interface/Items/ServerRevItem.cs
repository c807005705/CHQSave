using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Items
{
    public class ServerRevItem
    {
        /// <summary>
        /// 服务接受数据项
        /// </summary>
       
            /// <summary>
            /// 接口类型
            /// </summary>
            public OptionRun OptionRun { get; set; }
            /// <summary>
            /// 返回的json数据对象
            /// </summary>
            public ServerSendItem ReturnObj { get; set; } = new ServerSendItem();
            /// <summary>
            /// 来自Socket
            /// </summary>
            public Socket FromSocket { get; set; }
            /// <summary>
            /// 参数
            /// </summary>
            private Dictionary<string, string> paramters = new Dictionary<string, string>();
            /// <summary>
            /// 接口参数
            /// </summary>
            /// <param name="paramter"></param>
            public ServerRevItem(string paramter)
            {
                paramters = JsonConvert.DeserializeObject<Dictionary<string, string>>(paramter);
            }
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="pairs"></param>
            public ServerRevItem(Dictionary<string, string> pairs)
            {
                paramters = pairs;
            }
            /// <summary>
            /// 获取值
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public string GetValue(string key) => paramters[key];
            /// <summary>
            /// 获取值
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key"></param>
            /// <returns></returns>
            public T GetValue<T>(string key)
            {
                T def = default(T);
                def = (T)Convert.ChangeType(paramters[key], typeof(T));
                return def;
            }
        }
        /// <summary>
        /// 服务回信的对象
        /// </summary>
        public class ServerSendItem
        {
            /// <summary>
            /// 执行结果情况
            /// </summary>
            public bool Result { get; set; } = true;
            /// <summary>
            /// 执行消息
            /// </summary>
            public string Msg { get; set; } = "";
            /// <summary>
            /// 参数
            /// </summary>
            public Dictionary<string, object> Paramters { get; set; } = new Dictionary<string, object>();
            /// <summary>
            /// 添加参数
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void AddParamter(string key, object value)
            {
                Paramters.Add(key, value);
            }

            /// <summary>
            /// 获取值
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key"></param>
            /// <returns></returns>
            public T GetValue<T>(string key)
            {
                T def = default(T);
                def = (T)Convert.ChangeType(Paramters[key], typeof(T));
                return def;
            }
        }
    }

