using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberPanel : MonoBehaviour
{
    public GameObject BG;

    public GameObject MoveTip;

    public List<MemberListItem> Members = new List<MemberListItem>();

    public void AddListener()
    {
        foreach (var item in Members)
        {
            item.AddListener();
        }
    }


    public void SetMoveTip(bool b = true)
    {
        MoveTip.SetActive(b);
    }

    public Vector3 GetCenterPos()
    {
        return BG.transform.position;
    }
    public Quaternion GetCenterRot()
    {
        return BG.transform.rotation;
    }
}
