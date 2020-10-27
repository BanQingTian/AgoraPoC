using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleModeCallerPanel : MonoBehaviour
{
    public GameObject DisconnectMode;
    public GameObject AudioMode;
    public GameObject AudioMode_voiceUI;
    public GameObject VideoMode;
    public Text ModeLabel;
    public Text ConnectLabel;


    // Auido
    public Toggle otherMuteBtn;
    public ZBtn SpeakingBtn;
    public ZBtn AudioCloseChannelBtn;

    // Video
    public ZBtn SwitchCameraBtn;
    public Toggle SelfMuteBtn;
    public ZBtn VoiceCloseChannelBtn;


    public void AddListener()
    {
        otherMuteBtn.onValueChanged.AddListener(CloseVioce);
        SpeakingBtn.ClkDown = () => 
        {
            if (!MainController.Instance.SomeOneIsSpeaking)
            {
                ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "open_speaking", ZClient.Instance.PlayerID));
            }
        };
        SpeakingBtn.ClkUp += () =>
        {
            ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "close_speaking", ZClient.Instance.PlayerID));
        };
        AudioCloseChannelBtn.ClkUp += CloseChannel;

        SwitchCameraBtn.ClkUp += SwitchCamera; 
        SelfMuteBtn.onValueChanged.AddListener(SelfMute);
        VoiceCloseChannelBtn.ClkUp += CloseChannel;
    }

    public void OpenAudioModeUI()
    {
        DisconnectMode.SetActive(false);
        AudioMode.SetActive(true);
        VideoMode.SetActive(false);
        AudioMode_voiceUI.SetActive(true);
        ModeLabel.text = "对讲模式";
        ConnectLabel.text = "· 已连接";

    }
    public void OpenVideoModeUI()
    {
        DisconnectMode.SetActive(false);
        AudioMode.SetActive(false);
        VideoMode.SetActive(true);
        AudioMode_voiceUI.SetActive(true);
        ModeLabel.text = "总控制台";
        ConnectLabel.text = "· 已连接";
    }

    public void OpenDisconnectMode()
    {
        DisconnectMode.SetActive(true);
        VideoMode.SetActive(false);
        AudioMode.SetActive(false);
        AudioMode_voiceUI.SetActive(false);
        ModeLabel.text = "当前频道";
        ConnectLabel.text = "· 未连接";
    }

    public void CloseVioce(bool mute)
    {
        MainController.Instance.OnMuteVoiceBtnClk(mute);
    }

    public void SwitchCamera()
    {
        MainController.Instance.OnSwitchCameraBtnClk();
    }

    public void CloseChannel()
    {
        ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "leave_channel_pass_sample_mode", ZClient.Instance.PlayerID));
        ConnectLabel.text = "· 未连接";
        MainController.Instance.OnLeaveBtnClk();
        OpenDisconnectMode();
    }

    public void SelfMute(bool isMute)
    {
        MainController.Instance.OnMuteAudioBtnClk(isMute);
    }
}
