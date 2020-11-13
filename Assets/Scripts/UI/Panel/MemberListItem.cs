using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberListItem : MonoBehaviour
{
    public ZUIButton Btn;

    public string ChannelName;
    public string Name;



    //public ZUIButton EqualSmallPoint;

    public void AddListener()
    {
        Btn.OnZCommonItemEnter = Enter;
        Btn.OnZCommonItemExit = Exit;
        Btn.OnZCommonItemUp = ShowDetail;

        //EqualSmallPoint = UIManager.Instance.MapP.MapPointsDic[ChannelName].GetComponent<ZUIButton>();
    }
    public void ShowDetail()
    {
        UIManager.Instance.MapP.MapPointsDic[ChannelName].ShowDetailView();
    }

    private void Enter()
    {
        //EqualSmallPoint.HoverImage.gameObject.SetActive(true);
    }
    private void Exit()
    {
        //EqualSmallPoint.HoverImage.gameObject.SetActive(false);

    }
}
