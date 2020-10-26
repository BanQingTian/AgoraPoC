using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconItemLeft : MonoBehaviour
{
    public LeftConnectionPanel Panel;

    public ZUIButton Btn;

    public Text Name;
    public Image ImageIcon;

    public VPlayerData m_Data;
    public string PlayerId { get; set; }


    public void AddListener()
    {
        Btn.OnZCommonItemUp += RemoveCallerToWaitingList;
    }

    public void SetData(VPlayerData data)
    {
        if(data == null)
        {
            Debug.LogError("data == null !!!");
            return;
        }
        m_Data = data;

        Name.text = data.Name;
        ImageIcon.sprite = data.Icon;
    }

    private void RemoveCallerToWaitingList()
    {
        Debug.Log("RemoveCallerToWaitingList");

        ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "leave_channel", PlayerId));

        Panel.RemoveCallerToWaitingList(PlayerId);
    }

}
