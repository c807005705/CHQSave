using Interface.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{

    public interface ITaskInoke
    {
        /// <summary>
        /// 是否都已经连接
        /// </summary>
        bool IsLink { get; }
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        void Link(string ip,int port);
        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
        /// <summary>
        /// 初始化接口
        /// </summary>
        void InitInterface();
        /// <summary>
        /// 配置
        /// </summary>
        Config config { get; set; }
        /// <summary>
        /// 加载出厂配置
        /// </summary>
        void LoadFactoryConfig();
        /// <summary>
        /// 加载用户默认配置
        /// </summary>

        void LoadDefaultUserConfig();
        /// <summary>
        /// 保存出厂配置
        /// </summary>
        void SaveFacktory();
        /// <summary>
        /// 执行接口
        /// </summary>
        /// <param name="interfaceItem">接口</param>
        Task<ServerRevItem> DoInterface(InterfaceItem interfaceItem);
        /// <summary>
        /// 获取接口
        /// </summary>
        /// <returns></returns>
        List<InterfaceItem> GetInterfaceList();
    }
}
