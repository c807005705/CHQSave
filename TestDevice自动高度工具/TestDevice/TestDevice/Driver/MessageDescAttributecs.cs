using System;
using System.Collections.Generic;
using System.Text;

namespace TestDevice
{
    public class MessageDescAttribute : Attribute
    {
        public DESCRIPTION Description { get; private set; }
        /// <summary>
        /// 特征码
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="name"></param>
        /// <param name="desc">字段描述，设null则为无数据。</param>
        /// <param name="linkedID"></param>
        public MessageDescAttribute(byte mid, string name, TYPE_DESC[] desc, byte linkedMid)
        {
            this.Description = new DESCRIPTION(mid, name, desc, linkedMid);
        }
        public MessageDescAttribute(MID mid, string name, TYPE_DESC[] desc, MID linkedMID) : this((byte)mid, name, desc, (byte)linkedMID)
        { }
    }
}
