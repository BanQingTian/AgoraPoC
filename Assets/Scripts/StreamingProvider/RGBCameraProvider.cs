using agora_gaming_rtc;
using NRKernal;
using UnityEngine;

public class RGBCameraProvider : IStreamingProvider
{
    NRRGBCamTexture RGBCamTexture;
    StreamingReceiver m_Receiver;

    public RGBCameraProvider()
    {
        RGBCamTexture = new NRRGBCamTexture();
        RGBCamTexture.Play();
        RGBCamTexture.OnRawDataUpdate += OnRawDataUpdate;
    }

    public void Init(StreamingReceiver receiver)
    {
        this.m_Receiver = receiver;
    }

    public void Commit(ExternalVideoFrame frame)
    {
        m_Receiver(frame);
    }

    private void OnRawDataUpdate(RGBRawDataFrame obj)
    {
        this.Commit(GenerateAFrame(obj));
    }

    private long TimeStamp = 0;
    public ExternalVideoFrame GenerateAFrame(RGBRawDataFrame rgbframe)
    {
        ExternalVideoFrame frame = new ExternalVideoFrame();
        frame.type = ExternalVideoFrame.VIDEO_BUFFER_TYPE.VIDEO_BUFFER_RAW_DATA;
        frame.format = ExternalVideoFrame.VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_I420;
        frame.buffer = rgbframe.data;
        frame.stride = 0;
        frame.stride = RGBCamTexture.Width;
        frame.height = RGBCamTexture.Height;
        frame.timestamp = TimeStamp;
        TimeStamp++;

        return frame;
    }

    public Resolution GetResolution()
    {
        var resolution = new Resolution();
        resolution.width = RGBCamTexture.Width;
        resolution.height = RGBCamTexture.Height;

        return resolution;
    }

    public void Play()
    {
        RGBCamTexture.Play();
    }

    public void Pause()
    {
        RGBCamTexture.Pause();
    }

    public void Dispose()
    {
        RGBCamTexture.Stop();
    }
}