using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Device_Link_LTSMC;

namespace Interface.Items
{
    public class AxisInfo:BaseNotify
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 比例
        /// </summary>
        private double scale = 1;
        /// <summary>
        /// 名称
        /// </summary>
        private string name = "";
        /// <summary>
        /// 方向
        /// </summary>
        private Direction direction;
        /// <summary>
        /// 回零速度
        /// </summary>       
        private double zeroSpeed;
        /// <summary>
        /// 低速回零
        /// </summary>
        private double lowZeroSpeed = 1000;
        /// <summary>
        /// 加速时间
        /// </summary>
        private double accTime;
        /// <summary>
        /// 减速时间
        /// </summary>
        private double dccTime;
        /// <summary>
        /// 运行速度
        /// </summary>
        private double runSpeed;
        /// <summary>
        /// 回零模式
        /// </summary>
        private ZeroMode zeroMode;
        public ZeroMode ZeroMode
        {
            get
            {
                return zeroMode;
            }

            set
            {
                zeroMode = value;
               // Changed("ZeroMode");
            }
        }
        /// <summary>
        /// 改变
        /// </summary>
        /// <param name="v"></param>
        //private void Changed(string v)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //}

        public double RunSpeed
        {
            get
            {
                return runSpeed;
            }

            set
            {
                runSpeed = value;
                //Changed("RunSpeed");
            }
        }
        /// <summary>
        /// 加速时间
        /// </summary>
        public double AccTime
        {
            get
            {
                return accTime;
            }

            set
            {
                accTime = value;
               // Changed("AccTime");
            }
        }
        /// <summary>
        /// 减速时间
        /// </summary>
        public double DecTime
        {
            get
            {
                return dccTime;
            }

            set
            {
                dccTime = value;
               // Changed("DccTime");
            }
        }
        /// <summary>
        /// 低速回零
        /// </summary>
        public double LowZeroSpeed
        {
            get
            {
                return lowZeroSpeed;
            }
            set
            {
                lowZeroSpeed = value;
                //Changed("LowZeroSpeed");
            }
        }
        //public Direction Direction { get; set; }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                //Changed("Name");
            }
        }
        /// <summary>
        /// 比例
        /// </summary>
        public double Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                //Changed("Scale");
            }
        }
        public double ZeroSpeed
        {
            get
            {
                return zeroSpeed;
            }

            set
            {
                zeroSpeed = value;
               // Changed("ZeroSpeed");
            }
        }
             public Direction Direction
        {
            get
            {
                return direction;
            }

            set
            {
                direction = value;
               // Changed("Direction");
            }
        }
    }
    
}
