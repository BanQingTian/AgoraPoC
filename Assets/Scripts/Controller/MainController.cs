using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if(UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
using UnityEngine.UI;
using Zrime;
#endif


public enum CurMode
{
    Video,
    Audio,
    None,
}

[System.Serializable]
public class VPlayerData
{
    public Sprite Icon;
    public string Name;
    public string Playerid;
    public string WorkNumber;
    public string WorkType;
    public string channelName;
}


public class MainController : MonoBehaviour
{
    public Text TOO;

    public static MainController Instance;

    public List<SmallView> m_SmallViews = new List<SmallView>();
    public Dictionary<uint, SmallView> SmallViewDic = new Dictionary<uint, SmallView>();


    public Dictionary<string, AgoraChannelData> ChannelDataDic = new Dictionary<string, AgoraChannelData>();

    // Use this for initialization
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList();
#endif
    static ZStreamingController app = null;

    private string CurChannelName;
    private bool JoiningChannel = false;

    public bool SomeOneIsSpeaking = false;


    // PLEASE KEEP THIS App ID IN SAFE PLACE
    // Get your own App ID at https://dashboard.agora.io/
    private string AppID = "d8579ada9246484eb4e80eb15af8bbfb";



    [Space(24), SerializeField, Header("--------模拟数据-------")]
    public List<VPlayerData> VPS = new List<VPlayerData>();


    #region Unity_Interface

    void Awake()
    {
        Application.targetFrameRate = 60;
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        permissionList.Add(Permission.Microphone);
        permissionList.Add(Permission.Camera);
#endif
        Instance = this;
        // keep this alive across scenes
        DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {
        CheckPermissions();

        CheckAppId();

        InitLocalAgoraChannelData();
    }

    void OnApplicationPause(bool paused)
    {
        if (!ReferenceEquals(app, null))
        {
            app.EnableVideo(paused);
        }
    }

    void OnApplicationQuit()
    {
        if (!ReferenceEquals(app, null))
        {
            app.unloadEngine();
        }
    }

    #endregion

    // 注册默认的3组model
    public void InitLocalAgoraChannelData()
    {
        ChannelDataDic.Add("nreal1", new AgoraChannelData("nreal1", UIManager.Instance.MapP.CallerMapItemPrefabs[0], m_SmallViews[0]));
        ChannelDataDic.Add("nreal2", new AgoraChannelData("nreal2", UIManager.Instance.MapP.CallerMapItemPrefabs[1], m_SmallViews[1]));
        ChannelDataDic.Add("nreal3", new AgoraChannelData("nreal3", UIManager.Instance.MapP.CallerMapItemPrefabs[2], m_SmallViews[2]));
    }

    //// 设置当前加入的频道名称， 用于后续加入成功保持数据使用
    //public void SetCurJoinChannelName(string name)
    //{
    //    CurChannelName = name;
    //    JoiningChannel = true;
    //}
    //// 获取当前选取的频道名称，获取后初始化状态
    //public string GetCurChannelName()
    //{
    //    JoiningChannel = false;
    //    return CurChannelName;
    //}
    //// 判断当前专题是否可以继续加入频道
    //public bool GetJoinStatus()
    //{
    //    return JoiningChannel;
    //}

    public void JoinNewUser(string cn, uint uid)
    {
        var data = ChannelDataDic[cn];
        data.SetUID(uid);
        data.SV.SetUid(uid);
        data.SV.LoadVideSurface();
    }

    public VPlayerData GetVirtualData()
    {
        if (VPS.Count == 0)
        {
            Debug.LogError("VPC.count == 0 !!!!");
            return null;
        }
        VPlayerData data = VPS[0];
        VPS.RemoveAt(0);
        return data;
    }

    public void SetVideoMode(string channelName,bool openVideo)
    {
        ChannelDataDic[channelName].SV.OpenVideoMode();
    }

    #region UI Logic


    public SmallView PickSmallView(uint uid)
    {
        for (int i = 0; i < m_SmallViews.Count; i++)
        {
            if (SmallViewDic.ContainsKey(uid))
            {
                return SmallViewDic[uid];
            }
            if (!m_SmallViews[i].Dirty)
            {
                SmallViewDic.Add(uid, m_SmallViews[i]);
                m_SmallViews[i].SetUid(uid);
                m_SmallViews[i].LoadVideSurface();
                return m_SmallViews[i];
            }
        }

        Debug.Log("[CZLOG] No View Can Use");
        return null;
    }

    public void SmallViewPlayMode(bool useV)
    {
        foreach (var item in SmallViewDic)
        {
            if (useV)
                item.Value.OpenVideoMode();
            else
                item.Value.OpenAudioMode();
        }
    }

    #endregion



    #region Clk Handler

    public void OnMuteVoiceBtnClk(bool m)
    {
        app.MuteVoice(m);
    }

    public void OnSwitchCameraBtnClk()
    {
        Debug.Log("OnSwitchCameraBtnClk");
        app.onSwitchCamera();
    }

    public void OnMuteAudioBtnClk(bool mute)
    {
        Debug.Log(mute);
        app.MuteLocalAudioStream(mute);
    }

    public void OnAudioBtnClk()
    {
        joinChannel(false);
    }

    public void OnVideoBtnClk()
    {
        joinChannel(true);
    }

    public void OnLeaveBtnClk()
    {
        //if (!ReferenceEquals(app, null))
        //{
        //    app.leave();

        //    foreach (var item in SmallViewDic)
        //    {
        //        item.Value.Release();
        //    }
        //    SmallViewDic.Clear();

        //}

        app.leave();
    }

    #endregion


    #region Msg handler

    public void MsgHandler(Message msg)
    {
        // msg.Content = msgType,executor id,

        var arrs = msg.Content.Split(',');
        if (TOO != null)
        {
            TOO.text = arrs[0] + "\n";
            TOO.text += ZClient.Instance.PlayerID + "\n";
            TOO.text += arrs[1];
        }

        switch (arrs[0])
        {
            // 加入频道
            case "join_channel":
                if (ZClient.Instance.PlayerID == arrs[1])
                {
                    OnVideoBtnClk();
                }
                break;

            // 离开频道
            case "leave_channel":
                if (ZClient.Instance.PlayerID == arrs[1])
                {
                    // fresh ui
                    if (UIManager_SampleMode.Instance != null)
                    {
                        UIManager_SampleMode.Instance.smcp.CloseChannel();
                    }
                }
                break;

            case "leave_channel_pass_sample_mode":
                {
                    if (ZMain.Instance.isMaster)
                    {
                        UIManager.Instance.LeftConPanel.RemoveCallerToWaitingList(msg.PlayerId);
                    }
                }
                break;

            // 发言ing
            case "open_speaking":
                SomeOneIsSpeaking = true;
                if (ZClient.Instance.PlayerID == arrs[1])
                {
                    Debug.LogError("open_speaking");
                    app.MuteLocalAudioStream(false);
                }
                break;

            // 结束发言
            case "close_speaking":
                SomeOneIsSpeaking = false;
                if (ZClient.Instance.PlayerID == arrs[1])
                {
                    Debug.LogError("close_speaking");
                    app.MuteLocalAudioStream(true);
                }
                break;

            // 全部打开麦克风
            case "open_all_local_voice":
                app.MuteLocalAudioStream(false);
                break;

            // 全部关闭麦克风
            case "mute_all_local_voice":
                app.MuteLocalAudioStream(true);
                break;

            case "audio_mode": // for sample mode
                if (!ZMain.Instance.isMaster)
                {
                    UIManager_SampleMode.Instance.OpenAudioModeUI();
                }
                break;

            case "video_mode": // for sample mode
                if (!ZMain.Instance.isMaster)
                {
                    UIManager_SampleMode.Instance.OpenVideoModeUI();
                }
                break;

            case "none_mode":
                if (!ZMain.Instance.isMaster)
                {
                    Debug.Log(33333333333333);
                    UIManager_SampleMode.Instance.OpenDisconnectUI();
                }
                break;


            default:
                break;
        }

    }

    #endregion


    public void JoinMultiChannel(string channelname)
    {
        if (ReferenceEquals(app, null))
        {
            app = new ZStreamingController();
            app.loadEngine(AppID);
        }
        app.JoinMultiChannel(channelname, true);
    }
    public void JoinChannelForAndroid(string cname)
    {
        if (ReferenceEquals(app, null))
        {
            app = new ZStreamingController();
            app.loadEngine(AppID);
        }
        app.join(cname);
    }
    private void joinChannel(bool useVideo = true)
    {
        if (ReferenceEquals(app, null))
        {
            app = new ZStreamingController();
            app.loadEngine(AppID);
        }
        app.join(CurChannelName, useVideo);
    }




    private void CheckAppId()
    {
        Debug.Assert(AppID.Length > 10, "Please fill in your AppId first on Game Controller object.");
    }

    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        foreach (string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
            }
        }
#endif
    }




}
