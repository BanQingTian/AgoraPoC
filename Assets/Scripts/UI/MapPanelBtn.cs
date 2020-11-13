using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPanelBtn : ZCommonItem
{
    [HideInInspector]
    public bool isClked = false;
    private void OnEnable()
    {
        base.exitLogic();
    }

    public override void enterLogic()
    {
        base.enterLogic();
    }
    public override void exitLogic()
    {
        isHovering = false;
        BtnHovering = false;

        if (!isClked)
        {
            Debug.Log("exitlog");
            RunBtnExitLogic();
        }

        isDowning = false;
    }
    public override void DownLogic()
    {
        mapBtnDownLogic();
    }
    public override void UpLogic()
    {
        mapBtnUpLogic();
    }

    private void mapBtnDownLogic()
    {
        if (PlayAuido)
        {
            SoundController.Instance.Play(AUDIOCLIPENUM.CLICK);
        }

        isDowning = true;
    }

    private void mapBtnUpLogic()
    {
        if (isHovering && isDowning)
        {
            isClked = true;
        }
        if (PressedImage != null)
        {
            PressedImage.gameObject.SetActive(true);
        }

        isDowning = false;
        BtnHovering = false;

    }

    private void mapBtnEnterLogic()
    {
        isHovering = true;
        BtnHovering = true;

        if (PlayAuido)
        {
            SoundController.Instance.Play(AUDIOCLIPENUM.HOVER);
        }

        InitAnimation();

        NormalImage.rectTransform.DOScale(HoveringScaleValue, 0.2f);
        if(HoverImage != null)
        {
            HoverImage.gameObject.SetActive(true);
        }
        if(HoverImage2 != null)
        {
            HoverImage2.gameObject.SetActive(true);
        }

    }

    public void RunBtnExitLogic()
    {
        base.exitLogic();
        isClked = false;
    }
}
