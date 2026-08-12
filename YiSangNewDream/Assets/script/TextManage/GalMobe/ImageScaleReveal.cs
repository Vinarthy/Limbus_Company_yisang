using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageScaleReveal : MonoBehaviour
{
    public float duration = 0.8f;

    [SerializeField] private Ease ease = Ease.OutBack;

    private RectTransform targetRectTransform;
    private Vector3 originalScale;
    private Tween scaleTween;

    private void Awake()
    {
        targetRectTransform = GetComponent<RectTransform>();
        originalScale = targetRectTransform.localScale;
    }

    private void Start()
    {
        Play();
    }

    private void OnDestroy()
    {
        scaleTween?.Kill();
    }

    public void Play()
    {
        scaleTween?.Kill();
        targetRectTransform.localScale = Vector3.zero;
        scaleTween = targetRectTransform
            .DOScale(originalScale, duration)
            .SetEase(ease);
    }

    public void ResetToZero()
    {
        scaleTween?.Kill();
        targetRectTransform.localScale = Vector3.zero;
    }
}
//image放大脚本，我不知道到时候我还能不能记得