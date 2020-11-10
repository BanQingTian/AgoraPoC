using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintPanel : MonoBehaviour
{

    public GameObject TalkBackMode;
    public TMPro.TextMeshProUGUI Name;
    public GameObject Part1;
    public GameObject Part2;

    public GameObject VideoOpen;

    public GameObject VideoClose;

    public ZUIButton CloseBtn;

    public void AddListener()
    {
        CloseBtn.OnZCommonItemUp = HideHint;
    }

    public void SetMode(int n = 0)
    {
        TalkBackMode.SetActive(n == 0);
        VideoOpen.SetActive(n == 1);
        VideoClose.SetActive(n == 2);
    }

    private void HideHint()
    {
        UIManager.Instance.HideHint();
    }


}
