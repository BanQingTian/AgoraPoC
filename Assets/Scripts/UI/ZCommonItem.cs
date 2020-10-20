using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using NRKernal;

public delegate void ZCommonEventHandler();
public enum HoverMode
{
    /// <summary>
    /// 额外出现
    /// </summary>
    Extra = 0,
    /// <summary>
    /// 替换出现
    /// </summary>
    Replace,
    /// <summary>
    /// 动画
    /// </summary>
    Animation,
}

public class ZCommonItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ZCommonEventHandler OnZCommonItemDown;
    public ZCommonEventHandler OnZCommonItemUp;
    public ZCommonEventHandler OnZCommonItemEnter;
    public ZCommonEventHandler OnZCommonItemExit;


    public static GameObject CurGO;
    public bool isHovering = false;


    public HoverMode Mode = HoverMode.Replace;

    public Image NormalImage;
    public Image HoverImage;
    public Image PressedImage;


    [Space(12), Header("缩放比例"), Range(0, 2)]
    public float HoveringScaleValue = 1.5f;
    [Space(12), Range(0, 2)]
    public float PressScaleValue = 0.7f;
    [Space(12), Header("缩放时间"),Range(0, 1)]
    public float HoverScaleBackDuration = 0.7f;
    public float PressScaleBackDuration = 0.5f;

    private bool m_InitDefaultScale = false;
    // 默认比例
    private float defaultScaleValue;


    #region IPoint Handler


    public void OnPointerDown(PointerEventData eventData)
    {
        OnZCommonItemDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnZCommonItemUp?.Invoke();
        if (PressedImage != null)
        {
            PressedImage.enabled = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnZCommonItemEnter?.Invoke();
        //CurGO = gameObject;
        isHovering = true;
        enterLogic();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnZCommonItemExit?.Invoke();
        isHovering = false;
        exitLogic();

    }


    #endregion

    private void InitAnimation()
    {
        if (!m_InitDefaultScale)
        {
            if (NormalImage == null)
            {
                Debug.LogError("[CZLOG] NormalImage Null !!");
            }
            m_InitDefaultScale = true;
            defaultScaleValue = NormalImage.rectTransform.localScale.x;
        }
    }

    public void enterLogic()
    {
        switch (Mode)
        {
            case HoverMode.Extra:
                if (HoverImage != null)
                    HoverImage.enabled = true;
                break;

            case HoverMode.Replace:
                if (NormalImage != null)
                    NormalImage.enabled = false;
                if (HoverImage != null)
                    HoverImage.enabled = true;
                break;

            case HoverMode.Animation:
                InitAnimation();
                NormalImage.rectTransform.DOScale(HoveringScaleValue, HoverScaleBackDuration);
                break;
        }
    }

    private void exitLogic()
    {
        switch (Mode)
        {
            case HoverMode.Extra:
                if (HoverImage != null)
                    HoverImage.enabled = false;
                break;

            case HoverMode.Replace:
                if (NormalImage != null)
                    NormalImage.enabled = true;
                if (HoverImage != null)
                    HoverImage.enabled = false;
                break;

            case HoverMode.Animation:
                InitAnimation();
                NormalImage.rectTransform.DOScale(defaultScaleValue, HoverScaleBackDuration-0.2f);
                break;
        }
    }

    //private void Update()
    //{
    //    if (!isHovering || CurGO != gameObject)
    //    {
    //        exitLogic();
    //    }
    //}
}
