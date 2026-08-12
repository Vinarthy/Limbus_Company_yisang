using DG.Tweening;
using UnityEngine;

public class ObjectFadeInOut : MonoBehaviour
{
    [Header("目标对象")]
    public GameObject targetObject;

    [Header("播放配置")]
    [Min(0f)] public float fadeInDuration = 0.25f;
    [Min(0f)] public float visibleDuration = 1f;
    [Min(0f)] public float fadeOutDuration = 0.25f;
    public bool playOnStart = true;

    private CanvasGroup canvasGroup;
    private Sequence fadeSequence;

    private void Awake()
    {
        if (targetObject == null)
            return;

        EnsureCanvasGroup();
        targetObject.SetActive(false);
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        fadeSequence?.Kill();
    }

    public void Play()
    {
        if (targetObject == null)
        {
            Debug.LogError("ObjectFadeInOut 缺少目标对象。", this);
            return;
        }

        EnsureCanvasGroup();
        fadeSequence?.Kill();

        targetObject.SetActive(true);
        canvasGroup.alpha = 0f;

        fadeSequence = DOTween.Sequence()
            .Append(canvasGroup.DOFade(1f, Mathf.Max(0f, fadeInDuration)))
            .AppendInterval(Mathf.Max(0f, visibleDuration))
            .Append(canvasGroup.DOFade(0f, Mathf.Max(0f, fadeOutDuration)))
            .OnComplete(() => targetObject.SetActive(false))
            .OnKill(() => fadeSequence = null);
    }

    private void EnsureCanvasGroup()
    {
        if (canvasGroup == null)
            canvasGroup = targetObject.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = targetObject.AddComponent<CanvasGroup>();
    }
}
