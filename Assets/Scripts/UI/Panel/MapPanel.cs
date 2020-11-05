using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapPanel : ZBasePanel
{

    [Space(24)]
    public float m_Speed = 1;
    public RectTransform MapBg;
    public ZUIButton Reset;

    public GameObject MoveTip;

    private bool m_Moving = false;

    public List<SmallPoint> CallerMapItemPrefabs = new List<SmallPoint>();
    // key - channel name 
    public Dictionary<string, SmallPoint> MapPointsDic = new Dictionary<string, SmallPoint>();



    // invaild
    private const int celling = 3;
    public Dictionary<string, SmallPoint> CallerMapUsedDic = new Dictionary<string, SmallPoint>();



    public void AddListener()
    {
        Reset.OnZCommonItemUp += MapPlayBackwards;
        for (int i = 0; i < CallerMapItemPrefabs.Count; i++)
        {
            MapPointsDic.Add(CallerMapItemPrefabs[i].OnwerChannelName, CallerMapItemPrefabs[i]);
        }
    }

    public void GetMapItem(string playerid, VPlayerData data)
    {
        int count = CallerMapUsedDic.Count;
        if (count < celling)
        {
            if (!CallerMapUsedDic.ContainsKey(playerid))
            {
                var sp = CallerMapItemPrefabs[count];
                sp.PlayerId = playerid;
                sp.gameObject.SetActive(true);
                CallerMapUsedDic.Add(playerid, sp);

                sp.SetData(data);
            }
            return;
        }
        Debug.LogError("CallerMapItemPrefabs is null !!!!");
    }

    public void DeleteMapCaller(string playerid)
    {
        SmallPoint sp;
        if (CallerMapUsedDic.TryGetValue(playerid, out sp))
        {
            sp.SPDelete();
            CallerMapUsedDic.Remove(playerid);
        }
    }

    public void ShowResetBtn(bool show)
    {
        Reset.gameObject.SetActive(show);
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        MapPlayBackwards();
    //    }
    //}

    public void DOTweenResize(float zoomScale, Vector3 zoomPos, float time)
    {
        ShowResetBtn(true);
        MapBg.transform.DOScale(zoomScale, time);
        MapBg.transform.DOLocalMove(zoomPos, time);

        //MapPlayForward(zoomScale, zoomPos, time);
    }



    private void MapPlayForward(float s, Vector3 v, float t)
    {
        ShowResetBtn(true);
        GetScaleTween(s, t).PlayForward();
        GetPoseTween(v, t).PlayForward();
    }
    private void MapPlayBackwards()
    {
        ShowResetBtn(false);
        MapBg.transform.DOScale(1, 0.5f);
        MapBg.transform.DOLocalMove(new Vector3(0, 0, 0), 0.5f);
        //mapScaleTween?.PlayBackwards();
        //mapPosTween?.PlayBackwards();
        ZoomChild();
    }

    private Tween mapScaleTween;
    private Tween mapPosTween;
    private Tween GetScaleTween(float s, float t)
    {
        if (mapScaleTween == null)
        {
            mapScaleTween = MapBg.DOScale(s, t).SetAutoKill(false).Pause();
        }
        return mapScaleTween;
    }
    private Tween GetPoseTween(Vector3 v, float t)
    {
        if (mapPosTween == null)
        {
            mapPosTween = MapBg.DOLocalMove(v, t).SetAutoKill(false).Pause();
        }
        return mapPosTween;
    }


    public System.Action SyncAction;
    public void ZoomChild()
    {
        SyncAction?.Invoke();
        SyncAction = null;
    }

    public void SetMoveTip(bool b = true)
    {
        MoveTip.SetActive(b);
    }

    public Vector3 GetCenterPos()
    {
        return transform.position;
    }
    public Quaternion GetCenterRot()
    {
        return transform.rotation;
    }
}
