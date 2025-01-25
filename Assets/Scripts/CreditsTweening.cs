using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CreditsTweening : MonoBehaviour
{
    [SerializeField]
    CanvasGroup CreditCanvas, MenuCanvas;

    public void OnCredits() {
        MenuCanvas.interactable = false;
        MenuCanvas.blocksRaycasts = false;
        CreditCanvas.DOFade(1, 2).SetEase(Ease.InBounce).OnComplete(() => {
            CreditCanvas.interactable = true;
            CreditCanvas.blocksRaycasts = true;
        });
    } 
    
    public void OffCredits() {
        CreditCanvas.interactable = false;
        CreditCanvas.blocksRaycasts = false;
        CreditCanvas.DOFade(0, 2).SetEase(Ease.OutBounce).OnComplete(() => {
            MenuCanvas.interactable = true;
            MenuCanvas.blocksRaycasts = true;
        });
    } 
}
