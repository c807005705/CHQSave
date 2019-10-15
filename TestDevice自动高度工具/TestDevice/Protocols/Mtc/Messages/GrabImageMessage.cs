using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Mobot.TestBox.Protocols.Mtc.Messages
{
    [MessageDesc(MessageID.GRAB_IMAGE_COM_ID, "GrabImage", null, MessageID.GRAB_IMAGE_RESP_ID)]
    public class GrabImageMessage : SimpleMessage
    {
        public override string ToString()
        {
            return "GrabImage()";
        }
    }

    [MessageDesc(MessageID.GRAB_IMAGE_RESP_ID, "GrabImageResponse",
        new TYPE_DESC[] {
            TYPE_DESC.UInt16,		// The width of the returned image in pixels
            TYPE_DESC.UInt16,		// The height of the returned image in pixels
            TYPE_DESC.UInt32,		// A value that indicates the format of the Blob
            TYPE_DESC.Blob		// The image
        }, MessageID.GRAB_IMAGE_COM_ID)]
    public class GrabImageResponseMessage : MessageBase
    {
        /// <summary>
        /// 图片宽度x长度（单位像素）
        /// </summary>
        public Size ImageSize { get; private set; }
        /// <summary>
        /// 图片格式（待定）
        /// </summary>
        public UInt32 Format { get; private set; }
        /// <summary>
        /// 图片的二进制数据
        /// </summary>
        public byte[] Image { get; private set; }

        public GrabImageResponseMessage()
        {
            this.ImageSize = Size.Empty;
            this.Format = 0;
            this.Image = null;
        }
        public GrabImageResponseMessage(Size ImageSize, UInt32 Format, byte[] Image)
        {
            this.ImageSize = ImageSize;
            this.Format = Format;
            this.Image = Image;
        }
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            UInt16 WIDTH = (UInt16)RawData[offset++];
            UInt16 HEIGHT = (UInt16)RawData[offset++];
            UInt32 FORMAT = (UInt32)RawData[offset++];
            byte[] IMAGE = (byte[])RawData[offset++];

            this.ImageSize = new Size(WIDTH, HEIGHT);
            this.Format = FORMAT;
            this.Image = IMAGE;
        }
        public override object[] ToRawData()
        {
            return new object[] {
                (UInt16)ImageSize.Width,
                (UInt16)ImageSize.Height,
                (UInt32)Format,
                (byte[])Image
            };
        }
    }
}
