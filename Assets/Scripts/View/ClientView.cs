using agora_gaming_rtc;
using NRKernal;
using UnityEngine;
using UnityEngine.UI;

public class ClientView : ViewBase
{
    public Material material;
    public Transform ScreenListRoot;
    public GameObject previewScreen;

    private Vector3 originPos;

    private void Start()
    {
        originPos = ScreenListRoot.position;
    }

    public override void LoadVideSurface(uint uid)
    {
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            return; // reuse
        }

        GameObject image = GameObject.Instantiate(previewScreen, previewScreen.transform.parent);
        image.SetActive(true);
        image.name = uid.ToString();
        image.transform.SetParent(ScreenListRoot);
        image.transform.localScale = Vector3.one;
        image.transform.localPosition = Vector3.zero;
        image.GetComponent<RawImage>().material = material;

        // create a GameObject and assign to this new user
        VideoSurface videoSurface = image.AddComponent<VideoSurface>();
        if (!ReferenceEquals(videoSurface, null))
        {
            // configure videoSurface
            videoSurface.SetForUser(uid);
            videoSurface.SetEnable(true);
            videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            videoSurface.SetGameFps(30);
        }
    }

    private void Update()
    {
        BindPreviewToCamera3dof();
    }

    private void BindPreviewTOController()
    {
        var inputAnchor = NRInput.AnchorsHelper.GetAnchor(ControllerAnchorEnum.RightModelAnchor);
        ScreenListRoot.position = inputAnchor.TransformPoint(Vector3.forward * 0.3f);
        ScreenListRoot.forward = inputAnchor.forward;
    }

    private void BindPreviewToCamera3dof()
    {
        ScreenListRoot.position = originPos + Camera.main.transform.position;
    }

    public override void OnLoad(IStreamingProvider provider)
    {
        base.OnLoad(provider);
    }

    public override void UnloadVideoSurface(uint uid)
    {
        base.UnloadVideoSurface(uid);
    }
}