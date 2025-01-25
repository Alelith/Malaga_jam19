using DG.Tweening;
using UnityEngine;

public class MobileController : MonoBehaviour
{
    [SerializeField] GameObject carrousel;
    
    public void OnChangeImage()
    {
        ((RectTransform)carrousel.transform).DOAnchorPosY(1080, 0.5f).OnComplete(() =>
        {
            carrousel.transform.GetChild(0).SetAsLastSibling();
            ((RectTransform)carrousel.transform).anchoredPosition = new Vector2(0, 0);
        });
    }
}
