using System;
using agora_gaming_rtc;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public RawImage screen;
    VirtualCameraProvider provider;

    void Start()
    {
        provider = new VirtualCameraProvider();
        provider.Init(ReceiverFrame);
    }

    private void ReceiverFrame(ExternalVideoFrame frame)
    {
        screen.texture = provider.readableTexture;
    }

    private void OnDestroy()
    {
        provider.Dispose();
    }
}
