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
    /// Create a YUV camera texture.
    /// </summary>
    public class NRRGBCamTextureYUV : CameraModelView
    {
        public Action<YUVTextureFrame> OnUpdate;
        public struct YUVTextureFrame
        {
            public UInt64 timeStamp;
            public Texture2D textureY;
            public Texture2D textureU;
            public Texture2D textureV;
            public byte[] YBuf;
            public byte[] UBuf;
            public byte[] VBuf;
        }
        private YUVTextureFrame m_FrameData;

        private void CreateTex()
        {
            if (m_FrameData.textureY == null)
            {
                m_FrameData = new YUVTextureFrame();
                m_FrameData.textureY = new Texture2D(Width, Height, TextureFormat.Alpha8, false);
                m_FrameData.textureU = new Texture2D(Width / 2, Height / 2, TextureFormat.Alpha8, false);
                m_FrameData.textureV = new Texture2D(Width / 2, Height / 2, TextureFormat.Alpha8, false);
            }
        }

        public YUVTextureFrame GetTexture()
        {
            if (m_FrameData.textureY == null)
            {
                CreateTex();
            }
            return m_FrameData;
        }

        public NRRGBCamTextureYUV() : base(CameraImageFormat.YUV_420_888) { }

        protected override void OnCreated()
        {
            base.OnCreated();
            CreateTex();
        }

        protected override void OnRawDataUpdate(FrameRawData frame)
        {
            LoadYUVTexture(frame);
            OnUpdate?.Invoke(m_FrameData);
        }

        private void LoadYUVTexture(FrameRawData frame)
        {
            if (frame.data == null || frame.data.Length == 0)
            {
                Debug.LogError("[NRRGBCamTextureYUV] LoadYUVTexture error: frame is null");
                return;
            }

            int size = frame.data.Length;
            if (m_FrameData.YBuf == null)
            {
                m_FrameData.YBuf = new byte[size * 2 / 3];
                m_FrameData.UBuf = new byte[size / 6];
                m_FrameData.VBuf = new byte[size / 6];
            }
            if (m_FrameData.textureY == null)
            {
                CreateTex();
            }
            m_FrameData.timeStamp = frame.timeStamp;

            Array.Copy(frame.data, 0, m_FrameData.YBuf, 0, m_FrameData.YBuf.Length);
            Array.Copy(frame.data, m_FrameData.YBuf.Length, m_FrameData.UBuf, 0, m_FrameData.UBuf.Length);
            Array.Copy(frame.data, m_FrameData.YBuf.Length + m_FrameData.UBuf.Length, m_FrameData.VBuf, 0, m_FrameData.VBuf.Length);

            m_FrameData.textureY.LoadRawTextureData(m_FrameData.YBuf);
            m_FrameData.textureU.LoadRawTextureData(m_FrameData.UBuf);
            m_FrameData.textureV.LoadRawTextureData(m_FrameData.VBuf);

            m_FrameData.textureY.Apply();
            m_FrameData.textureU.Apply();
            m_FrameData.textureV.Apply();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
            GameObject.Destroy(m_FrameData.textureY);
            GameObject.Destroy(m_FrameData.textureU);
            GameObject.Destroy(m_FrameData.textureV);
            m_FrameData.textureY = null;
            m_FrameData.textureU = null;
            m_FrameData.textureV = null;
            m_FrameData.YBuf = null;
            m_FrameData.UBuf = null;
            m_FrameData.VBuf = null;
        }
    }
}
