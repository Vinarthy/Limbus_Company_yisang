using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterMovesh1 : MonoBehaviour
{
    private enum FaceState
    {
        Shrenne = 0,
        Mumble = 1
    }

    private static readonly int FaceStateId = Animator.StringToHash("FaceState");

    private RecordLineNumber record;
    private Plot_Dy plot;
    private Animator animator;
    private GameObject produce;
    private Tween exitTween;

    [Header("离场动作")]
    [SerializeField] private float exitX = 12.4f;
    [SerializeField] private float exitDuration = 1f;

    private void Start()
    {
        record = GetComponent<RecordLineNumber>();
        plot = GetComponent<Plot_Dy>();
        animator = GetComponent<Animator>();
        produce = FindProduceObject();

        if (record == null || plot == null || animator == null)
        {
            Debug.LogError("CharacterMovesh1 缺少 Animator、Plot_Dy 或 RecordLineNumber 组件", this);
            enabled = false;
            return;
        }

        ChangeFace(FaceState.Shrenne);
        record.OnLineChanged += OnLineChanged;
    }

    private void OnDestroy()
    {
        exitTween?.Kill();

        if (record != null)
            record.OnLineChanged -= OnLineChanged;
    }

    private void OnLineChanged(int line)
    {
        if (plot.x == 0)
        {
            if (line == 1)
                ChangeFace(FaceState.Mumble);
            else if (line == 3)
                ChangeFace(FaceState.Shrenne);
            else if (line == 4)
                StartCoroutine(WaitAndEnableProduce());
        }
        else if (plot.x == 1)
        {
            if (line == 1)
                ChangeFace(FaceState.Mumble);
            else if (line == 2)
                ChangeFace(FaceState.Shrenne);
            else if (line == 3)
            {
                MoveOutToRight();
                AdvanceScene();
            }
        }
    }

    private void MoveOutToRight()
    {
        exitTween?.Kill();
        exitTween = transform
            .DOMoveX(exitX, exitDuration)
            .SetEase(Ease.InQuad);
    }

    private void ChangeFace(FaceState state)
    {
        int value = (int)state;
        if (animator.GetInteger(FaceStateId) != value)
            animator.SetInteger(FaceStateId, value);
    }

    private static void AdvanceScene()
    {
        if (TimeBool.Instance != null)
            TimeBool.Instance.AdvanceType = StoryAdvanceType.Scene;
    }

    private IEnumerator WaitAndEnableProduce()
    {
        yield return new WaitForSeconds(0.3f);

        if (produce == null)
        {
            Debug.LogWarning("未找到 Canvas 下的 produce", this);
            yield break;
        }

        produce.SetActive(true);

        CanvasGroup canvasGroup = produce.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = produce.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvasGroup.DOFade(1f, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
    }

    private static GameObject FindProduceObject()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>(true);
        foreach (Canvas canvas in canvases)
        {
            if (canvas.name != "Canvas")
                continue;

            Transform target = canvas.transform.Find("produce");
            if (target != null)
                return target.gameObject;
        }

        return null;
    }
}
