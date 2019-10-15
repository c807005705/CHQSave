// Copyright 2011 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobot.Utils;

namespace Mobot.TestBox
{
    /// <summary>
    /// 测试盒工作区域。
    /// </summary>
    public class WorkspaceInfo
    {
        public Size2D Size { get; private set; }
        public Point2D TopLeft { get; private set; }
        public Point2D BottomRight { get; private set; }
        /// <summary>
        /// 基准点的区域，实际尺寸，毫米。
        /// </summary>
        public RectangleD DatumMarks { get; private set; }
        public WorkspaceInfo(Size2D size, Point2D topLeft, Point2D bottomRight, RectangleD datumMarks)
        {
            this.Size = size;
            this.TopLeft = topLeft;
            this.BottomRight = bottomRight;
            this.DatumMarks = datumMarks;
        }
    }
}
