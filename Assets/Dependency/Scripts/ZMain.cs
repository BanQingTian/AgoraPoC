using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public class ZMain : MonoBehaviour
{
    [HideInInspector]
    public bool isMaster = true;

    [Space(12)]
    public DeviceTypeEnum DeviceType;

    [Space(12)]
    public LanguageEnum LanguageType;

    [Space(12)]
    public string IPAdress = "127.0.0.1";

    void Start()
    {
        Begin();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 5;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Time.timeScale = 1;
        }

#endif

    }

    private void Begin()
    {
        deviceCheck();

        LoadManager();
        LoadLocalization();
        LoadNetworkingModule();
    }

    private void deviceCheck()
    {
        Global.DeviceType = DeviceType;

        if (DeviceType == DeviceTypeEnum.NRLight)
        {
            var nrCam = GameObject.Find("NRCameraRig");
            var nrInput = NRInput.AnchorsHelper.GetAnchor(ControllerAnchorEnum.RightModelAnchor);
            ZClient.Instance.Model = nrCam;
            ZClient.Instance.Controller = nrInput;
            ZClient.Instance.extraContent = "";

        }
        else if (DeviceType == DeviceTypeEnum.Pad)
        {
            var arCam = GameObject.Find("First Person Camera");
            ZClient.Instance.Model = arCam;
        }
        else
        {

        }
    }

    public void LoadManager()
    {
        // GameManager.Instance.Init(this);
    }

    public void LoadLocalization()
    {
        Global.Languge = LanguageType;
        // ZLocalizationHelper.Instance.Switch(LanguageType);
    }



    // 网络所需组件，实例化网络组件
    public void LoadNetworkingModule()
    {
        ZMessageManager.Instance.Init();
        ZMessageManager.Instance.SendConnectAndJoinRoom(ZUtils.GetIPAdress(IPAdress), "50010"); //192.168.0.33 //192.168.69.39
    }


}
