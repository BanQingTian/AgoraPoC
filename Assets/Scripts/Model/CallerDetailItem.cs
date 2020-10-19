using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallerDetailItem : MonoBehaviour
{
    public Text Name;
    public Image Icon;
    public CallerPanel ParentsPanel;
    public string PlayerId { get; set; }


    public void SetName(string callerName)
    {
        Name.text = callerName;
    }

    public void SetIcon(Sprite s)
    {
        Icon.sprite = s;
    }

    public void MoveToChannel_BtnClked()
    {
        // 告诉caller加入频道
        ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "join_channel", PlayerId));
        ParentsPanel.MoveCallerToChannel(this);
        ParentsPanel.gameObject.SetActive(false);
    }
}
