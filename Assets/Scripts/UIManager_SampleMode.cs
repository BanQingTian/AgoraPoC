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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "join_channel", ZClient.Instance.PlayerID));

            
        }
    }


    public void smcp_RegisterBtnEvent()
    {
        smcp.RegisterSampleBtn();
    }

}
