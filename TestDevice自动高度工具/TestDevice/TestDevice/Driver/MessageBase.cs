using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestDevice
{
    public abstract class MessageBase
    {
        public MessageBase()
        {
        }
        public abstract object[] ToRawData();
        public abstract void SetRawData(object[] RawData);

        //public virtual MessageID MessageID
        //{
        //    get { return MessageDesc.ID; }
        //}

        public static byte GetMessageID(Type type)
        {
            MessageDescAttribute[] attribs = type.GetCustomAttributes(typeof(MessageDescAttribute), false) as MessageDescAttribute[];
            if (attribs.Length > 0 && attribs[0] != null)
                return attribs[0].Description.MID;

            return 0;
        }

        public override string ToString()
        {
            MessageDescAttribute[] attribs = this.GetType().GetCustomAttributes(typeof(MessageDescAttribute), false) as MessageDescAttribute[];
            if (attribs.Length < 1 || attribs[0] == null)
                return "Message(Unknown)";

            return string.Format("Message(MID=0x{0:X2}, NAME={1})",
                attribs[0].Description.MID,
                attribs[0].Description.name);
        }
    }
}
