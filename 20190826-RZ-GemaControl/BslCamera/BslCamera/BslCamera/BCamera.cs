using PylonC.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BslCamera
{
    /// <summary>
    /// bsl相机类
    /// </summary>
    public class BCamera
    {
        /// <summary>
        /// 转换器
        /// </summary>
        protected PYLON_IMAGE_FORMAT_CONVERTER_HANDLE m_hConverter; /* The format converter is used mainly for coverting color images. It is not used for Mono8 or RGBA8packed images. */
        /// <summary>
        /// 转换器的buffer
        /// </summary>
        protected Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> m_convertedBuffers; /* Holds handles and buffers used for converted images. It is not used for Mono8 or RGBA8packed images.*/
        /// <summary>
        /// 相机设备句柄
        /// </summary>
        protected PYLON_DEVICE_HANDLE m_hDevice;           /* Handle for the pylon device. */
        /// <summary>
        /// 采集句柄
        /// </summary>
        protected PYLON_STREAMGRABBER_HANDLE m_hGrabber;   /* Handle for the pylon stream grabber. */
        /// <summary>
        /// 移除回调
        /// </summary>
        protected PYLON_DEVICECALLBACK_HANDLE m_hRemovalCallback;    /* Required for deregistering the callback. */
        /// <summary>
        /// 等待
        /// </summary>
        protected PYLON_WAITOBJECT_HANDLE m_hWait;         /* Handle used for waiting for a grab to be finished. */
        /// <summary>
        /// 相机拍摄数据
        /// </summary>
        protected Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> m_buffers; /* Holds handles and buffers used for grabbing. */
        /// <summary>
        /// 相机回调
        /// </summary>
        protected DeviceCallbackHandler m_callbackHandler; /* Handles callbacks from a device .*/
        /// <summary>
        /// 构造函数
        /// </summary>
        public BCamera()
        {
            m_buffers = new Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>>();
            /* Create handles. */
            m_hGrabber = new PYLON_STREAMGRABBER_HANDLE();
            m_hDevice = new PYLON_DEVICE_HANDLE();
            m_hRemovalCallback = new PYLON_DEVICECALLBACK_HANDLE();
            m_hConverter = new PYLON_IMAGE_FORMAT_CONVERTER_HANDLE();
            /* Create callback handler and attach the method. */
            m_callbackHandler = new DeviceCallbackHandler();
            m_callbackHandler.CallbackEvent += new DeviceCallbackHandler.DeviceCallback(RemovalCallbackHandler);
        }
        #region public
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsOpen { get; set; }
        /// <summary>
        /// 获取最后相机的错误
        /// </summary>
        public string LastError
        {
            get
            {
                string lastErrorMessage = GenApi.GetLastErrorMessage();
                string lastErrorDetail = GenApi.GetLastErrorDetail();
                string lastErrorText = lastErrorMessage;
                if (lastErrorDetail.Length > 0)
                {
                    lastErrorText += "\n\nDetails:\n";
                }
                lastErrorText += lastErrorDetail;
                return lastErrorText;
            }
        }
        #endregion
        #region public method
        /// <summary>
        /// 打开相机
        /// </summary>
        /// <param name="index">相机序号</param>
        /// <returns></returns>
        public bool Open(int index)
        {
            try
            {

                uint count = Pylon.EnumerateDevices();
                //获取相机列表
                List<DeviceEnumerator.Device> list = DeviceEnumerator.EnumerateDevices();
                ///创建设备
                m_hDevice = Pylon.CreateDeviceByIndex((uint)index);

                /* Before using the device, it must be opened. Open it for configuring
                parameters and for grabbing images. */
                Pylon.DeviceOpen(m_hDevice, Pylon.cPylonAccessModeControl | Pylon.cPylonAccessModeStream);

                /* Register the callback function. */
                m_hRemovalCallback = Pylon.DeviceRegisterRemovalCallback(m_hDevice, m_callbackHandler);

                /* For GigE cameras, we recommend increasing the packet size for better
                   performance. When the network adapter supports jumbo frames, set the packet
                   size to a value > 1500, e.g., to 8192. In this sample, we only set the packet size
                   to 1500. */
                /* ... Check first to see if the GigE camera packet size parameter is supported and if it is writable. */
                if (Pylon.DeviceFeatureIsWritable(m_hDevice, "GevSCPSPacketSize"))
                {
                    /* ... The device supports the packet size feature. Set a value. */
                    Pylon.DeviceSetIntegerFeature(m_hDevice, "GevSCPSPacketSize", 1500);
                }

                /* The sample does not work in chunk mode. It must be disabled. */
                if (Pylon.DeviceFeatureIsWritable(m_hDevice, "ChunkModeActive"))
                {
                    /* Disable the chunk mode. */
                    Pylon.DeviceSetBooleanFeature(m_hDevice, "ChunkModeActive", false);
                }

                /* Disable acquisition start trigger if available. */
                if (Pylon.DeviceFeatureIsAvailable(m_hDevice, "EnumEntry_TriggerSelector_AcquisitionStart"))
                {
                    Pylon.DeviceFeatureFromString(m_hDevice, "TriggerSelector", "AcquisitionStart");
                    Pylon.DeviceFeatureFromString(m_hDevice, "TriggerMode", "Off");
                }

                /* Disable frame burst start trigger if available */
                if (Pylon.DeviceFeatureIsAvailable(m_hDevice, "EnumEntry_TriggerSelector_FrameBurstStart"))
                {
                    Pylon.DeviceFeatureFromString(m_hDevice, "TriggerSelector", "FrameBurstStart");
                    Pylon.DeviceFeatureFromString(m_hDevice, "TriggerMode", "Off");
                }

                /* Disable frame start trigger if available. */
                if (Pylon.DeviceFeatureIsAvailable(m_hDevice, "EnumEntry_TriggerSelector_FrameStart"))
                {
                    Pylon.DeviceFeatureFromString(m_hDevice, "TriggerSelector", "FrameStart");
                    Pylon.DeviceFeatureFromString(m_hDevice, "TriggerMode", "Off");
                }

                /* Image grabbing is done using a stream grabber.
                  A device may be able to provide different streams. A separate stream grabber must
                  be used for each stream. In this sample, we create a stream grabber for the default
                  stream, i.e., the first stream ( index == 0 ).
                  */

                /* Get the number of streams supported by the device and the transport layer. */
                if (Pylon.DeviceGetNumStreamGrabberChannels(m_hDevice) < 1)
                {
                    throw new Exception("The transport layer doesn't support image streams.");
                }

                /* Create and open a stream grabber for the first channel. */
                m_hGrabber = Pylon.DeviceGetStreamGrabber(m_hDevice, 0);
                Pylon.StreamGrabberOpen(m_hGrabber);

                /* Get a handle for the stream grabber's wait object. The wait object
                   allows waiting for m_buffers to be filled with grabbed data. */
                m_hWait = Pylon.StreamGrabberGetWaitObject(m_hGrabber);

                IsOpen = true;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("相机异常 : {0}", LastError);
                this.Close();
                return false;
            }

        }
        /// <summary>
        /// 关闭相机
        /// </summary>
        public void Close()
        {
            if (m_hGrabber.IsValid)
            {
                /* Try to close the stream grabber. */
                try
                {

                    Pylon.StreamGrabberClose(m_hGrabber);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            if (m_hDevice.IsValid)
            {
                /* Try to deregister the removal callback. */
                try
                {
                    if (m_hRemovalCallback.IsValid)
                    {
                        Pylon.DeviceDeregisterRemovalCallback(m_hDevice, m_hRemovalCallback);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                /* Try to close the device. */
                try
                {
                    /* ... Close and release the pylon device. */
                    if (Pylon.DeviceIsOpen(m_hDevice))
                    {
                        Pylon.DeviceClose(m_hDevice);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                /* Try to destroy the device. */
                try
                {
                    Pylon.DestroyDevice(m_hDevice);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            m_hGrabber.SetInvalid();
            m_hRemovalCallback.SetInvalid();
            m_hDevice.SetInvalid();
        }
        /// <summary>
        /// 拍摄一张图片
        /// </summary>
        /// <returns></returns>
        public Bitmap Snap()
        {
            Bitmap bitmap = null;
            try
            {
                /* Set up everything needed for grabbing. */
                SetupGrab(true);
                /* Wait for the next buffer to be filled. Wait up to 15000 ms. */
                Pylon.WaitObjectWait(m_hWait, 15000);
                PylonGrabResult_t grabResult; /* Stores the result of a grab operation. */
                                              /* Since the wait operation was successful, the result of at least one grab
                                                  operation is available. Retrieve it. */
                if (!Pylon.StreamGrabberRetrieveResult(m_hGrabber, out grabResult))
                {
                    /* Oops. No grab result available? We should never have reached this point.
                        Since the wait operation above returned without a timeout, a grab result
                        should be available. */
                    throw new Exception("Failed to retrieve a grab result.");
                }
                /* Check to see if the image was grabbed successfully. */
                if (grabResult.Status == EPylonGrabStatus.Grabbed)
                {
                    bitmap = EnqueueTakenImage(grabResult);
                }
                else if (grabResult.Status == EPylonGrabStatus.Failed)
                {
                    ///采集图像失败
                    throw new Exception(string.Format("A grab failure occurred. See the method ImageProvider::Grab for more information. The error code is {0:X08}.", grabResult.ErrorCode));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("相机异常 : {0}", LastError);
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                /* Tear down everything needed for grabbing. */
                CleanUpGrab();
            }
            return bitmap;
        }
        #endregion
        #region protect method
        /// <summary>
        /// 相机被拔出移除回调
        /// </summary>
        /// <param name="hDev"></param>
        private void RemovalCallbackHandler(PYLON_DEVICE_HANDLE hDev)
        {
            IsOpen = false;
        }
        /// <summary>
        /// 启动采集
        /// </summary>
        /// <param name="isOneFrame"></param>
        protected void SetupGrab(bool isOneFrame)
        {
            uint m_numberOfBuffersUsed = 5;
            /* Set the acquisition mode */
            if (isOneFrame)
            {
                /* We will use the single frame mode, to take one image. */
                Pylon.DeviceFeatureFromString(m_hDevice, "AcquisitionMode", "SingleFrame");
            }
            else
            {
                /* We will use the Continuous frame mode, i.e., the camera delivers
                images continuously. */
                Pylon.DeviceFeatureFromString(m_hDevice, "AcquisitionMode", "Continuous");
            }

            /* Clear the grab buffers to assure proper operation (because they may
             still be filled if the last grab has thrown an exception). */
            foreach (KeyValuePair<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> pair in m_buffers)
            {
                pair.Value.Dispose();
            }
            m_buffers.Clear();

            /* Determine the required size of the grab buffer. */
            uint payloadSize = checked((uint)Pylon.DeviceGetIntegerFeature(m_hDevice, "PayloadSize"));

            /* We must tell the stream grabber the number and size of the m_buffers
                we are using. */
            /* .. We will not use more than NUM_m_buffers for grabbing. */
            Pylon.StreamGrabberSetMaxNumBuffer(m_hGrabber, m_numberOfBuffersUsed);

            /* .. We will not use m_buffers bigger than payloadSize bytes. */
            Pylon.StreamGrabberSetMaxBufferSize(m_hGrabber, payloadSize);

            /*  Allocate the resources required for grabbing. After this, critical parameters
                that impact the payload size must not be changed until FinishGrab() is called. */
            Pylon.StreamGrabberPrepareGrab(m_hGrabber);

            /* Before using the m_buffers for grabbing, they must be registered at
               the stream grabber. For each buffer registered, a buffer handle
               is returned. After registering, these handles are used instead of the
               buffer objects pointers. The buffer objects are held in a dictionary,
               that provides access to the buffer using a handle as key.
             */
            for (uint i = 0; i < m_numberOfBuffersUsed; ++i)
            {
                PylonBuffer<Byte> buffer = new PylonBuffer<byte>(payloadSize, true);
                PYLON_STREAMBUFFER_HANDLE handle = Pylon.StreamGrabberRegisterBuffer(m_hGrabber, ref buffer);
                m_buffers.Add(handle, buffer);
            }

            /* Feed the m_buffers into the stream grabber's input queue. For each buffer, the API
               allows passing in an integer as additional context information. This integer
               will be returned unchanged when the grab is finished. In our example, we use the index of the
               buffer as context information. */
            foreach (KeyValuePair<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> pair in m_buffers)
            {
                Pylon.StreamGrabberQueueBuffer(m_hGrabber, pair.Key, 0);
            }

            /* The stream grabber is now prepared. As soon the camera starts acquiring images,
               the image data will be grabbed into the provided m_buffers.  */

            /* Set the handle of the image converter invalid to assure proper operation (because it may
             still be valid if the last grab has thrown an exception). */
            m_hConverter.SetInvalid();

            /* Let the camera acquire images. */
            Pylon.DeviceExecuteCommandFeature(m_hDevice, "AcquisitionStart");
        }

        /// <summary>
        /// 关闭采集
        /// </summary>
        protected void CleanUpGrab()
        {
            /*  ... Stop the camera. */
            Pylon.DeviceExecuteCommandFeature(m_hDevice, "AcquisitionStop");

            /* Destroy the format converter if one was used. */
            if (m_hConverter.IsValid)
            {
                /* Destroy the converter. */
                Pylon.ImageFormatConverterDestroy(m_hConverter);
                /* Set the handle invalid. The next grab cycle may not need a converter. */
                m_hConverter.SetInvalid();
                /* Release the converted image buffers. */
                foreach (KeyValuePair<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> pair in m_convertedBuffers)
                {
                    pair.Value.Dispose();
                }
                m_convertedBuffers = null;
            }

            /* ... We must issue a cancel call to ensure that all pending m_buffers are put into the
               stream grabber's output queue. */
            Pylon.StreamGrabberCancelGrab(m_hGrabber);

            /* ... The m_buffers can now be retrieved from the stream grabber. */
            {
                bool isReady; /* Used as an output parameter. */
                do
                {
                    PylonGrabResult_t grabResult;  /* Stores the result of a grab operation. */
                    isReady = Pylon.StreamGrabberRetrieveResult(m_hGrabber, out grabResult);

                } while (isReady);
            }

            /* ... When all m_buffers are retrieved from the stream grabber, they can be deregistered.
                   After deregistering the m_buffers, it is safe to free the memory. */

            foreach (KeyValuePair<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> pair in m_buffers)
            {
                Pylon.StreamGrabberDeregisterBuffer(m_hGrabber, pair.Key);
            }

            /* The buffers can now be released. */
            foreach (KeyValuePair<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<Byte>> pair in m_buffers)
            {
                pair.Value.Dispose();
            }
            m_buffers.Clear();

            /* ... Release grabbing related resources. */
            Pylon.StreamGrabberFinishGrab(m_hGrabber);

            /* After calling PylonStreamGrabberFinishGrab(), parameters that impact the payload size (e.g.,
            the AOI width and height parameters) are unlocked and can be modified again. */
        }

        /// <summary>
        /// 尝试获取图片
        /// </summary>
        /// <param name="grabResult"></param>
        /// <returns></returns>
        protected Bitmap EnqueueTakenImage(PylonGrabResult_t grabResult)
        {
            bool m_converterOutputFormatIsColor = false;
            //填充拍摄结果数据的引用
            PylonBuffer<Byte> buffer;

            //填充数据
            if (!m_buffers.TryGetValue(grabResult.hBuffer, out buffer))
            {
                /* Oops. No buffer available? We should never have reached this point. Since all buffers are
                   in the dictionary. */
                throw new Exception("Failed to find the buffer associated with the handle returned in grab result.");
            }
            //创建一个采集结果
            GrabResult newGrabResultInternal = new GrabResult();
            newGrabResultInternal.Handle = grabResult.hBuffer; /* Add the handle to requeue the buffer in the stream grabber queue. */

            //判断相机的图像输出格式，并且获取图像数据
            if (grabResult.PixelType == EPylonPixelType.PixelType_Mono8 || grabResult.PixelType == EPylonPixelType.PixelType_RGBA8packed)
            {
                newGrabResultInternal.ImageData = new ImageRawData(grabResult.SizeX, grabResult.SizeY, buffer.Array, grabResult.PixelType == EPylonPixelType.PixelType_RGBA8packed ? PixelFormat.Format32bppArgb : PixelFormat.Format8bppIndexed);
            }
            else
            {
                //创建一个对应格式的图像转换器
                if (!m_hConverter.IsValid)
                {
                    m_convertedBuffers = new Dictionary<PYLON_STREAMBUFFER_HANDLE, PylonBuffer<byte>>(); /* Create a new dictionary for the converted buffers. */
                    m_hConverter = Pylon.ImageFormatConverterCreate(); /* Create the converter. */
                    m_converterOutputFormatIsColor = !Pylon.IsMono(grabResult.PixelType) || Pylon.IsBayer(grabResult.PixelType);
                }
                /* Reference to the buffer attached to the grab result handle. */
                PylonBuffer<Byte> convertedBuffer = null;
                /* Look up if a buffer is already attached to the handle. */
                bool bufferListed = m_convertedBuffers.TryGetValue(grabResult.hBuffer, out convertedBuffer);
                /* Perform the conversion. If the buffer is null a new one is automatically created. */
                Pylon.ImageFormatConverterSetOutputPixelFormat(m_hConverter, m_converterOutputFormatIsColor ? EPylonPixelType.PixelType_BGRA8packed : EPylonPixelType.PixelType_Mono8);
                Pylon.ImageFormatConverterConvert(m_hConverter, ref convertedBuffer, buffer, grabResult.PixelType, (uint)grabResult.SizeX, (uint)grabResult.SizeY, (uint)grabResult.PaddingX, EPylonImageOrientation.ImageOrientation_TopDown);
                if (!bufferListed) /* A new buffer has been created. Add it to the dictionary. */
                {
                    m_convertedBuffers.Add(grabResult.hBuffer, convertedBuffer);
                }
                /* Add the image data. */
                newGrabResultInternal.ImageData = new ImageRawData(grabResult.SizeX, grabResult.SizeY, convertedBuffer.Array, m_converterOutputFormatIsColor ? PixelFormat.Format32bppArgb : PixelFormat.Format8bppIndexed);
            }
            ///如果正确获取到数据
            if (newGrabResultInternal.ImageData != null)
            {
                return newGrabResultInternal.ImageData.ConvertImageRawDataToBitmap();
            }
            else
            {
                throw new Exception("未正常取到数据!");
            }
        }
        #endregion
        #region Static
        /// <summary>
        /// 释放相机总资源
        /// </summary>
        public static void Terminate()
        {
            Pylon.Terminate();
        }
        /// <summary>
        /// 静态初始化
        /// </summary>
        public static void StaticInit()
        {
            Pylon.Terminate();
            Pylon.Initialize();
            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "1000");
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            StaticInit();
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.Close();
            }
            catch (Exception)
            {

            }
        }
        #endregion
    }
}
