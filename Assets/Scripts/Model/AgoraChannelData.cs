using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;

public class AgoraChannelData
{
    public AgoraChannelData(string _ChannelName, SmallPoint _SP = null, SmallView _SV = null)
    {
        ChannelName = _ChannelName;
        SP = _SP;
        SV = _SV;
        SV.ChannelName = _ChannelName;
        SP.OnwerChannelName = _ChannelName;
    }

    public void SetUID(uint uid)
    {
        UID = uid;
    }

    public AgoraChannel AC;
    public string ChannelName;
    public uint UID;
    public SmallPoint SP; // 地图对应view
    public SmallView SV; // 视频对应view
}
