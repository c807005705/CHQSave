using Driver;
using Motor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestDevice;

namespace DeviceServer
{
    public partial class MainformTool : Form
    {
        BackgroundWorker bw = null;
        ITestBox testDevice = null;
        MotorDriver device = null;
        DeviceConfig deviceConfig = new DeviceConfig();
        private static readonly string ConfigPath = "DeviceConfig.config";
        private bool IsRunning = false;
        private bool IsAutoing = false;
        private Thread RunningThread = null;
        private bool i = true;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MainformTool()   
        {
            InitializeComponent();
            InitCom();
            //初始化命令执行服务
            //InitBW();
            Open.Click += Open_Click;
            Reset.Click += Reset_Click;
            AutoHeight_btn.Click += AutoHeight_Click;
            Run.Click += Run_Click;
            Add.Click += Add_Click;
            addToLast_btn.Click += AddToLast_btn_Click;
            Click.Click += Click_Click;
            slide.Click += Slide_Click;
            wait.Click += Wait_Click;
            Remove.Click += Remove_Click;
            moveZero.Click += MoveZero_Click;
            Click.Checked = true;
            cbxUSB.SelectedIndex= 0;
            this.listView1.Columns.Add("类型", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("ID", 100, HorizontalAlignment.Left);
            this.listView1.Columns.Add("参数", 300, HorizontalAlignment.Left);
            
            this.listView1.View = System.Windows.Forms.View.Details;
            //TypeSelection.SelectedIndex = 0;         


            //读取和保存配置文件操作
            var config = DeviceConfig.ReadConfigFromFile(ConfigPath);
            if(config != null)
            {
                this.deviceConfig = config;
            }
            else
            {
                DeviceConfig.SaveDeviceConfigToFile(ConfigPath, this.deviceConfig);
            }
            
        }

        

        /// <summary>
        /// 归位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveZero_Click(object sender, EventArgs e)
        {
            this.testDevice.StylusReset();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            List<ListViewItem> deleteItems = new List<ListViewItem>();
            foreach (ListViewItem lvi in listView1.SelectedItems) 
            {
                deleteItems.Add(listView1.Items[listView1.SelectedIndices[0]]);
            }
            foreach (var item in deleteItems)
            {
                RemoveToListViewForParamter(item);
            }
        }

        private void Wait_Click(object sender, EventArgs e)
        {
         
            if (wait.Checked)
            {
                panel3.Visible = true;
                panel1.Visible = false;
                panel2.Visible = false;
            }
            else panel3.Visible = false;
        }
        private void Slide_Click(object sender, EventArgs e)
        {
            
            if (slide.Checked)
            {
                panel2.Visible = true;
                panel1.Visible = false;
                panel3.Visible = false;
            }
            else panel2.Visible = false;
          
        }
        private void Click_Click(object sender, EventArgs e)
        {
            if (Click.Checked)
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
            }
            else panel1.Visible = false;
        }
        /// <summary>
        /// 获取自动高低 ----------------------
        /// </summary>
        /// <returns></returns>
        private double GetAutoHeight()
        {
            return this.deviceConfig.AutoHeight;
        }
        /// <summary>
        /// 设置自动 高度
        /// </summary>
        /// <param name="value"></param>
        private void SetAutoHeight(double value)
        {
            this.deviceConfig.AutoHeight = value;
        }
        /// <summary>
        /// 启动一个线程定位自动高度
        /// </summary>
        /// <param name="target"></param>
        private void StartAutoThread(double target)
        {
            new Thread((o) => {
                try
                {
                    double targetValue = o.ToBaseType<double>();
                    double currentValue = 0;
                    double everyHei = 0.1d;
                    double currentHei = 0.1d;
                    int sleepTime = 2;
                    bool isFirst = true;
                    while (currentValue < targetValue || isFirst)
                    {
                        testDevice.StylusMoveZ(currentHei);
                        currentHei += everyHei;
                        Thread.Sleep(sleepTime);
                        currentValue = testDevice.StylusStatue();
                        Console.WriteLine("当前压力 : " + currentValue);
                        if(currentValue >= 20)
                        {
                            sleepTime = 100;
                        }
                        if (!IsAutoing)
                        {
                            break;
                        }
                        //第一次触碰压力达到要求时候，反弹重新调整
                        if(currentValue >= targetValue && isFirst)
                        {
                            currentHei -= 2;
                            testDevice.StylusMoveZ(currentHei);
                            Thread.Sleep(sleepTime);
                            currentValue = testDevice.StylusStatue();
                            isFirst = false;
                        }
                    }



                    this.deviceConfig.AutoHeight = currentHei;
                }
                catch (Exception ex)
                {
                    this.BeginInvoke(new Action(() => {
                        AutoHeight_btn.Text = "启动自动高度";
                        IsAutoing = false;
                    }));
                }
                this.BeginInvoke(new Action(() => {
                    autoHeight_label.Text = this.deviceConfig.AutoHeight.ToString("0.00");
                }));
            }) { IsBackground = true }.Start(target);
        }
        /// <summary>
        /// 将参数添加到listView中
        /// </summary>
        /// <param name="paramter"></param>
        private void AddToListViewForParamter(IParamter paramter, bool isSave = true, int insertIndex = -1)
        {
            this.listView1.BeginUpdate();
            ListViewItem item = new ListViewItem(paramter.Type);
            //item.SubItems.Add();
            item.SubItems.Add(paramter.ID);
            item.SubItems.Add(paramter.ToDisplayString());
            if(insertIndex == -1)
                listView1.Items.Add(item);
            else
            {
                this.listView1.Items.Insert(insertIndex, item);
            }
            listView1.EndUpdate();
            if (isSave)
            {
                if (insertIndex == -1)
                    this.deviceConfig.Paramters.Add(paramter);
                else
                {
                    this.deviceConfig.Paramters.Insert(insertIndex, paramter);
                }
                DeviceConfig.SaveDeviceConfigToFile(ConfigPath, this.deviceConfig);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        private void RemoveToListViewForParamter(ListViewItem item)
        {
            ///获取id
            var id = item.SubItems[1].Text;
            var paramter = deviceConfig.Paramters.Where(p => p.ID.Equals(id)).FirstOrDefault();
            if(paramter != null)
            {
                this.deviceConfig.Paramters.Remove(paramter);
            }
            listView1.Items.Remove(item);
            DeviceConfig.SaveDeviceConfigToFile(ConfigPath, this.deviceConfig);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="isToLast"></param>
        private void AddClick(bool isToLast)
        {
            IParamter paramter = null;
            if (Click.Checked)
            {
                ClickParamter clickParamter = new ClickParamter();
                clickParamter.ClickPosition = new Point(Click_X.Text.ToBaseType<int>(), Click_Y.Text.ToBaseType<int>());
                paramter = clickParamter;
            }
            else if (slide.Checked)
            {
                DragParamter dragParamter = new DragParamter();
                dragParamter.StartPosition = new Point(Start_X.Text.ToBaseType<int>(), Start_Y.Text.ToBaseType<int>());
                dragParamter.EndPosition = new Point(End_X.Text.ToBaseType<int>(), End_Y.Text.ToBaseType<int>());
                paramter = dragParamter;
            }
            else
            {
                WaitParamter waitParamter = new WaitParamter();
                waitParamter.WaitTime = SleepTime.Text.ToBaseType<int>();
                paramter = waitParamter;
            }
            if (this.listView1.SelectedIndices.Count == 0 || isToLast)
                AddToListViewForParamter(paramter);
            else
            {
                AddToListViewForParamter(paramter, insertIndex: this.listView1.SelectedIndices[0]);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, EventArgs e)
        {
            AddClick(false);
        }
        /// <summary>
        /// 添加到最后一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddToLast_btn_Click(object sender, EventArgs e)
        {
            AddClick(true);
        }
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Run_Click(object sender, EventArgs e)
        {
            if (!IsRunning)
            {
                Run.BackColor = Color.Red;
                Run.Text = "暂停";
                IsRunning = true;
                Add.Enabled = false;
                Remove.Enabled = false;
                if (this.RunningThread != null)
                {
                    this.RunningThread.Abort();
                }
                this.RunningThread = new Thread(RunningCore);
                this.RunningThread.IsBackground = true;
                this.RunningThread.Start();

            } else
            {
                Run.BackColor = Color.Lime;
                Run.Text = "运行";
                IsRunning = false;
                Add.Enabled = true;
                Remove.Enabled = true;
                //if (this.RunningThread != null)
                //{
                //    this.RunningThread.Abort();
                //    this.RunningThread = null;
                //}
            }

        }
        /// <summary>
        /// 运行任务的核心
        /// </summary>
        private void RunningCore()
        {
            try
            {
                while (true)
                {
                    foreach (var item in this.deviceConfig.Paramters)
                    {
                        item.Doing(testDevice, this.deviceConfig);
                        if (!IsRunning)
                        {
                            throw new Exception("任务停止!");
                        }
                    }
                    this.BeginInvoke(new Action(() => {
                        Number_Clicks.Text = this.deviceConfig.ClickTimes.ToString();
                        Num_Sliding.Text = this.deviceConfig.DragTimes.ToString();
                    }));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("停止运行!");
            }
        }
        /// <summary>
        /// 自动高度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoHeight_Click(object sender, EventArgs e)
        {
            if (!IsAutoing)
            {
                IsAutoing = true;
                (sender as Button).Text = "停止自动高度";
                StartAutoThread(weight.Text.ToBaseType<double>());
            }
            else
            {
                (sender as Button).Text = "启动自动高度";
                IsAutoing = false;
            }
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_Click(object sender, EventArgs e)
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
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, EventArgs e)
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
                Open.Text = "连接";
                DeviceType.Text = "设备已断开...";
            }
            else
            {         
               testDevice.Open();
                Open.Text = "关闭";
                DeviceType.Text = "设备已连接...";
            }
        }

        /// <summary>
        /// 初始化
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

        private void MainformTool_Load(object sender, EventArgs e)
        {
            foreach (var item in deviceConfig.Paramters)
            {
                AddToListViewForParamter(item, false);
            }
            autoHeight_label.Text = this.deviceConfig.AutoHeight.ToString("0.00");

            Number_Clicks.Text = this.deviceConfig.ClickTimes.ToString();

            Num_Sliding.Text = this.deviceConfig.DragTimes.ToString();
        }

        private void AddTo_Enter(object sender, EventArgs e)
        {
            
           
        }
        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void But_Clear_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("是否重置计数....", "提示!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.deviceConfig.ClickTimes = 0;
                this.deviceConfig.DragTimes = 0;
                Number_Clicks.Text = 0.ToString() ;
                Num_Sliding.Text =0.ToString();
                DeviceConfig.SaveDeviceConfigToFile(ConfigPath, this.deviceConfig);
            }
        }
    }
}
