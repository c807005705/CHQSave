using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.Reset, "Reset", null, MID.ResetResponse)]
    public class ResetMessage : SimpleMessage
    { }

    [MessageDesc(MID.ResetResponse, "ResetResponse", null, MID.Reset)]
    public class ResetResponseMessage : SimpleMessage
    { }
}
