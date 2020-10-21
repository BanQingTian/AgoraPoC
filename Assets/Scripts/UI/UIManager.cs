using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    //[HideInInspector]
    //public bool BtnHovering = false;

    // 用于注册模拟鼠标
    public GazeTracker VirturlMouseHelper;

    [Space(12)]
    public LeftConnectionPanel LeftConPanel;
    public CallerPanel CallerP;

    public Transform Point;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        init();
        LeftConPanel.AddListener();
        CallerP.AddListener();
    }


    private void init()
    {
        // 移除手柄原因的模型
        NRInput.ControllerVisualActive = false;
    }

    #region Logic

    public void UpdateCallerPanel(string playerid, bool add = true)
    {
        if (add)
        {
            CallerP.CreateCallerToWaitingList(playerid);
        }
        else
        {
            DeleteCaller(playerid);
        }
    }


    public void DeleteCaller(string playerid)
    {
        CallerP.DeleteCaller(playerid);
        LeftConPanel.DeleteCaller(playerid);
    }

    #endregion


}
