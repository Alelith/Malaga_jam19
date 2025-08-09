using System;
using DG.Tweening;
using DialogSystem;
using TMPro;
using UnityEngine;

public class MobileController : IInitialSettings
{
    [SerializeField]
    [Tooltip("Carousel game object.")]
    GameObject carrousel;

    [SerializeField]
    [Tooltip("Text component for displaying messages.")]
    CanvasGroup text;

    [SerializeField]
    [Tooltip("Array of typewriter components for displaying responses.")]
    Typewriter[] responses;

    [SerializeField]
    [Tooltip("Dialog data.")]
    Dialog dialogs;

    /// <summary>
    /// Current message index.
    /// </summary>
    int messageIndex = 0;

    /// <summary>
    /// Number of iterations for each message.
    /// </summary>
    int[] iterations = new[] { 2, 1, 2 };
    /// <summary>
    /// Current iteration count for each message.
    /// </summary>
    int[] currentIteration = new[] { 0, 0, 0 };
    
    /// <summary>
    /// Number of messages to show.
    /// </summary>
    int showMessage = 2;

    /// <summary>
    /// Handles the change of image in the carousel.
    /// </summary>
    public void OnChangeImage()
    {
        ((RectTransform)carrousel.transform).DOAnchorPosY(1080, 0.5f).OnComplete(() =>
        {
            showMessage--;
            if (showMessage == 0)
            {
                // Not implemented yet
            }
            carrousel.transform.GetChild(0).SetAsLastSibling();
            ((RectTransform)carrousel.transform).anchoredPosition = new Vector2(0, 0);
        });
    }

    /// <summary>
    /// Continues the iterations for the current message.
    /// </summary>
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

    /// <summary>
    /// Sets the initial settings for the mobile controller.
    /// </summary>
    public override void SetInitialSettings()
    {
        CanvasGroup self = GetComponent<CanvasGroup>();

        self.interactable = true;
        self.blocksRaycasts = true;
    }
}
