using DG.Tweening;
using DialogSystem;
using TMPro;
using UnityEngine;

public class MobileController : IInitialSettings
{
    [SerializeField] GameObject carrousel;
    [SerializeField] GameObject interactionsCover;
    [SerializeField] CanvasGroup text;
    [SerializeField] Typewriter[] responses;
    [SerializeField] Dialog dialogs;

    int messageIndex = 0;

    int[] iterations = new[] { 2, 1, 2 };
    
    int showMessage = 2;
    
    public void OnChangeImage()
    {
        ((RectTransform)carrousel.transform).DOAnchorPosY(1080, 0.5f).OnComplete(() =>
        {
            showMessage--;
            if (showMessage == 0)
            {
                interactionsCover.SetActive(true);

                text.GetComponentInChildren<Typewriter>().Write(CustomTagsManager.ProccessMessage(dialogs[messageIndex], out string cText), cText);

                messageIndex++;
                
                text.DOFade(1, 0.5f);
                
                foreach (var response in responses)
                {
                    response.Write(CustomTagsManager.ProccessMessage(dialogs[messageIndex], out string bText), bText);
                    
                    messageIndex++;
                }
            }
            carrousel.transform.GetChild(0).SetAsLastSibling();
            ((RectTransform)carrousel.transform).anchoredPosition = new Vector2(0, 0);
        });
    }

    public override void SetInitialSettings()
    {
        CanvasGroup self = GetComponent<CanvasGroup>();

        self.interactable = true;
        self.blocksRaycasts = true;
    }
}
