using Driver;
using Mobot.Utils.IO;
using Motor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Drawing;
using TestDevice;
using DeviceServer.Properties;
using System.Data;

namespace DeviceServer
{
    public partial class MianForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MianForm()
        {
            InitializeComponent();
            InitCom();
            //初始化服务端
            this.InitServer();
            //初始化命令执行服务
            InitBW();
            GetSerialNumbers();
            but_X.Click += But_X_Click;
            but_y.Click += But_y_Click;
            but_z.Click += But_z_Click;
            bnt_clear.Click += Bnt_clear_Click;
            but_Setting.Click += But_Setting_Click;
            lab_message.Text = "设备未连接...";

            //初始化值
            txt_leftUp.Text = Settings.Default.LeftUpPoint.ToString();
            txt_rightDown.Text = Settings.Default.RigntDownPoint.ToString();
            txt_widht.Text = Settings.Default.ImageSize.Width.ToString();
            txt_height.Text = Settings.Default.ImageSize.Height.ToString();
            txt_Z.Text = Settings.Default.TouchPadClickZ.ToString();

        }

        private void Bnt_clear_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (cbxUSB.Items.Count > 1)
            {
                cbxUSB.SelectedIndex = 1;
                //button1.PerformClick();
            }
        }

        private void But_Setting_Click(object sender, EventArgs e)
        {
            if (this.testDevice != null && this.testDevice.IsOpen)
            {
                int width = 0;
                int height = 0;
                if (int.TryParse(txt_widht.Text.Trim(), out width) && int.TryParse(txt_height.Text, out height))
                {
                    this.testDevice.ImageSize = new Size(width, height);
                    Settings.Default.ImageSize = this.testDevice.ImageSize;
                }
                else
                {
                    MessageBox.Show("width或者height输入有误，请重新输入！");
                }
            }
            else
            {
                MessageBox.Show("请先连接设备，然后在操作！");
            }
        }

        private void But_z_Click(object sender, EventArgs e)
        {
            SqLiteHelperDao helperDao = new SqLiteHelperDao();
            helperDao.InitFileName(@"C:\Users\ASUS\Desktop\MTKScript - 副本 (2).ms");
            DataTable table = helperDao.ExecuteDataSet("select Ckey,Cvalue from DeviceAgentConnectionCollect where Ckey = 'Touchpad.BottomRight' or  Ckey = 'Touchpad.TopLeft'").Tables[0];
            //GetXYZPosition(Axis.Z);
        }

        private void But_y_Click(object sender, EventArgs e)
        {
            GetXYZPosition(Axis.Y);
        }

        private void But_X_Click(object sender, EventArgs e)
        {
            GetXYZPosition(Axis.X);
        }

        /// <summary>
        /// 初始化端口
        /// </summary>
        private void InitCom()
        {
            cbxUSB.Items.Clear();
            cbxUSB.DropDown += cbxPort_DropDown;
            cbxPort_DropDown(cbxUSB, EventArgs.Empty);
        }

        void cbxPort_DropDown(object sender, EventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            string value = cbx.SelectedItem as string;

            cbx.Items.Clear();
            cbx.Items.Add("请选择端口...");
            foreach (string portName in System.IO.Ports.SerialPort.GetPortNames())
                cbx.Items.Add(portName);

            cbx.SelectedItem = value;
        }

        private string GetSerialNumbers()
        {
            List<USBDeviceHelper.DeviceProperties> usbDPs = new List<USBDeviceHelper.DeviceProperties>();
            List<USBDeviceHelper.DeviceProperties> result = new List<USBDeviceHelper.DeviceProperties>();
            USBDeviceHelper.GetUSBDevices(USBDeviceHelper.DefaultDeviceVID, USBDeviceHelper.DefaultDevicePID, ref usbDPs, true);
            foreach (USBDeviceHelper.DeviceProperties dp in usbDPs)
            {
                if (!dp.BaseFilename.Contains("Testbox Power Controler"))
                    return dp.BaseFilename;
            }
            return string.Empty;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(cbxUSB.Text))
            {
                MessageBox.Show("端口不能为空！");
                return;
            }
            if (testDevice == null)
            {
                testDevice = new TestBoxDriver(cbxUSB.Text);
                device = testDevice.Driver as MotorDriver;
                testDevice.IsReset = false;
                testDevice.DebugMode = false;
                device.RequestTimeout = 30 * 1000;
            }
            if (testDevice.IsOpen)
            {
                testDevice.Close();
                button1.Text = "Open";
                lab_message.Text = "设备已断开...";
            }
            else
            {
                testDevice.LeftUpPoint = Settings.Default.LeftUpPoint;
                testDevice.RigntDownPoint = Settings.Default.RigntDownPoint;
                testDevice.ImageSize = Settings.Default.ImageSize;
                testDevice.TouchPadClickZ = Settings.Default.TouchPadClickZ;

                testDevice.Open();
                button1.Text = "Close";
                lab_message.Text = "设备已连接...";
            }
        }

        private void SetValue2(string value, params object[] param)
        {
            SetValue(string.Format(value, param));
        }

        private void SetValue(string value = null, Socket socket = null)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string, Socket>(SetValue), value, socket);
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(value)) return;
                string time = DateTime.Now.ToString("MM-dd HH:mm:ss:f");
                if (socket != null)
                    time = string.Format("{0}<{1}>", time, socket.RemoteEndPoint);
                log.Debug(string.Format("{0} {1}", time, value));
                listView1.Items.Add(string.Format("{0} {1}", time, value));
                if (listView1.Items.Count > 250)
                {
                    listView1.Items.RemoveAt(0);
                }
                listView1.Items[listView1.Items.Count - 1].EnsureVisible();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.testDevice != null && this.testDevice.IsOpen)
            {
                this.testDevice.StylusReset();
            }
            else
            {
                MessageBox.Show("请先连接设备，然后在操作！");
            }
        }

        private void test_Click(object sender, EventArgs e)
        {
            if (testDevice != null && this.testDevice.IsOpen)
            {
                int pix_x = 0;
                int pix_y = 0;
                if (int.TryParse(txt_x.Text.Trim(), out pix_x) && int.TryParse(txt_y.Text, out pix_y))
                {
                    this.testDevice.TouchClick(pix_x, pix_y);
                }
                else
                {
                    MessageBox.Show("X或者Y输入有误，请重新输入！");
                }
            }
            else
            {
                MessageBox.Show("请先连接设备，然后在操作！");
            }
        }

        private void GetXYZPosition(Axis type)
        {
            if (this.testDevice != null && this.testDevice.IsOpen)
            {
                MotorPosition _MotorPosition = new MotorPosition(this.testDevice);
                if (_MotorPosition.ShowDialog() == DialogResult.OK)
                {
                    switch (type)
                    {
                        case Axis.X:
                            this.testDevice.LeftUpPoint = new PointF(_MotorPosition.X, _MotorPosition.Y);
                            Settings.Default.LeftUpPoint = new Point(_MotorPosition.X, _MotorPosition.Y);
                            txt_leftUp.Text = this.testDevice.LeftUpPoint.ToString();
                            break;
                        case Axis.Y:
                            this.testDevice.RigntDownPoint = new PointF(_MotorPosition.X, _MotorPosition.Y);
                            Settings.Default.RigntDownPoint = new Point(_MotorPosition.X, _MotorPosition.Y);
                            txt_rightDown.Text = this.testDevice.RigntDownPoint.ToString();
                            break;
                        case Axis.Z:
                            this.testDevice.TouchPadClickZ = _MotorPosition.Z;
                            Settings.Default.TouchPadClickZ = _MotorPosition.Z;
                            txt_Z.Text = this.testDevice.TouchPadClickZ.ToString();
                            break;
                    }
                    Settings.Default.Save();
                }
            }
            else
            {
                MessageBox.Show("请先连接设备，然后在操作！");
            }
        }
    }
}
