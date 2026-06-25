using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterMoveFaust2 : MonoBehaviour
{
    private static readonly int FaceStateId = Animator.StringToHash("FaceState");

    private RecordLineNumber record;
    private Plot_Dy plot;
    private Animator animator;
    private GameObject produce;

    private void Start()
    {
        record = GetComponent<RecordLineNumber>();
        plot = GetComponent<Plot_Dy>();
        animator = GetComponent<Animator>();
        produce = FindProduceObject();

        if (record == null || plot == null || animator == null)
        {
            Debug.LogError("CharacterMoveFaust2 缺少 Animator、Plot_Dy 或 RecordLineNumber 组件", this);
            enabled = false;
            return;
        }

        animator.SetInteger(FaceStateId, 0);
        record.OnLineChanged += OnLineChanged;
    }

    private void OnDestroy()
    {
        if (record != null)
            record.OnLineChanged -= OnLineChanged;
    }

    private void OnLineChanged(int line)
    {
        animator.SetInteger(FaceStateId, 0);

        if (plot.x == 0 && line == 5)
        {
            StartCoroutine(WaitAndEnableProduce());
        }
        else if (plot.x == 1 && line == 10)
        {
            if (TimeBool.Instance != null)
                TimeBool.Instance.AdvanceType = StoryAdvanceType.Scene;
        }
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
