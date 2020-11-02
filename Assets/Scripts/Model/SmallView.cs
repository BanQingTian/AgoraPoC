using agora_gaming_rtc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallView : MonoBehaviour
{
    [SerializeField]
    private RawImage m_Image;
    [SerializeField]
    private uint m_Uid;
    public Texture defaultTex = null;

    public string ChannelName;

    private bool isDirty = false;

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
        m_Image.transform.SetAsLastSibling();

        MainController.Instance.m_SmallViews.Remove(this);
        MainController.Instance.m_SmallViews.Add(this);

        //gameObject.SetActive(false);
    }

    public void LoadVideSurface()
    {
        if (defaultTex == null)
            defaultTex = m_Image.texture;
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

    }
}
