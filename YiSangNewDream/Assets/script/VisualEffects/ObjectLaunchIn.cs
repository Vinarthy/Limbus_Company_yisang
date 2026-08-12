using DG.Tweening;
using UnityEngine;

[DisallowMultipleComponent]
public class ObjectLaunchIn : MonoBehaviour
{
    [Header("入场位置")]
    [SerializeField] private Vector3 startOffset = new Vector3(-12f, 0f, 0f);

    [Header("播放配置")]
    [SerializeField, Min(0f)] private float delay;
    [SerializeField, Min(0f)] private float duration = 0.6f;
    [SerializeField] private Ease ease = Ease.OutBack;
    [SerializeField, Min(0f)] private float overshoot = 1.7f;

    private Tween launchTween;

    private void Start()
    {
        Vector3 targetPosition = transform.localPosition;
        transform.localPosition = targetPosition + startOffset;

        launchTween = transform
            .DOLocalMove(targetPosition, Mathf.Max(0f, duration))
            .SetDelay(Mathf.Max(0f, delay))
            .SetEase(ease, overshoot);
    }

    private void OnDestroy()
    {
        launchTween?.Kill();
    }
}
