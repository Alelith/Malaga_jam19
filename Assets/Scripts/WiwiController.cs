using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiwiController : IInitialSettings
{
    public override void SetInitialSettings()
    {
        CanvasGroup self = GetComponent<CanvasGroup>();
        
        self.interactable = true;
        self.blocksRaycasts = true;
    }
}
