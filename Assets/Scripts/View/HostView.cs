using agora_gaming_rtc;
using UnityEngine;
using UnityEngine.UI;

public class HostView : ViewBase
{
    public Text tips;
    public GameObject screen;
    public Transform ScreenListRoot;

    public override void LoadVideSurface(uint uid)
    {
        Debug.Log("onUserJoined: uid = " + uid);
        // this is called in main thread

        // find a game object to render video stream from 'uid'
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            return;
        }

        GameObject image = GameObject.Instantiate(screen, screen.transform.parent);
        image.SetActive(true);
        image.name = uid.ToString();
        //image.AddComponent<RawImage>();
        //image.transform.SetParent(ScreenListRoot);
        // create a GameObject and assign to this new user
        VideoSurface videoSurface = image.gameObject.AddComponent<VideoSurface>();
        if (!ReferenceEquals(videoSurface, null))
        {
            // configure videoSurface
            videoSurface.SetForUser(uid);
            videoSurface.SetEnable(true);
            videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            videoSurface.SetGameFps(30);
        }
        tips.text = "onUserJoined: uid = " + uid;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadVideSurface(123123123);
        }
    }

    public override void OnLoad(IStreamingProvider provider)
    {

    }

    public override void UnloadVideoSurface(uint uid)
    {
        Debug.Log("onUserOffline: uid = " + uid);
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            Object.Destroy(go);
        }
    }
}
