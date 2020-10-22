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

    private bool m_Moving = false;

    public List<SmallPoint> CallerMapItemPrefabs = new List<SmallPoint>();
    private const int celling = 3;
    public Dictionary<string, SmallPoint> CallerMapUsedDic = new Dictionary<string, SmallPoint>();

    public void AddListener()
    {
        Reset.OnZCommonItemUp += MapPlayBackwards;
    }

    public void GetMapItem(string playerid)
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
        MapBg.transform.DOLocalMove(new Vector3(0,0,0), 0.5f);
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














    public void Resize(float scaleValue, Vector3 centerPos, float time)
    {
        if (m_Moving)
        {
            Debug.LogError("[CZLOG] is moving !!!");
            return;
        }

        float defaultScale = MapBg.localScale.x; // default 1:1:1
        float defaultPosX = MapBg.localPosition.x;
        float defaultPosY = MapBg.localPosition.y;

        float scaleRate = (scaleValue - defaultScale) / time;
        Vector2 posRate = new Vector2((centerPos.x - defaultPosX) / time, (centerPos.y - defaultPosY) / time);

        StartCoroutine(ResizeCor(scaleRate, posRate, defaultScale + scaleValue, centerPos, time));
    }
    private IEnumerator ResizeCor(float scaleRate, Vector2 posRate, float endScale, Vector3 endPos, float time)
    {
        m_Moving = true;
        float t = 0;
        while (t < time)
        {
            MapBg.localPosition += new Vector3(posRate.x, posRate.y) * Time.deltaTime * m_Speed;
            MapBg.localScale += Vector3.one * scaleRate * Time.deltaTime * m_Speed;
            t += Time.deltaTime;
            yield return null;
        }

        MapBg.localPosition = endPos;
        MapBg.localScale = Vector3.one * endScale;

        m_Moving = false;
    }

}
