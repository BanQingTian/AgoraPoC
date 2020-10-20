using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleModeCallerPanel : MonoBehaviour
{
    public ZBtn ASpeakingBtn;

    public void RegisterSampleBtn()
    {
        ASpeakingBtn.ClkDown += () =>
        {
            if (!MainController.Instance.SomeOneIsSpeaking)
            {
                ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "open_speaking", "shelter"));
            }
        };

        ASpeakingBtn.ClkUp += () =>
        {
            ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "close_speaking", "shelter"));
        };
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
}
