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

    public ZUIButton SurroundBtnUnselect;
    public GameObject SurroundBtnSelect;
    public ZUIButton MainSubBtnUnselect;
    public GameObject MainSubBtnSelect;

    public GameObject MoveTip;


    public void AddListener()
    {
        SurroundBtnUnselect.OnZCommonItemUp += SurroundBtnClked;
        MainSubBtnUnselect.OnZCommonItemUp += MainSubBtnClked;

        SurroundBtnClked();
    }

    public void SurroundBtnClked()
    {
        SurroundBtnSelect.SetActive(true);
        MainSubBtnSelect.gameObject.SetActive(false);

        SetViewMode(ZViewMode.SurroundMode);
    }
    public void MainSubBtnClked()
    {
        SurroundBtnSelect.SetActive(false);
        MainSubBtnUnselect.gameObject.SetActive(true);

        SetViewMode(ZViewMode.MainSubMode);
    }

    public void SetViewMode(ZViewMode m)
    {
        mode = m;
        switch (mode)
        {
            case ZViewMode.SurroundMode:

                m_Views[0].localPosition = Vector3.zero;
                m_Views[1].localPosition = new Vector3(-295.4f, 0, 0);
                m_Views[2].localPosition = new Vector3(295.4f, 0, 0);

                m_Views[0].localScale = Vector3.one;
                m_Views[1].localScale = Vector3.one;
                m_Views[2].localScale = Vector3.one;

                UIManager.Instance.BarP.SetVideoBarHoverMode(mode);

                break;


            case ZViewMode.MainSubMode:

                m_Views[0].localPosition = new Vector3(-152.5f, 0, 0);
                m_Views[1].localPosition = new Vector3(104.9f, 64.5f, 0);
                m_Views[2].localPosition = new Vector3(104.9f, -64.5f, 0);

                m_Views[0].localScale = Vector3.one;
                m_Views[1].localScale = Vector3.one * 0.5f;
                m_Views[2].localScale = Vector3.one * 0.5f;

                UIManager.Instance.BarP.SetVideoBarHoverMode(mode);

                break;
        }


    }

    public void SetMoveTip(bool b = true)
    {
        MoveTip.SetActive(b);
    }

    public Vector3 GetCenterPos()
    {
        return m_Views[0].position;
    }
    public Vector3 GetCenterRot()
    {
        Vector3 v = m_Views[0].rotation.eulerAngles;
        return new Vector3(v.x, v.y, v.z-90);
    }
}
