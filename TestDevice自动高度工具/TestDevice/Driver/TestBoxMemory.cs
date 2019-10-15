// Copyright 2011 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mobot.Utils;
using System.Drawing;
using Mobot;
using TestDevice;
using Motor;

namespace Driver
{
    /// <summary>
    /// 电机控制器配置存储空间。256×16bytes的配置存储区。
    /// </summary>
    public class TestBoxMemory : SettingFile, IMotorSettings
    {
        /// <summary>
        /// Flash总空间数。
        /// </summary>
        public const int MemorySize = 1024 * 4;

        const string Key_DeviceGuid = "device.guid";
        const string Key_DeviceType = "device.type";
        const string Key_DeviceSerialNumber = "device.serialnumber";
        const string Key_DeviceDate = "device.date";
        const string Key_DeviceManufacturer = "device.manufacturer";

        const string KEY_MotorRangeTicksX = "stylus.range.x";
        const string KEY_MotorRangeTicksY = "stylus.range.y";
        const string KEY_MotorRangeTicksZ = "stylus.range.z";
        const string KEY_ZeroTicksX = "stylus.zero.x";
        const string KEY_ZeroTicksY = "stylus.zero.y";
        const string KEY_ZeroTicksZ = "stylus.zero.z";
        const string KEY_TicksPerMMX = "stylus.tickspermm.x";
        const string KEY_TicksPerMMY = "stylus.tickspermm.y";
        const string KEY_TicksPerMMZ = "stylus.tickspermm.z";
        const string KEY_CameraType = "camera.type";
        const string KEY_CameraSerialNumber = "camera.serialnumber";

        const string KEY_CalculateCoefficient_K1 = "CalculateCoefficient.K1";
        const string KEY_CalculateCoefficient_C1 = "CalculateCoefficient.C1";
        const string KEY_CalculateCoefficient_K2 = "CalculateCoefficient.K2";
        const string KEY_CalculateCoefficient_C2 = "CalculateCoefficient.C2";

        const string KEY_DevbiceConfigData = "device.config";
        const string KEY_CheckData = "device.check";

        readonly Stream stream = null;
        public TestBoxMemory(Stream stream)
        {
            this.stream = stream;

            SetByteCount(4);
            Add(Key_DeviceGuid, SettingType.Guid);//设备唯一序列号。
            Add(Key_DeviceType, SettingType.UInt32);//设备类型，对应设备型号(device.model)
            Add(Key_DeviceSerialNumber, SettingType.String, 16);//出厂编号
            Add(Key_DeviceDate, SettingType.DateTime);//出厂日期
            Add(Key_DeviceManufacturer, SettingType.String, 32);//制造商

            SetByteCount(128);
            Add(KEY_MotorRangeTicksX, SettingType.Range);//笔移动范围 X
            Add(KEY_MotorRangeTicksY, SettingType.Range);//笔移动范围 Y
            Add(KEY_MotorRangeTicksZ, SettingType.Range);//笔移动范围 Z
            Add(KEY_ZeroTicksX, SettingType.Int32);//笔零点位置 X
            Add(KEY_ZeroTicksY, SettingType.Int32);//笔零点位置 Y
            Add(KEY_ZeroTicksZ, SettingType.Int32);//笔零点位置 Z
            //移除
            Add("stylus.safez", SettingType.Int32);//笔的安全高度
            Add(KEY_TicksPerMMX, SettingType.Double);//每毫米的刻度数 X
            Add(KEY_TicksPerMMY, SettingType.Double);//每毫米的刻度数 Y
            Add(KEY_TicksPerMMZ, SettingType.Double);//每毫米的刻度数 Z
            //移除
            Add("stylus.speed.range.x", SettingType.Range);//电机移动速度范围
            Add("stylus.speed.range.y", SettingType.Range);//电机移动速度范围
            Add("stylus.speed.range.z", SettingType.Range);//电机移动速度范围
            Add("stylus.speed.x", SettingType.Int32);//笔移动速度 X
            Add("stylus.speed.y", SettingType.Int32);//笔移动速度 Y
            Add("stylus.speed.z", SettingType.Int32);//笔移动速度 Z

            Add(KEY_CameraType, SettingType.UInt8);//相机类型
            Add(KEY_CameraSerialNumber, SettingType.String, 16);//相机编号

            //移除
            Add("camera.settings", SettingType.Boolean);//是否有值
            Add("camera.settings.exposureTime", SettingType.Double);//快门（秒）
            //移除
            Add("camera.transform", SettingType.Boolean);
            Add("deleted.camera.transform.rotateAngle", SettingType.Double);
            Add("deleted.camera.transform.clipBounds", SettingType.Rectangle);
            Add("display", SettingType.Boolean);
            Add("display.resolution", SettingType.Size2);
            //移除
            Add("workspace", SettingType.Boolean);//是否有值
            Add("workspace.size", SettingType.Size2D);//毫米
            Add("workspace.topLeft", SettingType.Point2D);//毫米
            Add("workspace.bottomRight", SettingType.Point2D);//毫米
            Add("workspace.datumMarks", SettingType.RectangleD);//毫米
            //移除
            Add("display.datumMarks", SettingType.Boolean);
            Add("display.datumMarks.exposureTime", SettingType.Double);
            Add("display.datumMarks.pixelBounds", SettingType.Rectangle);
            Add("display.datumMarks.blobSizeRange", SettingType.Range);
            //移除
            Add("camera.transform.center", SettingType.Point2D);
            Add("camera.transform.size", SettingType.Size2D);
            Add("camera.transform.angle", SettingType.Single);
            Add("camera.transform.direction", SettingType.UInt8);

            Add(KEY_CalculateCoefficient_K1, SettingType.Double);
            Add(KEY_CalculateCoefficient_C1, SettingType.Double);
            Add(KEY_CalculateCoefficient_K2, SettingType.Double);
            Add(KEY_CalculateCoefficient_C2, SettingType.Double);

            //图像校准点
            Add(KEY_CheckData, SettingType.Point2);
            Add("(empty)", SettingType.Bytes, MemorySize - base.ByteCount);
        }

        protected override int Read(int position, byte[] buffer, int offset, int count)
        {
            stream.Seek(position, SeekOrigin.Begin);
            return stream.Read(buffer, offset, count);
        }
        protected override void Write(int position, byte[] buffer, int offset, int count)
        {
            stream.Seek(position, SeekOrigin.Begin);
            stream.Write(buffer, offset, count);
        }

        public void Save()
        {
            stream.Flush();
        }

        public double? ExposureTime { get; set; }
        public DatumMarks DatumMarks { get; set; }
        public DisplayInfo DisplayInfo { get; set; }
        public WorkspaceInfo WorkspaceInfo { get; set; }

        /// <summary>
        /// 设备校准数据
        /// </summary>
        public Point2 DeviceCheckData
        {
            get
            {
                object result;
                TryGetValue(KEY_CheckData, out result);
                return (Point2)result;
            }
            set
            {
                SetValue(KEY_CheckData, (Point2)value);
            }
        }

        public CalculateCoefficient CalculateCoefficient
        {
            get
            {
                return new CalculateCoefficient(
                    (double)GetValue(KEY_CalculateCoefficient_K1),
                    (double)GetValue(KEY_CalculateCoefficient_C1),
                    (double)GetValue(KEY_CalculateCoefficient_K2),
                    (double)GetValue(KEY_CalculateCoefficient_C2));
            }
            set
            {
                if (value == null)
                {
                    SetValue(KEY_CalculateCoefficient_K1, 0.0);
                    SetValue(KEY_CalculateCoefficient_C1, 0.0);
                    SetValue(KEY_CalculateCoefficient_K2, 0.0);
                    SetValue(KEY_CalculateCoefficient_C2, 0.0);
                }
                else
                {
                    SetValue(KEY_CalculateCoefficient_K1, value.CalculateCoefficient_K1);
                    SetValue(KEY_CalculateCoefficient_C1, value.CalculateCoefficient_C1);
                    SetValue(KEY_CalculateCoefficient_K2, value.CalculateCoefficient_K2);
                    SetValue(KEY_CalculateCoefficient_C2, value.CalculateCoefficient_C2);
                }
            }
        }

        #region 设备信息
        /// <summary>
        /// 获取 设备名称。
        /// </summary>
        public Guid DeviceGuid
        {
            get { return (Guid)GetValue(Key_DeviceGuid); }
            set { SetValue(Key_DeviceGuid, value); }
        }
        /// <summary>
        /// 获取 设备类型。
        /// </summary>
        public DeviceType DeviceType
        {
            get { return (DeviceType)((uint)GetValue(Key_DeviceType)); }
            set { SetValue(Key_DeviceType, (uint)value); }
        }
        /// <summary>
        /// 获取 厂商。
        /// </summary>
        public string DeviceManufacturer
        {
            get { return (string)GetValue(Key_DeviceManufacturer); }
            set { SetValue(Key_DeviceManufacturer, value); }
        }
        /// <summary>
        /// 出厂编号。
        /// </summary>
        public string DeviceSerialNumber
        {
            get { return (string)GetValue(Key_DeviceSerialNumber); }
            set { SetValue(Key_DeviceSerialNumber, value); }
        }
        public DateTime DeviceDate
        {
            get { return (DateTime)GetValue(Key_DeviceDate); }
            set { SetValue(Key_DeviceDate, value); }
        }

        #endregion

        #region 坐标系
        /// <summary>
        /// 每毫米的刻度数
        /// </summary>
        public Point3D TicksPerMM
        {
            get
            {
                return new Point3D(
                    (double)GetValue(KEY_TicksPerMMX),
                    (double)GetValue(KEY_TicksPerMMY),
                    (double)GetValue(KEY_TicksPerMMZ));
            }
            set
            {
                SetValue(KEY_TicksPerMMX, value.X);
                SetValue(KEY_TicksPerMMY, value.Y);
                SetValue(KEY_TicksPerMMZ, value.Z);
            }
        }

        /// <summary>
        /// 机械校零值
        /// </summary>
        public Point3 ZeroTicks
        {
            get
            {
                return new Point3(
                    (Int32)GetValue(KEY_ZeroTicksX),
                    (Int32)GetValue(KEY_ZeroTicksY),
                    (Int32)GetValue(KEY_ZeroTicksZ));
            }
            set
            {
                SetValue(KEY_ZeroTicksX, value.X);
                SetValue(KEY_ZeroTicksY, value.Y);
                SetValue(KEY_ZeroTicksZ, value.Z);
            }
        }

        /// <summary>
        /// 坐标范围
        /// </summary>
        public Range[] MotorRangeTicks
        {
            get
            {
                return new Range[] {
                    (Range)GetValue(KEY_MotorRangeTicksX),
                    (Range)GetValue(KEY_MotorRangeTicksY),
                    (Range)GetValue(KEY_MotorRangeTicksZ),
                };
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("MotorRanges", "值不能为空。");
                if (value.Length < 3)
                    throw new ArgumentException("MotorRanges", "必须是三轴的范围值。");

                SetValue(KEY_MotorRangeTicksX, value[0]);
                SetValue(KEY_MotorRangeTicksY, value[1]);
                SetValue(KEY_MotorRangeTicksZ, value[2]);
            }
        }

        #endregion
    }
}
