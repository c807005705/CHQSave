// Copyright 2011 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobot.TestBox
{
    public class CameraSettings
    {
        public double ExposureTime { get; private set; }
        public CameraSettings(double exposureTime)
        {
            this.ExposureTime = exposureTime;
        }
    }
}
