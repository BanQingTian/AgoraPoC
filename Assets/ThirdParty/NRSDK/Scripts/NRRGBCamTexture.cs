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

    /// <summary>
    /// Create a rgb camera texture.
    /// </summary>
    public class NRRGBCamTexture : CameraModelView
    {
        public Action<CameraTextureFrame> OnUpdate;
        public CameraTextureFrame CurrentFrame;
        private Texture2D m_Texture;

        private Texture2D CreateTexture()
        {
            return new Texture2D(Width, Height, TextureFormat.RGB24, false);
        }

        public Texture2D GetTexture()
        {
            if (m_Texture == null)
            {
                m_Texture = CreateTexture();
            }
            return m_Texture;
        }

        protected override void OnCreated()
        {
            this.m_Texture = CreateTexture();
        }

        protected override void OnRawDataUpdate(FrameRawData rgbRawDataFrame)
        {
            m_Texture.LoadRawTextureData(rgbRawDataFrame.data);
            m_Texture.Apply();

            CurrentFrame.timeStamp = rgbRawDataFrame.timeStamp;
            CurrentFrame.texture = m_Texture;

            OnUpdate?.Invoke(CurrentFrame);
        }

        protected override void OnStopped()
        {
            GameObject.Destroy(m_Texture);
            m_Texture = null;
        }
    }
}
