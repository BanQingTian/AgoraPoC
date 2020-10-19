using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ZBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image mImage;

    private Sprite DefaultSprite;
    public Sprite HoverSprite;

    public Action ClkDown;
    public Action ClkUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        ClkDown?.Invoke();

        if (!ReferenceEquals(mImage.sprite, null))
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

}
