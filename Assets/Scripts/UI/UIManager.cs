using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public enum MOVEBAR
{
    MAP = 0,
    MEMBER,
    VIDEO1,// channel1 is center
    VIDEO2,
    VIDEO3,
}

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
    public Animator Anim;

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

        GetPanelInitPose();
    }

    private void Update()
    {
        if (NRInput.GetButtonDown(ControllerButton.APP) && !BarP.isSelect)
        {
            ResetAllPanel();
            NRInput.RecenterController();
        }
    }

    bool once = false;
    float time = 0;
    private bool CheckDoubleClk()
    {
        if (once)
        {
            time += Time.deltaTime;
        }

        if (time < 0.6f)
        {
            if (once && NRInput.GetButtonDown(ControllerButton.APP))
            {
                return true;
            }
        }

        if (NRInput.GetButtonDown(ControllerButton.APP))
        {
            once = true;
        }
       

        if (time >= 0.6f)
        {
            time = 0;
            once = false;
        }

        return false;

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
        yield return new WaitForSeconds(1f);
        HintP.Part1.SetActive(false);
        HintP.Part2.SetActive(true);
        yield return new WaitForSeconds(8f);
        HintP.gameObject.SetActive(false);
    }

    public void PlayBarAnim(MOVEBAR mb, bool enter = true)
    {
        switch (mb)
        {
            case MOVEBAR.MAP:
                if (enter)
                {
                    Anim.Play("MoveAnim_Map");
                }
                else
                {
                    Anim.Play("MoveAnim_Map_Inverse");
                }

                break;
            case MOVEBAR.MEMBER:
                if (enter)
                {
                    Anim.Play("MoveAnim_Member");
                }
                else
                {
                    Anim.Play("MoveAnim_Member_Inverse");
                }
                break;
            case MOVEBAR.VIDEO1:
                if (enter)
                {
                    Anim.Play("MoveAnim_Video1");
                }
                else
                {
                    Anim.Play("MoveAnim_Video1_Inverse");
                }
                break;
            case MOVEBAR.VIDEO2:
                if (enter)
                {
                    Anim.Play("MoveAnim_Video2");
                }
                else
                {
                    Anim.Play("MoveAnim_Video2_Inverse");
                }
                break;
            case MOVEBAR.VIDEO3:
                if (enter)
                {
                    Anim.Play("MoveAnim_Video3");
                }
                else
                {
                    Anim.Play("MoveAnim_Video3_Inverse");
                }
                break;
            default:
                break;
        }
    }

    private List<Pose> MovableGOPoseList = new List<Pose>();
    private List<Vector3> MovableGOScaleList = new List<Vector3>();
    private void GetPanelInitPose()
    {
        MovableGOPoseList.Add(new Pose(BarP.VideoMoveCenter.transform.localPosition, BarP.VideoMoveCenter.transform.localRotation));
        MovableGOPoseList.Add(new Pose(BarP.MapMoveCenter.transform.localPosition, BarP.MapMoveCenter.transform.localRotation));
        MovableGOPoseList.Add(new Pose(BarP.MemberMoveCenter.transform.localPosition, BarP.MemberMoveCenter.transform.localRotation));
        MovableGOPoseList.Add(new Pose(BarP.MainPanel.transform.localPosition, BarP.MainPanel.transform.localRotation));

        MovableGOScaleList.Add(BarP.VideoMoveCenter.transform.localScale);
        MovableGOScaleList.Add(BarP.MapMoveCenter.transform.localScale);
        MovableGOScaleList.Add(BarP.MemberMoveCenter.transform.localScale);
        MovableGOScaleList.Add(BarP.MainPanel.transform.localScale);

    }
    public void ResetAllPanel()
    {
        BarP.VideoMoveCenter.transform.localPosition = MovableGOPoseList[0].position;
        BarP.VideoMoveCenter.transform.localRotation = MovableGOPoseList[0].rotation;

        BarP.MapMoveCenter.transform.localPosition = MovableGOPoseList[1].position;
        BarP.MapMoveCenter.transform.localRotation = MovableGOPoseList[1].rotation;

        BarP.MemberMoveCenter.transform.localPosition = MovableGOPoseList[2].position;
        BarP.MemberMoveCenter.transform.localRotation = MovableGOPoseList[2].rotation;

        BarP.MainPanel.transform.localPosition = MovableGOPoseList[3].position;
        BarP.MainPanel.transform.localRotation = MovableGOPoseList[3].rotation;

        BarP.VideoMoveCenter.transform.localScale = MovableGOScaleList[0];
        BarP.MapMoveCenter.transform.localScale = MovableGOScaleList[1];
        BarP.MemberMoveCenter.transform.localScale = MovableGOScaleList[2];
        BarP.MainPanel.transform.localScale = MovableGOScaleList[3];
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
