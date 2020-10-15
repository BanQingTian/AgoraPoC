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

    public void OpenAudioMode()
    {
        VideoSurface vs = GetComponent<VideoSurface>();
        if(vs != null)
        {
            vs.SetEnable(false);
        }
        m_Image.texture = defaultTex;
    }
    public void OpenVideoMode()
    {
        VideoSurface vs = GetComponent<VideoSurface>();
        if (vs != null)
        {
            vs.SetEnable(true);
        }
    }

    public void Release()
    {
        isDirty = false;
        m_Image.texture = defaultTex;
        m_Uid = 0;
        Destroy(Image.gameObject.GetComponent<VideoSurface>());
    }

    public void LoadVideSurface()
    {
        Debug.Log("onUserJoined: uid = " + m_Uid);

        VideoSurface videoSurface = Image.gameObject.AddComponent<VideoSurface>();
        if (!ReferenceEquals(videoSurface, null))
        {
            // configure videoSurface
            videoSurface.SetForUser(m_Uid);
            videoSurface.SetEnable(true);
            videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            videoSurface.SetGameFps(30);
        }
    }
}
