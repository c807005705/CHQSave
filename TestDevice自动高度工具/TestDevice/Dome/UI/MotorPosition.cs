using Mobot;
using System;
using System.Windows.Forms;
using TestDevice;

namespace DeviceServer
{
    public partial class MotorPosition : Form
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        private int coefficient = 1;
        private ITestBox testDevice = null;
        public MotorPosition(ITestBox testDevice)
        {
            InitializeComponent();
            this.testDevice = testDevice;
            bnt_C.Click += Bnt_C_Click;
            bnt_ok.Click += Bnt_ok_Click;

            but_XPlus.Click += But_XPlus_Click;
            but_XReduce.Click += But_XReduce_Click;

            but_YPlus.Click += But_YPlus_Click;
            but_YReduce.Click += But_YReduce_Click;

            but_ZPlus.Click += But_ZPlus_Click;
            but_ZReduce.Click += But_ZReduce_Click;

            but_Reset.Click += But_Reset_Click;

            rad_1.CheckedChanged += Rad_1_CheckedChanged;
            rad_10.CheckedChanged += Rad_10_CheckedChanged;
            rad_100.CheckedChanged += Rad_100_CheckedChanged;
        }

        private void Rad_100_CheckedChanged(object sender, EventArgs e)
        {
            if (rad_100.Checked)
                coefficient = 100;
        }

        private void Rad_10_CheckedChanged(object sender, EventArgs e)
        {
            if (rad_10.Checked)
                coefficient = 10;
        }

        private void Rad_1_CheckedChanged(object sender, EventArgs e)
        {
            if (rad_1.Checked)
                coefficient = 1;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys keyCode = keyData & Keys.KeyCode;
            if (keyCode == Keys.Up || keyCode == Keys.Down || keyCode == Keys.Left || keyCode == Keys.Right
                || keyCode == Keys.Oemplus || keyCode == Keys.OemMinus)
            {
                switch (keyCode)
                {
                    case Keys.Left: but_XReduce.PerformClick(); break;
                    case Keys.Right: but_XPlus.PerformClick(); break;
                    case Keys.Down: but_YPlus.PerformClick(); break;
                    case Keys.Up: but_YReduce.PerformClick(); break;
                    case Keys.OemMinus: but_ZReduce.PerformClick(); break;//减号
                    case Keys.Oemplus: but_ZPlus.PerformClick(); break;//加号
                    default:
                        break;
                }
                return true;

            }

            return base.ProcessCmdKey(ref msg, keyData); ;
        }

        private void But_Reset_Click(object sender, EventArgs e)
        {
            this.testDevice.StylusReset();
            X = 0;
            Y = 0;
            Z = 0;
        }

        private void But_ZReduce_Click(object sender, EventArgs e)
        {
            Z -= coefficient;
            Z = GetZ(Z);
            this.testDevice.StylusMoveZ(Z);
            RefreshXYZ();
        }

        private void But_ZPlus_Click(object sender, EventArgs e)
        {
            Z += coefficient;
            Z = GetZ(Z);
            this.testDevice.StylusMoveZ(Z);
            RefreshXYZ();
        }

        private void But_YReduce_Click(object sender, EventArgs e)
        {
            Y -= coefficient;
            Y = GetY(Y);
            this.testDevice.StylusMoveXY(X, Y);
            RefreshXYZ();
        }

        private void But_YPlus_Click(object sender, EventArgs e)
        {
            Y += coefficient;
            Y = GetY(Y);
            this.testDevice.StylusMoveXY(X, Y);
            RefreshXYZ();
        }

        private void But_XReduce_Click(object sender, EventArgs e)
        {
            X -= coefficient;
            X = GetX(X);
            this.testDevice.StylusMoveXY(X, Y);
            RefreshXYZ();
        }

        private void But_XPlus_Click(object sender, EventArgs e)
        {
            X += coefficient;
            X = GetX(X);
            this.testDevice.StylusMoveXY(X, Y);
            RefreshXYZ();
        }

        private void RefreshXYZ()
        {
            lab_X.Text = "X=" + X.ToString();
            lab_Y.Text = "Y=" + Y.ToString();
            lab_Z.Text = "Z=" + Z.ToString();
        }

        private void Bnt_ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Bnt_C_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private int GetX(int value)
        {
            return GetValue(value, testDevice.ControllerSettings.RangeX);
        }
        private int GetY(int value)
        {
            return GetValue(value, testDevice.ControllerSettings.RangeY);
        }
        private int GetZ(int value)
        {
            return GetValue(value, testDevice.ControllerSettings.RangeZ);
        }
        private int GetValue(int value, RangeD range)
        {
            double min = range.Minimum;
            double max = range.Maximum;
            if (value > max)
                value = (int)max;
            if (value < min)
                value = (int)min;
            return value;
        }

    }
}
