
using System;
using System.Drawing;
using Motor;
using TestDevice;
using Mobot;

namespace Driver
{
    partial class TestBoxDriver
    {
        public DisplayInfo DisplayInfo { get; set; }
        public WorkspaceInfo WorkspaceInfo { get; set; }
        public DatumMarks DatumMarks { get; set; }
        public CalculateCoefficient CalculateCoefficient { get; set; }

        /// <summary>
        /// 将触屏的坐标转换为电机坐标。
        /// </summary>
        /// <returns></returns>
        public void PointToMotor(int pixelX, int pixelY, out double x, out double y)
        {
            if (this.LeftUpPoint == null || this.RigntDownPoint == null || this.ImageSize == null)
                throw new Exception("触屏参数尚未配置。");

            double rateWidth, rateHeight;
            TestBoxPosition(out rateWidth, out rateHeight);

            x = (int)((double)this.LeftUpPoint.X + rateWidth * (double)pixelX);
            y = (int)((double)this.LeftUpPoint.Y + rateHeight * (double)pixelY);
        }

        /// <summary>
        /// 获取转换系数
        /// </summary>
        /// <param name="rateWidth"></param>
        /// <param name="rateHeight"></param>
        public void TestBoxPosition(out double rateWidth, out double rateHeight)
        {
            try
            {
                rateWidth = ((double)this.RigntDownPoint.X - (double)this.LeftUpPoint.X) / (double)this.ImageSize.Width;
                rateHeight = ((double)this.RigntDownPoint.Y - (double)this.LeftUpPoint.Y) / (double)this.ImageSize.Height;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
