using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallerDetailItem : MonoBehaviour
{
    public Text Name;
    public Text JobNumber;
    public Text JobType;

    public Image Icon;
    public CallerPanel ParentsPanel;
    public ZUIButton Btn;

    public VPlayerData m_data;

    public string PlayerId { get; set; }

    public void AddListener()
    {
        Btn.OnZCommonItemUp += MoveToChannel_BtnClked;
        Btn.OnZCommonItemEnter += Enter;
        Btn.OnZCommonItemExit += Exit;
    }

    public void SetData(VPlayerData pd)
    {
        Debug.Log(pd.Icon.name + "===========");
        m_data = pd;
        Name.text = pd.Name;
        JobNumber.text = pd.WorkNumber;
        JobType.text = pd.WorkType;
        Icon.sprite = pd.Icon;
    }

    private void Enter()
    {
        SetTextColor(Color.black);
    }
    private void Exit()
    {
        SetTextColor(Color.white);
    }
    private void SetTextColor(Color c)
    {
        Name.color = c;
        JobNumber.color = c;
        JobType.color = c;
    }

    public void SetName(string callerName)
    {
        Name.text = callerName;
    }

    public void SetIcon(Sprite s)
    {
        Icon.sprite = s;
    }

    public void MoveToChannel_BtnClked()
    {
        ParentsPanel.MoveCallerToChannel(PlayerId);
        //ParentsPanel.gameObject.SetActive(false);
    }
}
