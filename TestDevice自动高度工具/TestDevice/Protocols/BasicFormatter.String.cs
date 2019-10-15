
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocols
{
    partial class BasicFormatter
    {
        public int FormatString(byte[] pDest, int offset, string pszArgument)
        {
            if (pszArgument == null)
                pszArgument = "";
            //判断是否含有Unicode。
            bool isUnicode = false;
            foreach (char ch in pszArgument)
            {
                if (ch > byte.MaxValue)
                {
                    isUnicode = true;
                    break;
                }
            }
            //int count = Encoding.GetEncoding("GB2312").GetByteCount(pszArgument);
            //isUnicode = (count > pszArgument.Length);

            return FormatString(pDest, offset, pszArgument, isUnicode);
        }
        public int FormatString(byte[] pDest, int offset, string pszArgument, bool isUnicode)
        {
            if (isUnicode)
                return this.FormatStringUnicode(pDest, offset, pszArgument);
            else
                return this.FormatStringANSI(pDest, offset, pszArgument);
        }
        public string GetString(byte[] pSrc, int offset, out int BytesWritten)
        {
            string result = null;

            int start = offset;
            // Get the string's flags
            Byte Int8 = pSrc[offset + 0];
            // Get the string's size
            UInt16 ByteCount = bitFormatter.GetUInt16(pSrc, offset + 1);
            // Is it Unicode ?
            if ((Int8 & UNICODE_FLAG) == 0)
            {	// No, MBCS
                int bytesConsumed;
                result = this.GetStringANSI(pSrc, offset, out bytesConsumed);
                offset += bytesConsumed;
            }
            else
            {	// Yes, Unicode
                int bytesConsumed;
                result = this.GetStringUnicode(pSrc, offset, out bytesConsumed);
                offset += bytesConsumed;
            }

            BytesWritten = offset - start;
            return result;
        }



        //For retrieving data from the raw stream of bytes received from the Agent.






        /// <summary>
        /// Puts the ANSI string to the Payload buffer
        /// 
        /// HRESULT FormatString(byte *pDest, int size, const char *pszArgument, long *BytesWritten);
        /// </summary>
        /// <param name="pDest">A buffer that will receive the formatted bytes</param>
        /// <param name="offset"></param>
        /// <param name="pszArgument">ANSI string to be converted into a byte block</param>
        /// <returns>BytesWritten: The number of bytes of pDest consumed to format the argument</returns>
        /// <remarks>
        /// Format of the output buffer:
        ///
        /// UInt8	- A value giving the attributes of the text.  Bit 7 is the UNICODE (on)
        ///           or ANSI (off) flag; should be off; bit 6 is the continuation flag.
        ///           If the length of a string can be more than 65,535 bytes long then
        ///           string is broken down into more than one segment.
        ///           The continuation flag is used to indicate this condition.
        ///           If the flag is on then this segment is continued in the immediately 
        ///           following segment.  The continuation flag is not set in the last segment of the text.
        ///
        /// UInt16	- value giving the length of the text in bytes not including the Blob terminator.
        ///           This length is the number of bytes of the text, equals the number of characters.
        ///           This value is sensitive to the byte order using for this Blob.
        ///
        /// UInt8[]	- An array of bytes with prefixes. If a text contains 0xF3 or 0xF5 character
        ///           then such characters is escaped - prefixed by 0xF3 and 6th bit of the its value is inverted
        ///
        /// UInt8	- Blob terminator - character with 0xF5 code
        /// </remarks>
        int FormatStringANSI(byte[] pDest, int offset, string pszArgument)
        {
            if (pDest == null || pszArgument == null)
                throw new ArgumentException("ERV_INVALID_PARAM");

            int BytesWritten = 0;

            byte[] data = bitFormatter.FormatStringANSI(pszArgument);
            int len = data.Length;// source string length
            int totalSize = len;// count of prefixes for Blob

            int size = pDest.Length - offset;
            // iterate possible string segments
            while (len >= 0)
            {
                totalSize += 4;
                if (totalSize > size)
                    throw new ArgumentException("ERV_INVALID_PARAM");

                // check for last segmnent
                if (len > 0xFFFF)
                {
                    // not last
                    pDest[offset + 0] = CONTINUE_FLAG;
                    // byte order not affects the data
                    pDest[offset + 1] = 0xFF;
                    pDest[offset + 2] = 0xFF;
                }
                else
                {
                    // last segment
                    pDest[offset + 0] = 0;
                    // consider byte order and store segment's length
                    bitFormatter.FormatUInt16(pDest, offset + 1, (UInt16)len);
                }
                // adjust destination pointer
                offset += 3;
                // write Blob
                for (int i = 0; i < len; i++)
                {
                    byte c = data[i];
                    if (c == ESCAPE || c == EOB)
                    {
                        // byte should be prefixed
                        pDest[offset++] = ESCAPE;
                        // invert 6th bit in the following character
                        c ^= FLIP_MASK;
                        // increase the total size of the Blob
                        totalSize++;
                        // check that the size of destination buffer is enough
                        if (totalSize > size)
                            throw new ArgumentException("ERV_INVALID_PARAM");
                    }
                    pDest[offset++] = c;
                }
                // write Blob terminator
                pDest[offset++] = EOB;
                // adjust source string length
                len -= 0xFFFF;
            }

            BytesWritten = totalSize;

            return BytesWritten;
        }

        /*
            Puts the UNICODE string to the Payload buffer

            Parameters:

            [OUT]pDest			- A buffer that will receive the formatted bytes
            [IN]size			- The size of the destination buffer
            [IN]pszArgument		- UNICODE string to be converted into a byte block
            [OUT]BytesWritten	- The number of bytes of pDest consumed to format the argument

            Format of the output buffer:

            UInt8	- A value giving the attributes of the text.  Bit 7 is the UNICODE (on)
                      or ANSI (off) flag; should be on; bit 6 is the continuation flag.
                      If the length of a string can be more than 65,534 bytes long then
                      string is broken down into more than one segment.
                      The continuation flag is used to indicate this condition.
                      If the flag is on then this segment is continued in the immediately 
                      following segment.  The continuation flag is not set in the last segment of the text.

            UInt16	- value giving the length of the text in bytes not including the Blob terminator.
                      This length is the number of bytes of the text, not the number of characters.
                      This value is sensitive to the byte order using for this Blob.

            UInt8[]	- An array of bytes with prefixes. If a text contains 0xF3 or 0xF5 character
                      then such characters is escaped - prefixed by 0xF3 and 6th bit of the its value is inverted

            UInt8	- Blob terminator - character with 0xF5 code

            Returned value:

            S_OK - if string successfully decoded to the output buffer
            E_FAIL - if parameters cause the incorrectness

        */
        //HRESULT FormatStringW(byte *pDest, int size, const char *pszArgument, long *BytesWritten);
        int FormatStringUnicode(byte[] pDest, int offset, string pszArgument)
        {
            if (pDest == null || pszArgument == null)
                throw new ArgumentException("ERV_INVALID_PARAM");

            int BytesWritten = 0;

            byte[] data = bitFormatter.FormatStringUnicode(pszArgument);
            int len = data.Length;	// source string length
            int totalSize = len;								// count of prefixes for Blob

            int size = pDest.Length - offset;
            // iterate possible string segments
            while (len >= 0)
            {
                totalSize += 4;
                if (totalSize > size)
                    throw new ArgumentException("ERV_INVALID_PARAM");

                // check for last segmnent
                if (len > 0xFFFE)
                {
                    // not last
                    pDest[offset + 0] = UNICODE_FLAG | CONTINUE_FLAG;
                    // byte order not affects the data
                    pDest[offset + 1] = 0xFF;
                    pDest[offset + 2] = 0xFF;
                }
                else
                {
                    // last segment
                    pDest[offset + 0] = UNICODE_FLAG;
                    // consider byte order and store segment's length
                    bitFormatter.FormatUInt16(pDest, offset + 1, (UInt16)len);
                }
                // adjust destination pointer
                offset += 3;
                // write Blob
                for (int i = 0; i < len; i++)
                {
                    byte c = data[i];
                    if (c == ESCAPE || c == EOB)	// another condition may be (c >= 0xF1 || c <= 0xF5)
                    {
                        // byte should be prefixed
                        pDest[offset++] = ESCAPE;
                        // invert 6th bit in the following character
                        c ^= FLIP_MASK;
                        // increase the total size of the Blob
                        totalSize++;
                        // check that the size of destination buffer is enough
                        if (totalSize > size)
                        {
                            throw new ArgumentException("ERV_INVALID_PARAM");
                        }
                    }
                    pDest[offset++] = c;
                }
                // write Blob terminator
                pDest[offset++] = EOB;
                // adjust source string length
                len -= 0xFFFE;
            }

            BytesWritten = totalSize;

            return BytesWritten;
        }



        //HRESULT CBasicCommandFormatter::GetStringSize(const byte *pSrc, int cbSrc, long *Size)
        //	Returns the size of the string for prepare a buffer for it
        //    HRESULT GetStringSize(byte[] pSrc, int cbSrc, out int Size)
        //    {
        //    long			totalSize = 0L;
        //    long			offset = 0L;
        //    unsigned short	len = 0;
        //    int				flag;
        //    long			Consumed;

        //    // check parameters
        //    if (pSrc == NULL || Size == NULL || cbSrc < 4)
        //    {
        //        SetLastError(ERV_INVALID_PARAM);
        //        return E_FAIL;
        //    }

        //    do
        //    {
        //        offset += 4L;
        //        // check the size of the source buffer
        //        if (offset > cbSrc)
        //        {
        //            // unexpected end of the source buffer
        //            SetLastError(ERV_INVALID_PARAM);
        //            return E_FAIL;
        //        }
        //        flag = *pSrc++;
        //        len = GetShort(pSrc, cbSrc, (short*)&, &Consumed);
        //        // adjust total size and offset
        //        totalSize += len;
        //        offset += len;
        //        // check source buffer boundaries
        //        if (offset > cbSrc)
        //        {
        //            // unexpected end of the source buffer
        //            SetLastError(ERV_INVALID_PARAM);
        //            return E_FAIL;
        //        }
        //        // find next segment
        ////		pSrc += 2;
        //        pSrc += Consumed;
        //        for (int i = 0; i < len; i++)
        //        {
        //            if (*pSrc++ == 0xF3)
        //            {
        //                len++;
        //                offset++;
        //                // check source buffer boundaries
        //                if (offset > cbSrc)
        //                {
        //                    // unexpected end of the source buffer
        //                    SetLastError(ERV_INVALID_PARAM);
        //                    return E_FAIL;
        //                }
        //            }
        //        }
        //        // check for the end of the segment
        //        if (*pSrc++ != EOB)
        //        {
        //            // incorrect end of the string's segment
        //            SetLastError(ERV_INVALID_PARAM);
        //            return E_FAIL;
        //        }
        //    }
        //    while ((flag & CONTINUE_FLAG) != 0);

        //    if ((flag & UNICODE_FLAG) != 0)
        //        totalSize += 2L;
        //    else totalSize++;

        //    *Size = totalSize;

        //    return S_OK;
        //}

        /// <summary>
        /// Gets the ANSI string from the Payload buffer
        /// 
        /// HRESULT GetString(const byte *pSrc, int cbSrc, char *Value, int cbValue, long *BytesWritten);
        /// </summary>
        /// <param name="pSrc">A pointer to the bytes returned from a data stream</param>
        /// <param name="Value">A pointer to the ANSI string data ('\0'-terminated)</param>
        /// <param name="cbValue"></param>
        /// <returns>
        /// S_OK - if string successfully decoded to the output buffer
        /// E_FAIL - if parameters cause the incorrectness
        /// </returns>
        /// <remarks>
        /// Format of the input buffer (Blob):
        ///
        /// UInt8	- A value giving the attributes of the text.  Bit 7 is the UNICODE (on)
        ///           or ANSI (off) flag; should be off; bit 6 is the continuation flag.
        ///           If the length of a string can be more than 65,535 bytes long then
        ///           string is broken down into more than one segment.
        ///           The continuation flag is used to indicate this condition.
        ///           If the flag is on then this segment is continued in the immediately 
        ///           following segment.  The continuation flag is not set in the last segment of the text.
        ///
        /// UInt16	- value giving the length of the text in bytes not including the Blob terminator.
        ///           This length is the number of bytes of the text, equals the number of characters.
        ///           This value is sensitive to the byte order using for this Blob.
        ///
        /// UInt8[]	- An array of bytes with prefixes. If a text contains 0xF3 or 0xF5 character
        ///           then such characters is escaped - prefixed by 0xF3 and 6th bit of the its value is inverted
        ///
        /// UInt8	- Blob terminator - character with 0xF5 code
        /// </remarks>
        string GetStringANSI(byte[] pSrc, int offset, out int BytesWritten)
        {
            byte[] Value = new Byte[100000];
            //The number of bytes of pSrc consumed to initialize Value
            BytesWritten = 0;
            //The max length of the buffer pSrc
            int cbSrc = pSrc.Length - offset;
            //The max length of the buffer Value
            int cbValue = Value.Length;
            int totalSize = 0;
            int stringSize = 0;
            byte flag;

            // check argumnets
            if (pSrc == null || Value == null || cbValue < 1)
                throw new Exception("ERV_INVALID_PARAM");

            do
            {
                totalSize += 4;
                // check the size of the source buffer
                if (totalSize > cbSrc)// unexpected end of the source buffer
                    throw new Exception("ERV_INVALID_PARAM");

                flag = pSrc[offset++];
                if ((flag & UNICODE_FLAG) != 0)// UNICODE string
                    throw new Exception("ERV_INVALID_PARAM");
                else
                {
                    // multi-byte character string
                    UInt16 len;
                    /*	10-14-04 SJK
                    Serious bug! Now that we're working with Palm, which sends back data in big endian format,
                    it is the second byte that is least significant, and that byte is ignored here altogether!
                    The writer knew it is a 2-byte quantity because a few lines below he was incrementing
                    pSrc by 2 to get to the start of text.

                    Another fine example of outsourcing!

                                if (m_bBigEndianByteOrder)
                                {
                                    // big endian
                                    len = *pSrc + ((unsigned)pSrc[1] << 8);
                                }
                                else
                                {
                                    // little endian
                                    len = ((unsigned)*pSrc << 8) + pSrc[1];
                                }
                    */
                    len = bitFormatter.GetUInt16(pSrc, offset);
                    totalSize += len;
                    // check the size of the source buffer
                    if (totalSize > cbSrc)
                    {// unexpected end of the source buffer
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                    // check destination buffer
                    if (stringSize + len >= cbValue)
                    {// incorrect size of the destination buffer
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                    // shift to the start of the encoded text in the Blob
                    offset += 2;
                    // iterate encoded text
                    for (int i = 0; i < len; i++)
                    {
                        byte c = pSrc[offset++];
                        if (c == EOB)
                        {
                            // wrong Blob format
                            throw new Exception("ERV_INVALID_PARAM");
                        }
                        if (c == ESCAPE)
                        {
                            // increase total size
                            totalSize++;
                            // get escaped character
                            c = pSrc[offset++];
                            // restore inverted 6th bit
                            c ^= FLIP_MASK;
                        }
                        // store decoded character
                        Value[stringSize++] = c;
                    }
                    // check for the end of the segment
                    if (pSrc[offset++] != EOB)
                    {// incorrect end of the string's segment
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                }
            }
            while ((flag & CONTINUE_FLAG) != 0);

            BytesWritten = totalSize;
            return bitFormatter.GetStringANSI(Value, 0, stringSize);
        }




        /*
            Gets the UNICODE string from the Payload buffer

            Parameters:

            [IN]pSrc			- A pointer to the bytes returned from a data stream
            [IN]cbSrc			- The max length of the buffer pSrc
            [OUT]Value			- A pointer to the UNICODE string data (NULL-terminated)
            [IN]cbValue			- The max length of the buffer Value
            [OUT]BytesWritten	- The number of bytes of pSrc consumed to initialize Value

            Format of the input buffer (Blob):

            UInt8	- A value giving the attributes of the text.  Bit 7 is the UNICODE (on)
                      or ANSI (off) flag; should be on; bit 6 is the continuation flag.
                      If the length of a string can be more than 65,534 bytes long then
                      string is broken down into more than one segment.
                      The continuation flag is used to indicate this condition.
                      If the flag is on then this segment is continued in the immediately 
                      following segment.  The continuation flag is not set in the last segment of the text.

            UInt16	- value giving the length of the text in bytes not including the Blob terminator.
                      This length is the number of bytes of the text, not the number of characters.
                      This value is sensitive to the byte order using for this Blob.

            UInt8[]	- An array of bytes with prefixes. If a text contains 0xF3 or 0xF5 character
                      then such characters is escaped - prefixed by 0xF3 and 6th bit of the its value is inverted

            UInt8	- Blob terminator - character with 0xF5 code

            Returned value:

            S_OK - if string successfully decoded to the output buffer
            E_FAIL - if parameters cause the incorrectness

        */
        //HRESULT GetStringW(const byte *pSrc, int cbSrc, char *Value, int cbValue, long *BytesWritten);
        string GetStringUnicode(byte[] pSrc, int offset, out int BytesWritten)
        {
            byte[] Value = new Byte[100000];
            //The number of bytes of pSrc consumed to initialize Value
            BytesWritten = 0;
            //The max length of the buffer pSrc
            int cbSrc = pSrc.Length - offset;
            //The max length of the buffer Value
            int cbValue = Value.Length;
            int totalSize = 0;
            int stringSize = 0;
            byte flag;


            // check argumnets
            if (pSrc == null || Value == null || cbValue < 1)
                throw new Exception("ERV_INVALID_PARAM");

            do
            {
                totalSize += 4;
                // check the size of the source buffer
                if (totalSize > cbSrc)// unexpected end of the source buffer
                    throw new Exception("ERV_INVALID_PARAM");

                flag = pSrc[offset++];
                if ((flag & UNICODE_FLAG) == 0)// ANSI string
                    throw new Exception("ERV_INVALID_PARAM");
                else
                {
                    // UNICODE
                    UInt16 len;
                    /*	10-14-04 SJK
                    Serious bug! Now that we're working with Palm, which sends back data in big endian format,
                    it is the second byte that is least significant, and that byte is ignored here altogether!
                    The writer knew it is a 2-byte quantity because a few lines below he was incrementing
                    pSrc by 2 to get to the start of text.

                    Another fine example of outsourcing!

                                if (m_bBigEndianByteOrder)
                                {
                                    // big endian
                                    len = *pSrc + ((unsigned)pSrc[1] << 8);
                                }
                                else
                                {
                                    // little endian
                                    len = ((unsigned)*pSrc << 8) + pSrc[1];
                                }
                    */
                    len = bitFormatter.GetUInt16(pSrc, offset);
                    totalSize += len;
                    // check the size of the source buffer
                    if (totalSize > cbSrc)
                    { // unexpected end of the source buffer
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                    // check destination buffer
                    if (stringSize + len >= cbValue - 1)
                    {// incorrect size of the destination buffer
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                    // shift to the start of the encoded text in the Blob
                    offset += 2;
                    // iterate encoded text
                    for (int i = 0; i < len; i++)
                    {
                        byte c = pSrc[offset++];
                        if (c == EOB)
                        {
                            // wrong Blob format
                            throw new Exception("ERV_INVALID_PARAM");
                        }
                        if (c == ESCAPE)
                        {
                            // increase total size
                            totalSize++;
                            // get escaped character
                            c = pSrc[offset++];
                            // restore inverted 6th bit
                            c ^= FLIP_MASK;
                        }
                        // store decoded character
                        Value[stringSize++] = c;
                    }
                    // check for the end of the segment
                    if (pSrc[offset++] != EOB)
                    {
                        // incorrect end of the string's segment
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                }
            }
            while ((flag & CONTINUE_FLAG) != 0);

            BytesWritten = totalSize;

            return bitFormatter.GetStringUnicode(Value, 0, stringSize);
        }
    }
}