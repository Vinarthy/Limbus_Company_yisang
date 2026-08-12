using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageZoomSlideshow : MonoBehaviour
{
    [Header("图片配置")]
    public Image targetImage;
    public List<Sprite> images = new List<Sprite>();

    [Header("播放配置")]
    [Min(1f)] public float startScale = 1.2f;
    [Min(0f)] public float shrinkDuration = 3f;
    [Min(0f)] public float displayDuration = 1f;
    [Min(0f)] public float fadeDuration = 0.5f;
    public Ease shrinkEase = Ease.OutSine;
    public bool loop = true;
    public bool playOnStart = true;

    [Header("文字移动（可选）")]
    public RectTransform targetText;
    public Vector2 textStartPosition;
    public Vector2 textTargetPosition;
    [Min(0f)] public float textMoveDuration = 10f;
    public Ease textMoveEase = Ease.Linear;

    private Vector3 originalScale;
    private Sequence slideshowSequence;
    private Tween textMoveTween;

    private void Awake()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        originalScale = targetImage.rectTransform.localScale;
    }

    private void Start()
    {
        if (playOnStart)
            PlaySlideshow();
    }

    private void OnDestroy()
    {
        StopSlideshow();
    }

    [ContextMenu("播放图片序列")]
    public void PlaySlideshow()
    {
        StopSlideshow();

        if (targetImage == null)
        {
            Debug.LogError("ImageZoomSlideshow 缺少目标 Image。", this);
            return;
        }

        List<Sprite> validImages = GetValidImages();
        if (validImages.Count == 0)
        {
            Debug.LogWarning("ImageZoomSlideshow 的图片列表为空。", this);
            return;
        }

        PlayTextMove();

        RectTransform imageTransform = targetImage.rectTransform;
        float safeStartScale = Mathf.Max(1f, startScale);
        float safeShrinkDuration = Mathf.Max(0f, shrinkDuration);
        float safeDisplayDuration = Mathf.Max(0f, displayDuration);
        float safeFadeDuration = Mathf.Max(0f, fadeDuration);

        slideshowSequence = DOTween.Sequence();

        for (int i = 0; i < validImages.Count; i++)
        {
            Sprite currentImage = validImages[i];
            bool shouldFadeOut = loop || i < validImages.Count - 1;

            slideshowSequence.AppendCallback(() =>
            {
                targetImage.sprite = currentImage;
                imageTransform.localScale = originalScale * safeStartScale;
                SetImageAlpha(0f);
            });

            slideshowSequence
                .Append(targetImage.DOFade(1f, safeFadeDuration))
                .Join(imageTransform
                    .DOScale(originalScale, safeShrinkDuration)
                    .SetEase(shrinkEase))
                .AppendInterval(safeDisplayDuration);

            if (shouldFadeOut)
                slideshowSequence.Append(targetImage.DOFade(0f, safeFadeDuration));
        }

        if (loop)
            slideshowSequence.SetLoops(-1, LoopType.Restart);

        slideshowSequence.OnKill(() => slideshowSequence = null);
    }

    public void StopSlideshow()
    {
        slideshowSequence?.Kill();
        slideshowSequence = null;

        textMoveTween?.Kill();
        textMoveTween = null;
    }

    public void PlayTextMove()
    {
        textMoveTween?.Kill();
        textMoveTween = null;

        if (targetText == null)
            return;

        targetText.anchoredPosition = textStartPosition;

        float safeDuration = Mathf.Max(0f, textMoveDuration);
        if (safeDuration <= 0f)
        {
            targetText.anchoredPosition = textTargetPosition;
            return;
        }

        textMoveTween = targetText
            .DOAnchorPos(textTargetPosition, safeDuration)
            .SetEase(textMoveEase)
            .OnKill(() => textMoveTween = null);
    }

    private List<Sprite> GetValidImages()
    {
        List<Sprite> validImages = new List<Sprite>();
        foreach (Sprite image in images)
        {
            if (image != null)
                validImages.Add(image);
        }

        return validImages;
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = targetImage.color;
        color.a = alpha;
        targetImage.color = color;
    }
}
