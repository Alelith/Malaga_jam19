using DG.Tweening;
using UnityEngine;

public class TotebagController : IInitialSettings
{
    [SerializeField] CanvasGroup exitButton;
    
    public void OnExit()
    {
        exitButton.interactable = true;
        exitButton.blocksRaycasts = true;
        exitButton.DOFade(1, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            exitButton.interactable = true;
            exitButton.blocksRaycasts = true;
        });
    }

    public override void SetInitialSettings()
    {
        CanvasGroup self = GetComponent<CanvasGroup>();

        self.interactable = true;
        self.blocksRaycasts = true;
    }
}
