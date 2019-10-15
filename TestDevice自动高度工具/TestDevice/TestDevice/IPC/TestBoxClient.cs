using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;
using System.Text;
using Mobot.Utils.IO;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Mobot.Utils.Caches;

namespace Mobot.TestBox
{
    public partial class TestBoxClient : IPCClient<ITestBox>, ITestBox
    {
        public static TestBoxClient instance;
        public static TestBoxClient Default
        {
            get
            {
                if (instance == null)
                {
                    instance = new TestBoxClient();
                }
                return instance;
            }
        }
        public TestBoxClient()
        {
            CheckImageMatchServer();
            base.Connect("TestBoxDriver");
        }
        private void CheckImageMatchServer()
        {
            string name = "Mobot.TestBox.Console";
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name + ".exe");
            if (!File.Exists(file)) return;
            Process[] list = Process.GetProcessesByName(name);
            if (list.Length == 1) return;

            Thread.Sleep(200);
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo(file)
            {
                UseShellExecute = false,
#if DEBUG
                CreateNoWindow = false,
#else
                CreateNoWindow = true,
#endif
            };
            process.StartInfo = startInfo;
            process.Start();
        }

        public event EventHandler LocationChanged;
        public event EventHandler StateChanged;
        //public event ImageReadyEventHandler ImageReadyEvent;

        public bool IsOpen
        {
            get
            {
                lock (obj)
                    return obj.IsOpen;
            }
        }

        public string PortName
        {
            get
            {
                lock (obj)
                    return obj.PortName;
            }
            set
            {
                lock (obj)
                    obj.PortName = value;
            }
        }

        public string SerialNumber
        {
            get
            {
                lock (obj)
                    return obj.PortName;
            }
            set
            {
                lock (obj)
                    obj.SerialNumber = value;
            }
        }

        public string LastErrorString
        {
            get
            {
                lock (obj)
                    return obj.LastErrorString;
            }
        }

        public void Close()
        {
            lock (obj)
                obj.Close();
        }

        public void Open()
        {
            lock (obj)
                obj.Open();
        }

        public bool UpdatePort()
        {
            lock (obj)
                return obj.UpdatePort();
        }

        public void UpdateErrorString(Exception ex)
        {
            lock (obj)
                obj.UpdateErrorString(ex);
        }

        public List<USBDeviceHelper.DeviceProperties> GetSerialNumbers()
        {
            lock (obj)
                return obj.GetSerialNumbers();
        }

        public void BatchActions(ActionBatch actions, int count = 0)
        {
            lock (obj)
                obj.BatchActions(actions, count);
        }

        public bool IsBright(Bitmap bitmap, double threshold, int fazhi)
        {
            RawImage temp = new RawImage(bitmap);
            lock (obj)
                return obj.IsBright(temp, threshold, fazhi);
        }
        public bool IsBright(RawImage bitmap, double threshold, int fazhi)
        {
            lock (obj)
                return obj.IsBright(bitmap, threshold, fazhi);
        }
    }
}
