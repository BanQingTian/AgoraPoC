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
    using System.Collections.Generic;

    internal partial class CameraProxyFactory
    {
        private static Dictionary<string, NativeCameraProxy> m_CameraControllerDict = new Dictionary<string, NativeCameraProxy>();

        public static NativeCameraProxy CreateRGBCameraProxy()
        {
            NativeCameraProxy controller;
            if (!m_CameraControllerDict.TryGetValue(NRRgbCamera.ID, out controller))
            {
                controller = new NRRgbCamera();
                m_CameraControllerDict.Add(NRRgbCamera.ID, controller);
            }
            return controller;
        }

        public static NativeCameraProxy GetInstance(string id)
        {
            return m_CameraControllerDict[id];
        }
    }
}
