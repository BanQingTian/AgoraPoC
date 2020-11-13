using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarPanel : MonoBehaviour
{
    public ZUIButton VideoBar1;
    public ZUIButton VideoBar2;
    public ZUIButton VideoBar3;
    public ZUIButton MainsubBar;
    public ZUIButton MapBar;
    public ZUIButton MemberBar;

    public ARInteractiveItem VideoMoveCenter;
    public ARInteractiveItem MapMoveCenter;
    public ARInteractiveItem MemberMoveCenter;

    public Transform VideoCenter;
    public Transform MapCenter;
    public Transform MemberCenter;

    public Transform MainPanel;

    public GameObject MoveRange;
    public GameObject MoveRangePart2; //991 740
    public GameObject MoveRangePart3;

    public bool isSelect = false;


    public void SetVideoBarHoverMode(ZViewMode vm)
    {
        switch (vm)
        {
            case ZViewMode.SurroundMode:
                VideoBar1.gameObject.SetActive(true);
                VideoBar2.gameObject.SetActive(true);
                VideoBar3.gameObject.SetActive(true);
                MainsubBar.gameObject.SetActive(false);
                break;
            case ZViewMode.MainSubMode:
                VideoBar1.gameObject.SetActive(false);
                VideoBar2.gameObject.SetActive(false);
                VideoBar3.gameObject.SetActive(false);
                MainsubBar.gameObject.SetActive(true);
                break;
            default:
                break;
        }

        VideoMoveCenter.transform.position = UIManager.Instance.VideoP.GetCenterPos(curEnterVideoNumber);
        VideoMoveCenter.transform.rotation = UIManager.Instance.VideoP.GetCenterRot(curEnterVideoNumber);
    }

    public void AddListener()
    {
        //VideoBar.OnZCommonItemUp = VideoBarClked;
        //VideoBar.OnZCommonItemEnter = VideoBarEnter;
        //VideoBar.OnZCommonItemExit = VideoBarExit;


        VideoBar1.OnZCommonItemEnter = VideoBarEnter1;
        VideoBar1.OnZCommonItemExit = VideoBarExit1;
        VideoBar1.OnZCommonItemUp = VideoBarClked;

        VideoBar2.OnZCommonItemEnter = VideoBarEnter2;
        VideoBar2.OnZCommonItemExit = VideoBarExit2;
        VideoBar2.OnZCommonItemUp = VideoBarClked;

        VideoBar3.OnZCommonItemEnter = VideoBarEnter3;
        VideoBar3.OnZCommonItemExit = VideoBarExit3;
        VideoBar3.OnZCommonItemUp = VideoBarClked;

        MainsubBar.OnZCommonItemEnter = VideoBarEnter1;
        MainsubBar.OnZCommonItemExit = VideoBarExit1;
        MainsubBar.OnZCommonItemUp = VideoBarClked;

        MapBar.OnZCommonItemUp = MapBarClked;
        MapBar.OnZCommonItemEnter = MapBarEnter;
        MapBar.OnZCommonItemExit = MapBarExit;

        MemberBar.OnZCommonItemUp = MemberBarClked;
        MemberBar.OnZCommonItemEnter = MemberBarEnter;
        MemberBar.OnZCommonItemExit = MemberBarExit;

        VideoMoveCenter.OnDeselected += reset;
        MapMoveCenter.OnDeselected += reset;
        MemberMoveCenter.OnDeselected += reset;

    }

    int curEnterVideoNumber = 1;
    public void VideoBarClked()
    {
        if (!isSelect)
        {
            MainPanel.SetParent(VideoCenter);

            VideoMoveCenter.TriggerAutoSelect();
            UIManager.Instance.VideoP.SetMoveTip(curEnterVideoNumber);
            MoveRange.SetActive(true);
            if (UIManager.Instance.VideoP.mode == ZViewMode.MainSubMode)
            {
                MoveRangePart2.transform.localPosition = new Vector3(670.3f, -132.5f, -245.6f);
                MoveRangePart3.transform.localPosition = new Vector3(670.3f, 181.6f, -245.6f);
            }
            else
            {
                MoveRangePart2.transform.localPosition = new Vector3(924.8f, -132.5f, -362.2f);
                MoveRangePart3.transform.localPosition = new Vector3(924.8f, 181.6f, -362.2f);
            }

        }
        isSelect = true;
    }
    public void VideoBarEnter1()
    {
        UIManager.Instance.PlayBarAnim(MOVEBAR.VIDEO1);
        curEnterVideoNumber = 1;
        ResetVideoMoveCenter();
        ShowAllBar();

        //var pos = UIManager.Instance.VideoP.m_Views[0].localPosition;
        //UIManager.Instance.VideoP.MoveTip1.transform.localPosition = new Vector3(pos.x, pos.y, UIManager.Instance.VideoP.MoveTip1.transform.localPosition.z);
    }
    public void VideoBarExit1()
    {
        UIManager.Instance.PlayBarAnim(MOVEBAR.VIDEO1, false);
        HideAllBar();
    }
    public void VideoBarEnter2()
    {
        UIManager.Instance.PlayBarAnim(MOVEBAR.VIDEO2);
        curEnterVideoNumber = 2;
        ResetVideoMoveCenter();
        ShowAllBar();
    }

    public void VideoBarExit2()
    {
        UIManager.Instance.PlayBarAnim(MOVEBAR.VIDEO2, false);
        HideAllBar();
    }
    public void VideoBarEnter3()
    {
        UIManager.Instance.PlayBarAnim(MOVEBAR.VIDEO3);
        curEnterVideoNumber = 3;
        ResetVideoMoveCenter();
        ShowAllBar();
    }
    public void VideoBarExit3()
    {
        UIManager.Instance.PlayBarAnim(MOVEBAR.VIDEO3, false);
        HideAllBar();
    }
    private void ResetVideoMoveCenter()
    {
        VideoMoveCenter.transform.position = UIManager.Instance.VideoP.GetCenterPos(curEnterVideoNumber);
        VideoMoveCenter.transform.rotation = UIManager.Instance.VideoP.GetCenterRot(curEnterVideoNumber);
    }
    public void MapBarClked()
    {
        if (!isSelect)
        {
            MainPanel.SetParent(MapCenter);

            MapMoveCenter.TriggerAutoSelect();
            UIManager.Instance.MapP.SetMoveTip();
            MoveRange.SetActive(true);
        }
        isSelect = true;
    }
    public void MapBarEnter()
    {
        UIManager.Instance.PlayBarAnim(MOVEBAR.MAP);
        ShowAllBar();
    }
    public void MapBarExit()
    {
        UIManager.Instance.PlayBarAnim(MOVEBAR.MAP, false);
        HideAllBar();
    }
    public void MemberBarClked()
    {
        if (!isSelect)
        {
            MainPanel.SetParent(MemberCenter);

            MemberMoveCenter.TriggerAutoSelect();
            UIManager.Instance.MemberP.SetMoveTip();
            MoveRange.SetActive(true);
        }

        isSelect = true;
    }
    public void MemberBarEnter()
    {
        UIManager.Instance.PlayBarAnim(MOVEBAR.MEMBER);
        ShowAllBar();
    }
    public void MemberBarExit()
    {
        UIManager.Instance.PlayBarAnim(MOVEBAR.MEMBER, false);

    }

    public void ShowAllBar()
    {
        VideoBar1.HoverImage.gameObject.SetActive(true);
        VideoBar2.HoverImage.gameObject.SetActive(true);
        VideoBar3.HoverImage.gameObject.SetActive(true);
        MainsubBar.HoverImage.gameObject.SetActive(true);
        MapBar.HoverImage.gameObject.SetActive(true);
        MemberBar.HoverImage.gameObject.SetActive(true);

        //VideoBar1.HoverImage2.gameObject.SetActive(false);
        //VideoBar2.HoverImage2.gameObject.SetActive(false);
        //VideoBar3.HoverImage2.gameObject.SetActive(false);
        //MainsubBar.HoverImage2.gameObject.SetActive(false);
        //MapBar.HoverImage2.gameObject.SetActive(false);
        //MemberBar.HoverImage2.gameObject.SetActive(false);
    }
    public void HideAllBar()
    {
        VideoBar1.HoverImage.gameObject.SetActive(false);
        VideoBar2.HoverImage.gameObject.SetActive(false);
        VideoBar3.HoverImage.gameObject.SetActive(false);
        MainsubBar.HoverImage.gameObject.SetActive(false);
        MapBar.HoverImage.gameObject.SetActive(false);
        MemberBar.HoverImage.gameObject.SetActive(false);
    }

    private void reset()
    {
        MainPanel.SetParent(UIManager.Instance.transform);
        MoveRange.SetActive(false);

        VideoMoveCenter.transform.position = UIManager.Instance.VideoP.GetCenterPos();
        VideoMoveCenter.transform.rotation = UIManager.Instance.VideoP.GetCenterRot();
        MapMoveCenter.transform.position = UIManager.Instance.MapP.GetCenterPos();
        MapMoveCenter.transform.rotation = UIManager.Instance.MapP.GetCenterRot();
        MemberMoveCenter.transform.position = UIManager.Instance.MemberP.GetCenterPos();
        MemberMoveCenter.transform.rotation = UIManager.Instance.MemberP.GetCenterRot();

        UIManager.Instance.VideoP.SetMoveTip(curEnterVideoNumber, false);
        UIManager.Instance.MapP.SetMoveTip(false);
        UIManager.Instance.MemberP.SetMoveTip(false);

        HideAllBar();

        isSelect = false;
    }
}
