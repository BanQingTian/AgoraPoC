using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    

    public void OnPointerDown(PointerEventData eventData)
    {
        OnZCommonItemDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnZCommonItemUp?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnZCommonItemEnter?.Invoke();
        CurGO = gameObject;
        isHovering = true;
        enterLogic();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnZCommonItemExit?.Invoke();
        isHovering = false;
        exitLogic();
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
        }
    }

    private void Update()
    {
        if (!isHovering || CurGO != gameObject)
        {
            exitLogic();
        }
    }
}
