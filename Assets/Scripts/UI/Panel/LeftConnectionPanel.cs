using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zrime;

public class LeftConnectionPanel : ZBasePanel
{
    public const float AddBtnInterval = 35.4f;

    [Space(24)]
    public CallerPanel CallerP;

    public Transform IconLayoutParent;
    public IconItemLeft IconItemPrefab;
    public Dictionary<string, IconItemLeft> ItemDic = new Dictionary<string, IconItemLeft>();

    public GameObject AudioMode;
    public GameObject VideoMode;
    public GameObject SwitchMode;

    public RectTransform AddBtn; //  rate-39

    [Space(24),Header("---------- DIY BUTTON ---------")]

    // Enter mode ui
    public ZUIButton AddCallerBtn;
    public ZUIButton EnterAudioBtn;
    public ZUIButton EnterVideoBtn;

    // Audio mode ui
    public ZUIButton SwitchToVideoBtn;
    public ZUIButton SpeakingBtn;
    public ZUIButton CloseBtn1;

    // Video mode ui
    public ZUIButton SwitchToAudioBtn;
    public ZUIButton MuteBtn;
    public ZUIButton CloseBtn2;


    public void AddListener()
    {
        AddCallerBtn.OnZCommonItemUp += OpenCallerPanel;
        EnterAudioBtn.OnZCommonItemUp += SwitchToAuido;
        EnterVideoBtn.OnZCommonItemUp += SwitchToVideo;


        SwitchToVideoBtn.OnZCommonItemUp += SwitchToVideo;
        SpeakingBtn.OnZCommonItemDown += () => 
        {
            if (ItemDic.Count == 0) return;
            if (!MainController.Instance.SomeOneIsSpeaking)
            {
                ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "open_speaking", "shelter"));
            }
        };
        SpeakingBtn.OnZCommonItemUp += () => 
        {
            if (ItemDic.Count == 0) return;
            ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "close_speaking", "shelter"));
        };
        CloseBtn1.OnZCommonItemUp += QuitChannel;


        SwitchToAudioBtn.OnZCommonItemUp += SwitchToAuido;
        MuteBtn.OnZCommonItemUp += SwitchMuteLocalAudio;
        CloseBtn2.OnZCommonItemUp += QuitChannel;
       

    }

    public void AddCallerToList(/*CallerData data*/string playerid)
    {
        if (ItemDic.ContainsKey(playerid))
        {
            Debug.LogError("ContainsKey : " + playerid);
            return;
        }

        var p = GameObject.Instantiate<IconItemLeft>(IconItemPrefab);
        p.AddListener();
        p.transform.SetParent(IconLayoutParent);
        p.gameObject.SetActive(true);
        p.GetComponent<RectTransform>().localRotation = Quaternion.identity;
        p.GetComponent<RectTransform>().localScale = Vector3.one;
        p.GetComponent<RectTransform>().localPosition = new Vector3(p.transform.localPosition.x, p.transform.localPosition.y, 0);

        p.PlayerId =playerid;
        ItemDic.Add(p.PlayerId, p);

        if (ItemDic.Count <= 3)
        {
            AddBtn.localPosition = new Vector3(AddBtn.localPosition.x + AddBtnInterval, AddBtn.localPosition.y, AddBtn.localPosition.z);
        }

    }

    public void RemoveCallerToWaitingList(string playerId)
    {
        IconItemLeft item;
        if (ItemDic.TryGetValue(playerId, out item))
        {
            Destroy(item.gameObject);
            ItemDic.Remove(playerId);
            AddBtn.localPosition = new Vector3(AddBtn.localPosition.x - AddBtnInterval, AddBtn.localPosition.y, AddBtn.localPosition.z);
            CallerP.CreateCallerToWaitingList(playerId);
        }
        else
        {
            Debug.Log("[CZLOG] Don't contain this player id ---" + playerId);
        }
    }

    public void DeleteCaller(string playerId)
    {
        IconItemLeft item;
        if (ItemDic.TryGetValue(playerId, out item))
        {
            Destroy(item.gameObject);
            ItemDic.Remove(playerId);
            AddBtn.localPosition = new Vector3(AddBtn.localPosition.x - 39, AddBtn.localPosition.y, AddBtn.localPosition.z);
        }
        else
        {
            Debug.Log("[CZLOG] Don't contain this player id ---" + playerId);
        }
    }


    public void OpenCallerPanel()
    {
        CallerP.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void SwitchToAuido()
    {
        MainController.Instance.OnAudioBtnClk();
        ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "mute_all_local_voice", "shelter"));
        AudioMode.SetActive(true);
        VideoMode.SetActive(false);
        SwitchMode.SetActive(false);
    }
    public void SwitchToVideo()
    {
        MainController.Instance.OnVideoBtnClk();
        ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "open_all_local_voice", "shelter"));
        AudioMode.SetActive(false);
        VideoMode.SetActive(true);
        SwitchMode.SetActive(false);
    }

    bool isMute = false;
    public void SwitchMuteLocalAudio()
    {
        MainController.Instance.OnMuteAudioBtnClk(isMute);
        isMute = !isMute;
    }
    public void QuitChannel()
    {
        MainController.Instance.OnLeaveBtnClk();
        AudioMode.SetActive(false);
        VideoMode.SetActive(false);
        SwitchMode.SetActive(true);
    }

}
