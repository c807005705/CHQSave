using Interface.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraLibs
{
    public class Camera : ICamera
    {

        /// <summary>
        /// 海康日志
        /// </summary>
        public ILog Log { get; set; }
        /// <summary>
        /// 相机操作
        /// </summary>
        private HvCameraOperator cameraOperator = new HvCameraOperator();
        /// <summary>
        /// 图片采集回调
        /// </summary>
        public Action<Bitmap> OnCreateImage { get; set; }
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            cameraOperator.Close();
            cameraOperator.GetOneBitmap();
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            this.Dispose();
            
        }
        /// <summary>
        /// 获取一张图片
        /// </summary>
        /// <returns></returns>
        public Task<Bitmap> GetOneBitmap()
        {
            return Task.Factory.StartNew<Bitmap>(() => {
                return cameraOperator.GetOneBitmap().Clone() as Bitmap;
            });
        }
        /// <summary>
        /// 获取一张图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Task<string> GetOnePicture(string path)
        {
            Task<string> task = new Task<string>(new Func<object, string>(t =>
            {
                try
                {
                    string mypath = t as string;
                    Log.log("图片保存为 : ", mypath);
                    cameraOperator.GetOneBitmap().Save(mypath);
                    return mypath;
                }
                catch (Exception ex)
                {
                    Log.error(ex);
                    return "";
                }
            }), path);
            task.Start();
            return task;
        }
        /// <summary>
        /// 打开相机
        /// </summary>
        public void Open()
        {
            cameraOperator.Open(0);
            
        }

        /// <summary>
        /// 保存相机参数
        /// </summary>
        public void SaveParamters()
        {
            //cameraOperator.SaveParamter();
        }
        /// <summary>
        /// 设置相机ROI
        /// </summary>
        /// <param name="rectangle"></param>
        public void SetROI(Rectangle rectangle)
        {

        }
        /// <summary>
        /// 开启取图
        /// </summary>
        public void StartGrabbing()
        {
            cameraOperator.StartGrabbing();
        }
        /// <summary>
        /// 停止采集
        /// </summary>
        public void StopGrabbing()
        {
            cameraOperator.StopGrabbing();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
          
        }
        /// <summary>
        /// 保存图片
        /// </summary>
        public void SaveImage()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 是否连接成功
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            throw new NotImplementedException();
        }
    }
}
