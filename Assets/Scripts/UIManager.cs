using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Space(12)]
    public bool SampleMode = false;

    [Space(12)]
    public LeftConnectionPanel LeftConPanel;
    public CallerPanel CallerP;




    private void Awake()
    {
        Instance = this;
    }


    #region Logic

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
    }

    #endregion


}
