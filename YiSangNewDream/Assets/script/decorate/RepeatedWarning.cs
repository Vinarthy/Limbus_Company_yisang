using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RepeatedWarning : MonoBehaviour
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void OnEnable()
    {
        PlayWarning();
    }

    void PlayWarning()
    {
        // 初始状态
        rectTransform.anchoredPosition =
            new Vector2(
                rectTransform.anchoredPosition.x,
                -2.51f
            );

        canvasGroup.alpha = 0.2f;

        Sequence seq = DOTween.Sequence();

        // 位置移动
        seq.Join(
            rectTransform.DOAnchorPosY(
                -0.63f,
                0.5f
            )
        );

        // 透明度
        seq.Join(
            canvasGroup.DOFade(
                0.76f,
                0.25f
            )
        );

        seq.Append(
            canvasGroup.DOFade(
                0.2f,
                0.25f
            )
        );

        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
//先从下面一点点飘上了在票上去，同时注意透明度
//
