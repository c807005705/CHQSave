// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;
using Mobot.TestBox.Protocols;
using Mobot.Utils.IO;

namespace Mobot.TestBox.Protocols.Mtc
{
    partial class MtcServer
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        protected virtual void OnMessageReceived(MessageBase message, int counter)
        {
            if (MessageReceived != null)
                MessageReceived(this, new MessageReceivedEventArgs(message, counter));
        }
        public event EventHandler<MessageReceivedEventArgs> MessageSent;
        protected virtual void OnMessageSent(MessageBase message, int counter)
        {
            if (MessageSent != null)
                MessageSent(this, new MessageReceivedEventArgs(message, counter));
        }
        public event EventHandler<SerialPortIOEventArgs> IO
        {
            add { device.IO += value; }
            remove { device.IO -= value; }
        }

        public bool IsOpen
        {
            get
            {
                bool isOpen;
                try
                {
                    isOpen = device.IsOpen;
                }
                catch (Exception)
                {
                    isOpen = false;
                }

                return isOpen;
            }
        }
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy == value)
                    return;
                isBusy = value;
            }
        }
        public Version Version
        {
            get
            {
                return System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            }
        }
    }
}
