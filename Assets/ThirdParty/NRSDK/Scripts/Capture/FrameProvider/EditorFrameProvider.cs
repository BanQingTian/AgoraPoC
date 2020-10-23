/****************************************************************************
* Copyright 2019 Nreal Techonology Limited. All rights reserved.
*                                                                                                                                                          
* This file is part of NRSDK.                                                                                                          
*                                                                                                                                                           
* https://www.nreal.ai/        
* 
*****************************************************************************/

namespace NRKernal.Record
{
    using NRKernal;
    using UnityEngine;
    using System.Collections;

    public class EditorFrameProvider : AbstractFrameProvider
    {
        private Texture2D m_DefaultTexture;
        private CameraTextureFrame m_DefaultFrame;
        private bool m_IsPlay = false;

        public EditorFrameProvider()
        {
            m_DefaultTexture = Resources.Load<Texture2D>("Record/Textures/captureDefault");
            m_DefaultFrame = new CameraTextureFrame();
            m_DefaultFrame.texture = m_DefaultTexture;

            NRKernalUpdater.Instance.StartCoroutine(UpdateFrame());
        }

        public IEnumerator UpdateFrame()
        {
            while (true)
            {
                if (m_IsPlay)
                {
                    m_DefaultFrame.timeStamp = NRTools.GetTimeStamp();
                    OnUpdate?.Invoke(m_DefaultFrame);
                    m_IsFrameReady = true;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public override Resolution GetFrameInfo()
        {
            Resolution resolution = new Resolution();
            resolution.width = m_DefaultTexture.width;
            resolution.height = m_DefaultTexture.height;
            return resolution;
        }

        public override void Play()
        {
            m_IsPlay = true;
        }

        public override void Stop()
        {
            m_IsPlay = false;
        }

        public override void Release()
        {
            m_IsPlay = false;
            NRKernalUpdater.Instance.StopCoroutine(UpdateFrame());
        }
    }
}
