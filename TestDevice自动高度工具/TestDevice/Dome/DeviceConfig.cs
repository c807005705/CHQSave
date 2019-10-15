using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;

namespace DeviceServer
{
    public class DeviceConfig
    {
        /// <summary>
        /// 参数列表
        /// </summary>
        public List<IParamter> Paramters { get; set; } = new List<IParamter>();

        /// <summary>
        /// /点击次数
        /// </summary>
        public int ClickTimes { get; set; }
        /// <summary>
        /// 滑屏次数
        /// </summary>
        public int DragTimes { get; set; }
        /// <summary>
        /// 自动高度
        /// </summary>
        public double AutoHeight { get; set; }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static DeviceConfig ReadConfigFromFile(string configPath)
        {
            if (!File.Exists(configPath)) return null;
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<DeviceConfig>(System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(configPath)), setting);
        }
        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="deviceConfig"></param>
        public static void SaveDeviceConfigToFile(string configPath, DeviceConfig deviceConfig)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            File.WriteAllBytes(configPath, System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(deviceConfig, setting)));
        }
    }
}
