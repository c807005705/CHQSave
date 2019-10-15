using System;
using System.Collections.Generic;
using System.Text;

namespace TestDevice
{
    public enum MID : byte
    {
        /// <summary>
        /// Error Message
        /// </summary>
        ErrorResponse = 0x7F,

        /// <summary>
        /// System Messages
        /// </summary>
        GetSysInfo = 0x02,
        GetSysInfoResponse = 0x05,

        /// <summary>
        /// Miscellaneous Messages
        /// </summary>
        Ping = 0x03,
        PingResponse = 0x06,

        /// <summary>
        /// 停止命令
        /// </summary>
        Stop = 0x2D,
        StopResponse = 0xAD,

        /// <summary>
        /// 画圆孤
        /// </summary>
        DrawCircle = 0x2E,
        DrawCircleResponse = 0xAE,

        /// <summary>
        /// Batch中止命令
        /// </summary>
        BatchStop = 0x2F,
        BatchStopResponse = 0xAF,

        #region 手动性能测试设备
        /// <summary>
        /// 手动测试触发命令
        /// </summary>
        Trigger = 0x30,
        TriggerResponse = 0xB0,

        #endregion

        #region 穿戴设备
        /// <summary>
        /// 摆臂起始位置配置
        /// </summary>
        ArmStart = 0x3A,
        ArmStartResponse = 0xBA,

        /// <summary>
        /// 摆臂终点位置配置
        /// </summary>
        ArmEnd = 0x3B,
        ArmEndResponse = 0xBB,

        /// <summary>
        /// 运动周期设置
        /// </summary>
        ArmCycle = 0x3C,
        ArmCycleResponse = 0xBC,

        /// <summary>
        /// 摆臂运动开始(次数)
        /// </summary>
        ArmRun = 0x3D,
        ArmRunResponse = 0xBD,

        /// <summary>
        /// 保存设备运动参数
        /// </summary>
        ArmSave = 0x3E,
        ArmSaveResponse = 0xBE,

        /// <summary>
        /// 获取设备运动参数
        /// </summary>
        ArmGet = 0x3F,
        ArmGetResponse = 0xBF,

        /// <summary>
        /// 获取摆臂次数
        /// </summary>
        ArmStatus = 0x40,
        ArmStatusResponse = 0xC0,

        /// <summary>
        /// 设备重置命令
        /// </summary>
        Reset = 0x41,
        ResetResponse = 0xC1,

        /// <summary>
        /// 摆臂暂停/继续
        /// </summary>
        ArmPause = 0x42,
        ArmPauseResponse = 0xC2,

        #endregion

        SetSpeed = 0x47,
        SetSpeedResponse = 0xC7,

        ResetDevice = 0x49,
        ResetDeviceResponse = 0xC9,

        ReadFlash = 0x4B,
        ReadFlashResponse = 0xCB,

        WriteFlash = 0x4C,
        WriteFlashResponse = 0xCC,

        BatchMode = 0x4D,
        BatchModeResponse = 0xCD,

        Sleep = 0x4E,
        SleepResponse = 0xCE,

        Move = 0x4F,
        MoveResponse = 0xCF,

        GetPosition = 0x50,
        GetPositionResponse = 0xD0,

        /// <summary>
        /// USB充电模式选择命令
        /// </summary>
        SetUsbMode = 0x52,
        SetUsbModeResponse = 0xD2,

        /// <summary>
        /// 手机测试状态
        /// </summary>
        TestStatu = 0x53,
        TestStatuResponse = 0xD3,

        /// <summary>
        /// 光感应等开关
        /// </summary>
        SetIndoorLight = 0x54,
        SetIndoorLightResponse = 0xD4,

        /// <summary>
        /// 手机测试查询
        /// </summary>
        TestQuery = 0x55,
        TestQueryResponse = 0xD5,

        /// <summary>
        /// 笔头振动检测
        /// </summary>
        PenVibration = 0x57,
        PenVibrationResponse = 0xD7,

        #region MMI设备
        /// <summary>
        /// 手机固定夹紧命令
        /// </summary>
        PhoneClamped = 0x58,
        PhoneClampedResponse = 0xD8,

        /// <summary>
        /// 三个侧键点击命令
        /// </summary>
        SideKey = 0x59,
        SideKeyResponse = 0xD9,

        /// <summary>
        /// 音频数据线插拔命令
        /// </summary>
        AudioSwap = 0x5A,
        AudioSwapResponse = 0xDA,

        /// <summary>
        /// 电源数据线插拔命令
        /// </summary>
        PowerSwap = 0x5B,
        PowerSwapResponse = 0xDB,

        /// <summary>
        /// 夹具推进推出命令
        /// </summary>
        FixInOut = 0x5C,
        FixInOutResponse = 0xDC,

        /// <summary>
        /// 前置摄像头色卡缩进推出命令
        /// </summary>
        PreCamera = 0x5D,
        PreCameraResponse = 0xDD,

        /// <summary>
        /// 后置摄像头背光灯命令
        /// </summary>
        BackCamera = 0x5E,
        BackCameraResponse = 0xDE,

        /// <summary>
        /// 麦克探头缩进伸出命令
        /// </summary>
        MikeInOut = 0x5F,
        MikeInOutResponse = 0xDF,

        /// <summary>
        /// 翻转测试命令
        /// </summary>
        Flip = 0x60,
        FlipResponse = 0xE0,

        /// <summary>
        /// 三色状态指示灯命令
        /// </summary>
        LightStatu = 0x61,
        LightStatuResponse = 0xE1,

        //USBHUB手机充电使能或关闭
        SetUsbHub = 0x63,
        SetUsbHubResponse = 0xE3,

        #endregion

        /// <summary>
        /// 指纹性能-压力设置
        /// </summary>
        TriggerSet = 0x66,
        TriggerSetResponse = 0xE6,

        /// <summary>
        /// 压力值校准
        /// </summary>
        Checked = 0x6A,
        CheckedResponse = 0xEA,

        #region 指纹设备
        /// <summary>
        /// 笔头选择协议
        /// </summary>
        Pen = 0x6C,
        PenResponse = 0xEC,
        /// <summary>
        /// 笔头水平旋转角度协议
        /// </summary>
        PenAngle = 0x6D,
        PenAngleResponse = 0xED,
        /// <summary>
        /// 笔头归位
        /// </summary>
        PenReset = 0x6E,
        PenResetResponse = 0xEE,
        /// <summary>
        /// 笔头角度归位
        /// </summary>
        PenAngleReset = 0x6F,
        PenAngleResetResponse = 0xEF,
        /// <summary>
        /// 压力检测
        /// </summary>
        Pressure = 0x70,
        PressureResponse = 0xF0,
        /// <summary>
        /// 压力克数
        /// </summary>
        PressureGrams= 0x76,
        PressureGramsResponse= 0xF6,

        /// <summary>
        /// Z轴归位
        /// </summary>
        StylusRestZ = 0x77,
        StylusRestZResponse = 0xF7,
        #endregion
    }
}
