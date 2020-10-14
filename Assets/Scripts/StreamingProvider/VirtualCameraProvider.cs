using agora_gaming_rtc;
using UnityEngine;
using System.Collections;

public class VirtualCameraProvider : IStreamingProvider
{
    Camera streamingCamera;
    StreamingReceiver m_Receiver;

    public int width = 1280;
    public int height = 720;
    private long TimeStamp = 0;

    private RenderTexture targetTexture;
    public Texture2D readableTexture;

    public VirtualCameraProvider()
    {
        Debug.Log("create VirtualCameraProvider");
        CameraProviderUpdater.Instance.StartCoroutine(RenderCorution());
    }

    public void Init(StreamingReceiver receiver)
    {
        this.m_Receiver = receiver;
    }

    public IEnumerator RenderCorution()
    {
        yield return new WaitForSeconds(1f);

        if (streamingCamera == null)
        {
            //streamingCamera = GameObject.Instantiate(Camera.main);
            //GameObject.DontDestroyOnLoad(streamingCamera.gameObject);
            //streamingCamera.depthTextureMode = DepthTextureMode.Depth;

            targetTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);

            streamingCamera.targetTexture = targetTexture;
            readableTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        }

        Debug.LogError("-----------------------");

        while (true)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            if (targetTexture != null)
            {
                ExternalVideoFrame frame = GenerateAFrame(targetTexture);
                this.Commit(frame);
            }
        }
    }

    public void Commit(ExternalVideoFrame frame)
    {
        m_Receiver?.Invoke(frame);
    }

    public ExternalVideoFrame GenerateAFrame(RenderTexture source)
    {
        ExternalVideoFrame frame = new ExternalVideoFrame();
        frame.type = ExternalVideoFrame.VIDEO_BUFFER_TYPE.VIDEO_BUFFER_RAW_DATA;
        frame.format = ExternalVideoFrame.VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_BGRA;
        frame.buffer = GetRawDataByRT(source);
        frame.stride = source.width;
        frame.height = source.height;
        frame.timestamp = TimeStamp;
        TimeStamp++;
        return frame;
    }

    private byte[] GetRawDataByRT(RenderTexture rt)
    {
        byte[] rawdata;
        var temp = RenderTexture.active;
        RenderTexture.active = rt;
        readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTexture.Apply();
        rawdata = readableTexture.GetRawTextureData();
        RenderTexture.active = temp;
        return rawdata;
    }

    public Resolution GetResolution()
    {
        var resolution = new Resolution();
        resolution.width = width;
        resolution.height = height;

        return resolution;
    }

    public void Play()
    {
        CameraProviderUpdater.Instance.StopCoroutine(RenderCorution());
        CameraProviderUpdater.Instance.StartCoroutine(RenderCorution());
    }

    public void Pause()
    {
        CameraProviderUpdater.Instance.StopCoroutine(RenderCorution());
    }

    public void Dispose()
    {
        CameraProviderUpdater.Instance.StopCoroutine(RenderCorution());
        if (streamingCamera != null)
        {
            GameObject.Destroy(streamingCamera.gameObject);
            streamingCamera = null;
            targetTexture.Release();
            targetTexture = null;
            GameObject.Destroy(readableTexture);
            readableTexture = null;
        }
    }
}