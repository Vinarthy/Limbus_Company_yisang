using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterMoveFaust1 : MonoBehaviour
{
    private enum FaceState
    {
        Transparent = 0
    }

    private static readonly int FaceStateId = Animator.StringToHash("FaceState");

    private RecordLineNumber record;
    private Plot_Dy plot;
    private GameObject produce;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        plot = GetComponent<Plot_Dy>();
        record = GetComponent<RecordLineNumber>();
        produce = FindProduceObject();

        if (record == null || plot == null || animator == null)
        {
            Debug.LogError("CharacterMoveFaust1 缺少 Animator、Plot_Dy 或 RecordLineNumber 组件", this);
            enabled = false;
            return;
        }

        ChangeFace(FaceState.Transparent);
        record.OnLineChanged += OnLineChanged;
    }

    private void OnDestroy()
    {
        if (record != null)
            record.OnLineChanged -= OnLineChanged;
    }

    private void OnLineChanged(int line)
    {
        ChangeFace(FaceState.Transparent);

        if (plot.x == 0 && line == 4)
        {
            StartCoroutine(WaitAndEnableProduce());
        }
        else if (plot.x == 1 && line == 1)
        {
            AdvanceScene();
        }
        else if (plot.x == 2 && line == 3)
        {
            AdvanceScene();
        }
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
