using System;
using System.Collections.Generic;
using System.Text;

namespace TestDevice
{
    public class DESCRIPTION
    {
        /// <summary>
        ///  string name of the command
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public byte MID { get; private set; }
        /// <summary>
        /// list of the fields
        /// 字段描述，设null则为无数据。
        /// </summary>
        public TYPE_DESC[] Fields { get; private set; }
        /// <summary>
        /// ID of the response/command to corresponding command/response
        /// </summary>
        public byte linkedMID { get; private set; }

        public DESCRIPTION(byte mid, string name, TYPE_DESC[] desc, byte linkedMID)
        {
            this.MID = mid;
            this.name = name;
            this.Fields = desc;
            this.linkedMID = linkedMID;
        }
    }
}
