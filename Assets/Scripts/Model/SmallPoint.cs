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
    public string Name;
    public Image Track;

    public Image Icon;

    public GameObject Btns;
    public ZUIButton FocusBtn;
    public ZUIButton AddBtn;
    public ZUIButton TalkbackBtn;
    public ZUIButton TrackBtn;
    public Image TalkbackIcon;
    public Image AddIcon;
    public Image FocusIcon;
    public Image TrackIcon;

    public MapPanelBtn ParentsBtn;


    public TMPro.TextMeshProUGUI NameLabel;

    private Vector3 icon_btns_offset;

    public VPlayerData m_Data;

    //data
    public string PlayerId { get; set; }

    private void Start()
    {
        AddListener();
        icon_btns_offset = Btns.transform.localPosition - transform.localPosition;
        DefaultPos = this.gameObject.GetComponent<RectTransform>().localPosition;
        NameLabel.text = Name;
    }

    public void AddListener()
    {
        ParentsBtn.OnZCommonItemUp = ShowDetailView;
        FocusBtn.OnZCommonItemUp = Focus;
        AddBtn.OnZCommonItemUp = AddCallerToList;
        TrackBtn.OnZCommonItemUp= TrackRun;
        TalkbackBtn.OnZCommonItemUp = TalkbackClked;

        //ParentsBtn.OnZCommonItemExit = leaveLate;
        FocusBtn.OnZCommonItemEnter = cancelLeaveLate;
        FocusBtn.OnZCommonItemExit = leaveLate;
        AddBtn.OnZCommonItemEnter = cancelLeaveLate;
        AddBtn.OnZCommonItemExit = leaveLate;
        TrackBtn.OnZCommonItemEnter = cancelLeaveLate;
        TrackBtn.OnZCommonItemExit = leaveLate;
        TalkbackBtn.OnZCommonItemEnter = cancelLeaveLate;
        TalkbackBtn.OnZCommonItemExit = leaveLate;

    }

    public void SetData(VPlayerData data)
    {
        if (data == null) return;
        m_Data = data;
        Icon.sprite = data.Icon;
    }

    public void ShowDetailView()
    {
        cancelLeaveLate();
        Btns.gameObject.SetActive(true);
        StartCoroutine("ShowDetailCor");
        ParentsBtn.OnZCommonItemExit = leaveLate;

        foreach (var item in UIManager.Instance.MapP.MapPointsDic)
        {
            if (item.Key != OnwerChannelName)
            {
                item.Value.Btns.gameObject.SetActive(false);
                item.Value.HideAllBtn();
            }
        }
    }

    public void HideAllBtn()
    {
        StopCoroutine("ShowDetailCor");
        TalkbackBtn.gameObject.SetActive(false);
        AddBtn.gameObject.SetActive(false);
        FocusBtn.gameObject.SetActive(false);
        TrackBtn.gameObject.SetActive(false);
        ParentsBtn.RunBtnExitLogic();
    }

    float speed = 20;
    float intervalRate = 4.5f;

    private IEnumerator ShowDetailCor()
    {
        TalkbackBtn.gameObject.SetActive(true);
        AddBtn.gameObject.SetActive(true);
        FocusBtn.gameObject.SetActive(true);
        TrackBtn.gameObject.SetActive(true);
        Color c = TalkbackBtn.NormalImage.color;
        Color c2 = TalkbackIcon.color;
        TalkbackBtn.NormalImage.color = new Color(c.r, c.g, c.b, 0);
        TalkbackIcon.color = new Color(c2.r, c2.g, c2.b, 0);
        AddBtn.NormalImage.color = new Color(c.r, c.g, c.b, 0);
        AddIcon.color = new Color(c2.r, c2.g, c2.b, 0);
        FocusBtn.NormalImage.color = new Color(c.r, c.g, c.b, 0);
        FocusIcon.color = new Color(c2.r, c2.g, c2.b, 0);
        TrackBtn.NormalImage.color = new Color(c.r, c.g, c.b, 0);
        TrackIcon.color = new Color(c2.r, c2.g, c2.b, 0);

        StartCoroutine(Shoot(TalkbackBtn, TalkbackIcon, c, c2));
        yield return new WaitForSeconds(1 / speed);
        StartCoroutine(Shoot(AddBtn, AddIcon, c, c2));
        yield return new WaitForSeconds(1 / speed);
        StartCoroutine(Shoot(FocusBtn, FocusIcon, c, c2));
        yield return new WaitForSeconds(1 / speed);
        StartCoroutine(Shoot(TrackBtn, TrackIcon, c, c2));
        yield return new WaitForSeconds(1 / speed);

    }


    private IEnumerator Shoot(ZUIButton btn, Image icon, Color c, Color c2)
    {
        float a = 0;
        while (a < 1)
        {
            a += 0.01f * intervalRate;
            btn.NormalImage.color = new Color(c.r, c.g, c.b, a);
            icon.color = new Color(c2.r, c2.g, c2.b, a);
            yield return null;
        }
        btn.NormalImage.color = new Color(c.r, c.g, c.b, 1); ;
        icon.color = new Color(c2.r, c2.g, c2.b, 1);
    }

    private void hideDetailView()
    {
        Btns.gameObject.SetActive(false);
        Track.fillAmount = 0;
        ParentsBtn.RunBtnExitLogic();
    }
    private void cancelLeaveLate()
    {
        if (isRun)
            StopCoroutine("leaveLateCor");
    }
    public void leaveLate()
    {
        if (isRun)
            StopCoroutine("leaveLateCor");
        StartCoroutine("leaveLateCor");
    }
    bool isRun = false;
    private IEnumerator leaveLateCor()
    {
        isRun = true;

        float time = 0;//time = 1
        while ((time += Time.deltaTime) < 2)
        {
            yield return null;
        }
        isRun = false;
        hideDetailView();
    }


    public void Focus()
    {
        //UIManager.Instance.MapP.Resize(MapZoomEndScale, MapZoomEndPos, 0.5f);
        UIManager.Instance.MapP.DOTweenResize(MapZoomEndScale, MapZoomEndPos, 0.5f);
        ParentsBtn.transform.DOLocalMove(ItemZoomEndPos, 0.5f);
        Btns.transform.DOLocalMove(ItemZoomEndPos + icon_btns_offset, 0.5f);

        UIManager.Instance.MapP.SyncAction = () =>
        {
            ParentsBtn.transform.DOLocalMove(DefaultPos, 0.5f);
            Tween tween = Btns.transform.DOLocalMove(DefaultPos + icon_btns_offset, 0.5f);
            tween.onComplete = () =>
            {
                foreach (var item in UIManager.Instance.MapP.MapPointsDic)
                {
                    if (item.Key != OnwerChannelName)
                    {
                        item.Value.gameObject.SetActive(true);
                    }
                }
            };
        };

        foreach (var item in UIManager.Instance.MapP.MapPointsDic)
        {
            if (item.Key != OnwerChannelName)
            {
                item.Value.gameObject.SetActive(false);
            }
        }
    }

    public void TalkbackClked()
    {
        UIManager.Instance.OpenHintPanel(Name);
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
        foreach (var item in UIManager.Instance.MapP.CallerMapItemPrefabs)
        {
            if (item.OnwerChannelName != OnwerChannelName)
            {
                item.Track.fillAmount = 0;
            }
        }
    }

    bool videoIsOpen = false;
    bool canclick = true;
    public void AddCallerToList()
    {
        if (!canclick) return;

        if (videoIsOpen)
        {
            MainController.Instance.ChannelDataDic[OnwerChannelName].SV.CloseBtnClked();
            UIManager.Instance.OpenHintPanel(Name, 2);
        }
        else
        {
            MainController.Instance.JoinMultiChannel(OnwerChannelName);
            UIManager.Instance.OpenHintPanel(Name, 1);
        }
        videoIsOpen = !videoIsOpen;

        StartCoroutine(WaitTIme());

    }

    private IEnumerator WaitTIme()
    {
        canclick = false;
        yield return new WaitForSeconds(0.6f);
        canclick = true;
    }

    public void SPDelete()
    {
        gameObject.SetActive(false);
        Track.fillAmount = 0;
    }
}
