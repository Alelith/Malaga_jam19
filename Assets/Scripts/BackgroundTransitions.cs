using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BackgroundTransitions : MonoBehaviour
{
    RectTransform rect;

    void Awake() => rect = transform as RectTransform;

    public void MoveTowards(RectTransform otherTrans)
    {
        float pivotX = otherTrans.anchoredPosition.x / 1920;
        float pivotY = otherTrans.anchoredPosition.y / 1080;

        rect.pivot = new(pivotX, pivotY);

        rect.DOScale(new Vector3(4, 4, 4), 1.75f).SetEase(Ease.InExpo).OnComplete(() => rect.localScale = Vector3.one);
    }
}
