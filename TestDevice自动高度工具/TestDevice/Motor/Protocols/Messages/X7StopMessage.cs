using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.Stop, "Stop", null, MID.StopResponse)]
    public class StopMessage : SimpleMessage
    { }

    [MessageDesc(MID.StopResponse, "StopResponse", null, MID.Stop)]
    public class StopResponseMessage : SimpleMessage
    { }
}
