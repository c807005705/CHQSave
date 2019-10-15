using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Motor
{
    partial class FlashStream
    {
        readonly FlashPage[] pageCaches = null;
        /// <summary>
        /// 查找指定的页。
        /// </summary>
        /// <returns></returns>
        FlashPage GetPage(int pageIndex)
        {
            for (int i = 0; i < pageCaches.Length; i++)
            {
                if(pageCaches[i] != null && pageCaches[i].PageIndex == pageIndex)
                    return pageCaches[i];
            }

            //寻找一个空的缓存位置
            int index = 0;
            for (int i = 0; i < pageCaches.Length; i++)
            {
                if (pageCaches[i] == null)
                {//找到一个空的
                    index = i;
                    break;
                }
                if (pageCaches[index] == null || pageCaches[index].DateAccess > pageCaches[i].DateAccess)
                {//如果cache[i]最后访问的时间更早，则
                    index = i;
                }
            }
            if (pageCaches[index] == null)
                pageCaches[index] = new FlashPage(this);

            FlashPage result = pageCaches[index];
            if(result.PageIndex != pageIndex)
                result.Load(pageIndex);
            
            return result;
        }

        class FlashPage
        {
            readonly byte[] buffer = null;

            /// <summary>
            /// 数据是否加载过。
            /// </summary>
            public bool Loaded { get; private set; }
            /// <summary>
            /// 是否被修改过。
            /// </summary>
            public bool Modified { get; private set; }
            /// <summary>
            /// 页索引号。
            /// </summary>
            public int PageIndex { get; private set; }
            /// <summary>
            /// 最后一次访问的时间。
            /// </summary>
            public long DateAccess { get; private set; }

            readonly FlashStream owner = null;
            internal FlashPage(FlashStream owner)
            {
                this.owner = owner;
                this.buffer = new byte[owner.PageSize];

                this.Clear();
            }

            /// <summary>
            /// 从Flash载入数据。切换页索引，如果已有数据更改，则立即写入到Flash。
            /// </summary>
            /// <param name="index"></param>
            public void Load(int index)
            {
                if(this.PageIndex == index)
                    return;
                
                //先保存原来页的数据
                this.Save();
                this.Clear();
                byte[] data = owner.driver.ReadFlash(index);
                Debug.Assert(data.Length == buffer.Length);
                Buffer.BlockCopy(data, 0, buffer, 0, Math.Min(data.Length, this.buffer.Length));
                
                this.PageIndex = index;
                this.Loaded = true;
                this.Modified = false;
            }

            /// <summary>
            /// 清理数据。
            /// </summary>
            public void Clear()
            {
                for(int i=0;i<this.buffer.Length;i++)
                    this.buffer[i] = 0;

                this.PageIndex = -1;
                this.Loaded = false;
                this.Modified = false;
            }


            /// <summary>
            /// 将更改写入到Flash。
            /// </summary>
            public void Save()
            {
                if (!this.Loaded)
                    return;
                if (!this.Modified)
                    return;

                owner.driver.WriteFlash(this.PageIndex, this.buffer);
                this.Modified = false;
            }

            public void Write(int position, byte[] data, int offset, int size)
            {
                if (!this.Loaded)
                    throw new InvalidOperationException("当前页尚未加载。");
                if (position + size > this.buffer.Length)
                    throw new IndexOutOfRangeException("超出页数据范围。");

                this.DateAccess = owner.stopwatch.ElapsedTicks;
                Buffer.BlockCopy(data, offset, buffer, position, size);

                this.Modified = true;
            }
            public int Read(int position, byte[] data, int offset, int size)
            {
                if(!this.Loaded)
                    throw new InvalidOperationException("当前页尚未加载。");
                if (position + size > this.buffer.Length)
                    throw new IndexOutOfRangeException("超出页数据范围。");

                this.DateAccess = owner.stopwatch.ElapsedTicks;
                Buffer.BlockCopy(buffer, position, data, offset, size);

                return size;
            }
        }
    }
}
