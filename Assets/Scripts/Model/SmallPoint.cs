using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallPoint : MonoBehaviour
{
    public string OnwerChannelName = "unity3d";
    //public Button AudioBtn;
    //public Button VideoBtn;
    //public Button CloseBtn;


    public void OnClkThisPerson()
    {
        //Btns.SetActive(!Btns.activeInHierarchy);
        //MainController.CurUid = 0;
        //if (Btns.activeInHierarchy)
        //{
        //    MainController.CurChannelName = OnwerChannelName;
        //    Debug.Log("[CZLOG] Cur Channel Name is ----- " + OnwerChannelName);
        //}
    }

    public void OnAudioBtnClk()
    {
        MainController.Instance.OnAudioBtnClk();
    }
    public void OnVideoBtnClk()
    {
        MainController.Instance.OnVideoBtnClk();
    }
    public void OnLeaveBtnClk()
    {
        MainController.Instance.OnLeaveBtnClk();
    }
}
