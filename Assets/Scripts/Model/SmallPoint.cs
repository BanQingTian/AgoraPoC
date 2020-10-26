using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SmallPoint : MonoBehaviour
{
    public Vector3 DefaultPos { get; set; }

    [Header("地图缩放的比例")]
    public float MapZoomEndScale = 1.8f;
    [Header("地图移动的位置")]
    public Vector3 MapZoomEndPos = new Vector3(5, -101, 0);
    [Header("icon移动的位置")]
    public Vector3 ItemZoomEndPos = new Vector3(0, 0, -5.2f);
    [Header("移动的时间")]
    public float ZoomDuration = 0.5f;

    [Space(24)]
    public string OnwerChannelName = "unity3d";
    public ZUIButton PointBtn;
    public Image Track;
    public Image Icon;

    public GameObject Btns;
    public ZUIButton FocusBtn;
    public ZUIButton AddBtn;
    public ZUIButton TrackBtn;

    private Vector3 icon_btns_offset;

    //data
    public string PlayerId { get; set; }

    private void Start()
    {
        AddListener();
        icon_btns_offset = Btns.transform.localPosition - transform.localPosition;
        DefaultPos = this.gameObject.GetComponent<RectTransform>().localPosition;
    }

    public void AddListener()
    {
        PointBtn.OnZCommonItemEnter += ShowDetailView;
        FocusBtn.OnZCommonItemUp += Focus;
        AddBtn.OnZCommonItemUp += AddCallerToList;
        TrackBtn.OnZCommonItemUp += TrackRun;

        PointBtn.OnZCommonItemExit += leaveLate;
        FocusBtn.OnZCommonItemEnter += cancelLeaveLate;
        FocusBtn.OnZCommonItemExit += leaveLate;
        AddBtn.OnZCommonItemEnter += cancelLeaveLate;
        AddBtn.OnZCommonItemExit += leaveLate;
        TrackBtn.OnZCommonItemEnter += cancelLeaveLate;
        TrackBtn.OnZCommonItemExit += leaveLate;

    }

    private void ShowDetailView()
    {
        cancelLeaveLate();
        Btns.gameObject.SetActive(true);

        foreach (var item in UIManager.Instance.MapP.CallerMapUsedDic)
        {
            if (item.Key != PlayerId)
            {
                item.Value.Btns.gameObject.SetActive(false);
            }
        }
    }

    private void hideDetailView()
    {
        Btns.gameObject.SetActive(false);
    }
    private void cancelLeaveLate()
    {
        StopCoroutine("leaveLateCor");
    }
    private void leaveLate()
    {
        StopCoroutine("leaveLateCor");
        StartCoroutine("leaveLateCor");
    }
    private IEnumerator leaveLateCor()
    {
        float time = 0;//time = 1
        while ((time += Time.deltaTime) < 1)
        {
            yield return null;
        }
        hideDetailView();
    }


    public void Focus()
    {
        //UIManager.Instance.MapP.Resize(MapZoomEndScale, MapZoomEndPos, 0.5f);
        UIManager.Instance.MapP.DOTweenResize(MapZoomEndScale, MapZoomEndPos, 0.5f);
        PointBtn.transform.DOLocalMove(ItemZoomEndPos, 0.5f);
        Btns.transform.DOLocalMove(ItemZoomEndPos + icon_btns_offset, 0.5f);

        UIManager.Instance.MapP.SyncAction = () =>
        {
            PointBtn.transform.DOLocalMove(DefaultPos, 0.5f);
            Tween tween = Btns.transform.DOLocalMove(DefaultPos + icon_btns_offset, 0.5f);
            tween.onComplete = () =>
            {
                foreach (var item in UIManager.Instance.MapP.CallerMapUsedDic)
                {
                    if (item.Key != PlayerId)
                    {
                        item.Value.gameObject.SetActive(true);
                    }
                }
            };
        };

        foreach (var item in UIManager.Instance.MapP.CallerMapUsedDic)
        {
            if (item.Key != PlayerId)
            {
                item.Value.gameObject.SetActive(false);
            }
        }
    }

    public void TrackRun()
    {
        StartCoroutine(trackCor());
    }
    private IEnumerator trackCor()
    {
        Track.fillAmount = 0;
        while (Track.fillAmount < 1)
        {
            Track.fillAmount += Time.deltaTime;
            yield return null;
        }
        Track.fillAmount = 1;
    }

    public void AddCallerToList()
    {
        //UIManager.Instance.LeftConPanel.AddCallerToList(PlayerId);
        UIManager.Instance.CallerP.MoveCallerToChannel(PlayerId);
    }

    public void SPDelete()
    {
        gameObject.SetActive(false);
        Track.fillAmount = 0;
    }
}
