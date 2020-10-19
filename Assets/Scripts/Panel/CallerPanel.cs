﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallerPanel : MonoBehaviour
{
    public LeftConnectionPanel LeftConPanel;

    public Transform DetailLayoutParent;
    public CallerDetailItem ItemPrefab;
    public Dictionary<string, CallerDetailItem> CallerDic = new Dictionary<string, CallerDetailItem>();

    // 上线但没有在频道里列表
    public void CreateCallerToWaitingList(string playerid)
    {
        Debug.Log("[CZLOG] player id is ---" + playerid);

        var item = GameObject.Instantiate<CallerDetailItem>(ItemPrefab);

        item.transform.SetParent(DetailLayoutParent);
        item.transform.localRotation = Quaternion.identity;
        item.transform.localScale = Vector3.one;
        item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);

        item.gameObject.SetActive(true);

        // save data
        item.PlayerId = playerid;

        CallerDic.Add(playerid, item);
    }

    // caller offline
    public void DeleteCaller(string playerid)
    {
        CallerDetailItem ci;
        if (CallerDic.TryGetValue(playerid, out ci))
        {
            CallerDic.Remove(playerid);
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
            Destroy(ci.gameObject);
        }
        else
        {
            Debug.LogError("[CZLOG] Don't contain this player id ---" + playerid);
        }
    }


    // 加入频道,从上线的列表中移除
    public void MoveCallerToChannel(CallerDetailItem cdi)
    {
        LeftConPanel.AddCallerToList(cdi.PlayerId);

        DeleteCaller(cdi.PlayerId);
    }

    public void HideSelf() // btn clk
    {
        gameObject.SetActive(false);
    }

}
