﻿using ControlDec;
using Device_Link_LTSMC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RZ_AutoGemaControl
{
    public partial class Setting : Form
    {
        ConDevice conDevice = new ConDevice();
        public Setting()
        {
            InitializeComponent();
            
        }
        
       
       
       
        private void Confirm_Click(object sender, EventArgs e)
        {
            conDevice.SaveCongigToDevice("");

        }

        private void Setting_Load(object sender, EventArgs e)
        {
          this.AxisBox.SelectedItem = "X轴";
           this.DirBox.SelectedItem = "Back";
           this.zeroBox.SelectedItem = "OTTZ";
        }
    }
}