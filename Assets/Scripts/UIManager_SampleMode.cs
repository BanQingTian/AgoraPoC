using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_SampleMode : MonoBehaviour
{
    public static UIManager_SampleMode Instance;

    public SampleModeCallerPanel smcp;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        smcp_RegisterBtnEvent();
    }

    public void smcp_RegisterBtnEvent()
    {
        smcp.RegisterSampleBtn();
    }

}
