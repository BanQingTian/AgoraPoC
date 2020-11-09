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

    public GameObject channel1Bg;

    public ZViewMode mode = ZViewMode.SurroundMode;

    public ZUIButton SurroundBtnUnselect;
    public GameObject SurroundBtnSelect;
    public ZUIButton MainSubBtnUnselect;
    public GameObject MainSubBtnSelect;

    public GameObject MoveTip1;
    public GameObject MoveTip2;
    public GameObject MoveTip3;

    public GameObject Channel1CloseBtn;
    public GameObject Channel1LabelTip;


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

    private string leftChannelName;
    public void SetSmallLayout(string cn)
    {
        //leftChannelName = cn;
        leftChannelName = "nreal2";
    }


    public void SetViewMode(ZViewMode m)
    {
        mode = m;
        switch (mode)
        {
            case ZViewMode.SurroundMode:
                // channel1 = 0.99/scale 0xyz
                // channel2 = 1/scale -171x0yz
                // channel3 = 1/scale 171x0yz

                m_Views[0].localPosition = Vector3.zero;

                //if (leftChannelName == "nreal2")
                {
                    m_Views[1].localPosition = new Vector3(-171f, 0, 0);
                    m_Views[2].localPosition = new Vector3(171f, 0, 0);
                }
                //else
                //{
                //    m_Views[2].localPosition = new Vector3(-171f, 0, 0);
                //    m_Views[1].localPosition = new Vector3(171f, 0, 0);
                //}
                Channel1CloseBtn.transform.localPosition = new Vector3(53.4f, Channel1CloseBtn.transform.localPosition.y, Channel1CloseBtn.transform.localPosition.z);
                Channel1LabelTip.transform.localPosition = new Vector3(-27.5f, Channel1LabelTip.transform.localPosition.y, Channel1LabelTip.transform.localPosition.z);

                m_Views[0].localScale = new Vector3(0.99f, 0.99f, 0.99f);
                m_Views[1].localScale = Vector3.one;
                m_Views[2].localScale = Vector3.one;

                channel1Bg.SetActive(true);

                UIManager.Instance.BarP.SetVideoBarHoverMode(mode);

                break;


            case ZViewMode.MainSubMode:
                // channel1 = 2.83/scale  -30.9x0yz
                // channel2 = 0.5/scale  224x63.8y0z
                // channel3 = 0.5/scale  224x-65y0z

                Channel1CloseBtn.transform.localPosition = new Vector3(161.5f, Channel1CloseBtn.transform.localPosition.y, Channel1CloseBtn.transform.localPosition.z);
                Channel1LabelTip.transform.localPosition = new Vector3(-193.3f, Channel1LabelTip.transform.localPosition.y, Channel1LabelTip.transform.localPosition.z);

                m_Views[0].localPosition = new Vector3(-30.9f, 0, 0);
                m_Views[0].localScale = Vector3.one * 2.83f;

                m_Views[1].localPosition = new Vector3(224, 63.8f, 0);
                m_Views[2].localPosition = new Vector3(224, -64.6f, 0);

                m_Views[1].localScale = Vector3.one * 0.5f;
                m_Views[2].localScale = Vector3.one * 0.5f;


                channel1Bg.SetActive(false);

                UIManager.Instance.BarP.SetVideoBarHoverMode(mode);

                var pos = m_Views[0].localPosition;

                MoveTip1.transform.localPosition = new Vector3(pos.x, pos.y, MoveTip1.transform.localPosition.z);

                break;
        }


    }


    public void SetMoveTip(int n = 1, bool b = true)
    {

        if (n == 1)
        {
            MoveTip1.SetActive(b);
            var pos = m_Views[0].localPosition;
            MoveTip1.transform.localPosition = new Vector3(pos.x, pos.y, MoveTip1.transform.localPosition.z);
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
        return channel1Bg.transform.position;
    }
    public Quaternion GetCenterRot(int n = 1)
    {
        if (n == 1)
        {
            return channel1Bg.transform.rotation;
        }
        else if (n == 2)
        {
            return m_Views[1].transform.rotation;
        }
        else if (n == 3)
        {
            return m_Views[2].transform.rotation;
        }
        return channel1Bg.transform.rotation;
    }
}
