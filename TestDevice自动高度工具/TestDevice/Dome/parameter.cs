using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestDevice;

namespace DeviceServer
{
    /// <summary>
    /// 参数类
    /// </summary>
    public interface IParamter
    {
        /// <summary>
        /// 类型
        /// </summary>
        string Type { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        string ID { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        void Doing(ITestBox testBox, DeviceConfig deviceConfig);
        /// <summary>
        /// 获取显示用的参数
        /// </summary>
        /// <returns></returns>
        string ToDisplayString();
    }

    /// <summary>
    /// 点击
    /// </summary>
    public class ClickParamter : IParamter
    {

        public Point ClickPosition { get; set; }

        public string Type { get; set; }
        public string ID { get; set; }

        public ClickParamter()
        {
            this.Type = "点击";
            this.ID = Guid.NewGuid().ToString();
        }

        public string ToDisplayString()
        {
            return string.Format("点击坐标 : ({0}, {1})", this.ClickPosition.X, this.ClickPosition.Y);
        }

        void IParamter.Doing(ITestBox testBox, DeviceConfig deviceConfig)
        {
            //testBox.StylusMoveZ(0);
            testBox.StylusRestZ();
            testBox.StylusMoveXY(ClickPosition.X, ClickPosition.Y);
            testBox.StylusMoveZ(deviceConfig.AutoHeight);
            Thread.Sleep(100);
            deviceConfig.ClickTimes++;
        }
    }
    /// <summary>
    /// 移动
    /// </summary>

    public class DragParamter : IParamter
    {
        public Point StartPosition { get; set; }
        public Point EndPosition { get; set; }
        public string Type { get; set; }
        public string ID { get; set; }

        public DragParamter()
        {
            this.Type = "滑动";
            this.ID = Guid.NewGuid().ToString();
        }
        public void Doing(ITestBox testBox, DeviceConfig deviceConfig)
        {
            //testBox.StylusMoveZ(0);
            testBox.StylusRestZ();
            testBox.StylusMoveXY(StartPosition.X, StartPosition.Y);
            testBox.StylusMoveZ(deviceConfig.AutoHeight);
            Thread.Sleep(100);
            testBox.StylusMoveXY(EndPosition.X, EndPosition.Y);
            deviceConfig.DragTimes++;
        }

        public string ToDisplayString()
        {
            return string.Format("起点 : ({0}, {1}), 终点 : ({2}, {3}) ", this.StartPosition.X, this.StartPosition.Y, 
                this.EndPosition.X, this.EndPosition.Y);
        }
    }
    /// <summary>
    /// 等待
    /// </summary>
    public class WaitParamter : IParamter
    {
        public int WaitTime { get; set; }
        public string Type { get; set; }
        public string ID { get; set; }

        public WaitParamter()
        {
            this.Type = "等待";
            this.ID = Guid.NewGuid().ToString();
        }
        public void Doing(ITestBox testBox, DeviceConfig deviceConfig)
        {
            Thread.Sleep(WaitTime);
        }

        public string ToDisplayString()
        {
            return string.Format("等待时间 : {0}", this.WaitTime);
        }
    }
}
