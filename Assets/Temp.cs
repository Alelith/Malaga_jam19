using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Temp : MonoBehaviour
{
    [SerializeField]
    CanvasGroup tmp;

    public void OnCredits() => tmp.DOFade(1, 2).SetEase(Ease.InBounce).OnComplete(() => {
        tmp.interactable = true;
        tmp.blocksRaycasts = true;
    });
}
