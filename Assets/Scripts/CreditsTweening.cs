using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CreditsTweening : MonoBehaviour
{
    [SerializeField]
    CanvasGroup MenuCanvas;

    public void OnCredits(CanvasGroup target) {
        MenuCanvas.interactable = false;
        MenuCanvas.blocksRaycasts = false;
        target.DOFade(1, 2).SetEase(Ease.InBounce).OnComplete(() => {
            target.interactable = true;
            target.blocksRaycasts = true;
        });
    } 
    
    public void OffCredits(CanvasGroup target) {
        target.interactable = false;
        target.blocksRaycasts = false;
        target.DOFade(0, 2).SetEase(Ease.OutBounce).OnComplete(() => {
            MenuCanvas.interactable = true;
            MenuCanvas.blocksRaycasts = true;
        });
    } 
}
