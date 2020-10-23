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
    using System;

    [ScriptOrder(-1000)]
    public class NRKernalUpdater : MonoBehaviour
    {
        private static NRKernalUpdater m_Instance;
        public static NRKernalUpdater Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = CreateInstance();
                }
                return m_Instance;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            m_Instance = CreateInstance();
        }

        private static NRKernalUpdater CreateInstance()
        {
            GameObject updateObj = new GameObject("NRKernalUpdater");
            GameObject.DontDestroyOnLoad(updateObj);
            return updateObj.AddComponent<NRKernalUpdater>();
        }

        public static event Action OnPreUpdate;
        public static event Action OnUpdate;
        public static event Action OnPostUpdate;

        private void Update()
        {
            OnPreUpdate?.Invoke();
            OnUpdate?.Invoke();
            OnPostUpdate?.Invoke();
        }
    }
}
