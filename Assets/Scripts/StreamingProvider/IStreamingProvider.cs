using agora_gaming_rtc;
using UnityEngine;
using System;


public interface IStreamingProvider:IDisposable
{
    void Init(StreamingReceiver receiver);

    void Commit(ExternalVideoFrame frame);

    void Begin();

    void End();

    Resolution GetResolution();
}

public delegate void StreamingReceiver(ExternalVideoFrame frame);