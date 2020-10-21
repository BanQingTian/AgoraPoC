using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MapPanel : ZBasePanel
{

    [Space(24)]
    public float m_Speed = 1;
    public RectTransform MapBg;

    private bool m_Moving = false;


    public void TweenTest()
    {
        transform.DOScale(2, 1);
    }

    public void Resize(float scaleValue, Vector3 centerPos, float time)
    {
        if (m_Moving)
        {
            Debug.LogError("[CZLOG] is moving !!!");
            return;
        }

        float defaultScale = MapBg.localScale.x; // default 1:1:1
        float defaultPosX = MapBg.localPosition.x;
        float defaultPosY = MapBg.localPosition.y;

        float scaleRate = (scaleValue - defaultScale) / time;
        Vector2 posRate = new Vector2((centerPos.x - defaultPosX) / time, (centerPos.y - defaultPosY) / time);

        StartCoroutine(ResizeCor(scaleRate, posRate, defaultScale + scaleValue, centerPos, time));
    }
    private IEnumerator ResizeCor(float scaleRate, Vector2 posRate, float endScale, Vector3 endPos, float time)
    {
        m_Moving = true;
        float t = 0;
        while (t < time)
        {
            MapBg.localPosition += new Vector3(posRate.x, posRate.y) * Time.deltaTime * m_Speed;
            MapBg.localScale += Vector3.one * scaleRate * Time.deltaTime * m_Speed;
            t += Time.deltaTime;
            yield return null;
        }

        MapBg.localPosition = endPos;
        MapBg.localScale = Vector3.one * endScale;

        m_Moving = false;
    }

}
