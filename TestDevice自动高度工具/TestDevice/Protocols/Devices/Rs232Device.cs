using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Mobot.Utils.IO;
using SerialPort = Mobot.Utils.IO.SerialPort;

namespace Protocols
{
    public class Rs232Device : IDevice
    {
        SerialPort serialPort = null;
        public SerialPort Port { get { return serialPort; } }
        public Rs232Device(string portName, int baudRate)
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.WriteBufferSize = 1024 * 1024;
            serialPort.ReadBufferSize = 1024 * 1024;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
        }

        public event EventHandler DataReceived;
        public event EventHandler<SerialPortIOEventArgs> IO
        {
            add { serialPort.SerialPortIO += value; }
            remove { serialPort.SerialPortIO -= value; }
        }

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                if (DataReceived != null)
                    DataReceived(this, EventArgs.Empty);
            }
        }

        public string PortName
        {
            get { return serialPort.PortName; }
            set { serialPort.PortName = value; }
        }
        public int BaudRate
        {
            get { return serialPort.BaudRate; }
            set { serialPort.BaudRate = value; }
        }

        #region IDevice 成员

        public void Open()
        {
            serialPort.Open();
        }

        public void Close()
        {
            serialPort.Close();
        }
        public bool IsOpen
        {
            get { return serialPort.IsOpen; }
        }

        public int Write(byte[] buffer, int offset, int count)
        {
            serialPort.Write(buffer, offset, count);
            return count;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return serialPort.Read(buffer, offset, count);
        }

        public int BytesToRead
        {
            get { return serialPort.BytesToRead; }
        }
        public int ReadByte()
        {
            return serialPort.ReadByte();
        }

        public byte[] ReadExists()
        {
            if (serialPort.BytesToRead < 1)
                return null;
            byte[] buffer = new byte[serialPort.BytesToRead];
            int count = serialPort.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public bool HasData
        {
            get { return (serialPort.BytesToRead > 0); }
        }

        #endregion
    }
}
