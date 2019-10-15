using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Mobot.TestBox.Protocols.Mtc.Messages;

namespace Mobot.TestBox.Protocols.Mtc
{
    public partial class MtcClient : ClientBase
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override Type[] GetMessageTypes()
        {
            return Mobot.TestBox.Protocols.Mtc.MtcMessages.GetMessageTypes();
        }

        protected MtcClient(IDevice device)
            : base(device)
        {
        }

        public MessageBase Request(MessageBase message)
        {
            MessageID respMid;
            MessageBase respMsg;
            try
            {
                byte midValue;
                base.Request(message, out midValue, out respMsg);
                respMid = (MessageID)midValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            if (respMid == MessageID.ERROR_RESP_ID)
            {
                FailureMessage failure = respMsg as FailureMessage;
                string desc = EnumHelper.GetName(typeof(ErrorValues), failure.Reason);
                log.ErrorFormat("错误: [MTCClient.Request]FailReasom:{0} desc:{1}", failure.Reason, desc);
                throw new ErrorException(1, failure.Reason, desc);
            }
            return respMsg;
        }
    }
}
