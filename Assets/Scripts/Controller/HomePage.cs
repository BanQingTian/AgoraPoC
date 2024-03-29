﻿using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
#if(UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif
using agora_gaming_rtc;

/// <summary>
///    TestHome serves a game controller object for this application.
/// </summary>
public class HomePage : MonoBehaviour
{
    // Use this for initialization
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList();
#endif
    static VideoStreamingController app = null;

    private string HomeSceneName = "Home";
    private string PlaySceneForAndroid = "VideoPageForAndroid";
    private string PlaySceneForWindows = "VideoPageForWindows";

    public Toggle useDebugToggle;
    public static bool useDebug = false;

    // PLEASE KEEP THIS App ID IN SAFE PLACE
    // Get your own App ID at https://dashboard.agora.io/
    [SerializeField]
    private string AppID = "your_appid";

    void Awake()
    {
        Application.targetFrameRate = 60;
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        permissionList.Add(Permission.Microphone);
        permissionList.Add(Permission.Camera);
#endif

        // keep this alive across scenes
        DontDestroyOnLoad(this.gameObject);

        useDebugToggle.onValueChanged.AddListener((select) =>
        {
            useDebug = select;
        });
    }

    void Start()
    {
        CheckAppId();

        this.onJoinButtonClicked();
    }

    void Update()
    {
        CheckPermissions();
    }

    private void CheckAppId()
    {
        Debug.Assert(AppID.Length > 10, "Please fill in your AppId first on Game Controller object.");
    }

    /// <summary>
    ///   Checks for platform dependent permissions.
    /// </summary>
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

    public void onJoinButtonClicked()
    {
        // get parameters (channel name, channel profile, etc.)
        GameObject go = GameObject.Find("ChannelName");
        InputField field = go.GetComponent<InputField>();

        // create app if nonexistent
        if (ReferenceEquals(app, null))
        {
            app = new VideoStreamingController(); // create app
            app.loadEngine(AppID); // load engine
        }
        // join channel and jump to next scene
        app.join(field.text);
        SceneManager.sceneLoaded += OnLevelFinishedLoading; // configure GameObject after scene is loaded

        if (Application.platform == RuntimePlatform.Android)
        {
            SceneManager.LoadScene(PlaySceneForAndroid, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(PlaySceneForWindows, LoadSceneMode.Single);
        }
    }

    public void onLeaveButtonClicked()
    {
        if (!ReferenceEquals(app, null))
        {
            app.leave(); // leave channel
            app.unloadEngine(); // delete engine
            app = null; // delete app
            SceneManager.LoadScene(HomeSceneName, LoadSceneMode.Single);
        }
        Destroy(gameObject);
    }

    public void onSwitchButtonClicked()
    {
        if (!ReferenceEquals(app, null))
        {
            app.onSwitchCamera();
        }
    }

    public void onExternalCameraClicked()
    {
        if (!ReferenceEquals(app, null))
        {
            app.startExternalVideoSource();
        }
    }

    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == PlaySceneForWindows)
        {
            if (!ReferenceEquals(app, null))
            {
                app.onSceneHelloVideoLoaded(); // call this after scene is loaded
            }
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
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
}
