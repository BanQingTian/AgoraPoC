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

    [HelpURL("https://developer.nreal.ai/develop/unity/customize-phone-controller")]
    public class NRMultiDisplayManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_DefaultVirtualDisplayer;
        private NRVirtualDisplayer m_VirtualDisplayer;

        private void Start()
        {
            NRSessionManager.Instance.AfterInitialized(Init);
        }

        private void Init()
        {
            m_VirtualDisplayer = FindObjectOfType<NRVirtualDisplayer>();
            if (m_VirtualDisplayer == null)
            {
                Instantiate(m_DefaultVirtualDisplayer);
            }
        }
    }
}
