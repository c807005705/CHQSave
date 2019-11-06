using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
    /// <summary>
    /// 相机接口
    /// </summary>
   public interface ICamera
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
       
        ///// <summary>
        ///// 获取当前图片
        ///// </summary>
        ///// <returns></returns>
        //Bitmap GetCurrentPictrue();
        /// <summary>
        /// 释放
        /// </summary>
        void Dispose();
        /// <summary>
        /// 保存图片路径
        /// </summary>
        void SaveImage();
        /// <summary>
        /// 打开相机
        /// </summary>
        void Open();
        /// <summary>
        /// 关闭相机
        /// </summary>
        void Close();
    
    }
}
