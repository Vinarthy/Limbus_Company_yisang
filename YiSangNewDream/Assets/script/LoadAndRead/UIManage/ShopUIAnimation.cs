using UnityEngine;
using DG.Tweening;

public class ShopUIAnimation : MonoBehaviour
{
    public float openDuration = 0.3f;
    public float closeDuration = 0.2f;


    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    //保存原始缩放
    private Vector3 originalScale;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        //记录预制体原本大小
        originalScale = rectTransform.localScale;


        //初始化
        canvasGroup.alpha = 0;
        rectTransform.localScale = Vector3.zero;
    }


    private void Start()
    {
        OpenAnimation();
    }



    public void OpenAnimation()
    {
        transform.DOKill();


        canvasGroup
            .DOFade(1, openDuration);


        rectTransform
            .DOScale(originalScale, openDuration)
            .SetEase(Ease.OutBack);
    }



    public void CloseAnimation()
    {
        transform.DOKill();


        Sequence sequence = DOTween.Sequence();


        sequence.Append(
            canvasGroup.DOFade(0, closeDuration)
        );


        sequence.Join(
            rectTransform
            .DOScale(originalScale * 0.8f, closeDuration)
            .SetEase(Ease.InBack)
        );


        sequence.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}