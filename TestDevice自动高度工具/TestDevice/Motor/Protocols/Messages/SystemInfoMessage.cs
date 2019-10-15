using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.GetSysInfo, "GetSysInfo", null, MID.GetSysInfoResponse)]
    public class SystemInfoMessage : SimpleMessage
    { }

    [MessageDesc(MID.GetSysInfoResponse, "GetSysInfoResponse",
        new TYPE_DESC[]{
            TYPE_DESC.UInt32,		//LONG_ONE: contains 1, help in determine byte-order
            TYPE_DESC.UInt8,		//OS_MAJOR_VERSION: Major version number of agents operating system
            TYPE_DESC.UInt8,		//OS_MINOR_VERSION: Minor version number of agents operating system
            TYPE_DESC.UInt16,		//OS_BUILD_NUMBER: Build number of agents operating system
            TYPE_DESC.UInt8,		//AGENT_MAJOR_VERSION: Major version number of the agent
            TYPE_DESC.UInt8,		//AGENT_MINOR_VERSION: Minor version number of the agent
            TYPE_DESC.UInt16,		//AGENT_BUILD_NUMBER: Build number of the agent
            TYPE_DESC.UInt16,		//片内FLASH页大小
            TYPE_DESC.UInt16,		//配置项片内FLASH页总数
            TYPE_DESC.UInt16,		//Z 最大行程
            TYPE_DESC.UInt32,		//MAX_TRANSMISSION: The size of the largest payload that the agent can accept from target
            TYPE_DESC.UInt32,		//MAX_RESULT: The size of the largest payload that the agent will send back to target
            TYPE_DESC.UInt32,		//BLOB_SIZE: The size of the Blob that follows; can be 0
            TYPE_DESC.Blob		//DATA: This contains any target/agent specific information that the target and agent agree upon
        }, MID.GetSysInfo)]
    public class SystemInfoResponseMessage : MessageBase
    {
        /// <summary>
        /// 操作系统主版本号 次版本号 Bulid Number
        /// </summary>
        public Version OSVersion { get; private set; }
        /// <summary>
        /// 固件版本，主版本号 次版本号 Build Number
        /// </summary>
        public Version HardwareVersion { get; private set; }
        /// <summary>
        /// 片内FLASH页大小
        /// </summary>
        public UInt16 FlashPageSize { get; private set; }
        /// <summary>
        /// 配置项片内FLASH页总数
        /// </summary>
        public UInt16 FlashPageCount { get; private set; }

        public SystemInfoResponseMessage(Version OSVersion, Version revision, UInt16 flashPageSize, UInt16 flashPageCount)
        {
            this.OSVersion = OSVersion;
            this.HardwareVersion = revision;
            this.FlashPageSize = flashPageSize;
            this.FlashPageCount = flashPageCount;
        }
        public SystemInfoResponseMessage()
        {
            this.OSVersion = new Version();
            this.HardwareVersion = new Version();
            this.FlashPageSize = 2048;
            this.FlashPageCount = 2;
        }

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
            UInt16 FLASH_PAGE_SIZE = (UInt16)RawData[offset++];//UInt16
            UInt16 FLASH_PAGE_COUNT = (UInt16)RawData[offset++];//UInt16
            UInt16 value3 = (UInt16)RawData[offset++];//UInt16
            UInt32 MAX_TRANSMISSION = (UInt32)RawData[offset++];//UInt32	Agent能接受的最大数据包
            UInt32 MAX_RESULT = (UInt32)RawData[offset++];//UInt32	Agent能发送的最大数据包

            UInt32 BLOB_SIZE = (UInt32)RawData[offset++];//UInt32	BLOB的大小，可以为0
            byte[] DATA = (byte[])RawData[offset++];//BLOB	附加数据，如果BLOB为0，则为空

            this.OSVersion = new Version(OS_MAJOR_VERSION, OS_MINOR_VERSION, OS_BUILD_NUMBER);
            this.HardwareVersion = new Version(AGENT_MAJOR_VERSION, AGENT_MINOR_VERSION, AGENT_BUILD_NUMBER);
            this.FlashPageSize = FLASH_PAGE_SIZE;
            this.FlashPageCount = FLASH_PAGE_COUNT;
        }
        public override object[] ToRawData()
        {
            object[] value = new object[14];

            int offset = 0;
            value[offset++] = (UInt32)1;//UInt32	1
            value[offset++] = (Byte)OSVersion.Major;//UInt8	操作系统主版本号
            value[offset++] = (Byte)OSVersion.Minor;//UInt8	操作系统次版本号
            value[offset++] = (UInt16)OSVersion.Build;//UInt16	操作系统Bulid Number
            value[offset++] = (Byte)HardwareVersion.Major;//UInt8	Agent主版本号
            value[offset++] = (Byte)HardwareVersion.Minor;//UInt8	Agent次版本号
            value[offset++] = (UInt16)HardwareVersion.Build;//UInt16	Agent的Build Number
            value[offset++] = (UInt16)FlashPageSize;//UInt16
            value[offset++] = (UInt16)FlashPageCount;//UInt16
            value[offset++] = (UInt16)0;//UInt16
            value[offset++] = (UInt32)0;//UInt32	Agent能接受的最大数据包
            value[offset++] = (UInt32)0;//UInt32	Agent能发送的最大数据包
            value[offset++] = (UInt32)0;//UInt32	BLOB的大小，可以为0
            value[offset++] = null;//BLOB	附加数据，如果BLOB为0，则为空

            return value;
        }
    }
}
