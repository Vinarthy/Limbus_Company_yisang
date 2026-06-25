using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterMoveHong1 : MonoBehaviour
{
    private static readonly int FaceStateId = Animator.StringToHash("FaceState");

    [SerializeField] private string followPath;

    private RecordLineNumber record;
    private Plot_Dy plot;
    private Animator animator;
    private GameObject produce;
    private bool switchingToFollow;

    private void Start()
    {
        record = GetComponent<RecordLineNumber>();
        plot = GetComponent<Plot_Dy>();
        animator = GetComponent<Animator>();
        produce = FindProduceObject();

        if (record == null || plot == null || animator == null)
        {
            Debug.LogError("CharacterMoveHong1 缺少 Animator、Plot_Dy 或 RecordLineNumber", this);
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
        switch (plot.x)
        {
            case 0:
                animator.SetInteger(FaceStateId, line == 1 || line == 3 ? 1 : 0);
                if (line == 5)
                    StartCoroutine(WaitAndEnableProduce());
                break;
            case 1:
                animator.SetInteger(FaceStateId, 1);
                if (line == 1)
                    PlayFollow();
                break;
            case 2:
                animator.SetInteger(FaceStateId, 0);
                if (line == 1)
                    PlayFollow();
                break;
            case 3:
                animator.SetInteger(FaceStateId, line == 2 || line == 5 ? 1 : 0);
                if (line == 7 && TimeBool.Instance != null)
                    TimeBool.Instance.AdvanceType = StoryAdvanceType.Scene;
                break;
        }
    }

    private void PlayFollow()
    {
        if (switchingToFollow || string.IsNullOrEmpty(followPath))
            return;

        switchingToFollow = true;
        plot.x = 3;
        plot.PlayNewPlot(followPath);
        switchingToFollow = false;
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
