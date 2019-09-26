using Interface.Interface;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLib
{
    public class ControlCom : IControlCom
    {
        private SerialPort port = new SerialPort();
        public ILog Log { get; set; }
        /// <summary>
        /// 是否成功操作上次指令
        /// </summary>
        public bool IsSuccessOption { get; private set;}
        /// <summary>
        /// 端口号
        /// </summary>
        public string PortName { get; private set; }
        /// <summary>
        /// 波特率
        /// </summary>
        public int BandRoate { get; private set; }
        /// <summary>
        /// 校验位
        /// </summary>
        public Parity Parity { get; private set; }
        /// <summary>
        /// 数据位
        /// </summary>
        public int DataP { get; private set; }
        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBit { get; private set; }
        /// <summary>
        /// 成功操作结束符
        /// </summary>
        private byte ok = 75;
        /// <summary>
        /// 错误操作结束符
        /// </summary>
        private byte er = 0x52;

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            if (port !=null &&port.IsOpen)
            {
                port.Close();
                Log.log("控制板被关闭");
            }
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Close();
            port.Dispose();
            port= null;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="bandRoate"></param>
        /// <param name="parity"></param>
        /// <param name="dataP"></param>
        /// <param name="stopBits"></param>
        public void Init(string portName, int bandRoate, Parity parity, int dataP, StopBits stopBits)
        {
            this.PortName = portName;
            this.BandRoate = bandRoate;
            this.Parity = parity;
            this.DataP = dataP;
            this.StopBit = stopBits;
        }
        /// <summary>
        /// 打开连接
        /// </summary>
        public void Open()
        {
            ok = Format("K");
            er = Format("R");
            if (!port.IsOpen)
            {
                port.PortName = this.PortName;
                port.BaudRate = this.BandRoate;
                port.Parity = this.Parity;
                port.DataBits = this.DataP;
                port.StopBits = this.StopBit;
            }
            else
            {
                port.Close();
                port.PortName = this.PortName;
                port.BaudRate = this.BandRoate;
                port.Parity = this.Parity;
                port.DataBits = this.DataP;
                port.StopBits = this.StopBit;
                port.Open();

            }
        }
        /// <summary>
        /// 规范化字符
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected byte Format(string msg)
        {
            return Convert.ToByte((msg[0] - '0').ToString("x8"), 16);
        }

        //public void ReadPressRadio1(int radio)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ReadPressRadio2(int radio)
        //{
        //    throw new NotImplementedException();
        //}

        //public double ReadPressValue1()
        //{
           
        //}

        //public double ReadPressValue2()
        //{
        //    throw new NotImplementedException();
        //}

        //public string ReadVersion()
        //{
        //    throw new NotImplementedException();
        //}

        //public void SetCanTriggerMinPressComm1(double value)
        //{
        //    throw new NotImplementedException();
        //}

        //public void SetCanTriggerMinPressComm2(double value)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
