using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleModeCallerPanel : MonoBehaviour
{
    public GameObject AudioMode;
    // Auido
    public ZUIButton SpeakingBtn;
    public ZUIButton CloseBtn1;


    public GameObject VideoMode;
    // video
    public ZUIButton SwitchCameraBtn;
    public ZUIButton CloseBtn2;
    public ZUIButton MuteBtn;



    public void AddListener()
    {
        SpeakingBtn.OnZCommonItemDown = () => 
        {
            if (!MainController.Instance.SomeOneIsSpeaking)
            {
                ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "open_speaking", "shelter"));
            }
        };
        SpeakingBtn.OnZCommonItemUp += () =>
        {
            ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "close_speaking", "shelter"));
        };
        CloseBtn1.OnZCommonItemUp += CloseChannel;


        SwitchCameraBtn.OnZCommonItemUp = SwitchCamera;
        CloseBtn2.OnZCommonItemUp = CloseChannel;
        MuteBtn.OnZCommonItemUp = Mute;
    }

    public void OpenAudioModeUI()
    {
        AudioMode.SetActive(true);
        VideoMode.SetActive(false);
    }
    public void OpenVideoModeUI()
    {
        AudioMode.SetActive(false);
        VideoMode.SetActive(true);
    }

    public void SwitchCamera()
    {
        MainController.Instance.OnSwitchCameraBtnClk();
    }

    public void CloseChannel()
    {
        ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "leave_channel_pass_sample_mode", ZClient.Instance.PlayerID));

        MainController.Instance.OnLeaveBtnClk();
    }

    bool isMute = false;
    public void Mute()
    {
        MainController.Instance.OnMuteAudioBtnClk(!isMute);
    }
}
