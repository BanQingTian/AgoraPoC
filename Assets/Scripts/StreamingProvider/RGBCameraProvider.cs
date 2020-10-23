using agora_gaming_rtc;
using NRKernal;
using UnityEngine;


public class RGBCameraProvider : CameraModelView,IStreamingProvider
{
    StreamingReceiver m_Receiver;

    public RGBCameraProvider()
    {
       
    }

    protected override void OnRawDataUpdate(FrameRawData rgbRawDataFrame)
    {
        this.Commit(GenerateAFrame(rgbRawDataFrame));
    }

    public void Init(StreamingReceiver receiver)
    {
        this.m_Receiver = receiver;
    }

    public void Commit(ExternalVideoFrame frame)
    {
        m_Receiver(frame);
    }

    private long TimeStamp = 0;
    public ExternalVideoFrame GenerateAFrame(FrameRawData rgbframe)
    {
        ExternalVideoFrame frame = new ExternalVideoFrame();
        frame.type = ExternalVideoFrame.VIDEO_BUFFER_TYPE.VIDEO_BUFFER_RAW_DATA;
        frame.format = ExternalVideoFrame.VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_I420;
        frame.buffer = rgbframe.data;
        frame.stride = 0;
        frame.stride = this.Width;
        frame.height = this.Height;
        frame.timestamp = TimeStamp;
        TimeStamp++;

        return frame;
    }

    public Resolution GetResolution()
    {
        var resolution = new Resolution();
        resolution.width = this.Width;
        resolution.height = this.Height;

        return resolution;
    }

    public void Begin()
    {
        base.Play();
    }

    public void End()
    {
        base.Pause();
    }

    public void Dispose()
    {
        base.Stop();
    }
}