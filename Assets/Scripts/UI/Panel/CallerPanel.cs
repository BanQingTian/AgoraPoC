using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallerPanel : ZBasePanel
{
    [Space(24)]
    public LeftConnectionPanel LeftConPanel;

    public Transform DetailLayoutParent;
    public CallerDetailItem ItemPrefab;
    public Dictionary<string, CallerDetailItem> CallerDic = new Dictionary<string, CallerDetailItem>();
    public List<CallerDetailItem> CallerList = new List<CallerDetailItem>();

    public ZUIButton BackBtn;
    public ZUIButton ComfirmBtn;

    public Dictionary<string, VPlayerData> ReadyList = new Dictionary<string, VPlayerData>();

    public List<GameObject> lines = new List<GameObject>();

    public void AddListener()
    {
        BackBtn.OnZCommonItemUp += Back;
        ComfirmBtn.OnZCommonItemUp += TogetherAdd;
    }

    public void LineShow()
    {
        int c = CallerDic.Count;
        if (c > 3)
            c = 2;
        for (int i = 0; i < c; i++)
        {
            lines[i].SetActive(true);
        }
    }
    public void LineHide(int index)
    {
        if (index < 3)
        {
            lines[index].SetActive(false);
            if (index - 1 >= 0)
            {
                lines[index - 1].SetActive(false);
            }
        }
    }

    // 上线但没有在频道里列表
    public void CreateCallerToWaitingList(string playerid)
    {
        Debug.Log("[CZLOG] player id is ---" + playerid);

        var item = GameObject.Instantiate<CallerDetailItem>(ItemPrefab);
        item.AddListener();

        var d = MainController.Instance.GetVirtualData();
        if (d != null)
            item.SetData(d);

        item.transform.SetParent(DetailLayoutParent);
        item.transform.localRotation = Quaternion.identity;
        item.transform.localScale = Vector3.one;
        item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);

        item.gameObject.SetActive(true);

        // save data
        item.PlayerId = playerid;

        CallerDic.Add(playerid, item);
        CallerList.Add(item);

        // 模拟加载地图数据
        UIManager.Instance.MapP.GetMapItem(playerid,d);
    }

    // caller offline
    public void DeleteCaller(string playerid)
    {
        CallerDetailItem ci;
        if (CallerDic.TryGetValue(playerid, out ci))
        {
            if (ci.m_data != null)
                MainController.Instance.VPS.Add(ci.m_data);
            CallerDic.Remove(playerid);
            CallerList.Remove(ci);
            Destroy(ci.gameObject);
        }
        else
        {
            Debug.LogError("[CZLOG] Don't contain this player id ---" + playerid);
        }
    }

    public void RemoveCallerFromWaitingList(string playerid)
    {
        CallerDetailItem ci;
        if (CallerDic.TryGetValue(playerid, out ci))
        {
            CallerDic.Remove(playerid);
            CallerList.Remove(ci);
            Destroy(ci.gameObject);
        }
        else
        {
            Debug.LogError("[CZLOG] Don't contain this player id ---" + playerid);
        }
    }

    public void TogetherAdd()
    {
        foreach (var item in ReadyList)
        {
            MoveCallerToChannel(item.Key);
        }
        ReadyList.Clear();
        Back();
    }


    // 加入频道,从上线的列表中移除
    public void MoveCallerToChannel(string pid)
    {
        // 告诉caller加入频道
        ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "join_channel", pid));

        LeftConPanel.AddCallerToList(pid,CallerDic[pid].m_data);

        DeleteCaller(pid);
    }

    public void Back() // btn clk
    {
        gameObject.SetActive(false);
        LeftConPanel.gameObject.SetActive(true);
    }

}
