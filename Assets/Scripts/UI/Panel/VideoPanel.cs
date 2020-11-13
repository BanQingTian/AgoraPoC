using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZViewMode
{
    /// <summary>
    /// 环绕模式
    /// </summary>
    SurroundMode = 0,
    /// <summary>
    /// 主副模式
    /// </summary>
    MainSubMode
}
public class VideoPanel : ZBasePanel
{
    public List<RectTransform> m_Views = new List<RectTransform>();

    public ZViewMode mode = ZViewMode.SurroundMode;

    public ZUIButton SwitchBtn_Surround;
    public ZUIButton SwitchBtn_Mainsub;

    public GameObject Channel1_bg;

    public GameObject MoveTip1;
    public GameObject MoveTip2;
    public GameObject MoveTip3;

    public void AddListener()
    {
        SwitchBtn_Surround.OnZCommonItemUp = SwitchMode;
        SwitchBtn_Mainsub.OnZCommonItemUp = SwitchMode;

        SurroundBtnClked();
    }



    public void SurroundBtnClked()
    {
        UIManager.Instance.BarP.SetVideoBarHoverMode(ZViewMode.MainSubMode);
        SetViewMode(ZViewMode.SurroundMode);
        SwitchBtn_Surround.gameObject.SetActive(false);
        SwitchBtn_Mainsub.gameObject.SetActive(true);
    }
    public void MainSubBtnClked()
    {
        UIManager.Instance.BarP.SetVideoBarHoverMode(ZViewMode.SurroundMode);
        SetViewMode(ZViewMode.MainSubMode);
        SwitchBtn_Surround.gameObject.SetActive(true);
        SwitchBtn_Mainsub.gameObject.SetActive(false);
    }

    public void SwitchMode()
    {
        if (mode == ZViewMode.SurroundMode)
        {
            MainSubBtnClked();
        }
        else if (mode == ZViewMode.MainSubMode)
        {
            SurroundBtnClked();
        }
    }

    public void SetViewMode(ZViewMode m)
    {
        mode = m;
        switch (mode)
        {
            case ZViewMode.SurroundMode:

                m_Views[1].localPosition = new Vector3(280.9f, 1.3462f, 0);
                m_Views[2].localPosition = new Vector3(452f, 1.3462f, 0);

                m_Views[1].localScale = Vector3.one;
                m_Views[2].localScale = Vector3.one;

                UIManager.Instance.BarP.SetVideoBarHoverMode(mode);

                break;


            case ZViewMode.MainSubMode:

                m_Views[1].localPosition = new Vector3(227.9f, 66.9f, 0f);
                m_Views[2].localPosition = new Vector3(227.9f, -62.5f, 0f);

                m_Views[1].localScale = Vector3.one * 0.49f;
                m_Views[2].localScale = Vector3.one * 0.49f;

                UIManager.Instance.BarP.SetVideoBarHoverMode(mode);

                var pos = m_Views[0].localPosition;

               // MoveTip1.transform.localPosition = new Vector3(pos.x, pos.y, MoveTip1.transform.localPosition.z);

                break;
        }


    }


    public void SetMoveTip(int n = 1, bool b = true)
    {

        if (n == 1)
        {
            MoveTip1.SetActive(b);
            var pos = m_Views[0].localPosition;
           // MoveTip1.transform.localPosition = new Vector3(pos.x, pos.y, MoveTip1.transform.localPosition.z);
        }
        else if (n == 2)
        {
            MoveTip2.SetActive(b);
        }
        else
        {
            MoveTip3.SetActive(b);
        }
    }




    public Vector3 GetCenterPos(int n = 1)
    {
        if (n == 1)
        {
            return m_Views[0].transform.position;
        }
        else if (n == 2)
        {
            return m_Views[1].transform.position;
        }
        else if (n == 3)
        {
            return m_Views[2].transform.position;
        }
        return m_Views[0].transform.position;
    }
    public Quaternion GetCenterRot(int n = 1)
    {
        if (n == 1)
        {
            return Channel1_bg.transform.rotation;
        }
        else if (n == 2)
        {
            return m_Views[1].transform.rotation;
        }
        else if (n == 3)
        {
            return m_Views[2].transform.rotation;
        }
        return Channel1_bg.transform.rotation;
    }
}
