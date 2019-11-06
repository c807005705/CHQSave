using BslCamera;
using Interface.Interface;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CameraLibs
{
    public class Performance : ICamera
    {

        /// <summary>
        /// 巴斯勒相机
        /// </summary>
       // private BCamera bCamera { get; set; } = new BCamera();



        public Performance()
        {
           
        }
        /// <summary>
        /// 日志
        /// </summary>
        public ILog log { get; set; }


        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            //bCamera.Dispose();
        }
        ///// <summary>
        ///// 获取当前图片
        ///// </summary>
        ///// <returns></returns>
        //public Bitmap GetCurrentPictrue()
        //{
        //    // return bCamera.Snap();

        //}
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
           // bCamera.Init();
        }
        /// <summary>
        /// 保存图片路径
        /// </summary>
        public void SaveImage()
        {

        }

        public void Open()
        {
           // BCamera.StaticInit();
            //bCamera.Open(0);
        }

        public void Close()
        {
            //if (bCamera.IsOpen)
            //    bCamera.Close();
            //else
            //{
            //    MessageBox.Show("相机未打开");
            //}
        }

        public bool IsOpen()
        {
            //return bCamera.IsOpen;
            return true;
        }
    }
}
