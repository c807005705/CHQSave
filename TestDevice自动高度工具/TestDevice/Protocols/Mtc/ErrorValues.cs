// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;

namespace Mobot.TestBox.Protocols.Mtc
{
    /// <summary>
    /// Error values
    /// </summary>
    public enum ErrorValues : uint
    {
        ERV_GENERAL_FAILURE = 0,
        ERV_REQ_NOT_EXISTS = 1,
        ERV_INVALID_PARAM = 2,
        ERV_OUT_OF_MEMORY = 3,
        ERV_STOP = 4,
        ERV_REQ_NOT_SUPPORTED = 5,
        ERV_TRANSPORT_FAILURE = 6,
        ERV_LOGIC = 7,
        ERV_ALREADY_OPEN = 8,
        ERV_NOT_CONNECTED = 9,
        ERV_WRONG_RESPONSE = 10,
        ERV_INVALID_FORMAT = 11,
        ERV_NO_ROOM = 12,
        ERV_NOT_ALLOWED = 13,
        ERV_ERROR_ON_SYSINFO = 14,
    }
}
