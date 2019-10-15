using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Mobot.TestBox.Motor;
using Mobot.TestBox.Cameras;
using Mobot.Utils;

namespace Mobot.TestBox
{
    public class DisplayParameters
    {
        readonly TestBoxDriver owner = null;
        public DisplayParameters(TestBoxDriver owner)
        {
            this.owner = owner;
            this.CameraType = CameraTypeEnum.DahengHV;
            this.CameraSerialNumber = "";
        }

        /// <summary>
        /// 将配置重置为测试盒的Flash配置。测试盒必须处于连接状态。
        /// </summary>
        public void Reset()
        {
            if (!owner.Controller.Driver.IsOpen)
                throw new Exception("连接设备后才可以获取设备配置。");

            FlashStream stream = null;
            TestBoxMemory settings = null;
            try
            {//存储配置
                stream = new FlashStream(owner.Controller.Driver);
                settings = new TestBoxMemory(stream);
                this.Import(settings);
            }
            finally
            {
                if (settings != null)
                {
                    try
                    {
                        settings.Save();
                    }
                    finally
                    {
                        settings = null;
                    }
                }
                if (stream != null)
                {
                    try
                    {
                        stream.Close();
                    }
                    finally
                    {
                        stream = null;
                    }
                }
            }
        }

        internal void Import(TestBoxMemory settings)
        {
            this.CameraType = settings.CameraType;
            this.CameraSerialNumber = settings.CameraSerialNumber;

            if (settings.ExposureTime != null)
                this.ExposureTime = settings.ExposureTime.Value;
            this.SetCameraSettings(settings.CameraSettings);

            this.CameraTransform = settings.CameraTransform;
            this.displayInfo = settings.DisplayInfo;
            this.WorkspaceInfo = settings.WorkspaceInfo;
            this.datumMarks = settings.DatumMarks;
            this.calculateCoefficient = settings.CalculateCoefficient;
        }

        /// <summary>
        /// 将配置存储到测试盒的Flash中。测试盒必须处于连接状态。
        /// </summary>
        /// <param name="settings"></param>
        public void Save()
        {
            if (!owner.Controller.Driver.IsOpen)
                throw new Exception("连接设备后才可以存储设备配置。");

            FlashStream stream = null;
            TestBoxMemory settings = null;
            try
            {//存储配置
                stream = new FlashStream(owner.Controller.Driver);
                settings = new TestBoxMemory(stream);
                this.Export(settings);
            }
            finally
            {
                if (settings != null)
                {
                    try
                    {
                        settings.Save();
                    }
                    finally
                    {
                        settings = null;
                    }
                }
                if (stream != null)
                {
                    try
                    {
                        stream.Close();
                    }
                    finally
                    {
                        stream = null;
                    }
                }
            }
        }
        internal void Export(TestBoxMemory settings)
        {
            //settings.CameraSettings = new CameraSettings(this.ExposureTime);
            //this.SetCameraSettings(settings.CameraSettings);
            //settings.CameraTransform = this.CameraTransform;
        }

        /// <summary>
        /// 将配置选项更新到设备。
        /// </summary>
        public void SyncCameraSettings(Point2 check)
        {
            if (owner.Camera == null)
                return;

            owner.Camera.SetCheck(check);
            if (this.cameraSettings != null)
                owner.Camera.SetSettings(this.cameraSettings);
        }

        ICameraSettings cameraSettings = null;
        public void SetCameraSettings(ICameraSettings value)
        {
            this.cameraSettings = value;

            if (owner.Camera != null)
                owner.Camera.SetSettings(this.cameraSettings);
        }
        public ICameraSettings GetCameraSettings()
        {
            if (owner.Camera == null)
                return this.cameraSettings;
            return owner.Camera.GetSettings();
        }


        public CameraTypeEnum CameraType { get; private set; }
        public string CameraSerialNumber { get; private set; }

        DisplayInfo displayInfo = null;
        public DisplayInfo DisplayInfo
        {
            get { return this.displayInfo; }
            set { this.displayInfo = value; }
        }

        double exposureTime = 60;
        public double ExposureTime
        {
            get
            {
                if (owner.Camera == null)
                    return this.exposureTime;
                return owner.Camera.ExposureTime;
            }
            set
            {
                this.exposureTime = value;

                if (owner.Camera != null)
                    owner.Camera.ExposureTime = this.exposureTime;
            }
        }

        WorkspaceInfo workspaceInfo = null;
        public WorkspaceInfo WorkspaceInfo
        {
            get { return this.workspaceInfo; }
            set { this.workspaceInfo = value; }
        }

        CameraTransform cameraTransform = null;
        public CameraTransform CameraTransform
        {
            get { return this.cameraTransform; }
            set { this.cameraTransform = value; }
        }

        DatumMarks datumMarks = null;
        public DatumMarks DatumMarks
        {
            get { return this.datumMarks; }
            set { this.datumMarks = value; }
        }

        CalculateCoefficient calculateCoefficient = null;
        public CalculateCoefficient CalculateCoefficient
        {
            get { return this.calculateCoefficient; }
            set { this.calculateCoefficient = value; }
        }
        public Size2D PixelPerMM
        {
            get
            {
                if (this.displayInfo == null || this.workspaceInfo == null)
                    return new Size2D(1, 1);

                Point2D topLeft = this.workspaceInfo.TopLeft;
                Point2D bottomRight = this.workspaceInfo.BottomRight;
                Size screenSize = this.displayInfo.Resolution;

                double ppmX, ppmY;
                if ((bottomRight.X > topLeft.X && bottomRight.Y < topLeft.Y)
                    && (bottomRight.X < topLeft.X && bottomRight.Y > topLeft.Y))
                {//屏幕X/Y轴与电机X/Y轴平行
                    ppmX = screenSize.Width / (bottomRight.X - topLeft.X);
                    ppmY = screenSize.Height / (bottomRight.Y - topLeft.Y);
                }
                else
                {//X/Y坐标轴互换
                    ppmY = screenSize.Height / (bottomRight.X - topLeft.X);
                    ppmX = screenSize.Width / (bottomRight.Y - topLeft.Y);
                }

                return new Size2D(Math.Abs(ppmX), Math.Abs(ppmY));
            }
        }

        /// <summary>
        /// 将触屏的坐标转换为电机坐标。
        /// </summary>
        /// <returns></returns>
        public void PointToMotor(int pixelX, int pixelY, out double x, out double y)
        {
            if (this.displayInfo == null || this.workspaceInfo == null)
                throw new Exception("触屏参数尚未配置。");

            Point2D topLeft = this.workspaceInfo.TopLeft;
            Point2D bottomRight = this.workspaceInfo.BottomRight;
            Size screenSize = this.displayInfo.Resolution;

            if ((bottomRight.X > topLeft.X && bottomRight.Y < topLeft.Y)
                && (bottomRight.X < topLeft.X && bottomRight.Y > topLeft.Y))
            {//屏幕X/Y轴与电机X/Y轴平行
                x = topLeft.X + pixelX * (bottomRight.X - topLeft.X) / screenSize.Width;
                y = topLeft.Y + pixelY * (bottomRight.Y - topLeft.Y) / screenSize.Height;
            }
            else
            {//X/Y坐标轴互换
                x = topLeft.X + pixelY * (bottomRight.X - topLeft.X) / screenSize.Height;
                y = topLeft.Y + pixelX * (bottomRight.Y - topLeft.Y) / screenSize.Width;
            }
        }
        /// <summary>
        /// 将电机的坐标转换为触屏坐标。
        /// </summary>
        /// <returns></returns>
        public void MotorToPoint(double x, double y, out int pixelX, out int pixelY)
        {
            if (this.displayInfo == null || this.workspaceInfo == null)
                throw new Exception("触屏参数尚未配置。");

            Point2D topLeft = this.workspaceInfo.TopLeft;
            Point2D bottomRight = this.workspaceInfo.BottomRight;
            Size screenSize = this.displayInfo.Resolution;

            if ((bottomRight.X > topLeft.X && bottomRight.Y < topLeft.Y)
                && (bottomRight.X < topLeft.X && bottomRight.Y > topLeft.Y))
            {//屏幕X/Y轴与电机X/Y轴平行
                pixelX = (int)((x - topLeft.X) / (bottomRight.X - topLeft.X) * screenSize.Width);
                pixelY = (int)((y - topLeft.Y) / (bottomRight.Y - topLeft.Y) * screenSize.Height);
            }
            else
            {//X/Y坐标轴互换
                pixelY = (int)((x - topLeft.X) / (bottomRight.X - topLeft.X) * screenSize.Height);
                pixelX = (int)((y - topLeft.Y) / (bottomRight.Y - topLeft.Y) * screenSize.Width);
            }
        }
    }
}
