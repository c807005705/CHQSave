using ControlDec;
using ControlLib;
using Interface.Interface;
using Interface.Items;
using LogInfo;
using ParamtersLib;
using ServerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskOption;

namespace RZ_AutoGemaControl
{
    public class StartUp
    {
        /// <summary>
        /// 自动注入服务
        /// </summary>
        public static IService Service = new services.AutoServiceImpl();
        /// <summary>
        /// 启动层级日志
        /// </summary>
        public static ILog Log = new LogInfo.LogInfo("启动层级日志");
        /// <summary>
        /// 窗口
        /// </summary>
        public static IWindow Window { get; set; }
        /// <summary>
        /// 静态参数
        /// </summary>
        public static IStaticParamters StaticParamters { get; set; } = new ParamtersConfig(Properties.Resources.LoadConfig);
        /// <summary>
        /// 启动容器
        /// </summary>
        public static void Start()
        {
            Service.InjectionSingleInstance<ILog>(new LogInfo.LogInfo("双指设备内容"));
            Service.InjectionSingleInstance<IControlDevice>(new ConDevice());
            Service.InjectionSingleInstance<IControlCom, ControlCom>();
            Service.InjectionSingleInstance<IWindow> (new GameConsole());
            Service.InjectionSingleInstance<IServer>(new Server());
            Service.InjectionSingleInstance<ITaskInoke, TaskFormat>();
            Service.InjectionSingleInstance<IActionInterface, Action>();
            Service.start();
        }
        public static void ModifyStaticParamters(string[] argvs)
        {
            if (argvs.Length <1)
            {
                return;
            }
            List<string> modifys = argvs.ToList();
            foreach (var item in modifys)
            {
                var key = item.Split(':')[0];
                var value = item.Split(':')[1];

                StaticParamters.SetValue(key, value);
            }
        }
        public static void Main(string[] argvs)
        {
            Start();
            try
            {
                Log.log(argvs.JoinToString(","));
                ModifyStaticParamters(argvs);
                //StaticParamters.SetValue("TaskInvoke.IsTest", "true");
                using (Window = Service.Resolve<IWindow>())
                {
                    Window.ShowWindow();
                }
            }
            catch (Exception ex)
            {
                Log.error("程序运行错误:", ex);
            }
        }
    }
}
