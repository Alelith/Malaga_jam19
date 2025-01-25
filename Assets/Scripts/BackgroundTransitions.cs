using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundTransitions : MonoBehaviour
{
    [SerializeField] int trapCount;
    [SerializeField] CanvasGroup[] trapMinigames;
    [SerializeField] CanvasGroup[] minigames;
    
    RectTransform rect;

    void Awake() => rect = transform as RectTransform;

    public void MoveTowards(RectTransform otherTrans)
    {
        float pivotX = otherTrans.anchoredPosition.x / 1920;
        float pivotY = otherTrans.anchoredPosition.y / 1080;

        rect.pivot = new(pivotX, pivotY);

        rect.DOScale(new Vector3(4, 4, 4), 1.75f).SetEase(Ease.InExpo).OnComplete(() => rect.localScale = Vector3.one);
    }

    public void OnStartMinigame(int minigameIndex)
    {
        if (trapCount > 0)
        {
            int selection = Random.Range(0, trapMinigames.Length);
            
            trapMinigames[selection].DOFade(0, 0.5f).OnComplete(() => trapMinigames[selection].GetComponent<IInitialSettings>().SetInitialSettings());
        }

        trapCount = 3;
    }
}
