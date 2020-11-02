using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

public class ZGazeRaycast : MonoBehaviour
{
    private ZBasePanel curBP;
    private bool find = false;

    public Transform Origin;

    private void Update()
    {
        return;
        Raycast();

        Debug.DrawRay(Origin.position, Origin.forward, Color.red);
    }


    private void Raycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(Origin.position, Origin.forward, out hit, 99999))
        {
            ZBasePanel bp = hit.transform.GetComponent<ZBasePanel>();
            if (bp != null && !NRInput.GetButton(ControllerButton.TRIGGER))
            {
                if(!NRInput.GetButton(ControllerButton.TRIGGER) || (curBP == null))
                {

                }
                if (curBP != null)
                {
                    curBP.HoverEnd();
                }
                curBP = bp;
                find = true;
                bp.Hovering();
            }
        }
        else
        {
            if (find && curBP != null && !NRInput.GetButton(ControllerButton.TRIGGER))
            {
                curBP.HoverEnd();
                find = false;
                curBP = null;
            }
        }
    }
}
