// Copyright 2011 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mobot.TestBox
{
    public class DisplayInfo
    {
        Size resolution = Size.Empty;
        /// <summary>
        /// 屏幕分辨率。
        /// </summary>
        public Size Resolution
        {
            get
            {
                if (this.resolution == Size.Empty)
                    return new Size(128, 160);
                return this.resolution;
            }
            private set
            {
                this.resolution = value;
            }
        }
        public UInt32 ScreenColors
        {
            get { return (UInt32)Math.Pow(2, 24); }
        }
        public DisplayInfo(Size resolution)
        {
            this.resolution = resolution;
        }
    }
}
