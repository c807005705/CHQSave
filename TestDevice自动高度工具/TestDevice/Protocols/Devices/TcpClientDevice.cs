// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Mobot.Utils.IO;

namespace Mobot.TestBox.Protocols
{
    public class TcpClientDevice : IDevice
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int port = 1898;
        /// <summary>
        /// 监听端口
        /// </summary>
        public int Port
        {
            get { return port; }
            set { this.port = value; }
        }
        IPAddress address = IPAddress.Loopback;// IPAddress.Parse("127.0.0.1");
        /// <summary>
        /// 监听本地地址。
        /// </summary>
        public IPAddress Address
        {
            get { return address; }
            set { this.address = value; }
        }

        #region IDevice 成员

        public void Open()
        {
            this.Start();
        }

        /// <summary>
        /// Occurs when [serial port IO].
        /// </summary>
        public event EventHandler<SerialPortIOEventArgs> IO;

        public int Write(byte[] buffer, int offset, int count)
        {
            int result = 0;
            // Send the recieved data to all clients (including sender for echo)
            foreach (SocketChatClient clientSend in m_aryClients)
            {
                try
                {
                    if (IO != null)
                        IO(this, new SerialPortIOEventArgs(true, buffer, offset, count));
                    result = clientSend.Sock.Send(buffer, offset, count, SocketFlags.None);
                }
                catch
                {
                    // If the send fails the close the connection
                    Console.WriteLine("Send to client {0} failed", clientSend.Sock.RemoteEndPoint);
                    clientSend.Sock.Close();
                    m_aryClients.Remove(clientSend);
                }
            }
            return result;
        }

        public event EventHandler DataReceived;

        #endregion

        Socket listener = null;
        public TcpClientDevice(IPAddress address, int port)
        {
            this.address = address;
            this.port = port;
        }

		// Attributes
		private ArrayList m_aryClients = new ArrayList();	// List of Client Connections
		/// <summary>
		/// Application starts here. Create an instance of this class and use it
		/// as the main object.
		/// </summary>
		/// <param name="args"></param>
		public void Start()
		{
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Connect(new IPEndPoint(address, port));

            NewConnection(listener);
        }
        public bool IsOpen
        {
            get
            {
                if (listener == null)
                    return false;
                return true;
            }
        }
        public void Close()
        {
            if (listener != null)
            {
                listener.Close();
                listener = null;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Add the given connection to our list of clients
        /// Note we have a new friend
        /// Send a welcome to the new client
        /// Setup a callback to recieve data
        /// </summary>
        /// <param name="sockClient">Connection to keep</param>
        //public void NewConnection( TcpListener listener )
        public void NewConnection(Socket sockClient)
        {
            // Program blocks on Accept() until a client connects.
            //SocketChatClient client = new SocketChatClient( listener.AcceptSocket() );
            SocketChatClient client = new SocketChatClient(sockClient);
            m_aryClients.Add(client);
            Console.WriteLine("Client {0}, joined", client.Sock.RemoteEndPoint);

            //// Get current date and time.
            //DateTime now = DateTime.Now;
            //String strDateLine = "Welcome " + now.ToString("G") + "\n\r";

            //// Convert to byte array and send.
            //Byte[] byteDateLine = System.Text.Encoding.ASCII.GetBytes(strDateLine.ToCharArray());
            //client.Sock.Send(byteDateLine, byteDateLine.Length, 0);

            client.SetupRecieveCallback(this);
        }
        byte[] data = null;
        public byte[] ReadExists()
        {
            byte[] result = this.data;
            this.data = null;

            return result;
        }

        public bool HasData
        {
            get
            {
                return (data != null && data.Length > 0);
            }
        }

        /// <summary>
        /// Get the new data and send it out to all other connections. 
        /// Note: If not data was recieved the connection has probably 
        /// died.
        /// </summary>
        /// <param name="ar"></param>
        public void OnRecievedData(IAsyncResult ar)
        {
            SocketChatClient client = (SocketChatClient)ar.AsyncState;
            try
            {
                byte[] aryRet = client.GetRecievedData(ar);

                // If no data was recieved then the connection is probably dead
                if (aryRet.Length < 1)
                {
                    Console.WriteLine("disconnected");
                    client.Sock.Close();
                    m_aryClients.Remove(client);
                    return;
                }
                this.data = aryRet;
            }
            catch (Exception ex)
            {
                log.Error("接收数据时失败。", ex);
            }
            if (IO != null)
                IO(this, new SerialPortIOEventArgs(false, data, 0, data.Length));
            if (DataReceived != null)
                DataReceived(this, EventArgs.Empty);
            //// Send the recieved data to all clients (including sender for echo)
            //foreach (SocketChatClient clientSend in m_aryClients)
            //{
            //    try
            //    {
            //        clientSend.Sock.Send(aryRet);
            //    }
            //    catch
            //    {
            //        // If the send fails the close the connection
            //        Console.WriteLine("Send to client {0} failed", client.Sock.RemoteEndPoint);
            //        clientSend.Sock.Close();
            //        m_aryClients.Remove(client);
            //        return;
            //    }
            //}
            try
            {
                client.SetupRecieveCallback(this);
            }
            catch (Exception ex)
            {
                log.Error("接收数据时失败。", ex);
            }
        }


        /// <summary>
        /// Class holding information and buffers for the Client socket connection
        /// </summary>
        private class SocketChatClient
        {
            private Socket m_sock;						// Connection to the client
            private byte[] m_byBuff = new byte[1024 * 1024 * 4];		// Receive data buffer
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="sock">client socket conneciton this object represents</param>
            public SocketChatClient(Socket sock)
            {
                m_sock = sock;
                m_sock.ReceiveBufferSize = 1024*1024;
                m_sock.NoDelay = true;
            }

            // Readonly access
            public Socket Sock
            {
                get { return m_sock; }
            }

            /// <summary>
            /// Setup the callback for recieved data and loss of conneciton
            /// </summary>
            /// <param name="app"></param>
            public void SetupRecieveCallback(TcpClientDevice app)
            {
                try
                {
                    AsyncCallback recieveData = new AsyncCallback(app.OnRecievedData);
                    m_sock.BeginReceive(m_byBuff, 0, m_byBuff.Length, SocketFlags.None, recieveData, this);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Recieve callback setup failed! {0}", ex.Message);
                }
            }

            /// <summary>
            /// Data has been recieved so we shall put it in an array and
            /// return it.
            /// </summary>
            /// <param name="ar"></param>
            /// <returns>Array of bytes containing the received data</returns>
            public byte[] GetRecievedData(IAsyncResult ar)
            {
                int nBytesRec = 0;
                try
                {
                    nBytesRec = m_sock.EndReceive(ar);
                }
                catch { }
                byte[] byReturn = new byte[nBytesRec];
                Array.Copy(m_byBuff, byReturn, nBytesRec);

                
                // Check for any remaining data and display it
                // This will improve performance for large packets 
                // but adds nothing to readability and is not essential
                int nToBeRead = m_sock.Available;
                if( nToBeRead > 0 )
                {
                    byte [] byData = new byte[nToBeRead];
                    m_sock.Receive( byData );
                    Array.Resize(ref byReturn,nToBeRead+nBytesRec);
                    Array.Copy(byData,0,byReturn,nBytesRec,nToBeRead);
                    nBytesRec += nToBeRead;
                    nToBeRead = m_sock.Available;
                    // Append byData to byReturn here
                }
                Thread.Sleep(30);
                return byReturn;
            }
        }

    }
}
