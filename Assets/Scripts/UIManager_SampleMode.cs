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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}","join_channel", ZClient.Instance.PlayerID));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MainController.Instance.OnAudioBtnClk();
        }
    }

    public void OpenAudioModeUI()
    {
        smcp.OpenAudioModeUI();
    }

    public void OpenVideoModeUI()
    {
        smcp.OpenVideoModeUI();
    }
    public void OpenDisconnectUI()
    {
        smcp.OpenDisconnectMode();
    }

    public void smcp_RegisterBtnEvent()
    {
        smcp.AddListener();
    }

}
