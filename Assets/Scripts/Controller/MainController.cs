using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if(UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
using UnityEngine.UI;
using Zrime;
#endif

public class MainController : MonoBehaviour
{

    public Text TOO;

    public static MainController Instance;

    public static uint CurUid;
    public static string CurChannelName = "chenzhuo";

    public List<SmallView> m_SmallViews = new List<SmallView>();

    public Dictionary<uint, SmallView> SmallViewDic = new Dictionary<uint, SmallView>();

    // Use this for initialization
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList();
#endif
    static ZStreamingController app = null;

    private const string ChannelName = "chenzhuo";

    public bool SomeOneIsSpeaking = false;


    // PLEASE KEEP THIS App ID IN SAFE PLACE
    // Get your own App ID at https://dashboard.agora.io/
    private string AppID = "d8579ada9246484eb4e80eb15af8bbfb";


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
                CurUid = uid;
                m_SmallViews[i].SetUid(uid);
                m_SmallViews[i].LoadVideSurface();
                return m_SmallViews[i];
            }
        }

        Debug.Log("[CZLOG] No View Can Use");
        return null;
    }

    #endregion



    #region Clk Handler

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
        if (!ReferenceEquals(app, null))
        {
            app.leave();
            SmallView sv = null;
            if (MainController.Instance.SmallViewDic.TryGetValue(CurUid, out sv))
            {
                sv.Release();
                MainController.Instance.SmallViewDic.Remove(CurUid);
            }
        }
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
                    OnLeaveBtnClk();
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
                if (ZClient.Instance.PlayerID != msg.PlayerId)
                {
                    app.MuteLocalAudioStream(true);
                    SomeOneIsSpeaking = true;
                }
                break;

            // 结束发言
            case "close_speaking":
                if (ZClient.Instance.PlayerID != msg.PlayerId)
                {
                    app.MuteLocalAudioStream(false);
                    SomeOneIsSpeaking = false;
                }
                break;

            default:
                break;
        }

    }

    #endregion


    private void joinChannel(bool useVideo = true)
    {
        if (ReferenceEquals(app, null))
        {
            app = new ZStreamingController();
            app.loadEngine(AppID);
        }
        app.join(ChannelName, useVideo);
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
