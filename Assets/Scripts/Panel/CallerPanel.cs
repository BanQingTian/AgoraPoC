using System.Collections;
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
        var item = GameObject.Instantiate<CallerDetailItem>(ItemPrefab);

        item.transform.SetParent(DetailLayoutParent);
        item.transform.localRotation = Quaternion.identity;
        item.transform.localScale = Vector3.one;
        item.gameObject.SetActive(true);

        // save data
        item.PlayerId = playerid;

        CallerDic.Add(playerid, item);
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
        CallerDetailItem ci;
        if (CallerDic.TryGetValue(cdi.PlayerId, out ci))
        {
            CallerDic.Remove(cdi.PlayerId);
            Destroy(cdi.gameObject);
        }
        else
        {
            Debug.LogError("[CZLOG] Don't contain this player id ---" + cdi.PlayerId);
        }
    }

    public void HideSelf()
    {
        gameObject.SetActive(false);
    }

}
