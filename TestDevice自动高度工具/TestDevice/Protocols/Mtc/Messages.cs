// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Mobot.TestBox.Protocols;
using Mobot.TestBox.Protocols.Mtc.Messages;

namespace Mobot.TestBox.Protocols.Mtc
{
    public enum SetCursorCaptureMsg
    {
        CURSOR_PROPERTY
    } ;

    public enum GrabImageExRequestMsg
    {
        EX_ORIGINX, EX_ORIGINY, EX_WIDTH, EX_HEIGHT, EX_BEHAVIOR, EX_TIMEOUT
    } ;

    public enum GrabImageExResponseMsg
    {
        EXRESP_ORIGINX, EXRESP_ORIGINY, EXRESP_WIDTH, EXRESP_HEIGHT, EXRESP_FORMAT, EXRESP_BEHAVIOR, EXRESP_IMAGE
    } ;

    public enum LaunchAppExRequest
    {
        EX_LAUNCHINFO
    } ;

    public enum LaunchAppExResponse
    {
        EXRESP_APP_HANDLE
    } ;

    public enum StopAppExRequest
    {
        EX_STOPINFO
    } ;

    public enum StopAppExResponse
    {
        EXRESP_STOP_FLAGS
    } ;

    public enum IsAppRunningRequest
    {
        IS_RUNNING_INFO
    } ;

    public enum IsAppRunningResponse
    {
        IS_RUNNING_STATUS
    } ;

    /*
     * 
            messageParser.AddDesc(new DESCRIPTION("GetScreenDimension",
                    new TYPE_DESC[]{
			            TYPE_DESC.UInt8,		// The Request Type, always a value of GET_SYS_INFO_COM_ID
		            },
                    MessageID.SCREEN_DIMENSION_COM_ID));
            messageParser.AddDesc(new DESCRIPTION("GetScreenDimensionResponse",
                    new TYPE_DESC[]{
			            TYPE_DESC.UInt8,		// The Response Type, always a value of GET_SYS_INFO_RESP_ID
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
		            },
                    MessageID.SCREEN_DIMENSION_RESP_ID));
            messageParser.AddDesc(new DESCRIPTION("LaunchApp",
                    new TYPE_DESC[]{
			            TYPE_DESC.UInt8,		// The Request Type, always a value of LAUNCH_APP_COM_ID
			            TYPE_DESC.TStr		//APP_NAME: The name of the application to be launched
		            },
                    MessageID.LAUNCH_APP_RESP_ID));
            messageParser.AddDesc(new DESCRIPTION("LaunchAppResponse",
                    new TYPE_DESC[]{
			            TYPE_DESC.UInt8,		// The Response Type, always a value of LAUNCH_APP_RESP_ID
			            TYPE_DESC.UInt32		//APP_HANDLE: A handle that represents the launched application
		            },
                    MessageID.LAUNCH_APP_COM_ID));
            messageParser.AddDesc(new DESCRIPTION("StopApp",
                    new TYPE_DESC[]{
			            TYPE_DESC.UInt8,		// The Request Type, always a value of STOP_APP_COM_ID
			            TYPE_DESC.UInt32		//APP_HANDLE: The handle returned by the Launch Application request
		            },
                    MessageID.STOP_APP_RESP_ID));
            messageParser.AddDesc(new DESCRIPTION("StopAppResponse",
                    new TYPE_DESC[]{
			            TYPE_DESC.UInt8		// The Response Type, always a value of STOP_APP_RESP_ID
		            },
                    MessageID.STOP_APP_COM_ID));
            messageParser.AddDesc(new DESCRIPTION("Reset",
                    new TYPE_DESC[]{
			            TYPE_DESC.UInt8,		// The Request Type, always a value of RESET_COM_ID
			            TYPE_DESC.UInt32		// The number of milliseconds to wait after the response
						            // is sent and before the system is actually reset
		            },
                    MessageID.RESET_RESP_ID));
            messageParser.AddDesc(new DESCRIPTION("ResetResponse",
                    new TYPE_DESC[]{
			            TYPE_DESC.UInt8		// The Response Type, always a value of RESET_RESP_ID
		            },
                    MessageID.RESET_COM_ID));
     */


    //enum
    //{
    //    MOUSE_CLICKS
    //};

    //enum
    //{
    //    MOUSE_BUTTON, MOUSE_X, MOUSE_Y
    //};

    public enum TC_MouseButton
    {
        TC_Left, TC_Middle, TC_Right
    } ;

    public enum MouseTapMsg
    {
        MOUSETAP_BUTTON, MOUSETAP_X, MOUSETAP_Y, MOUSETAP_NUMBER, MOUSETAP_DOWN_DELAY, MOUSETAP_DELAY
    } ;

    public enum GetMemoryStatsResponse
    {
        MSR_FreeMemory,
        MSR_UsedMemory
    } ;

    public enum DecreaseFreeMemoryRequest
    {
        DFM_AmtToRetain
    } ;

    public enum DecreaseFreeMemoryResponse
    {
        DFM_Handle
    } ;

    public enum RestoreFreeMemoryRequest
    {
        RFM_Handle
    } ;

    public enum DriveStatsRequest
    {
        DS_DriveInfo
    } ;

    public enum DriveStatsResponse
    {
        DSR_FreeSpace,
        DSR_UsedSpace
    } ;

    public enum DecreaseFreeDriveSpaceRequest
    {
        DFDS_AmtToRetain,
        DFDS_DriveInfo
    } ;

    public enum DecreaseFreeDriveSpaceResponse
    {
        DFDS_Handle
    } ;

    public enum RestoreFreeDriveSpaceRequest
    {
        RFDS_Handle
    } ;

    public enum ResetOneDriveRequest
    {
        ROD_DriveInfo
    } ;

    public enum DoesFileExistRequest
    {
        DFE_Filename
    } ;

    public enum DoesFileExistResponse
    {
        DFE_Exists,
        DFE_Attrs
    } ;

    public enum DuplicateFileRequest
    {
        DF_Behavior,
        DF_FileSpecs
    } ;

    public enum MoveFileRequest
    {
        MF_Behavior,
        MF_FileSpecs
    } ;

    public enum MoveFolderRequest
    {
        MFO_Behavior,
        MFO_FileSpecs
    } ;

    public enum DeleteFileRequest
    {
        DEF_Behavior,
        DEF_Filename
    } ;

    public enum DeleteFolderRequest
    {
        DEFO_Behavior,
        DEFO_Filename
    } ;


    public static class MtcMessages
    {
        public static Type[] GetMessageTypes()
        {
            List<Type> listTypes = new List<Type>();

            //Failure消息
            listTypes.Add(typeof(FailureMessage));
            //PING消息
            listTypes.Add(typeof(PingMessage));
            listTypes.Add(typeof(PingRespMessage));

            listTypes.Add(typeof(GrabImageMessage));
            listTypes.Add(typeof(GrabImageResponseMessage));

            listTypes.Add(typeof(TapMessage));
            listTypes.Add(typeof(TapResponseMessage));

            listTypes.Add(typeof(StylusDownMessage));
            listTypes.Add(typeof(StylusDownResponseMessage));

            listTypes.Add(typeof(StylusUpMessage));
            listTypes.Add(typeof(StylusUpResponseMessage));

            listTypes.Add(typeof(MoveMessage));
            listTypes.Add(typeof(MoveResponseMessage));

            listTypes.Add(typeof(BatchMessage));
            listTypes.Add(typeof(BatchResponseMessage));

            listTypes.Add(typeof(SleepMessage));
            listTypes.Add(typeof(SleepResponseMessage));

            listTypes.Add(typeof(KeyPressMessage));
            listTypes.Add(typeof(KeyPressResponseMessage));

            listTypes.Add(typeof(KeySequenceMessage));
            listTypes.Add(typeof(KeySequenceResponseMessage));
            //Get System Information 消息
            listTypes.Add(typeof(SystemInfoMessage));
            listTypes.Add(typeof(SystemInfoResponseMessage));

            /*
            listTypes.Add(typeof(GetScreenDimensionMessage));
            listTypes.Add(typeof(GetScreenDimensionResponseMessage));
            listTypes.Add(typeof(LaunchAppMessage));
            listTypes.Add(typeof(LaunchAppResponseMessage));
            listTypes.Add(typeof(StopAppMessage));
            listTypes.Add(typeof(StopAppResponseMessage));
            listTypes.Add(typeof(ResetMessage));
            listTypes.Add(typeof(ResetResponseMessage));
            listTypes.Add(typeof(SetCursorCapture));
            listTypes.Add(typeof(MouseRotateWheel));
            listTypes.Add(typeof(MouseHold));
            listTypes.Add(typeof(MouseRelease));
            listTypes.Add(typeof(MouseTap));
            listTypes.Add(typeof(GrabImageEx));
            listTypes.Add(typeof(LaunchAppEx));
            listTypes.Add(typeof(StopAppEx));
            listTypes.Add(typeof(IsAppRunningRequest));
            listTypes.Add(typeof(GetMemoryStatsRequest));
            listTypes.Add(typeof(DecFreeMemoryRequest));
            listTypes.Add(typeof(RestoreFreeMemoryRequest));
            listTypes.Add(typeof(GetDriveStatsRequest));
            listTypes.Add(typeof(DecFreeDriveSpaceRequest));
            listTypes.Add(typeof(RestoreFreeDriveSpaceRequest));
            listTypes.Add(typeof(ResetFreeMemoryRequest));
            listTypes.Add(typeof(ResetOneDriveRequest));
            listTypes.Add(typeof(ResetAllDriveRequest));
            listTypes.Add(typeof(DoesFileExistRequest));
            listTypes.Add(typeof(DuplicateFileRequest));
            listTypes.Add(typeof(MoveFileRequest));
            listTypes.Add(typeof(DeleteFileRequest));
            listTypes.Add(typeof(MoveFolderRequest));
            listTypes.Add(typeof(DeleteFolderRequest));
            listTypes.Add(typeof());
	        */

            return listTypes.ToArray();
        }
    }
}
