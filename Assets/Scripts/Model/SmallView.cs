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
    private Texture defaultTex = null;

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

    public void Release()
    {
        isDirty = false;
        m_Image.texture = defaultTex;
        m_Uid = 0;
        Destroy(GetComponent<VideoSurface>());
    }

    public void LoadVideSurface()
    {
        Debug.Log("[CZLOG]  LoadVideSurface: uid = " + m_Uid + "UseVideo : " + MainController.Instance.UseVideo);

        var sv = Image.gameObject.AddComponent<VideoSurface>();
        if (!ReferenceEquals(sv, null))
        {
            sv.enabled = MainController.Instance.UseVideo;
            // configure videoSurface
            sv.SetForUser(m_Uid);
            sv.SetEnable(MainController.Instance.UseVideo);
            sv.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            sv.SetGameFps(30);
        }

    }
}
