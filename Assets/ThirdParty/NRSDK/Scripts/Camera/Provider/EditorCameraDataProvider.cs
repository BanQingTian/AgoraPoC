/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

namespace NRKernal
{
    using System;

    public class EditorCameraDataProvider : ICameraDataProvider
    {
        public bool Create()
        {
            return true;
        }

        public bool DestroyImage(ulong imageHandle)
        {
            return true;
        }

        public ulong GetHMDTimeNanos(ulong imageHandle, int eye)
        {
            return 0;
        }

        public bool GetRawData(ulong imageHandle, int eye, ref IntPtr ptr, ref int size)
        {
            return true;
        }

        public NativeResolution GetResolution(ulong imageHandle, int eye)
        {
            return new NativeResolution(1280, 720);
        }

        public bool Release()
        {
            return true;
        }

        public bool SetCaptureCallback(CameraImageCallback callback, ulong userdata = 0)
        {
            return true;
        }

        public bool SetImageFormat(CameraImageFormat format)
        {
            return true;
        }

        public bool StartCapture()
        {
            return true;
        }

        public bool StopCapture()
        {
            return true;
        }
    }
}
