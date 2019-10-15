using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{
    [MessageDesc(MID.BatchStop, "BatchStop", null, MID.BatchStopResponse)]
    public class BatchStopMessage : SimpleMessage { }

    [MessageDesc(MID.BatchStopResponse, "BatchStopResponse", null, MID.BatchStop)]
    public class BatchStopResponseMessage : SimpleMessage { }
}
