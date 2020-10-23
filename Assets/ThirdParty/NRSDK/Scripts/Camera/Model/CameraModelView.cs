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
    using UnityEngine;

    public partial class CameraModelView
    {
        public int Width
        {
            get
            {
                return m_NativeCameraProxy.Resolution.width;
            }
        }

        public int Height
        {
            get
            {
                return m_NativeCameraProxy.Resolution.height;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return m_State == State.Playing;
            }
        }

        public enum State
        {
            Playing,
            Paused,
            Stopped
        }
        private State m_State = State.Stopped;

        public bool DidUpdateThisFrame
        {
            get
            {
                return m_NativeCameraProxy.HasFrame();
            }
        }

        public int FrameCount { get; protected set; }
        protected NativeCameraProxy m_NativeCameraProxy;

        /// <summary>
        /// Use RGB_888 format default.
        /// </summary>
        /// <param name="format"></param>
        public CameraModelView(CameraImageFormat format = CameraImageFormat.RGB_888)
        {
            m_NativeCameraProxy = CameraProxyFactory.CreateRGBCameraProxy();
            m_NativeCameraProxy.Regist(this);
            m_NativeCameraProxy.SetImageFormat(format);

            OnCreated();
        }

        /// <summary>
        /// On texture created.
        /// </summary>
        protected virtual void OnCreated() { }

        public void Play()
        {
            if (m_State == State.Playing)
            {
                return;
            }
            NRKernalUpdater.OnUpdate += UpdateTexture;
            m_NativeCameraProxy.Play();
            m_State = State.Playing;
        }

        public void Pause()
        {
            if (m_State == State.Paused || m_State == State.Stopped)
            {
                return;
            }
            NRKernalUpdater.OnUpdate -= UpdateTexture;
            m_State = State.Paused;
        }

        private void UpdateTexture()
        {
            if (!DidUpdateThisFrame || !IsPlaying)
            {
                return;
            }

            FrameRawData frame = m_NativeCameraProxy.GetFrame();
            if (frame.data == null)
            {
                Debug.LogError("Get camera raw data faild...");
                return;
            }
            FrameCount++;
            OnRawDataUpdate(frame);
        }

        /// <summary>
        /// Load raw texture data.
        /// </summary>
        /// <param name="frame"></param>
        protected virtual void OnRawDataUpdate(FrameRawData frame) { }

        public void Stop()
        {
            if (m_State == State.Stopped)
            {
                return;
            }

            m_NativeCameraProxy.UnRegist(this);
            m_NativeCameraProxy.Stop();
            NRKernalUpdater.OnUpdate -= UpdateTexture;

            FrameCount = 0;
            m_State = State.Stopped;
            this.OnStopped();
        }

        /// <summary>
        /// On texture stopped.
        /// </summary>
        protected virtual void OnStopped() { }
    }
}
