using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;
using NRKernal;
using System;

// this is an example of using Agora Unity SDK
// It demonstrates:
// How to enable video
// How to join/leave channel
// 
public class ZStreamingController
{
    // instance of agora engine
    private IRtcEngine mRtcEngine;
    private IStreamingProvider mStreamingProvider;
    public Text FrameCount;

    private void ReceiveFrame(ExternalVideoFrame frame)
    {
        if (mRtcEngine != null)
        {
            int a = mRtcEngine.PushVideoFrame(frame);
            Debug.Log("PushVideoFrame ret " + a);
        }
    }

    // load agora engine
    public void loadEngine(string appId)
    {
        // start sdk
        Debug.Log("initializeEngine");
        if (mRtcEngine != null)
        {
            Debug.Log("Engine exists. Please unload it first!");
            return;
        }

        // init engine
        mRtcEngine = IRtcEngine.GetEngine(appId);

        // enable log
        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
#if UNITY_ANDROID
        //mRtcEngine.setLogFile("/sdcard/log.log");
#endif
    }

    public void join(string channel, bool useVideo = true)
    {
        Debug.Log("calling join (channel = " + channel + ")");

        if (mRtcEngine == null)
            return;

        // set callbacks (optional)
        //mRtcEngine.OnRtcStats = (RtcStats stats) =>
        //{
        //    string rtcStatsMessage = string.Format("onRtcStats callback duration {0}, tx: {1}, rx: {2}, tx kbps: {3}, rx kbps: {4}, tx(a) kbps: {5}, rx(a) kbps: {6} users {7}",
        //        stats.duration, stats.txBytes, stats.rxBytes, stats.txKBitRate, stats.rxKBitRate, stats.txAudioKBitRate, stats.rxAudioKBitRate, stats.userCount);
        //    Debug.Log(rtcStatsMessage);

        //   int lengthOfMixingFile = mRtcEngine.GetAudioMixingDuration();
        //   int currentTs = mRtcEngine.GetAudioMixingCurrentPosition();

        //   string mixingMessage = string.Format("Mixing File Meta {0}, {1}", lengthOfMixingFile, currentTs);
        //   Debug.Log(mixingMessage);
        //};

        mRtcEngine.OnError += (int error, string msg) =>
        {
            string description = IRtcEngine.GetErrorDescription(error);
            string errorMessage = string.Format("onError callback {0} {1} {2}", error, msg, description);
            Debug.Log(errorMessage);
        };

        mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
        mRtcEngine.OnUserJoined = onUserJoined;
        mRtcEngine.OnUserOffline = onUserOffline;


        // enable video
        mRtcEngine.EnableVideo();
        // allow camera output callback
        mRtcEngine.EnableVideoObserver();

        if (!MainController.Instance.EnterChannel)
        {
            MainController.Instance.EnterChannel = true;
            // join channel
            mRtcEngine.JoinChannel(channel, null, 0);
        }

        MainController.Instance.UseVideo = useVideo;

        MainController.Instance.SmallViewPlayMode(MainController.Instance.UseVideo);

        Debug.Log(string.Format("[CZLOG] JoinChanel , usingVideo : {0} ", useVideo));
    }

    public string getSdkVersion()
    {
        return IRtcEngine.GetSdkVersion();
    }

    public void leave()
    {
        Debug.Log("calling leave");

        if (mRtcEngine == null)
            return;

        mStreamingProvider?.Dispose();

        // leave channel
        mRtcEngine.LeaveChannel();
        // deregister video frame observers in native-c code
        mRtcEngine.DisableVideoObserver();
    }

    public void MuteLocalAudioStream(bool mute)
    {
        mRtcEngine.MuteLocalAudioStream(mute);
    }

    // unload agora engine
    public void unloadEngine()
    {
        Debug.Log("calling unloadEngine");

        // delete
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();  // Place this call in ApplicationQuit
            mRtcEngine = null;
        }
    }

    public void EnableVideo(bool pauseVideo)
    {
        if (mRtcEngine != null)
        {
            if (!pauseVideo)
            {
                mRtcEngine.EnableVideo();
                mStreamingProvider?.Play();
            }
            else
            {
                mStreamingProvider?.Pause();
                mRtcEngine.DisableVideo();
            }
        }
    }

    // accessing GameObject in Scnene1
    // set video transform delegate for statically created GameObject
    public void onSceneHelloVideoLoaded()
    {
        ViewBase view = GameObject.FindObjectOfType<ViewBase>();
        if (view == null)
        {
            Debug.LogError("Can not find a view.");
            return;
        }

        view.OnLoad(mStreamingProvider);
    }

    // implement engine callbacks
    private void onJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        Debug.Log("JoinChannelSuccessHandler: uid = " + uid);
        //GameObject textVersionGameObject = GameObject.Find("VersionText");
        //textVersionGameObject.GetComponent<Text>().text = "SDK Version : " + getSdkVersion();
    }

    // When a remote user joined, this delegate will be called. Typically
    // create a GameObject to render video on it
    private void onUserJoined(uint uid, int elapsed)
    {
        Debug.Log("[VideoStreamingController] onUserJoined:" + uid);

        SmallView sv = MainController.Instance.PickSmallView(uid);


    }

    // When remote user is offline, this delegate will be called. Typically
    // delete the GameObject for this user
    private void onUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        Debug.Log("[VideoStreamingController] onUserOffline:" + uid);

        if (MainController.Instance.m_SmallViewsUid.Contains(uid))
        {
            MainController.Instance.m_SmallViewsUid.Remove(uid);
        }

        MainController.Instance.RefreshSmallView();
    }

    public void onSwitchCamera()
    {
        bool find_RGB = false;
        if (mRtcEngine != null)
        {
            IVideoDeviceManager dm = mRtcEngine.GetVideoDeviceManager();
            bool r = dm.CreateAVideoDeviceManager();
            Debug.Log("onSwitchCamera CreateAVideoDeviceManager " + r);

            int count = dm.GetVideoDeviceCount();
            Debug.Log("onSwitchCamera count " + count);

            for (int i = 0; i < count; i++)
            {
                string name = System.String.Empty;
                string id = System.String.Empty;
                dm.GetVideoDevice(i, ref name, ref id);
                Debug.Log("onSwitchCamera device " + name + " id: " + id);

                if (name.IndexOf("RGB") != -1)
                {
                    dm.SetVideoDevice(id);
                    Debug.Log("onSwitchCamera SetVideoDevice " + name + " id: " + id);
                    find_RGB = true;
                }
            }
#if UNITY_ANDROID
            if (!find_RGB)
            {
                Debug.Log("onSwitchCamera find_RGB " + find_RGB);
                int rc = mRtcEngine.SwitchCamera();
                Debug.Log("onSwitchCamera SwitchCamera return  " + rc);
            }
#endif
            dm.ReleaseAVideoDeviceManager();
        }
    }

    public void startExternalVideoSource()
    {
        Debug.Log("startExternalVideoSource ");
        //#if UNITY_ANDROID
        if (mRtcEngine != null)
        {
            mRtcEngine.SetExternalVideoSource(true, false);

            this.mStreamingProvider = StreamingProviderFactory.Create();
            this.mStreamingProvider.Init(ReceiveFrame);

            VideoEncoderConfiguration config = new VideoEncoderConfiguration();
            var resolution = this.mStreamingProvider.GetResolution();
            Debug.Log("startExternalVideoSource w:" + resolution.width + "h:" + resolution.height);

            config.bitrate = 2048;
            config.dimensions.width = resolution.width;
            config.dimensions.height = resolution.height;
            config.frameRate = FRAME_RATE.FRAME_RATE_FPS_15;
            mRtcEngine.SetVideoEncoderConfiguration(config);
        }
        //#endif
    }
}
