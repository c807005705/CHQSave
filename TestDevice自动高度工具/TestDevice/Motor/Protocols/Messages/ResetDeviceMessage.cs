using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.ResetDevice, "ResetDevice", null, MID.ResetDeviceResponse)]
    public class ResetDeviceMessage : SimpleMessage
    { }

    [MessageDesc(MID.ResetDeviceResponse, "ResetDeviceResponse", null, MID.ResetDevice)]
    public class ResetDeviceResponseMessage : SimpleMessage
    { }
}
