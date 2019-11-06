using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BslCamera
{
    internal static class BitmapFactoryExpends
    {
        /* Calculates the length of one line in byte. */
        private static int GetStride(int width, PixelFormat color)
        {
            return color == PixelFormat.Format32bppArgb ? width * 4 : width;
        }

        /// <summary>
        /// 转换图像原始数据到bitmap图像
        /// </summary>
        /// <param name="imageRawData"></param>
        /// <returns></returns>
        public static Bitmap ConvertImageRawDataToBitmap(this ImageRawData imageRawData)
        {
            int width = imageRawData.Width;
            int height = imageRawData.Height;

            Bitmap bitmap = new Bitmap(width, height, imageRawData.Color);

            if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette colorPalette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = colorPalette;
            }

            /* Lock the bitmap's bits. */
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            /* Get the pointer to the bitmap's buffer. */
            IntPtr ptrBmp = bmpData.Scan0;
            /* Compute the width of a line of the image data. */
            int imageStride = GetStride(width, imageRawData.Color);
            /* If the widths in bytes are equal, copy in one go. */
            if (imageStride == bmpData.Stride)
            {
                System.Runtime.InteropServices.Marshal.Copy(imageRawData.Buffer, 0, ptrBmp, bmpData.Stride * bitmap.Height);
            }
            else /* The widths in bytes are not equal, copy line by line. This can happen if the image width is not divisible by four. */
            {
                for (int i = 0; i < bitmap.Height; ++i)
                {
                    Marshal.Copy(imageRawData.Buffer, i * imageStride, new IntPtr(ptrBmp.ToInt64() + i * bmpData.Stride), width);
                }
            }
            /* Unlock the bits. */
            bitmap.UnlockBits(bmpData);

            return bitmap;
        }
    }
}
