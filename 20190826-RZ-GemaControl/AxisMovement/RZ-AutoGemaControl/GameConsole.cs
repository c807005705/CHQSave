﻿using ControlDec;
using ControlLib;
using Device_Link_LTSMC;
using Interface.Interface;
using Interface.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        public IActionInterface actionInterface { get; set; }
        private Thread thread = null;
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

        private Thread backtozerothread = null;
        public bool IsLink;
        public bool IsOver;
        public Point PointXY;
        public Point PointZ;
        public Point PointUV;
        public Point PointW;


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
                ConDevice.Link(Config.FactoryConfig.AxisOtherInfo);
                TaskInoke.InitInterface();
                TaskInoke.Link("127.0.0.1", 8080);
                IsLink = true;
                MessageBox.Show("设备连接成功");

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
            GetCoordinates();
            label4.Text = ConDevice.GetPositionInfo();
        }
        /// <summary>
        /// 获取实时坐标
        /// </summary>
        private void GetCoordinates()
        {

            double X;
            double Y;
            double Z;
            double U;
            double V;
            double W;
            new Thread(() =>
            {
                while (true)
                {
                    //if (ConDevice.GetAxisPosition(Axis.X)-Convert.ToDouble( movingDistance.Text)>0)
                    X = ConDevice.GetAxisPosition(Axis.X);
                    Y = ConDevice.GetAxisPosition(Axis.Y);
                    Z = ConDevice.GetAxisPosition(Axis.Z);
                    U = ConDevice.GetAxisPosition(Axis.U);
                    V = ConDevice.GetAxisPosition(Axis.V);
                    W = ConDevice.GetAxisPosition(Axis.W);
                    try
                    {
                        PointXY.Y = Convert.ToInt32(Y / (5000 / 16));
                        PointXY.X = Convert.ToInt32(X );
                        PointXY.Y = Convert.ToInt32(Y);
                        PointZ.X = Convert.ToInt32(Z );
                        movePosition.BeginInvoke(new Action(() =>
                           movePosition.Text = string.Format("X:{0},Y:{1},Z:{2}", PointXY.X, PointXY.Y, PointZ.X)));
                        PointUV.X = Convert.ToInt32(U);
                        PointUV.Y = Convert.ToInt32(V );
                        PointW.X = Convert.ToInt32(W );
                        killPosition.BeginInvoke(new Action(() =>
                               killPosition.Text = string.Format("X:{0},Y:{1},Z:{2}", PointUV.X, PointUV.Y, PointW.X)));
                        SpeedDisplay.BeginInvoke(new Action(() =>
                          SpeedDisplay.Text = SpeedRun.Value.ToString()));
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
            BackZero();
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
        public void BackZero()
        {
            backtozerothread = new Thread(BackTozeroThread)
            {

                IsBackground = true
            };
            backtozerothread.Start();
        }

        private void BackTozeroThread()
        {
            ConDevice.AllToZero();
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisconnectDevice_Click(object sender, EventArgs e)
        {
            try
            {

                controlCom.Close();


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
                            if (PointXY.Y * (5000 / 16) >= Convert.ToInt32(SpeedDisplay.Text) * (5000.00 / 16.00))
                                ConDevice.MoveAxis(Axis.Y, (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 16)), true, Direction.Back);
                            else
                                ConDevice.MoveAxis(Axis.Y, -PointXY.Y * (5000 / 16), true, Direction.Back);
                            break;
                        case "MoveForward"://后Y
                            if (PointXY.Y + Convert.ToInt32(SpeedDisplay.Text) <= 200)
                                ConDevice.MoveAxis(Axis.Y, (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 16)), true, Direction.Forward);
                            else
                                MessageBox.Show("超过可移动范围");
                            break;
                        case "MoveLift"://左X
                            if (PointXY.X * (5000 / 25) >= Convert.ToDouble(Convert.ToInt32(SpeedDisplay.Text) * (5000 / 25)))
                                ConDevice.MoveAxis(Axis.X, (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 25)), true, Direction.Back);
                            else
                                ConDevice.MoveAxis(Axis.X, -PointXY.X * (5000 / 25), true, Direction.Back);
                            break;
                        case "MoveRight"://右X
                            if (PointXY.X + Convert.ToInt32(SpeedDisplay.Text) <= 100)
                                ConDevice.MoveAxis(Axis.X, (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 25)), true, Direction.Forward);
                            else
                                MessageBox.Show("超过可移动范围");
                            break;
                        case "MoveUp"://上Z
                            if (PointZ.X * (5000 / 7) >= Convert.ToDouble(Convert.ToInt32(SpeedDisplay.Text) * (5000 / 7)))
                                ConDevice.MoveAxis(Axis.Z, (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 7)), true, Direction.Forward);
                            else

                                ConDevice.MoveAxis(Axis.Z, -PointZ.X * (5000 / 7), true, Direction.Forward);
                            break;
                        case "MoveDown"://下Z
                            if (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 7) < (100000 / 7) && (PointZ.X + Convert.ToInt32(SpeedDisplay.Text)) <= 20)
                                ConDevice.MoveAxis(Axis.Z, (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 7)), true, Direction.Back);
                            else
                                MessageBox.Show("超过可移动范围");
                            break;
                        case "SkillBackword":
                            if (PointUV.Y * (5000 / 16) >= (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 16)))
                                ConDevice.MoveAxis(Axis.V, (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 16)), true, Direction.Back);
                            else
                                ConDevice.MoveAxis(Axis.V, -PointUV.Y * (5000 / 16), true, Direction.Back);
                            break;
                        case "SkillForword":
                            if (PointUV.Y + Convert.ToInt32(SpeedDisplay.Text) <= 200)
                                ConDevice.MoveAxis(Axis.V, (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 16)), true, Direction.Forward);
                            else
                                MessageBox.Show("超过可移动范围");
                            break;
                        case "SkillLift":
                            if (PointUV.X * (5000 / 25) >= (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 25)))
                                ConDevice.MoveAxis(Axis.U, (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 25)), true, Direction.Back);
                            else
                                ConDevice.MoveAxis(Axis.U, -PointUV.X * (5000 / 25), true, Direction.Back);
                            break;
                        case "SkillRight":
                            if (PointUV.X + Convert.ToInt32(SpeedDisplay.Text) <= 200)
                                ConDevice.MoveAxis(Axis.U, (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 25)), true, Direction.Forward);
                            else
                                MessageBox.Show("超过可移动范围");
                            break;
                        case "SkillUp":
                            if (PointW.X * (5000 / 7) >= (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 7)))
                                ConDevice.MoveAxis(Axis.W, (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 7)), true, Direction.Forward);
                            else
                                ConDevice.MoveAxis(Axis.W, -PointUV.X * (5000 / 7), true, Direction.Forward);
                            break;
                        case "SkillDown":
                            if (Convert.ToInt32(SpeedDisplay.Text) * (5000 / 7) < (100000 / 7) && (PointW.X + Convert.ToInt32(SpeedDisplay.Text) <= 23))
                                ConDevice.MoveAxis(Axis.W, Convert.ToInt32(SpeedDisplay.Text) * (5000 / 7), true, Direction.Back);
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



        public void Button1_Click(object sender, EventArgs e)
        {
            ConDevice.moveX();

        }


        private static void test(object source, ElapsedEventArgs e)
        {
            MessageBox.Show("");

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            actionInterface.FlatA(8);
            actionInterface.Skill1(70.00, 100.00, 7);
            actionInterface.FlatA(-8);

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            actionInterface.FlatA(8);

            actionInterface.FlatA(-8);

        }

        private void GameConsole_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }



        private void GameConsole_KeyUp(object sender, KeyEventArgs e)
        
        {
            try
            {
                if (Keys.A == e.KeyCode || Keys.D == e.KeyCode)
                {
                    //ConDevice.stopAxis(Axis.X);
                    //MessageBox.Show("a||d");
                }
                else if (Keys.S == e.KeyCode || Keys.W == e.KeyCode)
                {
                   // ConDevice.stopAxis(Axis.Y);
                   // MessageBox.Show("s||w");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("错误" + ex);
            }
        }

        private void GameConsole_KeyDown(object sender, KeyEventArgs e)
        {
            
            double XPosition=  ConDevice.GetAxisPosition(Axis.X);
            double  YPosition= ConDevice.GetAxisPosition(Axis.Y);
            double UPosition = ConDevice.GetAxisPosition(Axis.U); 
            double VPosition = ConDevice.GetAxisPosition(Axis.V);

            try
            {
                if (Keys.D == e.KeyCode)
                {                 
                   
                   
                    if (IsCenter)
                    {
                       // ConDevice.PreHeight(Axis.Z, 12);
                        ConDevice.PreHeight(Axis.Z, 6);
                        IsCenter = false;
                    }
                    ConDevice.DirMove(330);
                }
                else if (Keys.W == e.KeyCode)
                {                   
                   
                    
                    if (IsCenter)
                    {
                       // ConDevice.PreHeight(Axis.Z, 12);
                        ConDevice.PreHeight(Axis.Z, 6);
                        IsCenter = false;
                    }
                    ConDevice.DirMove(40);
                }
                else if (Keys.A == e.KeyCode)
                {                  
                   
                   
                    if (IsCenter)
                    {
                        //ConDevice.PreHeight(Axis.Z, 12);
                        ConDevice.PreHeight(Axis.Z, 6);
                        IsCenter = false;
                    }
                    ConDevice.DirMove(150);
                }
                else if (Keys.S == e.KeyCode)
                {
                   
                   
                    if (IsCenter)
                    {
                        //ConDevice.PreHeight(Axis.Z, 12);
                        ConDevice.PreHeight(Axis.Z, 6);
                        IsCenter = false;
                    }
                    ConDevice.DirMove(240);
                }
                switch (e.KeyCode)
                {
                   
                    case Keys.K:
                        ConDevice.MoveToPositionUV(61,143);
                       
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
                        ConDevice.MoveClick(40000,9);
                        IsCenter = true;
                        break;
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误" + ex);
            }
        }
    }
}
