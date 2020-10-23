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
    using System.Runtime.InteropServices;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void CameraImageCallback(UInt64 rgb_camera_handle,
               UInt64 rgb_camera_image_handle, UInt64 userdata);

    public interface ICameraDataProvider
    {
        bool Create();

        bool GetRawData(UInt64 imageHandle, int eye, ref IntPtr ptr, ref int size);

        NativeResolution GetResolution(UInt64 imageHandle, int eye);

        UInt64 GetHMDTimeNanos(UInt64 imageHandle, int eye);

        bool SetCaptureCallback(CameraImageCallback callback, UInt64 userdata = 0);

        bool SetImageFormat(CameraImageFormat format);

        bool StartCapture();

        bool StopCapture();

        bool DestroyImage(UInt64 imageHandle);

        bool Release();
    }
}