using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NRKernal;

/*****************************************************

    - gaze选中panel:ZBasePanel, 调用ZBasePanel中的Point模拟鼠标

******************************************************/
public class ZBasePanel : MonoBehaviour
{
    /// <summary>
    /// 选中该panel后额外出现的选中示意图
    /// </summary>
    [SerializeField, Header("选中后额外出现的示意图")]
    private Image m_HoverExtraImage;

    /// <summary>
    /// Panel中的模拟鼠标
    /// </summary>
    [SerializeField, Header("模拟鼠标图标")]
    private RectTransform m_VirtualMouse;
    [SerializeField]
    private RectTransform m_MouseHover;

    [SerializeField, Header("模拟鼠标移动区域")]
    private RectTransform m_RangeImage;

    private bool m_Hovering = false;

    /// <summary>
    /// 模拟鼠标的x轴的移动范围
    /// </summary>
    private Vector2 rangeX;
    /// <summary>
    /// 模拟鼠标y轴的移动范围
    /// </summary>
    private Vector2 rangeY;

    private void Start()
    {
        initMouseRange();
        initMouseColor();
    }

    private void initMouseRange()
    {
        if (m_HoverExtraImage == null)
        {
            Debug.LogError("m_HoverExtraImage is null !!!!");
            return;
        }

        float centerX = m_RangeImage.position.x;
        float centerY = m_RangeImage.position.y;
        float width = m_RangeImage.sizeDelta.x;
        float height = m_RangeImage.sizeDelta.y;

        rangeX = new Vector2(centerX - width / 2, centerX + width / 2);
        rangeY = new Vector2(centerY - height / 2, centerY + height / 2);
    }

    private void initMouseColor()
    {
        Color c = new Color(0f, 1f, 0.8f, 1);
        m_VirtualMouse.GetComponent<Image>().color = c;
        m_MouseHover.GetComponent<Image>().color = c;

    }

    public virtual void Hovering()
    {
        m_Hovering = true;
        HoverStatus(true);
        registerMouse();
    }
    public virtual void HoverEnd()
    {
        m_Hovering = false;
        HoverStatus(false);
        removeMouse();
    }

    public virtual void HoverStatus(bool hover = true)
    {
        if (m_HoverExtraImage != null && m_VirtualMouse != null)
        {
            m_HoverExtraImage.gameObject.SetActive(hover);
            m_VirtualMouse.gameObject.SetActive(hover);
        }
    }

    private void registerMouse()
    {
        UIManager.Instance.Point.SetParent(m_VirtualMouse);
        UIManager.Instance.Point.localPosition = Vector3.zero;
        UIManager.Instance.Point.localScale = Vector3.one;
        UIManager.Instance.Point.localRotation = Quaternion.identity;
    }
    private void removeMouse()
    {
        UIManager.Instance.Point.SetParent(UIManager.Instance.LeftConPanel.transform);
    }

    Vector2 lastPos = new Vector2();
    public virtual void MouseMove()
    {
        Vector2 curPos = NRInput.GetTouch();
        if (curPos.x != 0 || curPos.y != 0)
        {
            Vector2 moved = lastPos == Vector2.zero ? Vector2.zero : curPos - lastPos;
            m_VirtualMouse.localPosition += new Vector3(moved.x, moved.y) * 50;
            lastPos = curPos;
        }
        else
        {
            lastPos = curPos;
        }

        m_VirtualMouse.localPosition = new Vector3(
            Mathf.Clamp(m_VirtualMouse.localPosition.x, rangeX.x, rangeX.y),
            Mathf.Clamp(m_VirtualMouse.localPosition.y, rangeY.x, rangeX.y),
            m_VirtualMouse.localPosition.z);
    }

    private void Update()
    {
        if (m_Hovering)
        {
            MouseMove();
            m_MouseHover.gameObject.SetActive(ZCommonItem.BtnHovering);
        }
    }
}
