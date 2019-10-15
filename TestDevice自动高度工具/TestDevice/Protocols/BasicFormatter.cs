// Copyright 2010 Ningbo Yichang Communication Equipment Co.,Ltd.
// Coded by chuan'gen http://chuangen.name.

using System;
using System.Collections.Generic;
using System.Text;
using Mobot.Utils;

namespace Protocols
{
    /// <summary>
    /// Basic Command Formatter.
    /// </summary>
    public partial class BasicFormatter
    {
        // Flags:
        const int EOB = 0xF5;
        // don't move this definition to any CommLibDefsXXX.h
        const int ESCAPE = 0xF6;

        const int FLIP_MASK = 0x40;

        const int UNICODE_FLAG = 0x40;
        const int CONTINUE_FLAG = 0x80;

        public bool IsLittleEndian
        {
            get { return bitFormatter.IsLittleEndian; }
        }
        readonly BitFormatter bitFormatter = null;
        public BasicFormatter(bool isLittleEndian)
        {
            bitFormatter = new BitFormatter(isLittleEndian);
        }

        public byte[] GetArray(byte[] value, int startIndex, out int BytesConsumed)
        {
            byte[] array = new byte[value.Length - startIndex];
            if (value == null)
                throw new ArgumentNullException("value", "value 不能为空。");
            if (startIndex < 0)
                throw new ArgumentException("startIndex", "startIndex 不能小于 0。");

            BytesConsumed = 0;

            int offset = startIndex;
            int BytesWritten = 0;
            while (offset < value.Length && value[offset] != EOB)
	        {
                byte ch = value[offset++];
                if (ch == ESCAPE)
		        {
                    ch = value[offset++];
                    if (offset >= value.Length || ch == ESCAPE || ch == EOB)
                    {
                        // incorrect buffer format
                        throw new Exception("ERV_INVALID_FORMAT");
			        }
                    ch ^= FLIP_MASK;
		        }
                array[BytesWritten++] = ch;
	        }

            BytesConsumed = offset - startIndex + 1;//因为有 EOB

            Array.Resize<byte>(ref array, BytesWritten);

            return array;
        }

        //HRESULT FormatArray(byte *pDest, int size, byte *pSrc, int cbSrc, long *BytesWritten);
        public int FormatArray(byte[] pDest, int offset, byte[] pSrc)
        {
            int size = pDest.Length - offset;
            if (pDest == null || pSrc == null || pSrc.Length >= size)
                throw new ArgumentException("ERV_INVALID_PARAM");


            int i = 0, consumed = 1;
            int sourceIndex = 0;
            // process array
            while (i < pSrc.Length)
            {
                byte c = pSrc[sourceIndex++];
                if (c == ESCAPE || c == EOB)
                {
                    pDest[offset++] = ESCAPE;
                    consumed++;
                    if (pSrc.Length + consumed > size)
                        throw new ArgumentException("ERV_NO_ROOM");

                    c ^= FLIP_MASK;
                }
                pDest[offset++] = c;
                i++;
            }
            pDest[offset] = EOB;
            int BytesWritten = pSrc.Length + consumed;

            return BytesWritten;
        }


    }
}
