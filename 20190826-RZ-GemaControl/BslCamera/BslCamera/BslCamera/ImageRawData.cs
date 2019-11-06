using PylonC.NET;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BslCamera
{
    internal class ImageRawData
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <param name="newBuffer"></param>
        /// <param name="color"></param>
        public ImageRawData(int newWidth, int newHeight, Byte[] newBuffer, PixelFormat color)
        {
            Width = newWidth;
            Height = newHeight;
            Buffer = newBuffer;
            Color = color;
        }

        public readonly int Width; /* The width of the image. */
        public readonly int Height; /* The height of the image. */
        public readonly Byte[] Buffer; /* The raw image data. */
        public readonly PixelFormat Color; /* If false the buffer contains a Mono8 image. Otherwise, RGBA8packed is provided. */
    }

    /* The class GrabResult is used internally to queue grab results. */
    internal struct GrabResult
    {
        public ImageRawData ImageData; /* Holds the taken image. */
        public PYLON_STREAMBUFFER_HANDLE Handle; /* Holds the handle of the image registered at the stream grabber. It is used to queue the buffer associated with itself for the next grab. */
    }
}
