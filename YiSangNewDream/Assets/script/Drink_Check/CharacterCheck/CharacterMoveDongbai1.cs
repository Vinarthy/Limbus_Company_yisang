using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterMoveDongbai1 : MonoBehaviour
{
    private static readonly int FaceStateId = Animator.StringToHash("FaceState");

    private RecordLineNumber record;
    private Plot_Dy plot;
    private Animator animator;
    private GameObject produce;
    private Tween exitTween;

    [Header("离场动作")]
    [SerializeField] private float exitX = -12.4f;
    [SerializeField] private float exitDuration = 1f;

    private void Start()
    {
        record = GetComponent<RecordLineNumber>();
        plot = GetComponent<Plot_Dy>();
        animator = GetComponent<Animator>();
        produce = FindProduceObject();

        if (record == null || plot == null || animator == null)
        {
            Debug.LogError("CharacterMoveDongbai1 缺少 Animator、Plot_Dy 或 RecordLineNumber", this);
            enabled = false;
            return;
        }

        animator.SetInteger(FaceStateId, 0);
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
            switch (line)
            {
                case 3:
                    animator.SetInteger(FaceStateId, 4);
                    break;

                case 5:
                    animator.SetInteger(FaceStateId, 0);
                    StartCoroutine(WaitAndEnableProduce());
                    break;
            }
        }
        else if (plot.x == 1)
        {
            switch (line)
            {
                case 3:
                    animator.SetInteger(FaceStateId, 1);
                    break;

                case 4:
                    animator.SetInteger(FaceStateId, 1);
                    break;

                case 5:
                    animator.SetInteger(FaceStateId, 1);
                    break;

                case 6:
                    animator.SetInteger(FaceStateId, 1);
                    MoveOutToLeft();
                    break;

                case 8:
                    animator.SetInteger(FaceStateId, 0);
                    if (TimeBool.Instance != null)
                        TimeBool.Instance.AdvanceType = StoryAdvanceType.Day;
                    break;

                default:
                    animator.SetInteger(FaceStateId, 0);
                    break;
            }
        }
    }

    private void MoveOutToLeft()
    {
        exitTween?.Kill();
        exitTween = transform
            .DOMoveX(exitX, exitDuration)
            .SetEase(Ease.InQuad);
    }
    private IEnumerator WaitAndEnableProduce()
    {
        yield return new WaitForSeconds(0.3f);
        if (produce == null)
            yield break;

        produce.SetActive(true);
        CanvasGroup group = produce.GetComponent<CanvasGroup>();
        if (group == null)
            group = produce.AddComponent<CanvasGroup>();

        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
        group.DOFade(1f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            group.interactable = true;
            group.blocksRaycasts = true;
        });
    }

    private static GameObject FindProduceObject()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>(true);
        foreach (Canvas canvas in canvases)
        {
            if (canvas.name == "Canvas")
            {
                Transform target = canvas.transform.Find("produce");
                if (target != null)
                    return target.gameObject;
            }
        }

        return null;
    }
}
