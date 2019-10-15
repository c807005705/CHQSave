
using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using System.Drawing;
using Motor.Protocols.Messages;

namespace Motor
{
    public static class MotorMessages
    {
        public static Type[] GetMessageTypes()
        {
            return new Type[] {
                typeof(FailureMessage),//Failure消息响应
                //Ping 命令
                typeof(PingMessage),
                typeof(PingRespMessage),
                //Reset 命令
                typeof(ResetMessage),
                typeof(ResetResponseMessage),
                //Stop 命令
                typeof(StopMessage),
                typeof(StopResponseMessage),

                typeof(SetSpeedMessage),
                typeof(SetSpeedResponseMessage),
                
                typeof(ResetDeviceMessage),
                typeof(ResetDeviceResponseMessage),
                //Batch 命令
                typeof(BatchModeMessage),
                typeof(BatchResponseMessage),
                //Sleep 命令
                typeof(SleepMessage),
                typeof(SleepResponseMessage),
                //Move 命令
                typeof(MoveMessage),
                typeof(MoveResponseMessage),
                //GetPosition 命令
                typeof(GetPositionMessage),
                typeof(GetPositionRespMessage),
                //ReadFlash 命令
                typeof(ReadFlashMessage),
                typeof(ReadFlashResponseMessage),
                //WriteFlash 命令
                typeof(WriteFlashMessage),
                typeof(WriteFlashResponseMessage),
                //Get System Information
                typeof(SystemInfoMessage),
                typeof(SystemInfoResponseMessage),
                //设置手机测试状态
                typeof(TestStatuMessage),
                typeof(TestStatuResponseMessage),
                //光感应等开关
                typeof(SetIndoorLightMessage),
                typeof(SetIndoorLightResponseMessage),
                //查询是否可以开始测试
                typeof(TestQueryMessage),
                typeof(TestQueryResponseMessage),
                //USBHUB手机充电使能或关闭
                typeof(SetUsbHubMessage),
                typeof(SetUsbHubResponseMessage),
                //USB充电模式选择命令
                typeof(SetUsbModeMessage),
                typeof(SetUsbModeResponseMessage),
                
                //手机固定夹紧命令
                typeof(PhoneClampedMessage),
                typeof(PhoneClampedResponseMessage),
                //三个侧键点击命令
                typeof(SideKeyMessage),
                typeof(SideKeyResponseMessage),
                //音频数据线插拔命令
                typeof(AudioSwapMessage),
                typeof(AudioSwapResponseMessage),
                //电源数据线插拔命令
                typeof(PowerSwapMessage),
                typeof(PowerSwapResponseMessage),
                //夹具推进推出命令
                typeof(FixInOutMessage),
                typeof(FixInOutResponseMessage),
                //前置摄像头色卡缩进推出命令
                typeof(PreCameraMessage),
                typeof(PreCameraResponseMessage),
                //后置摄像头背光灯命令
                typeof(BackCameraMessage),
                typeof(BackCameraResponseMessage),
                //麦克探头缩进伸出命令
                typeof(MikeInOutMessage),
                typeof(MikeInOutResponseMessage),
                //翻转测试命令
                typeof(FlipMessage),
                typeof(FlipResponseMessage),
                //三色状态指示灯命令
                typeof(LightStatuMessage),
                typeof(LightStatuResponseMessage),

                //笔头选择协议
                typeof(PenMessage),
                typeof(PenResponseMessage),
                //笔头水平旋转角度协议
                typeof(PenAngleMessage),
                typeof(PenAngleResponseMessage),
                //笔头归位
                typeof(PenResetMessage),
                typeof(PenResetResponseMessage),
                //笔头角度归位
                typeof(PenAngleResetMessage),
                typeof(PenAngleResetResponseMessage),

                //压力检测
                typeof(PressureMessage),
                typeof(PressureResponseMessage),

                //查询压力值
                typeof(PressureGramsMessage),
                typeof(PressureGramsResponseMessage),
                
                //Batch停止
                typeof(BatchStopMessage),
                typeof(BatchStopResponseMessage),
                
                //画圆孤命令
                typeof(DrawCircleMessage),
                typeof(DrawCircleResponseMessage),

                //手动测试触发命令
                typeof(TriggerMessage),
                typeof(TriggerResponseMessage),


                //Z轴归位
                typeof(StylusRestZMessage),
                typeof(StylusRestZResponseMessage),

            };
        }
    }
}