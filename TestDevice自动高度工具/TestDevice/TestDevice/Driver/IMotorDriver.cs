// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using Mobot;

namespace TestDevice
{
    public interface IMotorDriver
    {
        Point3 Speed { get; set; }
        Point3 Ticks { get; set; }
        void SetBatchMode(BatchMode mode, int addTimeOut = 0);
        void SetBatchTimes(int times);
        MessageBase Request(MessageBase message, double addTimeOut = 0, bool iReset = true);
    }
}
