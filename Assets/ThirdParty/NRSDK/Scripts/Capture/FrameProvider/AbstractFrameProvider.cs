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
    using System;
    using UnityEngine;

    public abstract class AbstractFrameProvider
    {
        public delegate void UpdateImageFrame(CameraTextureFrame frame);
        public UpdateImageFrame OnUpdate;
        protected bool m_IsFrameReady = false;

        public virtual Resolution GetFrameInfo() { return new Resolution(); }

        public virtual bool IsFrameReady() { return m_IsFrameReady; }

        public virtual void Play() { }

        public virtual void Stop() { }

        public virtual void Release() { }
    }
}
