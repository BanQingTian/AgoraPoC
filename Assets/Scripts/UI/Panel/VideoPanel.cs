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

    private ZViewMode mode = ZViewMode.SurroundMode;


    private void Start()
    {
        //SetViewMode(ZViewMode.MainSubMode);
    }


    public ZViewMode GetViewMode()
    {
        return mode;
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
                break;


            case ZViewMode.MainSubMode:

                m_Views[0].localPosition = new Vector3(-152.5f, 0, 0);
                m_Views[1].localPosition = new Vector3(104.9f, 64.5f, 0);
                m_Views[2].localPosition = new Vector3(104.9f, -64.5f, 0);

                m_Views[0].localScale = Vector3.one;
                m_Views[1].localScale = Vector3.one * 0.5f;
                m_Views[2].localScale = Vector3.one * 0.5f;
                break;
        }
    }
}
