using Interface.Interface;
using Interface.Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskOption
{
    /// <summary>
    /// 任务
    /// </summary>
    public partial class TaskFormat : ITaskInoke

    {
        
        #region 接口对象
        /// <summary>
        /// 通讯服务
        /// </summary>
        public IServer server { get; set;}
        /// <summary>
        /// 日志
        /// </summary>
        public ILog Log { get; set; }
        /// <summary>
        /// 控制板端口
        /// </summary>
        public IControlCom ControlCom { get; set; }
        /// <summary>
        /// 控制器接口
        /// </summary>
        public IControlDevice ControlDevice { get; set; }
        /// <summary>
        /// 动作接口
        /// </summary>
        public IActionInterface ActionInterface { get; set; }
        /// <summary>
        /// 静态参数接口 
        /// </summary>
        public IStaticParamters StaticParamters { get; set; }

        #endregion
        #region 属性
        protected bool IsWaring
        {
            get 
            {
                return isWaring;
            }
        }
        private bool isWaring=false;
        /// <summary>
        /// 是否处于安全高度
        /// </summary>
        private bool isSafeHieght = true;
        /// <summary>
        /// 是否连接
        /// </summary>
        private bool isLink = false;
        /// <summary>
        /// 等待超时时间
        /// </summary>
        private int timeOut = 300;
        /// <summary>
        /// 危险信号检测线程
        /// </summary>
        private Thread waringThread = null;
        /// <summary>
        /// 配置文件
        /// </summary>
        public Config Config { get; set; } = new Config();
        public bool IsLink => isInit && isLink;
        //public bool IsLink => throw new NotImplementedException();
        /// <summary>
        /// 是否初始化接口过
        /// </summary>
        private bool isInit = false;
        /// <summary>
        /// 接口函数映射
        /// </summary>
        private Dictionary<OptionRun, Action<ServerRevItem>> runFunction = new Dictionary<OptionRun, Action<ServerRevItem>>();
        #endregion
        #region 接口方法
        /// <summary>
        /// 初始化接口
        /// </summary>
        public void InitInterface()
        {
            // if (isInit) return;
            runFunction.Clear();
            runFunction.Add(OptionRun.DirMove, DirMove);
            runFunction.Add(OptionRun.FlatA, FlatA);
            runFunction.Add(OptionRun.Skill1, Skill1);
            runFunction.Add(OptionRun.Skill2, Skill2);
            runFunction.Add(OptionRun.Skill3, Skill3);
            runFunction.Add(OptionRun.SkillPlus1, SkillPlus1);
            runFunction.Add(OptionRun.SkillPlus2, SkillPlus2);
            runFunction.Add(OptionRun.SkillPlus3, SkillPlus3);
            runFunction.Add(OptionRun.StopMove, StopMove);
            runFunction.Add(OptionRun.ClickWeapons, ClickWeapons);

        }

       

        public Config config { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        

        public Task<ServerRevItem> DoInterface(InterfaceItem interfaceItem)
        {
            try
            {
                //Log.log("开始执行接口,", interfaceItem.InterfaceName);
                OptionRun optionRun = interfaceItem.InterfaceName.GetEnumType<OptionRun>();
                Dictionary<string, string> directory = new Dictionary<string, string>();
                interfaceItem.Paramters.Foreach(c =>
                {
                    directory.Add(c.Name, c.Value);
                });
                return DoInterface(new ServerRevItem(directory)
                {
                    OptionRun = optionRun
                });
            }
            catch (Exception ex)
            {
                //Log.log("执行接口异常!");
                // Log.error(ex);
                return null;
            }
        }
        public Task<ServerRevItem> DoInterface(ServerRevItem serverRevItem)
        {
            Task<ServerRevItem> task = new Task<ServerRevItem>(new Func<object, ServerRevItem>(obj => {
                ServerRevItem revItem = obj as ServerRevItem;
                try
                {
                    if (IsWaring)
                    {
                        throw new Exception("Waring!");
                    }
                    runFunction[revItem.OptionRun](
                        revItem
                    );
                }
                catch (Exception ex)
                {
                    //Log.waring("执行接口异常!");
                    //Log.error(ex);
                    revItem.ReturnObj.Msg = ex.Message;
                    revItem.ReturnObj.Result = false;
                }
                return revItem;
            }), serverRevItem);
            task.Start();
            return task;
        }
        /// <summary>
        /// 接受接口请求
        /// </summary>
        /// <param name="obj">消息封装</param>
        private void ReciverRequest(ServerRevItem obj)
        {
            this.DoInterface(obj).ContinueWith(t => {
                ServerRevItem serverRev = t.Result;
                var sendmsg = JsonConvert.SerializeObject(serverRev.ReturnObj);
                // Log.log("发送数据：{0}", sendmsg);
                server.Send(serverRev.FromSocket, sendmsg);
            });
        }
        #endregion
        /// <summary>
        /// 获取接口列表
        /// </summary>
        /// <returns></returns>
        public List<InterfaceItem> GetInterfaceList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Link(string ip, int port)
        {
            try
            {
                server.Start(ip, port);
                server.ReciverRequest += ReciverRequest;
                Log.log("启动服务器成功");
                ControlCom.Open();
                Log.log("连接控制板成功");
                ControlDevice.Link(config.FactoryConfig.AxisOtherInfo);
                Log.log("连接控制器成功");
                isLink = true;
            }

            catch (Exception ex)
            {
                Log.log("连接错误"+ex.ToString());
            }
        }
        public void Close()
        {
            try
            {
                Log.log("开始关闭服务器!");
                server.Close();
                Log.log("开始关闭控制板!");
                ControlCom.Close();  
            }
            catch (Exception ex)
            {
                Log.waring("关闭异常!");
                Log.error(ex);
            }
        }

        /// <summary>
        /// 加载默认用户配置
        /// </summary>

        public void LoadDefaultUserConfig()
        {
            Log.log(string.Format("开始加载根目录下{0}的默认用户配置!", StaticParamters.GetValue("DefaultUserConfigPath")));
            Config.UserConfig = StaticParamters.GetValue<string>("DefaultUserConfigPath").ReadFromFile<UserConfig>();
            Log.log("加载默认设备配置成功！");
        }
        /// <summary>
        /// 加载出厂配置
        /// </summary>
        public void LoadFactoryConfig()
        {
            try
            {
                Log.log("开始加载出厂配置！");
                Config.FactoryConfig = StaticParamters.GetValue("DefaultConfigPath").ReadFromFile<FactoryConfig>();
                if (Config.FactoryConfig == null)
                {
                    Log.error("读取出厂配置失败,使用默认配置!");
                    Config.FactoryConfig = new FactoryConfig();
                }
                else
                {
                    Log.log("出厂配置加载成功！");
                }
            }
            catch (Exception ex)
            {
                Log.log("加载出厂配置失败！");
                Config.FactoryConfig = new FactoryConfig();
                Log.error(ex);
            }
        }
        /// <summary>
        /// 保存出厂配置
        /// </summary>
        public void SaveFacktory()
        {
            try
            {
                config.FactoryConfig.ToFile(StaticParamters.GetValue("DefaultConfigPath"));
            }
            catch(Exception ex)
            {
                Log.log("保存出厂配置失败！");
                Log.error(ex);
            }
            }
    }
}
