// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;

namespace Mobot.TestBox.Protocols.Mtc
{
    public enum MessageID : byte
    {
        /// <summary>
        /// 未知。
        /// </summary>
        [StringValue("")]
        Unknown = 0,

        // Error Message
        [StringValue("ERROR 响应")]
        ERROR_RESP_ID = 0x7F,
        // Miscellaneous Messages
        [StringValue("PING 请求")]
        PING_COM_ID = 3,
        [StringValue("PING 响应")]
        PING_RESP_ID = 6,
        // Image Source Messages
        [StringValue("GRAB_IMAGE 请求")]
        GRAB_IMAGE_COM_ID = 1,
        [StringValue("GRAB_IMAGE 响应")]
        GRAB_IMAGE_RESP_ID = 4,
        [StringValue("GRAB_IMAGE_EX 请求")]
        GRAB_IMAGE_EX_COM_ID = 40,
        [StringValue("GRAB_IMAGE_EX 响应")]
        GRAB_IMAGE_EX_RESP_ID = 41,
        [StringValue("SCREEN_DIMENSION 请求")]
        SCREEN_DIMENSION_COM_ID = 78,
        [StringValue("SCREEN_DIMENSION 响应")]
        SCREEN_DIMENSION_RESP_ID = 79,
        // Touch Screen Messages
        [StringValue("TAP 请求")]
        TAP_COM_ID = 9,
        [StringValue("TAP 响应")]
        TAP_RESP_ID = 17,
        [StringValue("STYLUS_DOWN 请求")]
        STYLUS_DOWN_COM_ID = 10,
        [StringValue("STYLUS_DOWN 响应")]
        STYLUS_DOWN_RESP_ID = 18,
        [StringValue("STYLUS_UP 请求")]
        STYLUS_UP_COM_ID = 11,
        [StringValue("STYLUS_UP 响应")]
        STYLUS_UP_RESP_ID = 19,
        [StringValue("MOVE 请求")]
        MOVE_COM_ID = 12,
        [StringValue("MOVE 响应")]
        MOVE_RESP_ID = 20,
        // Batch Of Commands Message
        [StringValue("BATCH 请求")]
        BATCH_COM_ID = 27,
        [StringValue("BATCH 响应")]
        BATCH_RESP_ID = 21,
        // Sleep Message
        [StringValue("SLEEP 请求")]
        SLEEP_COM_ID = 28,
        [StringValue("SLEEP 响应")]
        SLEEP_RESP_ID = 29,
        // Keypad Messages
        [StringValue("KEY_PRESS 请求")]
        KEY_PRESS_COM_ID = 8,
        [StringValue("KEY_PRESS 响应")]
        KEY_PRESS_RESP_ID = 22,
        [StringValue("KEY_SEQUENCE 请求")]
        KEY_SEQUENCE_COM_ID = 25,
        [StringValue("KEY_SEQUENCE 响应")]
        KEY_SEQUENCE_RESP_ID = 26,
        // System Messages
        [StringValue("GET_SYS_INFO 请求")]
        GET_SYS_INFO_COM_ID = 2,
        [StringValue("GET_SYS_INFO 响应")]
        GET_SYS_INFO_RESP_ID = 5,

        [StringValue("LAUNCH_APP 请求")]
        LAUNCH_APP_COM_ID = 13,
        [StringValue("LAUNCH_APP 响应")]
        LAUNCH_APP_RESP_ID = 14,
        [StringValue("STOP_APP 请求")]
        STOP_APP_COM_ID = 15,
        [StringValue("STOP_APP 响应")]
        STOP_APP_RESP_ID = 23,
        [StringValue("RESET 请求")]
        RESET_COM_ID = 16,
        [StringValue("RESET 响应")]
        RESET_RESP_ID = 24,
        [StringValue("LAUNCH_APP_EX 请求")]
        LAUNCH_APP_EX_COM_ID = 42,
        [StringValue("LAUNCH_APP_EX 响应")]
        LAUNCH_APP_EX_RESP_ID = 43,
        [StringValue("STOP_APP_EX 请求")]
        STOP_APP_EX_COM_ID = 44,
        [StringValue("STOP_APP_EX 响应")]
        STOP_APP_EX_RESP_ID = 45,
        [StringValue("IS_APP_RUNNING 请求")]
        IS_APP_RUNNING_COM_ID = 46,
        [StringValue("IS_APP_RUNNING 响应")]
        IS_APP_RUNNING_RESP_ID = 47,
        [StringValue("GET_MEMORY_STATS 请求")]
        GET_MEMORY_STATS_COM_ID = 48,
        [StringValue("GET_MEMORY_STATS 响应")]
        GET_MEMORY_STATS_RESP_ID = 49,
        [StringValue("DEC_FREE_MEMORY 请求")]
        DEC_FREE_MEMORY_COM_ID = 50,
        [StringValue("DEC_FREE_MEMORY 响应")]
        DEC_FREE_MEMORY_RESP_ID = 51,
        [StringValue("RESTORE_FREE_MEMORY 请求")]
        RESTORE_FREE_MEMORY_COM_ID = 52,
        [StringValue("RESTORE_FREE_MEMORY 响应")]
        RESTORE_FREE_MEMORY_RESP_ID = 53,
        [StringValue("GET_DRIVE_STATS 请求")]
        GET_DRIVE_STATS_COM_ID = 54,
        [StringValue("GET_DRIVE_STATS 响应")]
        GET_DRIVE_STATS_RESP_ID = 55,
        [StringValue("DEC_FREE_DRIVE_SPACE 请求")]
        DEC_FREE_DRIVE_SPACE_COM_ID = 56,
        [StringValue("DEC_FREE_DRIVE_SPACE 响应")]
        DEC_FREE_DRIVE_SPACE_RESP_ID = 57,
        [StringValue("RESTORE_FREE_DRIVE_SPACE 请求")]
        RESTORE_FREE_DRIVE_SPACE_COM_ID = 58,
        [StringValue("RESTORE_FREE_DRIVE_SPACE 响应")]
        RESTORE_FREE_DRIVE_SPACE_RESP_ID = 59,
        [StringValue("RESET_FREE_MEMORY 请求")]
        RESET_FREE_MEMORY_COM_ID = 60,
        [StringValue("RESET_FREE_MEMORY 响应")]
        RESET_FREE_MEMORY_RESP_ID = 61,
        [StringValue("RESET_ONE_DRIVE 请求")]
        RESET_ONE_DRIVE_COM_ID = 62,
        [StringValue("RESET_ONE_DRIVE 响应")]
        RESET_ONE_DRIVE_RESP_ID = 63,
        [StringValue("RESET_ALL_DRIVE 请求")]
        RESET_ALL_DRIVE_COM_ID = 64,
        [StringValue("RESET_ALL_DRIVE 响应")]
        RESET_ALL_DRIVE_RESP_ID = 65,
        [StringValue("DOES_FILE_EXIST 请求")]
        DOES_FILE_EXIST_COM_ID = 66,
        [StringValue("DOES_FILE_EXIST 响应")]
        DOES_FILE_EXIST_RESP_ID = 67,
        [StringValue("DUP_FILE 请求")]
        DUP_FILE_COM_ID = 68,
        [StringValue("DUP_FILE 响应")]
        DUP_FILE_RESP_ID = 69,
        [StringValue("MOVE_FILE 请求")]
        MOVE_FILE_COM_ID = 70,
        [StringValue("MOVE_FILE 响应")]
        MOVE_FILE_RESP_ID = 71,
        [StringValue("DELETE_FILE 请求")]
        DELETE_FILE_COM_ID = 72,
        [StringValue("DELETE_FILE 响应")]
        DELETE_FILE_RESP_ID = 73,
        [StringValue("MOVE_FOLDER 请求")]
        MOVE_FOLDER_COM_ID = 74,
        [StringValue("MOVE_FOLDER 响应")]
        MOVE_FOLDER_RESP_ID = 75,
        [StringValue("DELETE_FOLDER 请求")]
        DELETE_FOLDER_COM_ID = 76,
        [StringValue("DELETE_FOLDER 响应")]
        DELETE_FOLDER_RESP_ID = 77,

        // Properties Messages
        [StringValue("SET_CURSOR_CAPTURE 请求")]
        SET_CURSOR_CAPTURE_COM_ID = 30,
        [StringValue("SET_CURSOR_CAPTURE 响应")]
        SET_CURSOR_CAPTURE_RESP_ID = 31,
        // Mouse messages
        [StringValue("MOUSE_ROTATE_WHEEL 请求")]
        MOUSE_ROTATE_WHEEL_COM_ID = 32,
        [StringValue("MOUSE_ROTATE_WHEEL 响应")]
        MOUSE_ROTATE_WHEEL_RESP_ID = 33,
        [StringValue("MOUSE_HOLD 请求")]
        MOUSE_HOLD_COM_ID = 34,
        [StringValue("MOUSE_HOLD 响应")]
        MOUSE_HOLD_RESP_ID = 35,
        [StringValue("MOUSE_RELEASE 请求")]
        MOUSE_RELEASE_COM_ID = 36,
        [StringValue("MOUSE_RELEASE 响应")]
        MOUSE_RELEASE_RESP_ID = 37,
        [StringValue("MOUSE_TAP 请求")]
        MOUSE_TAP_COM_ID = 38,
        [StringValue("MOUSE_TAP 响应")]
        MOUSE_TAP_RESP_ID = 39,
    }
}
