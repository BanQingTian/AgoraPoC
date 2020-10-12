using UnityEngine;

public class StreamingProviderFactory
{
    public static IStreamingProvider Create()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            return new RGBCameraProvider();
        }
        else
        {
            return new VirtualCameraProvider();
        }
    }
}