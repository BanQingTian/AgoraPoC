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
    public Transform MainPanel; // all ui parents
    public LeftConnectionPanel LeftConPanel;
    public CallerPanel CallerP;
    public MapPanel MapP;
    public VideoPanel VideoP;
    public BarPanel BarP;
    public HintPanel HintP;
    public MemberPanel MemberP;

    public Transform Point;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 移除手柄的模型
        NRInput.ControllerVisualActive = false;

        LeftConPanel.AddListener();
        CallerP.AddListener();
        MapP.AddListener();
        VideoP.AddListener();
        BarP.AddListener();
        HintP.AddListener();
        MemberP.AddListener();
    }


    #region UILogic

    public void OpenHintPanel(string name)
    {
        Debug.Log("OpenHintPanel(string name)");
        HintP.Name.text = name;
        StartCoroutine("Shoot");
    }
    public void HideHint()
    {
        StopCoroutine("Shoot");
        HintP.gameObject.SetActive(false);
    }
    private IEnumerator Shoot()
    {
        HintP.gameObject.SetActive(true);
        HintP.Part1.SetActive(true);
        HintP.Part2.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        HintP.Part1.SetActive(false);
        HintP.Part2.SetActive(true);
        yield return new WaitForSeconds(3f);
        HintP.gameObject.SetActive(false);
    }

    #endregion


    #region Invalid

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
        MapP.DeleteMapCaller(playerid);
    }

    #endregion


}
