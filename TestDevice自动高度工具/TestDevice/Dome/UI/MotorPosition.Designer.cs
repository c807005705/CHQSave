namespace DeviceServer
{
    partial class MotorPosition
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lab_X = new System.Windows.Forms.Label();
            this.lab_Y = new System.Windows.Forms.Label();
            this.lab_Z = new System.Windows.Forms.Label();
            this.but_Reset = new System.Windows.Forms.Button();
            this.but_YReduce = new System.Windows.Forms.Button();
            this.but_YPlus = new System.Windows.Forms.Button();
            this.but_XPlus = new System.Windows.Forms.Button();
            this.but_XReduce = new System.Windows.Forms.Button();
            this.bnt_ok = new System.Windows.Forms.Button();
            this.bnt_C = new System.Windows.Forms.Button();
            this.but_ZPlus = new System.Windows.Forms.Button();
            this.but_ZReduce = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rad_1 = new System.Windows.Forms.RadioButton();
            this.rad_10 = new System.Windows.Forms.RadioButton();
            this.rad_100 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lab_X
            // 
            this.lab_X.AutoSize = true;
            this.lab_X.Location = new System.Drawing.Point(13, 13);
            this.lab_X.Name = "lab_X";
            this.lab_X.Size = new System.Drawing.Size(35, 12);
            this.lab_X.TabIndex = 0;
            this.lab_X.Text = "X=000";
            // 
            // lab_Y
            // 
            this.lab_Y.AutoSize = true;
            this.lab_Y.Location = new System.Drawing.Point(95, 13);
            this.lab_Y.Name = "lab_Y";
            this.lab_Y.Size = new System.Drawing.Size(35, 12);
            this.lab_Y.TabIndex = 1;
            this.lab_Y.Text = "Y=000";
            // 
            // lab_Z
            // 
            this.lab_Z.AutoSize = true;
            this.lab_Z.Location = new System.Drawing.Point(177, 13);
            this.lab_Z.Name = "lab_Z";
            this.lab_Z.Size = new System.Drawing.Size(35, 12);
            this.lab_Z.TabIndex = 2;
            this.lab_Z.Text = "Z=000";
            // 
            // but_Reset
            // 
            this.but_Reset.Location = new System.Drawing.Point(77, 152);
            this.but_Reset.Name = "but_Reset";
            this.but_Reset.Size = new System.Drawing.Size(41, 37);
            this.but_Reset.TabIndex = 3;
            this.but_Reset.Text = "归位";
            this.but_Reset.UseVisualStyleBackColor = true;
            // 
            // but_YReduce
            // 
            this.but_YReduce.Location = new System.Drawing.Point(77, 109);
            this.but_YReduce.Name = "but_YReduce";
            this.but_YReduce.Size = new System.Drawing.Size(41, 37);
            this.but_YReduce.TabIndex = 4;
            this.but_YReduce.Text = "Y-";
            this.but_YReduce.UseVisualStyleBackColor = true;
            // 
            // but_YPlus
            // 
            this.but_YPlus.Location = new System.Drawing.Point(77, 195);
            this.but_YPlus.Name = "but_YPlus";
            this.but_YPlus.Size = new System.Drawing.Size(41, 37);
            this.but_YPlus.TabIndex = 5;
            this.but_YPlus.Text = "Y+";
            this.but_YPlus.UseVisualStyleBackColor = true;
            // 
            // but_XPlus
            // 
            this.but_XPlus.Location = new System.Drawing.Point(124, 152);
            this.but_XPlus.Name = "but_XPlus";
            this.but_XPlus.Size = new System.Drawing.Size(41, 37);
            this.but_XPlus.TabIndex = 6;
            this.but_XPlus.Text = "X+";
            this.but_XPlus.UseVisualStyleBackColor = true;
            // 
            // but_XReduce
            // 
            this.but_XReduce.Location = new System.Drawing.Point(30, 152);
            this.but_XReduce.Name = "but_XReduce";
            this.but_XReduce.Size = new System.Drawing.Size(41, 37);
            this.but_XReduce.TabIndex = 7;
            this.but_XReduce.Text = "X-";
            this.but_XReduce.UseVisualStyleBackColor = true;
            // 
            // bnt_ok
            // 
            this.bnt_ok.Location = new System.Drawing.Point(105, 254);
            this.bnt_ok.Name = "bnt_ok";
            this.bnt_ok.Size = new System.Drawing.Size(75, 31);
            this.bnt_ok.TabIndex = 8;
            this.bnt_ok.Text = "确定";
            this.bnt_ok.UseVisualStyleBackColor = true;
            // 
            // bnt_C
            // 
            this.bnt_C.Location = new System.Drawing.Point(187, 254);
            this.bnt_C.Name = "bnt_C";
            this.bnt_C.Size = new System.Drawing.Size(75, 31);
            this.bnt_C.TabIndex = 9;
            this.bnt_C.Text = "取消";
            this.bnt_C.UseVisualStyleBackColor = true;
            // 
            // but_ZPlus
            // 
            this.but_ZPlus.Location = new System.Drawing.Point(201, 177);
            this.but_ZPlus.Name = "but_ZPlus";
            this.but_ZPlus.Size = new System.Drawing.Size(41, 37);
            this.but_ZPlus.TabIndex = 12;
            this.but_ZPlus.Text = "Z+";
            this.but_ZPlus.UseVisualStyleBackColor = true;
            // 
            // but_ZReduce
            // 
            this.but_ZReduce.Location = new System.Drawing.Point(201, 122);
            this.but_ZReduce.Name = "but_ZReduce";
            this.but_ZReduce.Size = new System.Drawing.Size(41, 37);
            this.but_ZReduce.TabIndex = 11;
            this.but_ZReduce.Text = "Z-";
            this.but_ZReduce.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(82, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 19);
            this.label4.TabIndex = 13;
            this.label4.Text = "XY";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(209, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 19);
            this.label5.TabIndex = 14;
            this.label5.Text = "Z";
            // 
            // rad_1
            // 
            this.rad_1.AutoSize = true;
            this.rad_1.Location = new System.Drawing.Point(18, 40);
            this.rad_1.Name = "rad_1";
            this.rad_1.Size = new System.Drawing.Size(29, 16);
            this.rad_1.TabIndex = 15;
            this.rad_1.Text = "1";
            this.rad_1.UseVisualStyleBackColor = true;
            // 
            // rad_10
            // 
            this.rad_10.AutoSize = true;
            this.rad_10.Checked = true;
            this.rad_10.Location = new System.Drawing.Point(84, 40);
            this.rad_10.Name = "rad_10";
            this.rad_10.Size = new System.Drawing.Size(35, 16);
            this.rad_10.TabIndex = 16;
            this.rad_10.TabStop = true;
            this.rad_10.Text = "10";
            this.rad_10.UseVisualStyleBackColor = true;
            // 
            // rad_100
            // 
            this.rad_100.AutoSize = true;
            this.rad_100.Location = new System.Drawing.Point(154, 40);
            this.rad_100.Name = "rad_100";
            this.rad_100.Size = new System.Drawing.Size(41, 16);
            this.rad_100.TabIndex = 17;
            this.rad_100.Text = "100";
            this.rad_100.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 237);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "XYZ分别可以通过<左、右、上、下、+、->键控制";
            // 
            // MotorPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 296);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rad_100);
            this.Controls.Add(this.rad_10);
            this.Controls.Add(this.rad_1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.but_ZPlus);
            this.Controls.Add(this.but_ZReduce);
            this.Controls.Add(this.bnt_C);
            this.Controls.Add(this.bnt_ok);
            this.Controls.Add(this.but_XReduce);
            this.Controls.Add(this.but_XPlus);
            this.Controls.Add(this.but_YPlus);
            this.Controls.Add(this.but_YReduce);
            this.Controls.Add(this.but_Reset);
            this.Controls.Add(this.lab_Z);
            this.Controls.Add(this.lab_Y);
            this.Controls.Add(this.lab_X);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MotorPosition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "电机坐标获取";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lab_X;
        private System.Windows.Forms.Label lab_Y;
        private System.Windows.Forms.Label lab_Z;
        private System.Windows.Forms.Button but_Reset;
        private System.Windows.Forms.Button but_YReduce;
        private System.Windows.Forms.Button but_YPlus;
        private System.Windows.Forms.Button but_XPlus;
        private System.Windows.Forms.Button but_XReduce;
        private System.Windows.Forms.Button bnt_ok;
        private System.Windows.Forms.Button bnt_C;
        private System.Windows.Forms.Button but_ZPlus;
        private System.Windows.Forms.Button but_ZReduce;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rad_1;
        private System.Windows.Forms.RadioButton rad_10;
        private System.Windows.Forms.RadioButton rad_100;
        private System.Windows.Forms.Label label1;
    }
}