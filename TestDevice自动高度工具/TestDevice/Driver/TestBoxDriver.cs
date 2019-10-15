using System;
using System.Collections.Generic;
using System.IO;
using Mobot.Utils;
using Mobot.Utils.IO;
using TestDevice;
using Mobot;
using Motor;

namespace Driver
{
    public partial class TestBoxDriver : ITestBox//, IPCServer<TestBoxDriver>
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public event EventHandler StateChanged
        {
            add { driver.StateChanged += value; }
            remove { driver.StateChanged -= value; }
        }
        public event EventHandler LocationChanged
        {
            add { driver.LocationChanged += value; }
            remove { driver.LocationChanged -= value; }
        }
        public bool IsOpen
        {
            get { return driver != null && driver.IsOpen; }
        }
        /// <summary>
        /// 基准点
        /// </summary>
        private Point2 check = Point2.Empty;
        public string PortName
        {
            get { return driver.PortName; }
            set { driver.PortName = value; }
        }
        private string serialNumber = string.Empty;
        public string SerialNumber
        {
            get { return this.serialNumber; }
            set { this.serialNumber = value; }
        }
        public string LastErrorString { get { return driver.LastErrorString; } }

        public TestBoxDriver() : this(null) { }
        public TestBoxDriver(string portName)
        {
            if (string.IsNullOrEmpty(portName))
            {
                portName = GetCom();
            }
            this.DelayBeforeReset = 100;
            this.driver = new MotorDriver(portName, 115200);
            this.settings = new ControllerSettings();
        }

        private string GetCom()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "Config.xml");
            if (!File.Exists(file)) return "COM1";

            DeviceInfo result = DeviceInfo.FromXml(file);
            KeyValueSettings settings = new KeyValueSettings(result.ConnectionSettings);
            string com;
            settings.TryGetValue<string>("EndPoint", out com);
            return com;
        }

        public void Open()
        {
            try
            {
                driver.Start();

                FlashStream stream = new FlashStream(this);
                TestBoxMemory menory = new TestBoxMemory(stream);
                check = menory.DeviceCheckData;

                DeviceInfo.SetMenory(menory, this);
                //设置默认速度
                if (this.MotorSpeed == MotorDriver.NormalSpeed)
                    this.MotorSpeed = Point3.Empty;
                Guid guid = menory.DeviceGuid;
                if (guid == Guid.Empty || guid == new Guid("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"))
                    throw new Exception("测试盒需要出厂配置后才可使用。");
                this.ControllerSettings.Import(menory);
                this.Import(menory);
                stream.Close();
                stream = null;

                //Reset();
            }
            catch (Exception ex)
            {
                log.Error("测试盒连接失败。", ex);
                throw ex;
            }
        }

        ~TestBoxDriver()
        {
            this.Close();
        }

        public void Close()
        {
            try
            {
                driver.Stop();
                log.WarnFormat("测试盒已断连:{0}", this.PortName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdatePort()
        {
            List<USBDeviceHelper.DeviceProperties> dps = GetSerialNumbers();
            foreach (USBDeviceHelper.DeviceProperties dp in dps)
            {
                if (dp.BaseFilename.Equals(this.SerialNumber))
                {
                    this.PortName = dp.COMPort;
                    return true;
                }
            }
            return false;
        }
        public List<USBDeviceHelper.DeviceProperties> GetSerialNumbers()
        {
            List<USBDeviceHelper.DeviceProperties> usbDPs = new List<USBDeviceHelper.DeviceProperties>();
            List<USBDeviceHelper.DeviceProperties> result = new List<USBDeviceHelper.DeviceProperties>();
            USBDeviceHelper.GetUSBDevices(USBDeviceHelper.DefaultDeviceVID, USBDeviceHelper.DefaultDevicePID, ref usbDPs, true);
            foreach (USBDeviceHelper.DeviceProperties dp in usbDPs)
            {
                if (!string.IsNullOrEmpty(dp.COMPort) && !string.IsNullOrEmpty(dp.BaseFilename) && !dp.BaseFilename.Contains("Virtual COM Port"))
                {
                    result.Add(dp);
                }
            }
            return result;
        }
    }
}
