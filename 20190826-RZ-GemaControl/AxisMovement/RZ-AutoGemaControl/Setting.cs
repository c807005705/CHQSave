using ControlDec;
using Device_Link_LTSMC;
using Interface.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace RZ_AutoGemaControl
{
    public partial class Setting : Form

    {
        UserControl UserControl = new UserControl();
         //private static readonly string ConfigPath = "DeviceConfig.config";

        ConDevice conDevice = new ConDevice();
        public Config config { get; set; } =new Config();

        public Setting()
        {
            InitializeComponent();
            this.FormClosing += Setting_FormClosing;
            
        }

        private void Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            try
            {
                config.UserConfig.DiscCenter =new System.Windows.Point(Convert.ToInt32(DiscCenterX.Text), Convert.ToInt32(DiscCenterY.Text));
                if (!Directory.Exists(ImageRoute.Text))//判断目录是否存在
                {
                    Directory.CreateDirectory(ImageRoute.Text);
                }
               
                this.Close();
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.ToString());
            }



        }

        private void Setting_Load(object sender, EventArgs e)
        {
          this.AxisBox.SelectedItem = "X轴";
           this.DirBox.SelectedItem = "Back";
           this.zeroBox.SelectedItem = "OTTZ";
        }

        private void Setting_Load_1(object sender, EventArgs e)
        {

           // XmlDataDocument doc = new XmlDataDocument();
          
           // XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf_8", null);
           // doc.AppendChild(dec);
           //XmlElement sitting= doc.CreateElement("Sitting");//根节点
           // doc.AppendChild(sitting);
           // XmlElement sit = doc.CreateElement("Sit");//子节点
           // sitting.AppendChild(sit);
           // doc.Save("DateSit.xml");



            DiscCenterX.Text = config.UserConfig.DiscCenter.X.ToString();
            DiscCenterY.Text = config.UserConfig.DiscCenter.Y.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            textBox4.Text = config.UserConfig.DiscCenter.X.ToString();
            textBox6.Text = config.UserConfig.DiscCenter.Y.ToString();
        }
    }
}
