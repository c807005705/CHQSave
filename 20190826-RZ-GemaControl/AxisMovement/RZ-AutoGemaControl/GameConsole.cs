using CameraLibs;
using ControlDec;
using ControlLib;
using Device_Link_LTSMC;
using Interface.Interface;
using Interface.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;


namespace RZ_AutoGemaControl
{
    public partial class GameConsole : Form, IWindow
    {

        public IServer Server { get; set; }

        public ITaskInoke TaskInoke { get; set; }
       public ICamera Camera { get; set; }
        public Camera camera { get; set; }
        public IActionInterface actionInterface { get; set; }
        private bool isClick = true;
        private bool IsCenter = true;
        /// <summary>
        /// 配置文件
        /// </summary>
        public Config Config { get; set; } = new Config();
        public ILog Log { get; set; }
        public IControlDevice ConDevice { get; set; }
        public IControlCom controlCom { get; set; }
        public IActionInterface action { get; set; }
        /// <summary>
        /// 普通日志
        /// </summary>
        public bool IsNormalLog { get; set; } = true;
        /// <summary>
        /// 警告日志
        /// </summary>
        public bool IsWaringLog { get; set; } = true;
        /// <summary>
        /// 错误日志
        /// </summary>
        public bool IsErrorLog { get; set; } = false;

        private Thread backtozerothread = null;
        public bool IsLink;
        public bool IsOver;
        public Point PointXY;
        public Point PointZ;
        public Point PointUV;
        public Point PointW;
        public bool IsCameraOpen;
        private bool Wdown;
        private bool Ddown;
        private bool Sdown;
        private bool Adown;
        private Thread getPictrue;

        public GameConsole()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectingDevice_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ConDevice.IsLink)
                {
                    ConDevice.Link(Config.FactoryConfig.AxisOtherInfo);
                    TaskInoke.InitInterface();
                    TaskInoke.Link("127.0.0.1", 8080);
                    if (ConDevice.IsLink)
                    {
                        MessageBox.Show("设备连接成功");
                        ConnectingDevice.Enabled = false;
                        StoRunning.Enabled = true;
                        GetCoordinates();
                        Camera.Open();
                       getPictrue = new Thread(GetOnrPictrue);
                        getPictrue.IsBackground = true;
                        getPictrue.Start();                                           
                    }
                    else
                    {
                        MessageBox.Show("设备连接失败，请检查！");
                    }    
                }
                else
                {
                    MessageBox.Show("设备已连接");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //string[] ports = SerialPort.GetPortNames();
            //PortNameText.Items.AddRange(ports);
            //PortNameText.SelectedItem = PortNameText.Items[0];
            //controlCom.Init(PortNameText.Text, 115200, Parity.None, 8, StopBits.None);
            //controlCom.Open();
           
        }

        private void GetOnrPictrue()
        {
            
            Task<Bitmap> bitmap = camera.GetOneBitmap();
            bitmap.Start();
            pictureBox11.Image = bitmap.Result;
        }

        /// <summary>
        /// 获取实时坐标
        /// </summary>
        private void GetCoordinates()
        {   
            new Thread(() =>
            {
                while (true)
                {          
                    try
                    {
                        PointXY.X = (int)(ConDevice.GetAxisPosition(Axis.X) );       
                        PointXY.Y = (int)(ConDevice.GetAxisPosition(Axis.Y));
                        PointZ.X = (int)(ConDevice.GetAxisPosition(Axis.Z));            
                        PointUV.X = (int)(ConDevice.GetAxisPosition(Axis.U));
                        PointUV.Y = (int)(ConDevice.GetAxisPosition(Axis.V));
                        PointW.X = (int)(ConDevice.GetAxisPosition(Axis.W));
                        AllPosition.BeginInvoke(new Action(() =>
                               AllPosition.Text = string.Format("X:{0},Y:{1},Z:{2},U:{3},V:{4},W:{5}", PointXY.X, PointXY.Y, PointZ.X,PointUV.X, PointUV.Y, PointW.X)));
                        AllPosition.BeginInvoke(new Action(() =>
                          AllPosition.Text = DragBar.Value.ToString())); 
                    }
                    catch (Exception ex)
                    {
                        Log.waring("读取坐标信息异常");
                        Log.error(ex);
                    }
                    Thread.Sleep(10);
                }
            })
            { IsBackground = true }.Start();

        }
        /// <summary>
        /// 回零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnToZero_Click(object sender, EventArgs e)
        {
            ConDevice.AllToZero();
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Setting_Click(object sender, EventArgs e)
        {
            Setting setting = new Setting();
            setting.ShowDialog();

        }
        /// <summary>
        /// 回零线程
        /// </summary>

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisconnectDevice_Click(object sender, EventArgs e)
        {
            try
            {   
                ConDevice.Stop();
                StoRunning.Enabled = false;
                ConnectingDevice.Enabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("设备断开失败" + e.ToString());
            }





        }
        /// <summary>
        /// 停止所有轴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StoRunning_Click(object sender, EventArgs e)
        {

            ConDevice.StopAll();
        }
        /// <summary>
        /// 方向轴归零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovePartialZeroReturn_Click(object sender, EventArgs e)
        {
            try
            {
                ConDevice.AxisRunToZero(Axis.Z);
                ConDevice.AxisRunToZero(Axis.X);
                ConDevice.AxisRunToZero(Axis.Y);
            }
            catch (Exception ex)
            {
                MessageBox.Show("方向轴归零失败:" + ex.ToString());
            }

        }
        /// <summary>
        /// 技能释放轴归零
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkillPartialZeroReturn_Click(object sender, EventArgs e)
        {
            try
            {
                ConDevice.AxisRunToZero(Axis.W);
                ConDevice.AxisRunToZero(Axis.V);
                ConDevice.AxisRunToZero(Axis.U);

            }
            catch (Exception ex)
            {
                MessageBox.Show("技能释放轴归零失败：" + ex.ToString());
            }
        }
        /// <summary>
        /// 方向控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Movebackward_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsLink)
                {
                    PictureBox pictureBox = sender as PictureBox;

                    switch (pictureBox.Name)
                    {
                        case "Movebackward"://前Y
                            if (PointXY.Y * (5000 / 16) >= Convert.ToInt32(moveDistance.Text) * (5000 / 16))
                                ConDevice.MoveAxis(Axis.Y, (Convert.ToInt32(moveDistance.Text) * (5000 / 16)), true, Direction.Back);
                            else
                                ConDevice.MoveAxis(Axis.Y, -PointXY.Y * (5000 / 16), true, Direction.Back);
                            break;
                        case "MoveForward"://后Y
                            if (PointXY.Y + Convert.ToInt32(moveDistance.Text) <= 200)
                                ConDevice.MoveAxis(Axis.Y, (Convert.ToInt32(moveDistance.Text) * (5000 / 16)), true, Direction.Forward);
                            else
                                MessageBox.Show("超过可移动范围");
                            break;
                        case "MoveLift"://左X
                            if (PointXY.X * (5000 / 25) >= Convert.ToDouble(Convert.ToInt32(moveDistance.Text) * (5000 / 25)))
                                ConDevice.MoveAxis(Axis.X, (Convert.ToInt32(moveDistance.Text) * (5000 / 25)), true, Direction.Back);
                            else
                                ConDevice.MoveAxis(Axis.X, -PointXY.X * (5000 / 25), true, Direction.Back);
                            break;
                        case "MoveRight"://右X
                            if (PointXY.X + Convert.ToInt32(moveDistance.Text) <= 100)
                                ConDevice.MoveAxis(Axis.X, (Convert.ToInt32(moveDistance.Text) * (5000 / 25)), true, Direction.Forward);
                            else
                                MessageBox.Show("超过可移动范围");
                            break;
                        case "MoveUp"://上Z
                            if (PointZ.X * (5000 / 7) >= Convert.ToDouble(Convert.ToInt32(moveDistance.Text) * (5000 / 7)))
                                ConDevice.MoveAxis(Axis.Z, (Convert.ToInt32(moveDistance.Text) * (5000 / 7)), true, Direction.Forward);
                            else

                                ConDevice.MoveAxis(Axis.Z, -PointZ.X * (5000 / 7), true, Direction.Forward);
                            break;
                        case "MoveDown"://下Z
                            if (Convert.ToInt32(moveDistance.Text) * (5000 / 7) < (100000 / 7) && (PointZ.X + Convert.ToInt32(moveDistance.Text)) <= 20)
                                ConDevice.MoveAxis(Axis.Z, (Convert.ToInt32(moveDistance.Text) * (5000 / 7)), true, Direction.Back);
                            else
                                MessageBox.Show("超过可移动范围");
                            break;
                        case "SkillBackword":
                            if (PointUV.Y * (5000 / 16) >= (Convert.ToInt32(moveDistance.Text) * (5000 / 16)))
                                ConDevice.MoveAxis(Axis.V, (Convert.ToInt32(moveDistance.Text) * (5000 / 16)), true, Direction.Back);
                            else
                                ConDevice.MoveAxis(Axis.V, -PointUV.Y * (5000 / 16), true, Direction.Back);
                            break;
                        case "SkillForword":
                            if (PointUV.Y + Convert.ToInt32(moveDistance.Text) <= 200)
                                ConDevice.MoveAxis(Axis.V, (Convert.ToInt32(moveDistance.Text) * (5000 / 16)), true, Direction.Forward);
                            else
                                MessageBox.Show("超过可移动范围");
                            break;
                        case "SkillLift":
                            if (PointUV.X * (5000 / 25) >= (Convert.ToInt32(moveDistance.Text) * (5000 / 25)))
                                ConDevice.MoveAxis(Axis.U, (Convert.ToInt32(moveDistance.Text) * (5000 / 25)), true, Direction.Back);
                            else
                                ConDevice.MoveAxis(Axis.U, -PointUV.X * (5000 / 25), true, Direction.Back);
                            break;
                        case "SkillRight":
                            if (PointUV.X + Convert.ToInt32(moveDistance.Text) <= 200)
                                ConDevice.MoveAxis(Axis.U, (Convert.ToInt32(moveDistance.Text) * (5000 / 25)), true, Direction.Forward);
                            else
                                MessageBox.Show("超过可移动范围");
                            break;
                        case "SkillUp":
                            if (PointW.X * (5000 / 7) >= (Convert.ToInt32(moveDistance.Text) * (5000 / 7)))
                                ConDevice.MoveAxis(Axis.W, (Convert.ToInt32(moveDistance.Text) * (5000 / 7)), true, Direction.Forward);
                            else
                                ConDevice.MoveAxis(Axis.W, -PointUV.X * (5000 / 7), true, Direction.Forward);
                            break;
                        case "SkillDown":
                            if (Convert.ToInt32(moveDistance.Text) * (5000 / 7) < (100000 / 7) && (PointW.X + Convert.ToInt32(moveDistance.Text) <= 23))
                                ConDevice.MoveAxis(Axis.W, Convert.ToInt32(moveDistance.Text) * (5000 / 7), true, Direction.Back);
                            else
                                MessageBox.Show("超过可移动范围");
                            break;
                    }
                }

                else
                {
                    MessageBox.Show("请先连接设备");
                }
                IsOver = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 展示窗口
        /// </summary>
        public void ShowWindow()
        {
            this.ShowDialog();

        }
        private void GameConsole_Load(object sender, EventArgs e)
        {
          this.KeyPreview = true;
            Log.LogMsg += LogMsg;

           
        }

        private void LogMsg(LogType obj, string msg)
        {
            switch (obj)
            {
                case LogType.Normal:
                    if (!IsNormalLog)
                        return;
                    break;
                case LogType.Waring:
                    if (!IsWaringLog)
                        return;
                    break;
                case LogType.Error:
                    if (!IsErrorLog)
                        return;
                    break;
                default:
                    break;
            }
        }

        private void GameConsole_KeyUp(object sender, KeyEventArgs e)
        
        {
            try
            {
              
                switch (e.KeyValue)
                {
                    case 'W':

                        Wdown = false;
                        break;
                    case 'D':

                        Ddown = false;
                        break;
                    case 'S':

                        Sdown = false;
                        break;
                    case 'A':

                        Adown = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误" + ex);
            }
        }

        private void GameConsole_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                switch (e.KeyValue)
                {
                    case 'W':
                        if (  Wdown == false)
                            Wdown = true;
                        break;
                    case 'D':
                        if (Ddown == false)
                            Ddown = true;
                        break;
                    case 'S':
                        if (Sdown == false)
                            Sdown = true;
                        break;
                    case 'A':
                        if (Adown == false)
                            Adown = true;
                        break;
                }

                if (Wdown && !Ddown && !Adown && !Sdown)
                {
                    if (IsCenter)
                    {
                        ConDevice.PreHeight(Axis.Z, 6);
                        IsCenter = false;
                    }
                    ConDevice.DirMove(90);

                }
                else if (Wdown && Ddown)
                {
                    if (IsCenter)
                    {
                        ConDevice.PreHeight(Axis.Z, 6);
                        IsCenter = false;
                    }
                    ConDevice.DirMove(45);

                }
                else if (!Wdown && Ddown && !Adown && !Sdown)
                {
                    if (IsCenter)
                    {
                        ConDevice.PreHeight(Axis.Z, 6);
                        IsCenter = false;
                    }
                    ConDevice.DirMove(0);
                }
                else if (Sdown && Ddown)
                {
                    if (IsCenter)
                    {
                        ConDevice.PreHeight(Axis.Z, 6);
                        IsCenter = false;
                    }
                    ConDevice.DirMove(315);
                }
                else if (!Wdown && !Ddown && !Adown && Sdown)
                {
                    if (IsCenter)
                    {
                        ConDevice.PreHeight(Axis.Z, 6);
                        IsCenter = false;
                    }
                    ConDevice.DirMove(270);
                }
                else if (Sdown && Adown)
                {
                    if (IsCenter)
                    {
                        ConDevice.PreHeight(Axis.Z, 6);
                        IsCenter = false;
                    }
                    ConDevice.DirMove(225);
                }
                else if (!Wdown && !Ddown && Adown && !Sdown)
                {
                    if (IsCenter)
                    {
                        ConDevice.PreHeight(Axis.Z, 6);
                        IsCenter = false;
                    }
                    ConDevice.DirMove(180);
                }
                else if (Adown && Wdown)
                {
                    if (IsCenter)
                    {
                        ConDevice.PreHeight(Axis.Z, 6);
                        IsCenter = false;
                    }
                    ConDevice.DirMove(135);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.K:
                        ConDevice.MoveToPositionUV(61, 143);

                        if (isClick)
                        {
                            ConDevice.PreHeight(Axis.W, 12);

                            isClick = false;
                        }
                        ConDevice.SkillClick(40000, 9);
                        break;
                    case Keys.J:
                        ConDevice.MoveToPositionUV(83, 145);
                        ConDevice.SkillClick(40000, 9);
                        break;
                    case Keys.I:
                        ConDevice.MoveToPositionUV(75, 133);
                        ConDevice.SkillClick(40000, 9);
                        break;
                    case Keys.O:
                        ConDevice.MoveToPositionUV(61, 123);
                        ConDevice.SkillClick(40000, 9);
                        break;
                    case Keys.B:
                        ConDevice.MoveToPositionUV(90, 136);
                        ConDevice.SkillClick(40000, 9);
                        break;
                    case Keys.H:
                        ConDevice.MoveToPositionUV(83, 123);
                        ConDevice.SkillClick(40000, 9);
                        break;
                    case Keys.U:
                        ConDevice.MoveToPositionUV(70, 115);
                        ConDevice.SkillClick(40000, 9);
                        break;
                    case Keys.Space:
                        ConDevice.MoveToPositionXY(70, 124);
                        ConDevice.AxisMoveTo(Axis.Z, -12);
                        IsCenter = true;
                        break;
                    case Keys.Q:
                        ConDevice.AxisMoveTo(Axis.Z, -12);
                        ConDevice.MoveToPositionXY(59, 140);
                        ConDevice.MoveClick(40000, 9);
                        IsCenter = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误" + ex);
            }
        }     
        /// <summary>
        /// 窗口关闭前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsCameraOpen)
            {
                Camera.Dispose();
                Camera.Close();
            }
           
           
        }

        private void StartRecording_Click(object sender, EventArgs e)
        {
           
        }

        private void SpeedDisplay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (moveDistance.Text == string.Empty)
                {
                    return;
                }
                else
                {

                    int MoveDistance = Convert.ToInt32(moveDistance.Text);

                    if (MoveDistance < DragBar.Minimum || MoveDistance > DragBar.Maximum)
                        return;
                    DragBar.Value = MoveDistance;
                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void moveDistance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键 
            {
                int len = moveDistance.Text.Length;
                if (len < 1 && e.KeyChar == '0')
                {
                    e.Handled = true;
                }
                else if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字 
                {
                    e.Handled = true;
                }
            }
        }

        private void DragBar_Scroll(object sender, EventArgs e)
        {
            DragBar.Minimum = 0;
            DragBar.Maximum = 100;
            moveDistance.Text = DragBar.Value.ToString();
        }
    }
}
