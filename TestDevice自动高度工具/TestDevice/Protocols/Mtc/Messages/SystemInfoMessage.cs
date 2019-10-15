using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Mobot.TestBox.Protocols.Mtc.Messages
{
    [MessageDesc(MessageID.GET_SYS_INFO_COM_ID, "GetSysInfo", null, MessageID.GET_SYS_INFO_RESP_ID)]
    public class SystemInfoMessage : SimpleMessage
    {
        public override string ToString()
        {
            return "SystemInfo()";
        }
    }

    [MessageDesc(MessageID.GET_SYS_INFO_RESP_ID, "GetSysInfoResponse",
        new TYPE_DESC[]{
            TYPE_DESC.UInt32,		// contains 1, help in determine byte-order
            TYPE_DESC.UInt8,		// Major version number of agents operating system
            TYPE_DESC.UInt8,		// Minor version number of agents operating system
            TYPE_DESC.UInt16,		// Build number of agents operating system
            TYPE_DESC.UInt8,		// Major version number of the agent
            TYPE_DESC.UInt8,		// Minor version number of the agent
            TYPE_DESC.UInt16,		// Build number of the agent
            TYPE_DESC.UInt16,		// The displays width in pixels
            TYPE_DESC.UInt16,		// The displays height in pixels
            TYPE_DESC.UInt32,		// The number of possible colors on the display
            TYPE_DESC.UInt32,		// The size of the largest payload that the agent can accept from target
            TYPE_DESC.UInt32,		// The size of the largest payload that the agent will send back to target
            TYPE_DESC.UInt32,		// The size of the Blob that follows; can be 0
            TYPE_DESC.Blob		// This contains any target/agent specific information that the target and agent agree upon
        }, MessageID.GET_SYS_INFO_COM_ID)]
    public class SystemInfoResponseMessage : MessageBase
    {
        /// <summary>
        /// 操作系统主版本号 次版本号 Bulid Number
        /// </summary>
        public Version OSVersion { get; private set; }
        /// <summary>
        /// Agent主版本号 次版本号 Build Number
        /// </summary>
        public Version AgentVersion { get; private set; }
        /// <summary>
        /// 显示像素（宽） 显示像素（长）
        /// </summary>
        public Size ScreenSize { get; private set; }
        /// <summary>
        /// 颜色数量
        /// </summary>
        public UInt32 Colors { get; private set; }
        /// <summary>
        /// Agent能接受的最大数据包
        /// </summary>
        public long MaxTransmission { get; private set; }
        /// <summary>
        /// Agent能发送的最大数据包
        /// </summary>
        public long MaxResult { get; private set; }
        /// <summary>
        /// 附加数据，如果BLOB为0，则为空
        /// </summary>
        public byte[] Data { get; private set; }
        public SystemInfoResponseMessage(Version OSVersion, Version AgentVersion, Size ScreenSize, UInt32 Colors, long MaxTransmission, long MaxResult, byte[] Data)
        {
            this.OSVersion = OSVersion;
            this.AgentVersion = AgentVersion;
            this.ScreenSize = ScreenSize;
            this.Colors = Colors;
            this.MaxTransmission = MaxTransmission;
            this.MaxResult = MaxResult;
            this.Data = Data;
        }
        public SystemInfoResponseMessage()
        {
            this.OSVersion = new Version();
            this.AgentVersion = new Version();
            this.ScreenSize = Size.Empty;
            this.Colors = 0;
            this.MaxTransmission = 0;
            this.MaxResult = 0;
            this.Data = null;
        }

        //SysInfoMsg
        //LONG_ONE, OS_MAJOR_VERSION, OS_MINOR_VERSION, OS_BUILD_NUMBER,
        //AGENT_MAJOR_VERSION, AGENT_MINOR_VERSION, AGENT_BUILD_NUMBER,
        //DISPLAY_WIDTH, DISPLAY_HEIGHT, NUM_COLORS,
        //MAX_TRANSMISSION, MAX_RESULT, BLOB_SIZE, DATA
        public override void SetRawData(object[] RawData)
        {
            int offset = 0;
            UInt32 LONG_ONE = (UInt32)RawData[offset++];//UInt32	1
            Byte OS_MAJOR_VERSION = (Byte)RawData[offset++];//UInt8	操作系统主版本号
            Byte OS_MINOR_VERSION = (Byte)RawData[offset++];//UInt8	操作系统次版本号
            UInt16 OS_BUILD_NUMBER = (UInt16)RawData[offset++];//UInt16	操作系统Bulid Number
            Byte AGENT_MAJOR_VERSION = (Byte)RawData[offset++];//UInt8	Agent主版本号
            Byte AGENT_MINOR_VERSION = (Byte)RawData[offset++];//UInt8	Agent次版本号
            UInt16 AGENT_BUILD_NUMBER = (UInt16)RawData[offset++];//UInt16	Agent的Build Number
            UInt16 DISPLAY_WIDTH = (UInt16)RawData[offset++];//UInt16	显示像素（宽）
            UInt16 DISPLAY_HEIGHT = (UInt16)RawData[offset++];//UInt16	显示像素（长）
            UInt32 NUM_COLORS = (UInt32)RawData[offset++];//UInt32	颜色数量
            UInt32 MAX_TRANSMISSION = (UInt32)RawData[offset++];//UInt32	Agent能接受的最大数据包
            UInt32 MAX_RESULT = (UInt32)RawData[offset++];//UInt32	Agent能发送的最大数据包

            UInt32 BLOB_SIZE = (UInt32)RawData[offset++];//UInt32	BLOB的大小，可以为0
            byte[] DATA = (byte[])RawData[offset++];//BLOB	附加数据，如果BLOB为0，则为空


            this.OSVersion = new Version(OS_MAJOR_VERSION, OS_MINOR_VERSION, OS_BUILD_NUMBER);
            this.AgentVersion = new Version(AGENT_MAJOR_VERSION, AGENT_MINOR_VERSION, AGENT_BUILD_NUMBER);
            this.ScreenSize = new Size(DISPLAY_WIDTH, DISPLAY_HEIGHT);
            this.Colors = NUM_COLORS;
            this.MaxTransmission = MAX_TRANSMISSION;
            this.MaxResult = MAX_RESULT;
            if (BLOB_SIZE > 0)
                this.Data = DATA;
            else
                this.Data = null;
        }
        public override object[] ToRawData()
        {
            object[] value = new object[14];

            int offset = 0;
            value[offset++] = (UInt32)1;//UInt32	1
            value[offset++] = (Byte)OSVersion.Major;//UInt8	操作系统主版本号
            value[offset++] = (Byte)OSVersion.Minor;//UInt8	操作系统次版本号
            value[offset++] = (UInt16)OSVersion.Build;//UInt16	操作系统Bulid Number
            value[offset++] = (Byte)AgentVersion.Major;//UInt8	Agent主版本号
            value[offset++] = (Byte)AgentVersion.Minor;//UInt8	Agent次版本号
            value[offset++] = (UInt16)AgentVersion.Build;//UInt16	Agent的Build Number
            value[offset++] = (UInt16)ScreenSize.Width;//UInt16	显示像素（宽）
            value[offset++] = (UInt16)ScreenSize.Height;//UInt16	显示像素（长）
            value[offset++] = (UInt32)Colors;//UInt32	颜色数量
            value[offset++] = (UInt32)MaxTransmission;//UInt32	Agent能接受的最大数据包
            value[offset++] = (UInt32)MaxResult;//UInt32	Agent能发送的最大数据包
            value[offset++] = (UInt32)((Data == null) ? 0 : Data.Length);//UInt32	BLOB的大小，可以为0
            value[offset++] = (byte[])Data;//BLOB	附加数据，如果BLOB为0，则为空

            return value;
        }
    }
}
