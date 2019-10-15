
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocols
{
    /// <summary>
    /// Transfer protocol.
    /// </summary>
    public partial class TransferProtocol
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 0xF1 The Start of Message
        /// </summary>
        const int SOM = 0xF1;
        /// <summary>
        /// 0xF2 The End of Message
        /// </summary>
        const int EOM = 0xF2;
        /// <summary>
        /// 0xF4 The start of verification.
        /// </summary>
        const int SOV = 0xF4;
        const int EOB = 0xF5;

        const int FLIP_MASK = 0x40;
        /// <summary>
        /// 0xF3 The escape character.
        /// The next character has the same value as control character, but bit6 is flipped.
        /// 0xF1 becomes 0xB1, 0xF2 becomes 0xB2, etc.
        /// </summary>
        const int ESCAPE = 0xF3;


        public static byte CalcCheckSum(byte[] pSrc, int start, int size)
        {
            byte result = 0;

            for (int i = start; i < size; i++)
                result += pSrc[i];

            return result;
        }



        /// <summary>
        /// Returns the size for buffer for transport
        /// public static HRESULT CalcBufferSize(const byte *pSrc, int size, long *needSize);
        /// </summary>
        /// <param name="pSrc">pointer to the payload</param>
        /// <param name="offset"></param>
        /// <returns>calculated size</returns>
        public static int CalcBufferSize(byte[] pSrc, int offset)
        {
            int needSize = 0;
            //size of the payload
            int size = pSrc.Length - offset;
            // check parameters
            if (pSrc == null || size < 0)
                throw new Exception("ERV_INVALID_PARAM");

            // add room for control bytes
            int s = size + 7;

            // add room for each prefix for escape
            for (int i = 0; i < size; i++)
                if (pSrc[i] >= 0xF1 && pSrc[i] <= 0xF4)
                    s++;

            needSize = s;

            return needSize;
        }

        /// <summary>
        /// Prepares the buffer with the given payload
        /// public static HRESULT PrepareBuffer(byte *pDest, int size, const byte *pSrc, int cbSrc);
        /// </summary>
        /// <param name="pDest">buffer for transport protocol to store payload there</param>
        /// <param name="offset"></param>
        /// <param name="pSrc">pointer to the payload</param>
        /// <param name="cbSrc">size of the payload</param>
        /// <returns></returns>
        /// <remarks>
        /// Format of the buffer:
        /// 
        /// UInt8	SOM
        /// Array of bytes	payload
        /// UInt8	SOV
        /// UInt8	MessageID (may be escaped -> +UInt8)
        /// UInt8	CheckSum (may be escaped -> +UInt8)
        /// UInt8	EOM
        /// 
        /// Format of the payload (BLOB2):
        /// 
        /// UInt8[]	- message data with escaped control characters
        /// 
        /// escaped characters:	0xF1, 0xF2, 0xF3 (ESCAPE), 0xF4
        /// </remarks>
        public static int PrepareBuffer(byte[] pDest, int offset, byte[] pSrc, int cbSrc, byte counter)
        {
            //maximum allowable size for transport buffer
            int size = pDest.Length - offset;
            // check parameters
            if (pDest == null || pSrc == null || size < 5 + cbSrc)
                throw new Exception("ERV_INVALID_PARAM");

            // save the start of the buffer
            int pStart = offset;
            int srcSize = cbSrc;
            int sourceIndex = 0;
            
            // write SOM
            pDest[offset++] = SOM;
            // write Blob
            for (int i = 0; i < srcSize; i++)
            {
                byte ch = pSrc[sourceIndex++];
                // check for control characters inside the Payload
                if (ch >= 0xF1 && ch <= 0xF4)
                {
                    // write ESCAPE
                    pDest[offset++] = ESCAPE;
                    // invert 6th bit
                    ch ^= FLIP_MASK;
                    // increase total payload's size
                    cbSrc++;
                    // check the size of the buffer
                    if (size < 5 + cbSrc)
                        throw new Exception("ERV_INVALID_PARAM");
                }
                // write character
                pDest[offset++] = ch;
            }

            // write SOV;
            pDest[offset++] = SOV;

            // write message ID
            if (counter >= 0xF1 && counter <= 0xF4)
            {
                pDest[offset++] = ESCAPE;
                counter ^= FLIP_MASK;
                cbSrc++;
            }
            pDest[offset++] = counter;
            // write CheckSum
            byte sum = CalcCheckSum(pDest, pStart, cbSrc + 3);
            if (sum >= 0xF1 && sum <= 0xF4)
            {
                pDest[offset++] = ESCAPE;
                sum ^= FLIP_MASK;
            }
            pDest[offset++] = sum;
            // write EOM
            pDest[offset] = EOM;

            return offset+1 - pStart;
        }

        /// <summary>
        /// Retruns the payload size for get it from the transport buffer
        /// public static HRESULT CalcPayloadSize(byte *pSrc, int size, long *needSize);
        /// </summary>
        /// <param name="pSrc">pointer to the transport buffer</param>
        /// <returns>size of the payload of the given transport buffer</returns>
        /// <remarks>
        /// 
        ////Format of the buffer:
        ///
        /// UInt8	SOM
        /// Array of bytes	payload
        /// UInt8	SOV
        /// UInt8	MessageID (may be escaped -> +UInt8)
        /// UInt8	CheckSum (may be escaped -> +UInt8)
        /// UInt8	EOM
        ///
        /// Format of the payload (BLOB2):
        ///
        /// UInt8[]	- message data with escaped control characters
        ///
        /// escaped characters:	0xF1, 0xF2, 0xF3 (ESCAPE), 0xF4
        /// </remarks>
        public static int CalcPayloadSize(byte[] pSrc)
        {
            //maximum possible size of the transport buffer
            int size = pSrc.Length;

            log.Info("CTransferProtocol::CalcPayloadSize: START.");
            if (pSrc == null || pSrc.Length < 5 || pSrc[0] != SOM)
            {
                // invalid transport buffer content
                log.Info("CTransferProtocol::CalcPayloadSize: FINISH Failed 1.");
                throw new Exception("ERV_INVALID_PARAM");
            }

            int needSize = 0;

            int result = 0;
            int i = 1;
            while (i < size - 1 && pSrc[i] != SOV)
            {
                switch (pSrc[i++])
                {
                    case ESCAPE:
                        break;
                    case SOM:
                    case EOM:
                        // invalid payload format
                        log.Info("CTransferProtocol::CalcPayloadSize: FINISH Failed 2.");
                        throw new Exception("ERV_INVALID_PARAM");
                    //break;
                    default:
                        result++;
                        break;
                }
            }

            if (i > size - 4)
            {
                // incorrect buffer format
                log.Info("CTransferProtocol::CalcPayloadSize: FINISH Failed 3.");
                throw new Exception("ERV_INVALID_PARAM");
            }

            if (pSrc[i + 1] == ESCAPE)
            {
                // ID is escaped
                if (pSrc[i + 3] == ESCAPE)
                {
                    // Checksum is escaped
                    if (i > size - 6 || pSrc[i + 5] != EOM ||
                        CalcCheckSum(pSrc, 0, i + 3) != (pSrc[i + 4] ^ FLIP_MASK))
                    {
                        // incorrect buffer format
                        log.Info("CTransferProtocol::CalcPayloadSize: FINISH Failed 4.");
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                }
                else
                {
                    // Checksum is not escaped
                    if (i > size - 5 || pSrc[i + 4] != EOM || CalcCheckSum(pSrc, 0, i + 3) != pSrc[i + 3])
                    {
                        // incorrect buffer format
                        log.Info("CTransferProtocol::CalcPayloadSize: FINISH Failed 5.");
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                }
            }
            else
            {
                // ID is not escaped
                if (pSrc[i + 2] == ESCAPE)
                {
                    // Checksum is escaped
                    if (i > size - 5 || pSrc[i + 4] != EOM ||
                        CalcCheckSum(pSrc, 0, i + 2) != (pSrc[i + 3] ^ FLIP_MASK))
                    {
                        // incorrect buffer format
                        log.Info("CTransferProtocol::CalcPayloadSize: FINISH Failed 6.");
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                }
                else
                {
                    // Checksum is not escaped
                    if (pSrc[i + 3] != EOM || CalcCheckSum(pSrc, 0, i + 2) != pSrc[i + 2])
                    {
                        // incorrect buffer format
                        log.Info("CTransferProtocol::CalcPayloadSize: FINISH Failed 7.");
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                }
            }

            needSize = result;

            log.Info("CTransferProtocol::CalcPayloadSize: FINISH Success.");

            return needSize;
        }
        /// <summary>
        /// Stores the payload from the transport buffer to the given buffer
        /// public static HRESULT GetPayloadTo(byte *pDest, int size, long *consumedSize, const byte *pSrc, int cbSrc);
        /// </summary>
        /// <param name="pDest">pointer to buffer to store the payload</param>
        /// <param name="offset">maximum possible size of the destination buffer</param>
        /// <param name="consumedSize">number of bytes in the destination buffer</param>
        /// <param name="pSrc">pointer to transport buffer</param>
        /// <param name="cbSrc">maximum size of the transport buffer</param>
        /// <returns></returns>
        /// <remarks>
        /// Format of the buffer:
        ///
        /// UInt8	SOM
        /// Array Of bytes	payload
        /// UInt8	SOV
        /// UInt8	MessageID (may be escaped -> +UInt8)
        /// UInt8	CheckSum (may be escaped -> + UInt8)
        /// UInt8	EOM
        ///
        /// Format of the payload (BLOB2):
        ///
        /// UInt8[]	- message data with escaped control characters
        ///
        /// escaped characters:	0xF1, 0xF2, 0xF3 (ESCAPE), 0xF4
        /// </remarks>
        public static bool GetPayloadFrom(byte[] payload, int offset, out byte counter, out long consumedSize, byte[] pSrc, int cbSrc)
        {
            int size = payload.Length - offset;
            log.InfoFormat("CTransferProtocol::GetPayloadTo: START. size=%i,cbSrc=%i.\r\n", size, cbSrc);

            // check parameters
            if (payload == null || pSrc == null || cbSrc < 5 || pSrc[0] != SOM)
            {
                // invalid transport buffer content
                throw new Exception("ERV_INVALID_PARAM");
            }
            // init size
            long total = 0;
            int index = 1;
            // init byte for check
            byte ch = pSrc[index];
            while (ch != SOV)
            {
                index++;
                if (index > cbSrc - 4)
                {
                    // invalid tarnsport buffer format
                    log.Info("CTransferProtocol::GetPayloadTo: FINISH Failed 1.\r\n");
                    throw new Exception("ERV_INVALID_PARAM");
                }
                // check for escape
                if (ch == ESCAPE)
                {
                    ch = (byte)(pSrc[index] ^ FLIP_MASK);
                    index++;
                    if (index > cbSrc - 4 || ch < 0xF1 || ch > 0xF4)
                    {
                        // invalid tarnsport buffer format
                        log.Info("CTransferProtocol::GetPayloadTo: FINISH Failed 2.\r\n");
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                }
                // increase bytes count
                total++;
                // check destination buffer
                if (total > size)
                {
                    log.Info("CTransferProtocol::GetPayloadTo: FINISH Failed 3.\r\n");
                    throw new Exception("ERV_INVALID_PARAM");
                }
                // write byte to the Payload
                payload[offset++] = ch;
                // get next byte
                ch = pSrc[index];
            }

            if (index > cbSrc - 4)
            {
                // incorrect buffer format
                log.Info("CTransferProtocol::GetPayloadTo: FINISH Failed 4.\r\n");
                throw new Exception("ERV_INVALID_PARAM");
            }

            if (pSrc[index + 1] == ESCAPE)
            {
                // ID is escaped
                counter = (byte)(pSrc[index + 2] ^ FLIP_MASK);
                if (pSrc[index + 3] == ESCAPE)
                {
                    // Checksum is escaped
                    if (index > cbSrc - 6 || pSrc[index + 5] != EOM ||
                        CalcCheckSum(pSrc, 0, index + 3) != (pSrc[index + 4] ^ FLIP_MASK))
                    {
                        // incorrect buffer format
                        log.Info("CTransferProtocol::GetPayloadTo: FINISH Failed 5.\r\n");
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                }
                else
                {
                    // Checksum is not escaped
                    if (index > cbSrc - 5 || pSrc[index + 4] != EOM ||
                        CalcCheckSum(pSrc, 0, index + 3) != pSrc[index + 3])
                    {
                        // incorrect buffer format
                        log.Info("CTransferProtocol::GetPayloadTo: FINISH Failed 6.\r\n");
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                }
            }
            else
            {
                // ID is not escaped
                counter = pSrc[index + 1];
                if (pSrc[index + 2] == ESCAPE)
                {
                    // Checksum is escaped
                    if (index > cbSrc - 5 || pSrc[index + 4] != EOM ||
                        CalcCheckSum(pSrc, 0, index + 2) != (pSrc[index + 3] ^ FLIP_MASK))
                    {
                        // incorrect buffer format
                        log.Info("CTransferProtocol::GetPayloadTo: FINISH Failed 7.\r\n");
                        throw new Exception("ERV_INVALID_PARAM");
                    }
                }
                else
                {
                    // Checksum is not escaped
                    if (pSrc[index + 3] != EOM ||
                        CalcCheckSum(pSrc, 0, index + 2) != pSrc[index + 2])
                    {
                        // incorrect buffer format
                        log.Info("CTransferProtocol::GetPayloadTo: FINISH Failed 8.");
                        //throw new Exception("ERV_INVALID_PARAM");
                    }
                }
            }

            consumedSize = total;

            log.Info("CTransferProtocol::GetPayloadTo: FINISH. Success.");

            return true;
        }
    }
}
