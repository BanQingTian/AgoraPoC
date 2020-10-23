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
    using UnityEngine;
    using System.Collections.Generic;

    public class NativeCameraProxy
    {
        public static string ID = "NativeCameraProxy";
        protected ICameraDataProvider CameraDataProvider { get; private set; }
        protected IntPtr m_TexturePtr = IntPtr.Zero;
        protected FrameRawData m_CurrentFrame;
        public virtual NativeResolution Resolution { get; }
        private bool m_IsInitialized = false;
        protected int m_LastFrame = -1;
        protected bool m_IsPlaying = false;
        public bool IsCamPlaying
        {
            get
            {
                return m_IsPlaying;
            }
        }

        public class FixedSizedQueue
        {
            private Queue<FrameRawData> m_Queue = new Queue<FrameRawData>();
            private object m_LockObj = new object();
            private ObjectPool m_ObjectPool;

            public FixedSizedQueue(ObjectPool pool)
            {
                m_ObjectPool = pool;
            }

            public int Limit { get; set; }

            public int Count
            {
                get
                {
                    lock (m_LockObj)
                    {
                        return m_Queue.Count;
                    }
                }
            }

            public void Enqueue(FrameRawData obj)
            {
                lock (m_LockObj)
                {
                    m_Queue.Enqueue(obj);
                    if (m_Queue.Count > Limit)
                    {
                        var frame = m_Queue.Dequeue();
                        m_ObjectPool.Put<FrameRawData>(frame);
                    }
                }

            }

            public FrameRawData Dequeue()
            {
                lock (m_LockObj)
                {
                    var frame = m_Queue.Dequeue();
                    m_ObjectPool.Put<FrameRawData>(frame);
                    return frame;
                }
            }
        }
        protected FixedSizedQueue m_CameraFrames;
        protected ObjectPool FramePool;
        protected List<CameraModelView> m_ActiveTextures;

        protected NativeCameraProxy(ICameraDataProvider provider)
        {
            CameraDataProvider = provider;
            this.Initialize();
        }

        public virtual void Initialize()
        {
            if (m_IsInitialized)
            {
                return;
            }
            NRDebugger.Log("[CameraController] Initialize");
            if (FramePool == null)
            {
                FramePool = new ObjectPool();
                FramePool.InitCount = 10;
            }
            if (m_CameraFrames == null)
            {
                m_CameraFrames = new FixedSizedQueue(FramePool);
                m_CameraFrames.Limit = 5;
            }

            m_ActiveTextures = new List<CameraModelView>();
            CameraDataProvider.Create();
            m_IsInitialized = true;
        }

        protected void RegistCaptureCallback(CameraImageCallback callback)
        {
            CameraDataProvider.SetCaptureCallback(callback);
        }

        public void Regist(CameraModelView tex)
        {
            m_ActiveTextures.Add(tex);
        }

        public void UnRegist(CameraModelView tex)
        {
            int index = -1;
            for (int i = 0; i < m_ActiveTextures.Count; i++)
            {
                if (tex == m_ActiveTextures[i])
                {
                    index = i;
                }
            }
            if (index != -1)
            {
                m_ActiveTextures.RemoveAt(index);
            }
        }

        public void SetImageFormat(CameraImageFormat format)
        {
#if !UNITY_EDITOR
            CameraDataProvider.SetImageFormat(format);
#endif
            NRDebugger.Log("[CameraController] SetImageFormat : " + format.ToString());
        }

        /// <summary>
        /// Start to play camera.
        /// </summary>
        public void Play()
        {
            if (!m_IsInitialized)
            {
                Initialize();
            }
            if (m_IsPlaying)
            {
                return;
            }
            NRDebugger.Log("[CameraController] Start to play");
#if !UNITY_EDITOR
            CameraDataProvider.StartCapture();
#endif
            m_IsPlaying = true;
        }

        public bool HasFrame()
        {
            return m_IsPlaying && (m_CameraFrames.Count > 0 || m_CurrentFrame.data != null);
        }

        public FrameRawData GetFrame()
        {
            if (Time.frameCount != m_LastFrame && m_CameraFrames.Count > 0)
            {
                m_CurrentFrame = m_CameraFrames.Dequeue();
                m_LastFrame = Time.frameCount;
            }

            return m_CurrentFrame;
        }

        public virtual void UpdateFrame(UInt64 camera_handle, UInt64 camera_image_handle, UInt64 userdata) { }

        protected void QueueFrame(IntPtr textureptr, int size, UInt64 timestamp)
        {
            if (!m_IsPlaying)
            {
                NRDebugger.LogError("camera was not stopped properly, it still sending data.");
                return;
            }
            FrameRawData frame = FramePool.Get<FrameRawData>();
            bool result = FrameRawData.MakeSafe(m_TexturePtr, size, timestamp, ref frame);
            if (result)
            {
                m_CameraFrames.Enqueue(frame);
            }
            else
            {
                FramePool.Put<FrameRawData>(frame);
            }
        }

        /// <summary>
        /// Stop the camera.
        /// </summary>
        public virtual void Stop()
        {
            if (!m_IsPlaying)
            {
                return;
            }
            NRDebugger.Log("[CameraController] Start to Stop");

            // If there is no a active texture, pause and release camera resource.
            if (m_ActiveTextures.Count == 0)
            {
                m_IsPlaying = false;
#if !UNITY_EDITOR
                CameraDataProvider.StopCapture();
#endif
                Release();
            }
        }

        /// <summary>
        /// Release the camera.
        /// </summary>
        public virtual void Release()
        {
            if (CameraDataProvider == null)
            {
                return;
            }

            NRDebugger.Log("[CameraController] Start to Release");

            //#if !UNITY_EDITOR
            //            CameraDataProvider.Release();
            //            CameraDataProvider = null;
            //#endif
            m_CurrentFrame.data = null;
            m_IsInitialized = false;
            m_IsPlaying = false;
        }
    }
}
