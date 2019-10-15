namespace DeviceServer
{
    partial class MianForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.cbxUSB = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.con_message = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button5 = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Button();
            this.txt_x = new System.Windows.Forms.TextBox();
            this.txt_y = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.but_X = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_leftUp = new System.Windows.Forms.TextBox();
            this.txt_rightDown = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Z = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.but_y = new System.Windows.Forms.Button();
            this.but_z = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_height = new System.Windows.Forms.TextBox();
            this.txt_widht = new System.Windows.Forms.TextBox();
            this.but_Setting = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lab_message = new System.Windows.Forms.ToolStripStatusLabel();
            this.bnt_clear = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(214, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Open";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbxUSB
            // 
            this.cbxUSB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUSB.FormattingEnabled = true;
            this.cbxUSB.Location = new System.Drawing.Point(90, 12);
            this.cbxUSB.Name = "cbxUSB";
            this.cbxUSB.Size = new System.Drawing.Size(108, 20);
            this.cbxUSB.TabIndex = 10;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(20, 15);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 12);
            this.label17.TabIndex = 9;
            this.label17.Text = "连接端口:";
            // 
            // listView1
            // 
            this.listView1.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.con_message});
            this.listView1.Location = new System.Drawing.Point(12, 192);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(756, 219);
            this.listView1.TabIndex = 31;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // con_message
            // 
            this.con_message.Text = "日志";
            this.con_message.Width = 754;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(311, 9);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 32;
            this.button5.Text = "Reset";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // test
            // 
            this.test.Location = new System.Drawing.Point(84, 82);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(75, 23);
            this.test.TabIndex = 39;
            this.test.Text = "test";
            this.test.UseVisualStyleBackColor = true;
            this.test.Click += new System.EventHandler(this.test_Click);
            // 
            // txt_x
            // 
            this.txt_x.Location = new System.Drawing.Point(59, 20);
            this.txt_x.Name = "txt_x";
            this.txt_x.Size = new System.Drawing.Size(100, 21);
            this.txt_x.TabIndex = 40;
            // 
            // txt_y
            // 
            this.txt_y.Location = new System.Drawing.Point(59, 51);
            this.txt_y.Name = "txt_y";
            this.txt_y.Size = new System.Drawing.Size(100, 21);
            this.txt_y.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 42;
            this.label1.Text = "X:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 43;
            this.label2.Text = "Y:";
            // 
            // but_X
            // 
            this.but_X.Location = new System.Drawing.Point(236, 29);
            this.but_X.Name = "but_X";
            this.but_X.Size = new System.Drawing.Size(75, 23);
            this.but_X.TabIndex = 44;
            this.but_X.Text = "获取坐标";
            this.but_X.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 45;
            this.label3.Text = "左上：";
            // 
            // txt_leftUp
            // 
            this.txt_leftUp.Location = new System.Drawing.Point(62, 31);
            this.txt_leftUp.Name = "txt_leftUp";
            this.txt_leftUp.Size = new System.Drawing.Size(152, 21);
            this.txt_leftUp.TabIndex = 46;
            // 
            // txt_rightDown
            // 
            this.txt_rightDown.Location = new System.Drawing.Point(62, 70);
            this.txt_rightDown.Name = "txt_rightDown";
            this.txt_rightDown.Size = new System.Drawing.Size(152, 21);
            this.txt_rightDown.TabIndex = 48;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 47;
            this.label4.Text = "右下：";
            // 
            // txt_Z
            // 
            this.txt_Z.Location = new System.Drawing.Point(62, 106);
            this.txt_Z.Name = "txt_Z";
            this.txt_Z.Size = new System.Drawing.Size(152, 21);
            this.txt_Z.TabIndex = 50;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 49;
            this.label5.Text = "Z轴：";
            // 
            // but_y
            // 
            this.but_y.Location = new System.Drawing.Point(236, 68);
            this.but_y.Name = "but_y";
            this.but_y.Size = new System.Drawing.Size(75, 23);
            this.but_y.TabIndex = 51;
            this.but_y.Text = "获取坐标";
            this.but_y.UseVisualStyleBackColor = true;
            // 
            // but_z
            // 
            this.but_z.Location = new System.Drawing.Point(236, 104);
            this.but_z.Name = "but_z";
            this.but_z.Size = new System.Drawing.Size(75, 23);
            this.but_z.TabIndex = 52;
            this.but_z.Text = "获取坐标";
            this.but_z.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(29, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 57;
            this.label6.Text = "Height:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 56;
            this.label7.Text = "Width:";
            // 
            // txt_height
            // 
            this.txt_height.Location = new System.Drawing.Point(77, 64);
            this.txt_height.Name = "txt_height";
            this.txt_height.Size = new System.Drawing.Size(100, 21);
            this.txt_height.TabIndex = 55;
            // 
            // txt_widht
            // 
            this.txt_widht.Location = new System.Drawing.Point(78, 20);
            this.txt_widht.Name = "txt_widht";
            this.txt_widht.Size = new System.Drawing.Size(100, 21);
            this.txt_widht.TabIndex = 54;
            // 
            // but_Setting
            // 
            this.but_Setting.Location = new System.Drawing.Point(70, 107);
            this.but_Setting.Name = "but_Setting";
            this.but_Setting.Size = new System.Drawing.Size(75, 23);
            this.but_Setting.TabIndex = 53;
            this.but_Setting.Text = "设置";
            this.but_Setting.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_leftUp);
            this.groupBox1.Controls.Add(this.but_X);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txt_rightDown);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.but_z);
            this.groupBox1.Controls.Add(this.txt_Z);
            this.groupBox1.Controls.Add(this.but_y);
            this.groupBox1.Location = new System.Drawing.Point(22, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(326, 144);
            this.groupBox1.TabIndex = 58;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "坐标参数设置";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_widht);
            this.groupBox2.Controls.Add(this.but_Setting);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txt_height);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(358, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(194, 144);
            this.groupBox2.TabIndex = 59;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "屏幕参数设置";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txt_x);
            this.groupBox3.Controls.Add(this.test);
            this.groupBox3.Controls.Add(this.txt_y);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(562, 42);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 110);
            this.groupBox3.TabIndex = 60;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "触屏测试";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lab_message});
            this.statusStrip1.Location = new System.Drawing.Point(0, 414);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(780, 22);
            this.statusStrip1.TabIndex = 61;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lab_message
            // 
            this.lab_message.Name = "lab_message";
            this.lab_message.Size = new System.Drawing.Size(83, 17);
            this.lab_message.Text = "lab_message";
            // 
            // bnt_clear
            // 
            this.bnt_clear.Location = new System.Drawing.Point(646, 163);
            this.bnt_clear.Name = "bnt_clear";
            this.bnt_clear.Size = new System.Drawing.Size(75, 23);
            this.bnt_clear.TabIndex = 62;
            this.bnt_clear.Text = "清除日志";
            this.bnt_clear.UseVisualStyleBackColor = true;
            // 
            // MianForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 436);
            this.Controls.Add(this.bnt_clear);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.cbxUSB);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.Name = "MianForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备服务端";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbxUSB;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader con_message;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button test;
        private System.Windows.Forms.TextBox txt_x;
        private System.Windows.Forms.TextBox txt_y;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button but_X;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_leftUp;
        private System.Windows.Forms.TextBox txt_rightDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_Z;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button but_y;
        private System.Windows.Forms.Button but_z;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_height;
        private System.Windows.Forms.TextBox txt_widht;
        private System.Windows.Forms.Button but_Setting;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lab_message;
        private System.Windows.Forms.Button bnt_clear;
    }
}

