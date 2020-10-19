using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zrime;

public class LeftConnectionPanel : MonoBehaviour
{
    public CallerPanel CallerP;

    public Transform IconLayoutParent;
    public IconItemLeft IconItemPrefab;
    public Dictionary<string, IconItemLeft> ItemDic = new Dictionary<string, IconItemLeft>();

    public GameObject AudioMode;
    public GameObject VideoMode;
    public GameObject SwitchMode;

    public RectTransform AddBtn; //  rate-39

    public ZBtn SpeakingBtn;

    public void AddCallerToList(/*CallerData data*/string playerid)
    {
        var p = GameObject.Instantiate<IconItemLeft>(IconItemPrefab);

        p.transform.SetParent(IconLayoutParent);
        p.gameObject.SetActive(true);
        p.transform.localRotation = Quaternion.identity;
        p.transform.localScale = Vector3.one;
        p.transform.localPosition = new Vector3(p.transform.localPosition.x, p.transform.localPosition.y, 0);

        p.PlayerId =playerid;
        ItemDic.Add(p.PlayerId, p);

        if (ItemDic.Count <= 3)
        {
            AddBtn.localPosition = new Vector3(AddBtn.localPosition.x + 39, AddBtn.localPosition.y, AddBtn.localPosition.z);
        }

    }

    public void RemoveCallerToWaitingList(string playerId)
    {
        IconItemLeft item;
        if (ItemDic.TryGetValue(playerId, out item))
        {
            Destroy(item.gameObject);
            ItemDic.Remove(playerId);
            AddBtn.localPosition = new Vector3(AddBtn.localPosition.x - 39, AddBtn.localPosition.y, AddBtn.localPosition.z);
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
    }
    public void SwitchToAuido()
    {
        MainController.Instance.OnAudioBtnClk();
        AudioMode.SetActive(true);
        VideoMode.SetActive(false);
        SwitchMode.SetActive(false);
    }
    public void SwitchToVideo()
    {
        MainController.Instance.OnVideoBtnClk();
        AudioMode.SetActive(false);
        VideoMode.SetActive(true);
        SwitchMode.SetActive(false);
    }
    public void MuteLocalAudio(bool mute)
    {
        MainController.Instance.OnMuteAudioBtnClk(mute);
    }
    public void QuitChannel()
    {
        MainController.Instance.OnLeaveBtnClk();
        AudioMode.SetActive(false);
        VideoMode.SetActive(false);
        SwitchMode.SetActive(true);
    }

    public void RegisterSampleBtn()
    {
        SpeakingBtn.ClkDown += () =>
        {
            if (!MainController.Instance.SomeOneIsSpeaking)
            {
                ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "open_speaking", "shelter"));
            }
        };

        SpeakingBtn.ClkUp += () =>
        {
            ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "close_speaking", "shelter"));
        };
    }
}
