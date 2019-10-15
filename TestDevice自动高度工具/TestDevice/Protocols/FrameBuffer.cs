
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocols
{
    public class FrameBuffer
    {
        Queue<byte[]> frames = new Queue<byte[]>();

        byte[] readBuffer = null;
        int bytesToRead = 0;
        public FrameBuffer(int bufferSize)
        {
            readBuffer = new byte[bufferSize];
        }
        /// <summary>
        /// 最短的帧长。
        /// </summary>
        const int MinFrameSize = 6;
        /// <summary>
        /// 0xF1 The Start of Message
        /// </summary>
        bool hasSOM = false;

        /// <summary>
        ///  获取或设置 System.IO.Ports.SerialPort 输入缓冲区的大小。
        /// </summary>
        /// <value>缓冲区大小。默认值为 4096。</value>
        public int ReadBufferSize
        {
            get { return readBuffer.Length; }
        }
        public byte[] ReadFrame()
        {
            return frames.Dequeue();
        }
        public bool ReceivedFrame
        {
            get { return (frames.Count > 0); }
        }

        public void Received(byte data)
        {
            //如果未开始，则期待 0xF1
            if (!hasSOM)
            {
                if (data == 0xF1)
                {//收到开始标识
                    bytesToRead = 0;
                    hasSOM = true;

                    readBuffer[bytesToRead++] = data;
                }

                return;
            }

            if (data == 0xF1)
            {//收到开始标识，重新开始
                bytesToRead = 0;
                hasSOM = true;

                readBuffer[bytesToRead++] = data;
                return;
            }
            if (data == 0xF2)
            {//收到结束标识
                readBuffer[bytesToRead++] = data;
                FinishFrame();
                //重置
                hasSOM = false;
                return;
            }
            //到达这里，则都是普通字符
            readBuffer[bytesToRead++] = data;
        }
        
        /// <summary>
        /// 已经收集了一帧数据。
        /// </summary>
        /// <param name="data"></param>
        void FinishFrame()
        {
            if (bytesToRead >= MinFrameSize)
            {//帧长足够
                byte[] frame = new byte[bytesToRead];
                Array.Copy(readBuffer, frame, frame.Length);
                frames.Enqueue(frame);
            }
            bytesToRead = 0;
        }
    }
}
