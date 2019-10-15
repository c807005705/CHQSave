using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// Flash芯片读写流。
    /// </summary>
    public partial class FlashStream : Stream
    {
        /// <summary>
        /// Flash扇区大小。
        /// </summary>
        public readonly int PageSize = 2048;
        /// <summary>
        /// 扇区总数。
        /// </summary>
        public readonly int MaxPageCount = 5;
        readonly Stopwatch stopwatch = null;
        readonly ITestBox driver = null;
        readonly MemoryStream stream = null;

        public FlashStream(ITestBox driver)
            : this(driver, -1)
        { }
        public FlashStream(ITestBox driver, int cachePageCount)
        {
            if (driver == null)
                throw new ArgumentNullException("driver", "driver不能为空。");
            //if (!driver.IsOpen)
            //    throw new IOException("测试盒设备尚未连接。");

            this.stream = new MemoryStream();
            this.driver = driver;
            this.PageSize = driver.FlashPageSize;
            this.MaxPageCount = driver.FlashPageCount;
            cachePageCount = (cachePageCount < 1) ? driver.FlashPageCount : cachePageCount;
            this.pageCaches = new FlashPage[cachePageCount];
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                stopwatch.Stop();
            }
            base.Dispose(disposing);
        }

        public override bool CanRead
        {
            get { return driver.IsOpen; }
        }

        public override bool CanSeek
        {
            get { return driver.IsOpen; }
        }

        public override bool CanWrite
        {
            get { return driver.IsOpen; }
        }

        public override void Flush()
        {
            foreach (FlashPage page in pageCaches)
            {
                if (page == null)
                    continue;

                page.Save();
            }
        }

        long length = -1;
        public override long Length
        {
            get { return length; }
        }
        public override void SetLength(long value)
        {
            this.length = value;
        }

        long position = 0;
        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "尝试将位置设置为负值。");
                if (value > PageSize * MaxPageCount - 1)
                    throw new IOException("Flash存储空间不足。");

                position = value;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.position = offset;
                    break;
                case SeekOrigin.Current:
                    this.position += offset;
                    break;
                case SeekOrigin.End:
                    this.position = this.Length - offset;
                    break;
            }

            return this.position;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            long start = position;
            long end = start + count;
            while (position < end)
            {
                int pageIndex = (int)Math.Floor((double)position / PageSize);
                //该页读取位置
                long pagePosition = position - pageIndex * PageSize;
                //该页要读取的长度
                int len = (int)Math.Min(PageSize - pagePosition, end - position);
                FlashPage page = GetPage(pageIndex);
                page.Read((int)pagePosition, buffer, offset, len);

                position += len;
                offset += len;
            }

            return (int)(position - start);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            long start = position;
            long end = start + count;
            while (position < end)
            {
                int pageIndex = (int)Math.Floor((double)position / PageSize);
                //该页读取位置
                long pagePosition = position - pageIndex * PageSize;
                //该页要读取的长度
                int len = (int)Math.Min(PageSize - pagePosition, end - position);
                FlashPage page = GetPage(pageIndex);
                page.Write((int)pagePosition, buffer, offset, len);

                position += len;
                offset += len;
            }
        }
    }
}
