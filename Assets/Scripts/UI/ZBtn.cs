using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ZBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Image mImage;

    private Sprite DefaultSprite;
    public Sprite HoverSprite;

    public Action ClkDown;
    public Action ClkUp;
    public Action Enter;
    public Action Exit;

    public void OnPointerDown(PointerEventData eventData)
    {
        ClkDown?.Invoke();
        if (!ReferenceEquals(mImage, null))
        {
            DefaultSprite = mImage.sprite;
        }
        if (!ReferenceEquals(mImage, null) && !ReferenceEquals(HoverSprite, null))
        {
            mImage.sprite = HoverSprite;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ClkUp?.Invoke();

        if (!ReferenceEquals(DefaultSprite, null))
        {
            mImage.sprite = DefaultSprite;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Enter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Exit?.Invoke();
    }
}
