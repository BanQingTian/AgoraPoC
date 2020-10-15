using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconItemLeft : MonoBehaviour
{
    public LeftConnectionPanel Panel;
    public string PlayerId { get; set; }

    public void RemoveCallerToWaitingList()
    {
        Debug.Log("RemoveCallerToWaitingList");
        Panel.RemoveCallerToWaitingList(PlayerId);
    }

}
