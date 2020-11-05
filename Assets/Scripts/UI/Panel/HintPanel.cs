using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintPanel : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Name;
    public GameObject Part1;
    public GameObject Part2;

    public ZUIButton CloseBtn;

    public void AddListener()
    {
        CloseBtn.OnZCommonItemUp = HideHint;
    }

    private void HideHint()
    {
        UIManager.Instance.HideHint();
    }

   
}
