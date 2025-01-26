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
        if (trapCount > 0 && minigameIndex > 1)
        {
            int selection = Random.Range(0, trapMinigames.Length);
            
            trapMinigames[selection].DOFade(0, 0.5f).OnComplete(() => trapMinigames[selection].gameObject.GetComponent<IInitialSettings>().SetInitialSettings());
        }
        else if (minigameIndex < 1)
            trapMinigames[minigameIndex].DOFade(0, 0.5f).OnComplete(() => trapMinigames[minigameIndex].gameObject.GetComponent<IInitialSettings>().SetInitialSettings());
        else if (minigameIndex > 1)
        {
            minigames[minigameIndex - 2].DOFade(0, 0.5f).OnComplete(() => minigames[minigameIndex - 2].gameObject.GetComponent<IInitialSettings>().SetInitialSettings());

            trapCount = 3;
        }
    }
}
