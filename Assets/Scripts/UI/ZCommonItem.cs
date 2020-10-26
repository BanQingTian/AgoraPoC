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
    /// <summary>
    /// 动画加hover图标出现
    /// </summary>
    AnimationAndExtra,
}

public class ZCommonItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ZCommonEventHandler OnZCommonItemDown;
    public ZCommonEventHandler OnZCommonItemUp;
    public ZCommonEventHandler OnZCommonItemEnter;
    public ZCommonEventHandler OnZCommonItemExit;
    public ZCommonEventHandler OnZCommonItemHover;

    public bool isHovering = false;
    public bool isDowning = false; // 为了防止点中之后移动到button之外再回来，还会执行按钮的up函数

    public static bool BtnHovering = false;

    public HoverMode Mode = HoverMode.Replace;

    public Image NormalImage;
    public Image HoverImage;
    public Image PressedImage;


    // 缩放比例
    private float HoveringScaleValue = 1.2f;
    private float PressScaleValue = 0.8f;
    // 缩放时间
    private float HoverScaleBackDuration = 0.2f;
    private float PressScaleBackDuration = 0.2f;

    private bool m_InitDefaultScale = false;
    // 默认比例
    private float defaultScaleValue;

    public bool runOnceHoverEvent = false;
    private void Update()
    {
        if (isHovering && runOnceHoverEvent)
        {
            runOnceHoverEvent = false;
            OnZCommonItemHover?.Invoke();
        }
    }

    #region IPoint Handler

    public void OnPointerDown(PointerEventData eventData)
    {
        OnZCommonItemDown?.Invoke();
        NormalImage.rectTransform.DOScale(PressScaleValue, PressScaleBackDuration);
        GetScaleTween(PressScaleValue, PressScaleBackDuration).PlayForward();
        isDowning = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isHovering && isDowning)
        {
            OnZCommonItemUp?.Invoke();
        }
        if (PressedImage != null)
        {
            PressedImage.enabled = true;
        }
        isDowning = false;
        BtnHovering = false;
        //NormalImage.rectTransform.DOScale(defaultScaleValue, PressScaleBackDuration);

        GetScaleTween(PressScaleValue, PressScaleBackDuration).PlayBackwards();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        runOnceHoverEvent = true;
        enterLogic();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
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

    public virtual void enterLogic()
    {
        OnZCommonItemEnter?.Invoke();
        isHovering = true;
        BtnHovering = true;

        switch (Mode)
        {
            case HoverMode.Extra:
                if (HoverImage != null)
                    HoverImage.gameObject.SetActive(true);
                break;

            case HoverMode.Replace:
                if (NormalImage != null)
                    NormalImage.gameObject.SetActive(false);
                if (HoverImage != null)
                    HoverImage.gameObject.SetActive(true);
                break;

            case HoverMode.Animation:
                InitAnimation();
                GetScaleTween(HoveringScaleValue, HoverScaleBackDuration).PlayForward();
                break;

            case HoverMode.AnimationAndExtra:
                InitAnimation();
                GetScaleTween(HoveringScaleValue, HoverScaleBackDuration).PlayForward();
                if (HoverImage != null)
                    HoverImage.gameObject.SetActive(true);
                break;
        }
    }

    public virtual void exitLogic()
    {
        OnZCommonItemExit?.Invoke();
        isHovering = false;
        BtnHovering = false;
        isDowning = false;

        switch (Mode)
        {
            case HoverMode.Extra:
                if (HoverImage != null)
                    HoverImage.gameObject.SetActive(false);
                break;

            case HoverMode.Replace:
                if (NormalImage != null)
                    NormalImage.gameObject.SetActive(true);
                if (HoverImage != null)
                    HoverImage.gameObject.SetActive(false);
                break;

            case HoverMode.Animation:
                InitAnimation();
                GetScaleTween(HoveringScaleValue, HoverScaleBackDuration).PlayBackwards();
                break;

            case HoverMode.AnimationAndExtra:
                InitAnimation();
                GetScaleTween(HoveringScaleValue, HoverScaleBackDuration).PlayBackwards();
                if (HoverImage != null)
                    HoverImage.gameObject.SetActive(false);
                break;
        }
    }

    private Tween m_ScaleTween;
    private Tween GetScaleTween(float scale, float duration)
    {
        if (m_ScaleTween == null)
        {
            m_ScaleTween = NormalImage.rectTransform.DOScale(HoveringScaleValue, HoverScaleBackDuration).SetAutoKill(false).Pause();
        }
        return m_ScaleTween;
    }
}
