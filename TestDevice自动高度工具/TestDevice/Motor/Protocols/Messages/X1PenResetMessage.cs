using System;
using System.Collections.Generic;
using System.Text;
using Protocols;
using TestDevice;

namespace Motor
{
    /// <summary>
    /// 笔头归位
    /// </summary>
    [MessageDesc(MID.PenReset, "PenReset", null, MID.PenResetResponse)]
    public class PenResetMessage : SimpleMessage
    { }
    [MessageDesc(MID.PenResetResponse, "PenResetResponse", null, MID.PenReset)]
    public class PenResetResponseMessage : SimpleMessage
    { }

    /// <summary>
    /// 笔头角度归位
    /// </summary>
    [MessageDesc(MID.PenAngleReset, "PenAngleReset", null, MID.PenAngleResetResponse)]
    public class PenAngleResetMessage : SimpleMessage
    { }
    [MessageDesc(MID.PenAngleResetResponse, "PenAngleResetResponse", null, MID.PenAngleReset)]
    public class PenAngleResetResponseMessage : SimpleMessage
    { }
}
