using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarPanel : MonoBehaviour
{
    public ZUIButton VideoBar;
    public ZUIButton MapBar;
    public ZUIButton MemberBar;

    public ARInteractiveItem VideoMoveCenter;
    public ARInteractiveItem MapMoveCenter;
    public ARInteractiveItem MemberMoveCenter;

    public Transform VideoCenter;
    public Transform MapCenter;
    public Transform MemberCenter;

    public Image VideoBarHover_surround;
    public Image VideoBarHover_mainsub;

    public Transform MainPanel;

    public GameObject MoveRange;

    private bool isSelect = false;


    public void SetVideoBarHoverMode(ZViewMode vm)
    {
        switch (vm)
        {
            case ZViewMode.SurroundMode:
                VideoBar.HoverImage = VideoBarHover_surround;
                break;
            case ZViewMode.MainSubMode:
                VideoBar.HoverImage = VideoBarHover_mainsub;
                break;
            default:
                break;
        }

        VideoMoveCenter.transform.position = UIManager.Instance.VideoP.GetCenterPos();
        VideoMoveCenter.transform.rotation = Quaternion.Euler(UIManager.Instance.VideoP.GetCenterRot());
    }

    public void AddListener()
    {
        VideoBar.OnZCommonItemUp = VideoBarClked;
        MapBar.OnZCommonItemUp = MapBarClked;
        MemberBar.OnZCommonItemUp = MemberBarClked;

        VideoMoveCenter.OnDeselected += reset;
        MapMoveCenter.OnDeselected += reset;
        MemberMoveCenter.OnDeselected += reset;
    }

    public void VideoBarClked()
    {
        if (!isSelect)
        {
            MainPanel.SetParent(VideoCenter);

            VideoMoveCenter.TriggerAutoSelect();
            UIManager.Instance.VideoP.SetMoveTip();
            MoveRange.SetActive(true);
        }
        isSelect = true;

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

    private void reset()
    {
        MainPanel.SetParent(UIManager.Instance.transform);
        MoveRange.SetActive(false);

        VideoMoveCenter.transform.position = UIManager.Instance.VideoP.GetCenterPos();
        VideoMoveCenter.transform.rotation = Quaternion.Euler(UIManager.Instance.VideoP.GetCenterRot());
        MapMoveCenter.transform.position = UIManager.Instance.MapP.GetCenterPos();
        MapMoveCenter.transform.rotation = UIManager.Instance.MapP.GetCenterRot();
        MemberMoveCenter.transform.position = UIManager.Instance.MemberP.GetCenterPos();
        MemberMoveCenter.transform.rotation = UIManager.Instance.MemberP.GetCenterRot();

        UIManager.Instance.VideoP.SetMoveTip(false);
        UIManager.Instance.MapP.SetMoveTip(false);
        UIManager.Instance.MemberP.SetMoveTip(false);


        isSelect = false;
    }
}
