using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallerDetailItem : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Name;
    public TMPro.TextMeshProUGUI JobNumber;
    public Text JobType;

    public Image Icon;
    public CallerPanel ParentsPanel;
    public ZUIButton Btn;

    private bool add = false;
    public Image Tick_Normal;
    public Image Tick_Normal_Parent;
    public Image Tick_Hover;


    public VPlayerData m_data;

    public string PlayerId { get; set; }

    public void AddListener()
    {
        Btn.OnZCommonItemUp += AddToReadyListUntilClkConfirm;
        Btn.OnZCommonItemEnter += Enter;
        Btn.OnZCommonItemExit += Exit;
    }

    public void SetData(VPlayerData pd)
    {
        Debug.Log(pd.Icon.name + "===========");
        m_data = pd;
        string nN = "";
        for (int i = 0; i < pd.Name.Length; i++)
        {
            if (i == 0) continue;
            nN += pd.Name[i];
        }
        Name.text = string.Format("<b>{0}</b> {1}", pd.Name[0], nN);
        JobNumber.text = "ID " + pd.WorkNumber;
        JobType.text = pd.WorkType;
        Icon.sprite = pd.Icon;
    }

    private void Enter()
    {
        for (int i = 0; i < ParentsPanel.CallerList.Count; i++)
        {
            if (ParentsPanel.CallerList[i].PlayerId == PlayerId)
            {
                ParentsPanel.LineHide(i);
            }
        }
    }
    private void Exit()
    {
        ParentsPanel.LineShow();
    }

    public void AddToReadyListUntilClkConfirm()
    {
        add = !add;
        Tick_Normal.gameObject.SetActive((add));
        Tick_Normal_Parent.color = add ? new Color(1, 0.63f, 0.149f, 1) : Color.white;
        Tick_Hover.gameObject.SetActive((add));

        if (add)
        {
            if (!ParentsPanel.ReadyList.ContainsKey(PlayerId))
            {
                ParentsPanel.ReadyList.Add(PlayerId, m_data);
            }
            else
            {
                Debug.LogError("ParentsPanel.ReadyList.ContainsKey(PlayerId) == true");
            }
        }
        else
        {
            if (ParentsPanel.ReadyList.ContainsKey(PlayerId))
            {
                ParentsPanel.ReadyList.Remove(PlayerId);
            }
            else
            {
                Debug.LogError("ParentsPanel.ReadyList.ContainsKey(PlayerId) == false");
            }
        }

    }
    public void MoveToChannel_BtnClked()
    {
        ParentsPanel.MoveCallerToChannel(PlayerId);
        //ParentsPanel.gameObject.SetActive(false);
    }
}
