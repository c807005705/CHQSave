using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Mobot;

namespace TestDevice
{
    /// <summary>
    /// 基准点信息。
    /// </summary>
    [Serializable]
    public class DatumMarks
    {
        /// <summary>
        /// 相机采样时的快门时间。(秒)
        /// </summary>
        public double ExposureTime { get; private set; }
        /// <summary>
        /// 四个基准点构成的区域（在屏幕上的像素值）
        /// </summary>
        public Rectangle PixelBounds { get; private set; }
        /// <summary>
        /// 图像斑点的大小范围。
        /// </summary>
        public Range BlobSizeRange { get; private set; }

        public DatumMarks(double exposureTime, Rectangle PixelBounds, Range blobSizeRange)
        {
            this.ExposureTime = exposureTime;
            this.PixelBounds = PixelBounds;
            this.BlobSizeRange = blobSizeRange;
        }
    }
}
