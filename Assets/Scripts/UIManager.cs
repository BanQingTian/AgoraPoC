using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public LeftConnectionPanel LeftConPanel;
    public CallerPanel CallerP;




    private void Awake()
    {
        Instance = this;
    }





    #region Logic

    public void UpdateCallerPanel(string playerid,bool add = true)
    {
        if (add)
        {
            CallerP.CreateCallerToWaitingList(playerid);
        }
        else
        {
            CallerP.RemoveCallerFromWaitingList(playerid);
        }
    }


    #endregion


}
