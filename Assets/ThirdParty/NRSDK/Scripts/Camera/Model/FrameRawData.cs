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
    using UnityEngine;

    public struct CameraTextureFrame
    {
        public UInt64 timeStamp;
        public Texture texture;
    }

    public partial struct FrameRawData
    {
        public UInt64 timeStamp;
        public byte[] data;

        public static bool MakeSafe(IntPtr textureptr, int size, UInt64 timestamp, ref FrameRawData frame)
        {
            if (textureptr == IntPtr.Zero || size <= 0)
            {
                return false;
            }
            if (frame.data == null || frame.data.Length != size)
            {
                frame.data = new byte[size];
            }
            frame.timeStamp = timestamp;
            Marshal.Copy(textureptr, frame.data, 0, size);
            return true;
        }

        public override string ToString()
        {
            return string.Format("timestamp :{0} data size:{1}", timeStamp, data.Length);
        }
    }
}
