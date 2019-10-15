using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Mobot.Utils;
using System.Drawing;
using TestDevice;
using Mobot;

namespace Driver
{
    public class DeviceInfo
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        const string NamespaceURI = "http://schemas.chuangen.name/mobot/2010";

        readonly KeyValueSettings dicSettings = null;
        public Guid Guid { get; set; }
        /// <summary>
        /// 手机型号。
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 制造商。
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// 配置文件描述。
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 手机照片。
        /// </summary>
        public Image Picture { get; set; }

        public string this[string key]
        {
            get { return dicSettings[key]; }
            set { dicSettings[key] = value; }
        }

        public IDictionary<string, string> ConnectionSettings
        {
            get { return this.dicSettings; }
        }
        public DeviceInfo()
        {
            this.dicSettings = new KeyValueSettings();
            this.Guid = Guid.NewGuid();
        }

        public static void SetMenory(TestBoxMemory menory, ITestBox testbox)
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "Config.xml");
            if (!File.Exists(file)) return;

            DeviceInfo result = DeviceInfo.FromXml(file);
            IDictionary<string, string> dic = result.ConnectionSettings;

            log.Info("载入手机配置 ...");

            KeyValueSettings settings = new KeyValueSettings(dic);

            menory.DisplayInfo = null;
            Size resolution;
            if (settings.TryGetValue<Size>("Display.Resolution", out resolution))
            {
                menory.DisplayInfo = new DisplayInfo(resolution);
            }

            //Workspace
            double tempPointZ;
            testbox.QuickPointZ = null;
            if (settings.TryGetValue("Workspace.QuickStart", out tempPointZ))
            {
                testbox.QuickPointZ = tempPointZ;
            }

            //Touchpad
            menory.WorkspaceInfo = null;
            try
            {
                Point2D topLeft = settings.GetValue<Point2D>("Touchpad.TopLeft");
                Point2D bottomRight = settings.GetValue<Point2D>("Touchpad.BottomRight");
                WorkspaceInfo info = new WorkspaceInfo(Size2D.Empty, topLeft, bottomRight, RectangleD.Empty);
                menory.WorkspaceInfo = info;
            }
            catch (Exception ex)
            {//载入失败，不启用触屏
                menory.WorkspaceInfo = null;
                log.Error(ex);
            }

            testbox.TouchPadClickZ = null;
            double clickZ;
            if (settings.TryGetValue<double>("Touchpad.ClickZ", out clickZ))
            {
                testbox.TouchPadClickZ = clickZ;
            }

            //Motor
            Point3 tempPoint3;
            if (settings.TryGetValue("Motor.Speed", out tempPoint3))
            {
                testbox.MotorSpeed = tempPoint3;
            }
            bool tempBoolean;
            if (settings.TryGetValue("Motor.ActionAsyncMode", out tempBoolean))
            {
                testbox.DebugMode = tempBoolean;
            }

            try
            {
                Point2D center = Point2D.Empty;
                Size2D size = Size2D.Empty;
                String directionString = String.Empty;
                Rectangle clipBounds = Rectangle.Empty;
                String method = String.Empty;
                double tempDouble;

                settings.TryGetValue<Point2D>("ImageMark.Result.Center", out center);
                settings.TryGetValue<Size2D>("ImageMark.Result.Size", out size);
                settings.TryGetValue<Double>("ImageMark.Result.Angle", out tempDouble);
                settings.TryGetValue<String>("ImageMark.Result.Direction", out directionString);
                settings.TryGetValue<Rectangle>("ImageMark.Result.ClipBounds", out clipBounds);
                settings.TryGetValue<String>("ImageMark.Result.Detection", out method);
            }
            catch (Exception)
            {
            }
            //Keyboard
            foreach (string key in settings.SettingKeys)
            {
                if (key == null || !key.StartsWith("KEY."))
                    continue;
                string value;
                if (!settings.TryGetValue(key, out value))
                    continue;
                try
                {
                    string keyName = key.Substring("KEY.".Length);
                    MobileKeyInfo keyInfo = MobileKeyInfo.Parse(value);
                    if (testbox.Keys.ContainsKey(key))
                    {
                        testbox.Keys[key] = keyInfo;
                    }
                    else
                    {
                        testbox.Keys.Add(key, keyInfo);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("键值配置格式不正确。", ex);
                    continue;
                }
            }
        }

        public static DeviceInfo FromXml(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName", "");
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("文件名无效。", "fileName");
            if (!File.Exists(fileName))
                throw new FileNotFoundException("未找到指定的设备文件。", fileName);

            string fullName = Path.GetFullPath(fileName);
            string basePath = Path.GetDirectoryName(fullName);

            XmlDocument xmldoc = new XmlDocument();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmldoc.NameTable);
            nsmgr.AddNamespace("m", NamespaceURI);

            try
            {
                xmldoc.Load(fileName);
            }
            catch (Exception ex)
            {
                throw new Exception("载入配置文件失败：" + ex.Message);
            }
            //deviceInfo节点
            XmlElement elemRoot = xmldoc.DocumentElement;
            if (elemRoot == null || elemRoot.Name != "deviceInfo")
                throw new Exception("设备文件格式不正确。");

            DeviceInfo result = new DeviceInfo();
            result.FromXml(basePath, elemRoot, nsmgr);
            return result;
        }
        private void FromXml(string basePath, XmlElement elemDeviceInfo, XmlNamespaceManager nsmgr)
        {
            Guid guid;
            XmlElement selected = null;

            this.Guid = Guid.Empty;
            selected = elemDeviceInfo.SelectSingleNode("m:id", nsmgr) as XmlElement;
            if (selected != null && Guid.TryParse(selected.InnerText, out guid))
                this.Guid = guid;
            else
                this.Guid = Guid.NewGuid();
            selected = elemDeviceInfo.SelectSingleNode("m:model", nsmgr) as XmlElement;
            this.Model = (selected == null) ? "" : selected.InnerText;
            selected = elemDeviceInfo.SelectSingleNode("m:manufacturer", nsmgr) as XmlElement;
            this.Manufacturer = (selected == null) ? "" : selected.InnerText;
            selected = elemDeviceInfo.SelectSingleNode("m:description", nsmgr) as XmlElement;
            this.Description = (selected == null) ? "" : selected.InnerText;
            selected = elemDeviceInfo.SelectSingleNode("m:connectMethod", nsmgr) as XmlElement;
            string strConnectMethod = (selected == null) ? "" : selected.InnerText;

            //读取LOGO图片
            string logoFile = Path.Combine(basePath, "logo.png");
            if (File.Exists(logoFile))
            {
                this.Picture = Image.FromStream(new MemoryStream(File.ReadAllBytes(logoFile))) as Bitmap;
            }

            XmlElement elemSettings = elemDeviceInfo.SelectSingleNode("m:settings", nsmgr) as XmlElement;
            KeyValueSettings settings = null;
            if (elemSettings != null)
                settings = KeyValueSettings.FromXml(elemSettings, nsmgr);
            if (settings != null)
            {
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    if (this.dicSettings.ContainsKey(pair.Key))
                        this.dicSettings[pair.Key] = pair.Value;
                    else
                        this.dicSettings.Add(pair.Key, pair.Value);
                }
            }
        }
    }
}
