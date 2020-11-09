using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ZBtn2 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public RawImage mImage;

    public Action ClkDown;
    public Action ClkUp;
    public Action Enter;
    public Action Exit;

    public void OnPointerDown(PointerEventData eventData)
    {
        ClkDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ClkUp?.Invoke();
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
