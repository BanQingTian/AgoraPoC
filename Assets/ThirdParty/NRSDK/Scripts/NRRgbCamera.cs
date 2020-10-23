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
    using AOT;
    using System;
    using UnityEngine;

    /// <summary>
    /// Manage the life cycle of the entire rgb camera.
    /// </summary>
    internal class NRRgbCamera : NativeCameraProxy
    {
        public new static string ID = "NRRgbCamera";

        public override NativeResolution Resolution
        {
            get
            {
#if !UNITY_EDITOR
                NativeResolution resolution = NRDevice.Instance.NativeHMD.GetEyeResolution((int)NativeEye.RGB);
#else   
                NativeResolution resolution = new NativeResolution(1280, 720);
#endif
                return resolution;
            }
        }

#if !UNITY_EDITOR
        public NRRgbCamera() : base(new NativeCamera()) { }
#else
        public NRRgbCamera() : base(new EditorCameraDataProvider()) { }
#endif

        public override void Initialize()
        {
            base.Initialize();
            RegistCaptureCallback(RGBCameraCapture);
        }

        /// <summary>
        /// RGB camera capture callback for native layer.
        /// Note: Here must be static to avoid function pointer lost in native layer.
        /// </summary>
        /// <param name="camera_handle"></param>
        /// <param name="camera_image_handle"></param>
        /// <param name="userdata"></param>
        [MonoPInvokeCallback(typeof(CameraImageCallback))]
        public static void RGBCameraCapture(UInt64 camera_handle, UInt64 camera_image_handle, UInt64 userdata)
        {
            var controller = CameraProxyFactory.GetInstance(NRRgbCamera.ID);
            if (controller == null)
            {
                Debug.LogError("[CameraController] get controller instance faild.");
                return;
            }
            controller.UpdateFrame(camera_handle, camera_image_handle, userdata);
        }

        public override void UpdateFrame(UInt64 camera_handle, UInt64 camera_image_handle, UInt64 userdata)
        {
            int RawDataSize = 0;
            this.CameraDataProvider.GetRawData(camera_image_handle, (int)NativeEye.RGB, ref this.m_TexturePtr, ref RawDataSize);
            var timestamp = this.CameraDataProvider.GetHMDTimeNanos(camera_image_handle, (int)NativeEye.RGB);
            this.QueueFrame(this.m_TexturePtr, RawDataSize, timestamp);
            this.CameraDataProvider.DestroyImage(camera_image_handle);
        }
    }
}
