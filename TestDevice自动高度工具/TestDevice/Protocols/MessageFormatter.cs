
using System;
using System.Collections.Generic;
using System.Text;
using Mobot.Utils;
using TestDevice;

namespace Protocols
{
    /// <summary>
    /// MessageFormatter = Message Formatter.
    /// MessageParser = 
    /// </summary>
    public partial class MessageFormatter
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        BasicFormatter format = new BasicFormatter(false);
        BitFormatter format2 = new BitFormatter(false);

        /// <summary>
        /// Key=MessageID Value=DESCRIPTION
        /// </summary>
        Dictionary<byte, DESCRIPTION> listDescs = new Dictionary<byte, DESCRIPTION>();
        //public List<DESCRIPTION> ListDescription
        //{
        //    get { return listDescs; }
        //}
        public void AddDesc(DESCRIPTION desc)
        {
            listDescs.Add(desc.MID, desc);
        }
        public void AddDesc(byte mid, string name, TYPE_DESC[] desc, byte linkedMID)
        {
            listDescs.Add(mid, new DESCRIPTION(mid, name, desc, linkedMID));
        }
        public MessageFormatter()
        {
        }
        /// <summary>
        /// 获取指定消息的字段列表。
        /// </summary>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        TYPE_DESC[] GetFields(byte MessageID)
        {
            DESCRIPTION desc = (listDescs.ContainsKey(MessageID)) ? listDescs[MessageID] : null;
            return (desc == null) ? null : desc.Fields;
        }

        /// <summary>
        /// create a description of the command with the given name in the buffer
        /// 
        /// HRESULT FormatCommand(byte *pDest, int size, const char *pszCommandName, long *BytesWritten);
        /// </summary>
        /// <param name="pDest"></param>
        /// <param name="size"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public int Format(byte[] dest, int offset, string pszCommandName, object[] value)
        {
            if (dest == null || dest.Length - offset < 4)
                throw new ArgumentException();


            byte id = 0;
            // search command description by its name
            foreach(KeyValuePair<byte, DESCRIPTION> pair in listDescs)
            {
                if (pszCommandName == pair.Value.name)
                {
                    id = pair.Key;
                    break;
                }
            }
            if (id == 0)
                return 0;

            return Format(dest, offset, id, value);
        }
        public int Format(byte[] dest, int offset, MessageBase value)
        {
            byte mid = MessageBase.GetMessageID(value.GetType());
            return Format(dest, offset, mid, value.ToRawData());
        }
        private int Format(byte[] dest, int offset, byte mid, object[] value)
        {
            //int BytesWritten = 0;
            int start = offset;
            int consumed = 0;

            //1 格式化命令头 The Request Type, always a value of **_ID
            dest[offset] = (byte)mid;
            offset += 1;

            //2 找到指定的 DESCRIPTION
            TYPE_DESC[] descs = this.GetFields(mid);
            if (descs == null)//该消息没有数据
                return offset - start;

            //判断数据长度是否一致
            //逐个字段格式化
            try
            {
                for (int i = 0; i < descs.Length; i++)
                {
                    int bytesConsumed = 0;
                    if (value.Length < i + 1) break;
                    switch (descs[i])
                    {
                        case TYPE_DESC.Int8:
                            dest[offset] = (Byte)value[i];
                            bytesConsumed = 1;
                            break;
                        case TYPE_DESC.UInt8:
                            dest[offset] = (Byte)value[i];
                            bytesConsumed = 1;
                            break;
                        case TYPE_DESC.Int16:
                            bytesConsumed = format2.FormatInt16(dest, offset, (Int16)value[i]);
                            break;
                        case TYPE_DESC.UInt16:
                            bytesConsumed = format2.FormatUInt16(dest, offset, (UInt16)value[i]);
                            break;
                        case TYPE_DESC.Int32:
                            bytesConsumed = format2.FormatInt32(dest, offset, (Int32)value[i]);
                            break;
                        case TYPE_DESC.UInt32:
                            bytesConsumed = format2.FormatUInt32(dest, offset, (UInt32)value[i]);
                            break;
                        case TYPE_DESC.TStr:
                            bytesConsumed = format.FormatString(dest, offset, (String)value[i]);
                            break;
                        case TYPE_DESC.Blob:
                            bytesConsumed = format.FormatArray(dest, offset, (byte[])value[i]);
                            break;
                        default:
                            break;
                    }
                    offset += bytesConsumed;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            consumed = offset - start;

            return consumed;
        }
        public object[] Parse(byte[] buffer, int offset, out int consumed)
        {
            int start = offset;
            consumed = 0;
            
            //1 获取命令头
            byte MID = buffer[offset];
            offset += 1;
            //2 找到指定的 DESCRIPTION
            TYPE_DESC[] descs = this.GetFields(MID);
            if (descs == null)//该消息没有数据
                return null;

            //逐个字段格式化
            object[] value = new object[descs.Length];
            try
            {
                for (int i = 0; i < descs.Length; i++)
                {
                    int bytesConsumed = 0;
                    switch (descs[i])
                    {
                        case TYPE_DESC.Int8:
                            value[i] = (SByte)buffer[offset];
                            bytesConsumed = 1;
                            break;
                        case TYPE_DESC.UInt8:
                            value[i] = (Byte)buffer[offset];
                            bytesConsumed = 1;
                            break;
                        case TYPE_DESC.Int16:
                            value[i] = format2.GetInt16(buffer, offset);
                            bytesConsumed = 2;
                            break;
                        case TYPE_DESC.UInt16:
                            value[i] = format2.GetUInt16(buffer, offset);
                            bytesConsumed = 2;
                            break;
                        case TYPE_DESC.Int32:
                            value[i] = format2.GetInt32(buffer, offset);
                            bytesConsumed = 4;
                            break;
                        case TYPE_DESC.UInt32:
                            value[i] = format2.GetUInt32(buffer, offset);
                            bytesConsumed = 4;
                            break;
                        case TYPE_DESC.TStr:
                            value[i] = format.GetString(buffer, offset, out bytesConsumed); 
                            break;
                        case TYPE_DESC.Blob:
                            value[i] = format.GetArray(buffer, offset, out bytesConsumed);
                            break;
                        default:
                            break;
                    }
                    offset += bytesConsumed;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            consumed = offset - start;
            return value;
        }
    }
}
