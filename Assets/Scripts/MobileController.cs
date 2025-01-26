using System;
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
    int[] currentIteration = new[] { 0, 0, 0 };
    
    int showMessage = 2;

    public void OnChangeImage()
    {
        ((RectTransform)carrousel.transform).DOAnchorPosY(1080, 0.5f).OnComplete(() =>
        {
            showMessage--;
            if (showMessage == 0)
            {
                /*interactionsCover.SetActive(true);

                text.DOFade(1, 0.5f);
                
                text.GetComponentInChildren<Typewriter>().Write(CustomTagsManager.ProccessMessage(dialogs[messageIndex], out string cText), cText);

                messageIndex++;
                
                text.DOFade(1, 0.5f);
                
                foreach (var response in responses)
                {
                    response.gameObject.SetActive(true);
                    
                    response.Write(CustomTagsManager.ProccessMessage(dialogs[messageIndex], out string bText), bText);
                    
                    messageIndex++;
                }

                if (messageIndex == 3)
                    currentIteration[0]++;
                else if (messageIndex == 16)
                    currentIteration[1]++;
                else if (messageIndex == 21)
                    currentIteration[2]++;*/
            }
            carrousel.transform.GetChild(0).SetAsLastSibling();
            ((RectTransform)carrousel.transform).anchoredPosition = new Vector2(0, 0);
        });
    }

    public void ContinueIterations(int index)
    {
        int currentIterationIndex = -1;
        
        if (messageIndex == 3)
            currentIterationIndex = 0;
        else if (messageIndex == 16)
            currentIterationIndex = 1;
        else if (messageIndex == 21)
            currentIterationIndex = 2;

        if (iterations[currentIterationIndex] != currentIteration[currentIterationIndex])
        {
            messageIndex += index;
            
            text.GetComponentInChildren<Typewriter>().Write(CustomTagsManager.ProccessMessage(dialogs[messageIndex], out string cText), cText);

            messageIndex += currentIteration[currentIterationIndex];
            messageIndex++;
            
            foreach (var response in responses)
            {
                response.Write(CustomTagsManager.ProccessMessage(dialogs[messageIndex], out string bText), bText);
                messageIndex++;
            }
        }
        else
        {
            foreach (var response in responses)
            {
                response.gameObject.SetActive(false);
                
                messageIndex += currentIteration[currentIterationIndex] + index;
                
                response.Write(CustomTagsManager.ProccessMessage(dialogs[messageIndex], out string bText), bText);
                
                if (messageIndex >= 9 && messageIndex <= 12)
                    messageIndex = 13;
                else if (messageIndex >= 16 && messageIndex <= 17)
                    currentIterationIndex = 18;
            }
        }
    }

    public override void SetInitialSettings()
    {
        CanvasGroup self = GetComponent<CanvasGroup>();

        self.interactable = true;
        self.blocksRaycasts = true;
    }
}
