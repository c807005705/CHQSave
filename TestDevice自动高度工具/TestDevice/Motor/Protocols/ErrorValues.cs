
using System;
using System.Collections.Generic;
using System.Text;
using Mobot;

namespace Motor
{
    /// <summary>
    ///  三轴控制器错误代码。Error values
    /// </summary>
    public enum ErrorValues : uint
    {
        //0101（协议解析错误类）
        [StringValue("‘F3’（ESCAPE字节）后数据不为‘B1’,’B2’,’B3’,’B4’")]
        E01010001 = 0x01010001,
        [StringValue("在应该获得‘F1’（消息开始标识）处获取了其他无效数据")]
        E01010002 = 0x01010002,
        [StringValue("协议包长校验失败")]
        E01010003 = 0x01010003,
        [StringValue("获取‘F4’（协议结束标示）失败")]
        E01010004 = 0x01010004,
        [StringValue("未知的协议解析状态")]
        E01010005 = 0x01010005,
        [StringValue("重包，message_id与上一个ID重复")]
        E01010006 = 0x01010006,
        //0102（数据正文解析错误类）
        [StringValue("未定义的请求类型")]
        E01020001 = 0x01020001,
        [StringValue("Move to命令中，错误长度的X、Y坐标")]
        E01020002 = 0x01020002,
        [StringValue("Move to命令中，超出范围的X、Y坐标")]
        E01020003 = 0x01020003,
        [StringValue("Tap命令中，错误的内容长度")]
        E01020004 = 0x01020004,
        [StringValue("Tap命令中，超出范围的X、Y、Z坐标")]
        E01020005 = 0x01020005,
        [StringValue("Tap命令中，超出范围的按键次数，暂定最大20次")]
        E01020006 = 0x01020006,
        [StringValue("Key_sequence和key_press命令中，内容长度不正确")]
        E01020007 = 0x01020007,
        [StringValue("Key_sequence和key_press命令中，超出范围的按键次数，暂定最大20次")]
        E01020008 = 0x01020008,
        [StringValue("Key_sequence和key_press命令中，x轴坐标超出范围")]
        E01020009 = 0x01020009,
        [StringValue("Key_sequence和key_press命令中，y轴坐标超出范围")]
        E0102000a = 0x0102000a,
        [StringValue("Key_sequence和key_press命令中，z轴坐标超出范围")]
        E0102000b = 0x0102000b,
        [StringValue("Move命令中，超出范围的X、Y坐标")]
        E0102000c = 0x0102000c,
        [StringValue("Move命令中，错误长度的X、Y坐标")]
        E0102000d = 0x0102000d,
        [StringValue("MOVEZ命令中，超出范围的Z坐标")]
        E0102000e = 0x0102000e,
        [StringValue("MOVEZ命令中，错误长度的Z坐标")]
        E0102000f = 0x0102000f,
        [StringValue("CLICK命令中，错误的内容长度")]
        E01020010 = 0x01020010,
        [StringValue("CLICK命令中，超出范围的Z坐标")]
        E01020011 = 0x01020011,
        [StringValue("CLICK命令中，超出范围的click次数")]
        E01020012 = 0x01020012,
        [StringValue("CLICK命令中，超出范围的duration时间")]
        E01020013 = 0x01020013,
        [StringValue("CLICK命令中，click次数为0")]
        E01020014 = 0x01020014,
        [StringValue("Initialite命令中，错误的内容长度")]
        E01020015 = 0x01020015,
        [StringValue("Initialite命令中，超出范围的Z坐标")]
        E01020016 = 0x01020016,
        [StringValue("Setspeedingling中，错误的内容长度")]
        E01020017 = 0x01020017,
        [StringValue("Setspeedingling中，错误的速度档位")]
        E01020018 = 0x01020018,
        [StringValue("在向flash写入舵机初始值失败")]
        E01020019 = 0x01020019,
        [StringValue("Slip_screen命令中，错误的内容长度")]
        E0102001a = 0x0102001a,
        [StringValue("Slip_screen命令中，起始坐标与当前机械手位置不符")]
        E0102001b = 0x0102001b,
        [StringValue("Slip_screen命令中，超出范围的触笔抬起位置")]
        E0102001c = 0x0102001c,
        [StringValue("在Slip_screen命令中，超出范围的Z轴深度")]
        E0102001d = 0x0102001d,
        [StringValue("Slip_screen命令中，超出范围的结束位置")]
        E0102001e = 0x0102001e,
        [StringValue("Mobot_config命令中，错误的内容长度")]
        E0102001f = 0x0102001f,
        [StringValue("write_flash命令中，配置项长度不对")]
        E01020020 = 0x01020020,
        [StringValue("read_flash，错误的配置项号")]
        E01020021 = 0x01020021,
        [StringValue("write_flash，错误的配置项号")]
        E01020022 = 0x01020022,
        [StringValue("Batch命令中，错误的内容长度")]
        E01020023 = 0x01020023,
        [StringValue("Batch命令中，错误的内容")]
        E01020024 = 0x01020024,
        [StringValue("Batch命令中，开始执行批处理时队列中无动作")]
        E01020025 = 0x01020025,
        [StringValue("Sleep命令中，错误的内容长度")]
        E01020026 = 0x01020026,
        [StringValue("Move_xyz命令中，错误的内容长度")]
        E01020027 = 0x01020027,
        [StringValue("Move_xyz命令中，超出范围的x轴坐标")]
        E01020028 = 0x01020028,
        [StringValue("Move_xyz命令中，超出范围的y轴坐标")]
        E01020029 = 0x01020029,
        [StringValue("Move_xyz命令中，超出范围的z轴坐标")]
        E0102002a = 0x0102002a,
        [StringValue("接收Move_xyz命令时，系统正忙")]
        E0102002b = 0x0102002b,
        [StringValue("接收sleep命令时，系统正忙")]
        E0102002c = 0x0102002c,
        [StringValue("接收batch命令时，系统正忙")]
        E0102002d = 0x0102002d,
        [StringValue("接收reset命令时，系统正忙")]
        E0102002e = 0x0102002e,
        [StringValue("接收set_speed命令时，系统正忙")]
        E0102002f = 0x0102002f,
        [StringValue("Set_speed命令中，x速度为-")]
        E01020030 = 0x01020030,
        [StringValue("Set_speed命令中，y速度为-")]
        E01020031 = 0x01020031,
        [StringValue("Set_speed命令中，z速度为-")]
        E01020032 = 0x01020032,
        [StringValue("Set_speed命令中，命令处理超时")]
        E01020033 = 0x01020033,
        [StringValue("Reset命令中，命令处理超时")]
        E01020034 = 0x01020034,
        [StringValue("MoveXYZ命令中，命令处理超时")]
        E01020035 = 0x01020035,
        [StringValue("Batch中，MoveXYZ命令处理超时")]
        E01020036 = 0x01020036,
        [StringValue("Batch中，Set_speed命令处理超时")]
        E01020037 = 0x01020037,
        [StringValue("Batch中，Reset命令处理超时")]
        E01020038 = 0x01020038,
        [StringValue("USB开断命令中，错误的内容长度")]
        E01020039 = 0x01020039,
        [StringValue("USB开断命令中，错误的内容数据")]
        E01020040 = 0x01020040,
        [StringValue("自动跑命令中，错误的内容长度")]
        E01020041 = 0x01020041,
        [StringValue("自动跑命令中，错误的内容数据")]
        E01020042 = 0x01020042,
        //0201（控制器设置错误类）
        [StringValue("计算步数时，错误的电机编号。")]
        E02010001 = 0x02010001,
        [StringValue("计算坐标时，错误的电机编号。")]
        E02010002 = 0x02010002,
        [StringValue("计算坐标时，超出范围的坐标。")]
        E02010003 = 0x02010003,
        [StringValue("计算Z轴坐标时，小于最小的Z坐标。")]
        E02010004 = 0x02010004,
        [StringValue("计算Z轴坐标时，大于最大的Z坐标。")]
        E02010005 = 0x02010005,
        [StringValue("实时获取坐标时，计算错误的距离")]
        E02010006 = 0x02010006,
        //0301（按键队列错误）
        [StringValue("加入sleep的动作序列时，超过队列最大值")]
        E03010001 = 0x03010001,
        [StringValue("加入move的动作序列时，超过队列最大值")]
        E03010002 = 0x03010002,
        [StringValue("do_next_action中，未定义的命令类型")]
        E03010003 = 0x03010003,
        //0303（FLASH错误）
        [StringValue("Flash写入异常")]
        E03030001 = 0x03030001,
        [StringValue("Flash擦除异常")]
        E03030002 = 0x03030002,

        [StringValue("X轴,电机移动超时")]
        E06010001 = 0x06010001,
        [StringValue("Y轴,电机移动超时")]
        E06020001 = 0x06020001,
        [StringValue("Z轴,电机移动超时")]
        E06030001 = 0x06030001,
        [StringValue("X轴,电机设置速度超时")]
        E06010002 = 0x06010002,
        [StringValue("Y轴,电机设置速度超时")]
        E06020002 = 0x06020002,
        [StringValue("Z轴,电机设置速度超时")]
        E06030002 = 0x06030002,
        [StringValue("X轴,电机归位超时")]
        E06010003 = 0x06010003,
        [StringValue("Y轴,电机归位超时")]
        E06020003 = 0x06020003,
        [StringValue("Z轴,电机归位超时")]
        E06030003 = 0x06030003,
        [StringValue("X轴,电机异步操作超时")]
        E06010004 = 0x06010004,
        [StringValue("Y轴,电机异步操作超时")]
        E06020004 = 0x06020004,
        [StringValue("Z轴,电机异步操作超时")]
        E06030004 = 0x06030004,
        E07000001 = 0x07000001,
    }

    public static class ErrorHelper
    {
        public static string ErrorCode(ErrorValues error)
        {
            string codes = ((uint)error).ToString("X8");
            string desc = null;
            if (codes.Substring(0, 2) == "06" && (desc = Command(codes.Substring(4, 2))) != null)
            {
                return string.Format("{0},{1}(异常号{2})", Axis(codes.Substring(2, 2)), desc, codes.Substring(6, 2));
            }
            try
            {
                return EnumHelper.GetName(typeof(ErrorValues), error);
            }
            catch
            {
                return "#ERROR 0x" + codes;
            }
        }
        public static string Axis(string code)
        {
            switch (code)
            {
                case "01": return "X轴";
                case "02": return "Y轴";
                case "03": return "Z轴";
                default: return null;
            }
        }
        public static string Command(string code)
        {
            switch (code)
            {
                case "01": return "电机响应移动命令异常";
                case "02": return "电机响应速度设置命令异常";
                case "03": return "电机响应归位命令异常";
                default: return null;
            }
        }
    }
}
