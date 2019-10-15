using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Device_Link_LTSMC;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using System.IO;

namespace Interface.Items
{
    public class BaseNotify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class UserConfig : BaseNotify
    {
        private Rect cameraRect = new Rect();
        /// <summary>
        /// 控制板串口号
        /// </summary>
        private string controlComName = "";
        /// <summary>
        /// 圆半径
        /// </summary>
        private double circleRadius = 10;
      
        /// <summary>
        /// 轮盘坐标
        /// </summary>
        public Point RoulettePosition = new Point();
        public Point FlatA=new Point();
        public Point Skill_1 = new Point();
        public Point Skill_2 = new Point();
        public Point Slill_3 = new Point();
        public Point SkillPlus_1 = new Point();
        public Point SkillPlus_2 = new Point();
        public Point SkillPlus_3 = new Point();
        /// <summary>
        /// 相机拍摄区域
        /// </summary>
        public Rect CameraRect
        {
            get => cameraRect;
            set
            {
                cameraRect = value;
                
            }
        }
        /// <summary>
        /// 圆半径
        /// </summary>
        public double CircleRadius
        {
            get => circleRadius;
            set => circleRadius = value;
        }
        /// <summary>
        /// 轮盘坐标
        /// </summary>
        public Point RoulettePosition1 { get => RoulettePosition; set => RoulettePosition = value; }
    }
        public class Config
    {
       
        /// <summary>
        /// 方向移动垂直轴
        /// </summary>
        public static Axis MovingZAxis= Axis.Z;
        /// <summary>
        /// 技能释放垂直轴
        /// </summary>
        public static Axis SkillReleaseWAxis=Axis.W;
        /// <summary>
        /// 方向移动X轴
        /// </summary>
        public static Axis MovingXAxis=Axis.X;
        /// <summary>
        /// 方向移动Y轴
        /// </summary>
        public static Axis MovingYAxis=Axis.Y;
        /// <summary>
        /// 技能释放X轴
        /// </summary>
        public static Axis SkillReleaseUAxis=Axis.U;
        /// <summary>
        /// 技能释放Y轴
        /// </summary>
        public static Axis SkillReleaseVAxis=Axis.V;
        /// <summary>
        /// 出厂配置
        /// </summary>
        private FactoryConfig factoryConfig = new FactoryConfig();
        /// <summary>
        /// 用户 配置
        /// </summary>
        private UserConfig userConfig = new UserConfig();
        /// <summary>
        /// 用户配置
        /// </summary>
        public UserConfig UserConfig
        {
            get
            {
                return userConfig;
            }
            set
            {
                userConfig = value;
               
            }
        }
        

        /// <summary>
        /// 出厂配置
        /// </summary>
        public FactoryConfig FactoryConfig
        {
            get => factoryConfig;
            set
            {
                factoryConfig = value;
             
            }
        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static Config ReadConfigFromFile(string configPath)
        {
            if (!File.Exists(configPath)) return null;
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(configPath)), setting);
        }
        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="configPath"></param>
        /// <param name="deviceConfig"></param>
        public static void SaveDeviceConfigToFile(string configPath, Config deviceConfig)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.TypeNameHandling = TypeNameHandling.All;
            File.WriteAllBytes(configPath, System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(deviceConfig, setting)));
        }

    }
    /// <summary>
    /// 出厂配置
    /// </summary>
    public class FactoryConfig:BaseNotify
    {
        /// <summary>
        /// 轴信息
        /// </summary>
        public Dictionary<Axis, AxisInfo> AxisOtherInfo = new Dictionary<Axis, AxisInfo>()
           {
                { Config.MovingXAxis,new AxisInfo() {
                    Direction =Direction.Back, ZeroSpeed = 40000.00,AccTime = 0.01, ZeroMode=ZeroMode.OTTZ,
                    DecTime =0.01,RunSpeed = 40000, Name = "方向轴X移动",Scale = 5000 / 25 }
                },
                {
                   Config.MovingYAxis,new AxisInfo(){
                    Direction =Direction.Back,ZeroSpeed=40000.0,AccTime=0.01,ZeroMode=ZeroMode.OTTZ,
                    DecTime =0.01, RunSpeed=40000,Name="方向轴Y移动",Scale= 5000 / 16}
                },
                {
                    Config.MovingZAxis,new AxisInfo(){
                        Direction =Direction.Forward,ZeroSpeed=50000.0,AccTime=0.01,ZeroMode=ZeroMode.OTTZ,
                        DecTime =0.01,RunSpeed=50000.00,Name="方向轴Z移动",Scale= 5000 / 7}
                },
                {
                    Config.SkillReleaseUAxis,new AxisInfo()  {
                        Direction=Direction.Back,ZeroSpeed=40000.0,AccTime=0.01,ZeroMode=ZeroMode.OTTZ,
                        DecTime =0.01,RunSpeed=40000.0,Name="技能释放轴U移动",Scale= 5000 / 25
                    }
                },
                {
                    Config.SkillReleaseVAxis,new AxisInfo(){
                        Direction=Direction.Back,ZeroSpeed=40000.0,AccTime=0.01,ZeroMode=ZeroMode.OTTZ,
                        DecTime=0.01,RunSpeed=40000.0,Name ="技能释放V轴",Scale= 5000 / 16
                    }
                },
                {
                    Config.SkillReleaseWAxis,new AxisInfo(){
                        Direction=Direction.Forward,ZeroSpeed=50000.0,AccTime=0.01,ZeroMode=ZeroMode.OTTZ,
                        DecTime=0.01,RunSpeed=50000.0,Name ="技能释放W轴",Scale= 5000 / 7
                    }

                 }
        };
        /// <summary>
        /// Z点击高度
        /// </summary>
        private double zClickHeight = 8;
        /// <summary>
        /// W点击高度
        /// </summary>
        private double wClickHeight = 8;       
        /// <summary>
        /// 垂直等待高度
        /// </summary>
        private double waitingHeight = 12;   
        /// <summary>
        /// 垂直等待高度
        /// </summary>
        public double WaitingHeight
        {
            get => waitingHeight;
            set => waitingHeight = value;
        }
        /// <summary>
        ///Z连续点击高度
        /// </summary>
        public double ZContinuousClickHeight
        {
            get => zClickHeight;
            set => zClickHeight = value;
        }
        /// <summary>
        /// W连续点击高度
        /// </summary>
        public double WContinuousClickHeight
        {
            get => wClickHeight;
            set => wClickHeight = value;
        }
    }
    public class AxisPosition
    {
        public Dictionary<Axis, double> Position = new Dictionary<Axis, double>();

        public double this[Axis axis]
        {
            get
            {
                return Position[axis];
            }
            set
            {
                Position[axis] = value;
            }
        }
    }
}
