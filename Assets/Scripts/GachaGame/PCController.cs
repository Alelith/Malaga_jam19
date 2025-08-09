using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCController : IInitialSettings
{
    [SerializeField]
    [Tooltip("Core game object.")]
    GameObject gameCore;
    
    public override void SetInitialSettings()
    {
        CanvasGroup self = GetComponent<CanvasGroup>();

        self.interactable = true;
        self.blocksRaycasts = true;

        gameCore.SetActive(true);
    }
}
