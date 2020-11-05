using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberListItem : MonoBehaviour
{
    public ZUIButton Btn;
    public string ChannelName;
    public string Name;

    public void AddListener()
    {
        Btn.OnZCommonItemEnter += ShowDetail;
    }
    public void ShowDetail()
    {
        UIManager.Instance.MapP.MapPointsDic[ChannelName].ShowDetailView();
    }
}
