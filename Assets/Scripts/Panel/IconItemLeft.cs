﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconItemLeft : MonoBehaviour
{
    public LeftConnectionPanel Panel;
    public string PlayerId { get; set; }

    public void RemoveCallerToWaitingList()
    {
        Debug.Log("RemoveCallerToWaitingList");

        ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "leave_channel", PlayerId));

        Panel.RemoveCallerToWaitingList(PlayerId);
    }

}
