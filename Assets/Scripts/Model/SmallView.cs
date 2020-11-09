using agora_gaming_rtc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ViewActionMode
{
    big, // channel_1
    small // channel_2_3
}
public class SmallView : MonoBehaviour
{
    [SerializeField]
    private RawImage m_Image;
    [SerializeField]
    private uint m_Uid;
    public Texture defaultTex = null;

    public string ChannelName;

    public ViewActionMode VAM = ViewActionMode.small;

    public ZBtn2 SwitchBtn;

    public ZUIButton CloseBtn;
    public TMPro.TextMeshProUGUI LabelTip;

    private bool isDirty = false;

    private void Start()
    {
        SwitchBtn.ClkUp = ClkBtn;
        CloseBtn.OnZCommonItemUp = () => 
        {
            Release();
           MainController.Instance.ChannelDataDic[ChannelName].AC?.LeaveChannel();
        };
    }

    public bool Dirty
    {
        get { return isDirty; }
    }

    public RawImage Image
    {
        get { return m_Image; }
    }

    public void SetUid(uint uid)
    {
        isDirty = true;
        m_Uid = uid;
        gameObject.SetActive(true);
    }
    public uint GetUid()
    {
        return m_Uid;
    }

    public void OpenAudioMode()
    {
        var s = Image.gameObject.GetComponent<VideoSurface>();
        if (s != null)
        {
            s.enabled = false;
            s.SetEnable(false);
        }
        m_Image.texture = defaultTex;
    }
    public void OpenVideoMode()
    {
        var s = Image.gameObject.GetComponent<VideoSurface>();
        if (s != null)
        {
            s.enabled = true;
            s.SetEnable(true);
        }
    }
    public void CloseVideoRenderer()
    {
        var s = Image.gameObject.GetComponent<VideoSurface>();
        if (s != null)
        {
            s.enabled = false;
            s.SetEnable(false);
        }
        m_Image.texture = defaultTex;
    }

    public void Release()
    {
        isDirty = false;
        m_Image.texture = defaultTex;
        m_Uid = 0;
        Destroy(GetComponent<VideoSurface>());

        CloseBtn.gameObject.SetActive(false);

        LabelTip.text = "未连接";
    }

    public void LoadVideSurface()
    {
        if (defaultTex == null)
            defaultTex = m_Image.texture;
        Image.texture = null;
        var sv = Image.gameObject.AddComponent<VideoSurface>();
        if (!ReferenceEquals(sv, null))
        {
            sv.enabled = true;
            // configure videoSurface
            //sv.SetForUser(m_Uid);
            //sv.SetEnable(true);
            //sv.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            //sv.SetGameFps(30);
            sv.SetForMultiChannelUser(ChannelName, m_Uid);
        }
        CloseBtn.gameObject.SetActive(true);
        LabelTip.text = ChannelName.Replace("nreal", "5G成员");
    }


    public void ClkBtn()
    {
        switch (VAM)
        {
            case ViewActionMode.big:

                if(UIManager.Instance.VideoP.mode == ZViewMode.SurroundMode)
                {
                    UIManager.Instance.BarP.SetVideoBarHoverMode(ZViewMode.MainSubMode);
                    UIManager.Instance.VideoP.MainSubBtnClked();
                }


                break;
            case ViewActionMode.small:

                if(UIManager.Instance.VideoP.mode == ZViewMode.MainSubMode)
                {
                    UIManager.Instance.VideoP.SetSmallLayout(ChannelName);
                    UIManager.Instance.BarP.SetVideoBarHoverMode(ZViewMode.SurroundMode);
                    UIManager.Instance.VideoP.SurroundBtnClked();
                }



                break;
            default:
                break;
        }
    }
}
