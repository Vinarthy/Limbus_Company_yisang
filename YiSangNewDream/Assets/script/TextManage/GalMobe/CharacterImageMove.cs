using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImageMove : MonoBehaviour
{
    public Image targetImage;

    [SerializeField] private float duration = 0.8f;
    [SerializeField] private Ease ease = Ease.InOutQuad;

    private Tween moveTween;
    private Tween fadeTween;

    private void OnDestroy()
    {
        moveTween?.Kill();
        fadeTween?.Kill();
    }

    public void MoveToX(float targetX)
    {
        if (targetImage == null)
            return;

        moveTween?.Kill();
        moveTween = targetImage.rectTransform
            .DOAnchorPosX(targetX, duration)
            .SetEase(ease);
    }

    public void MoveByX(float offsetX, float moveDuration)
    {
        if (targetImage == null)
            return;

        moveTween?.Kill();
        moveTween = targetImage.rectTransform
            .DOAnchorPosX(targetImage.rectTransform.anchoredPosition.x + offsetX, moveDuration)
            .SetEase(ease);
    }

    public void FadeTo(float targetAlpha, float fadeDuration)
    {
        if (targetImage == null)
            return;

        fadeTween?.Kill();
        fadeTween = targetImage
            .DOFade(Mathf.Clamp01(targetAlpha), fadeDuration)
            .SetEase(ease);
    }
    public void StopMove()
    {
        moveTween?.Kill();
        moveTween = null;
    }
}